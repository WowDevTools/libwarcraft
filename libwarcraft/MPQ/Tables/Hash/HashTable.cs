//
//  HashTable.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;
using Warcraft.Core.Interfaces;
using Warcraft.MPQ.Crypto;

namespace Warcraft.MPQ.Tables.Hash
{
    /// <summary>
    /// The hash table is a table containing hashed file paths for quick lookup in the archive. When stored
    /// in binary format, it can be both compressed and encrypted.
    /// </summary>
    [PublicAPI]
    public class HashTable : IBinarySerializable
    {
        /// <summary>
        /// The encryption key for the hash table data.
        /// </summary>
        [PublicAPI]
        public static readonly uint TableKey = MPQCrypt.Hash("(hash table)", HashType.FileKey);

        /// <summary>
        /// The entries contained in the hash table.
        /// </summary>
        private readonly List<HashTableEntry> _entries = new List<HashTableEntry>(65536);

        /// <summary>
        /// Initializes a new instance of the <see cref="HashTable"/> class from
        /// a block of data containing hash table entries.
        /// </summary>
        /// <param name="data">ExtendedData.</param>
        [PublicAPI]
        public HashTable([JetBrains.Annotations.NotNull] byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    for (long i = 0; i < data.Length; i += HashTableEntry.GetSize())
                    {
                        var entryBytes = br.ReadBytes((int)HashTableEntry.GetSize());
                        var newEntry = new HashTableEntry(entryBytes);
                        _entries.Add(newEntry);
                    }
                }
            }
        }

        /// <summary>
        /// Finds a valid entry for a given filename.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The entry.</returns>
        /// <exception cref="FileNotFoundException">
        /// Thrown if the given file path has no entry in the table.
        /// </exception>
        [PublicAPI, JetBrains.Annotations.NotNull]
        public HashTableEntry FindEntry([JetBrains.Annotations.NotNull] string fileName)
        {
            if (!TryFindEntry(fileName, out var entry))
            {
                throw new FileNotFoundException("The given file has no entry in the hash table.", fileName);
            }

            return entry;
        }

        /// <summary>
        /// Finds a valid entry for a given hash pair, starting at the specified offset.
        /// </summary>
        /// <param name="hashA">A hash of the filename (Algorithm A).</param>
        /// <param name="hashB">A hash of the filename (Algorithm B).</param>
        /// <param name="entryHomeIndex">The home index for the file we're searching for. Reduces lookup times.</param>
        /// <returns>The entry.</returns>
        /// <exception cref="FileNotFoundException">
        /// Thrown if the given hash combination has no entry in the table.
        /// </exception>
        [PublicAPI, JetBrains.Annotations.NotNull]
        public HashTableEntry FindEntry(uint hashA, uint hashB, uint entryHomeIndex)
        {
            if (!TryFindEntry(hashA, hashB, entryHomeIndex, out var entry))
            {
                throw new FileNotFoundException("The given hash combination has no entry in the hash table.");
            }

            return entry;
        }

        /// <summary>
        /// Attempts to find a valid entry for a given filename.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="tableEntry">The entry, or null.</param>
        /// <returns>The entry.</returns>
        [PublicAPI]
        [ContractAnnotation("=> false, tableEntry : null; => true, tableEntry : notnull")]
        public bool TryFindEntry
        (
            string fileName,
            [NotNullWhen(true)] out HashTableEntry? tableEntry
        )
        {
            var entryHomeIndex = MPQCrypt.Hash(fileName, HashType.FileHashTableOffset) & (uint)_entries.Count - 1;
            var hashA = MPQCrypt.Hash(fileName, HashType.FilePathA);
            var hashB = MPQCrypt.Hash(fileName, HashType.FilePathB);

            if (!TryFindEntry(hashA, hashB, entryHomeIndex, out tableEntry))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to find a valid entry for a given hash pair, starting at the specified offset.
        /// </summary>
        /// <param name="hashA">A hash of the filename (Algorithm A).</param>
        /// <param name="hashB">A hash of the filename (Algorithm B).</param>
        /// <param name="entryHomeIndex">The home index for the file we're searching for. Reduces lookup times.</param>
        /// <param name="tableEntry">The entry, or null.</param>
        /// <returns>The entry.</returns>
        [PublicAPI]
        [ContractAnnotation("=> false, tableEntry : null; => true, tableEntry : notnull")]
        public bool TryFindEntry
        (
            uint hashA,
            uint hashB,
            uint entryHomeIndex,
            [NotNullWhen(true)] out HashTableEntry? tableEntry
        )
        {
            tableEntry = null;

            // First, see if the file has ever existed. If it has and matches, return it.
            if (_entries[(int)entryHomeIndex].HasFileEverExisted())
            {
                if (_entries[(int)entryHomeIndex].GetPrimaryHash() == hashA && _entries[(int)entryHomeIndex].GetSecondaryHash() == hashB)
                {
                    tableEntry = _entries[(int)entryHomeIndex];
                    return true;
                }
            }
            else
            {
                return false;
            }

            // If that file doesn't match (but has existed, or is occupied, let's keep looking down the table.
            HashTableEntry currentEntry;
            HashTableEntry? deletionEntry = null;
            for (var i = (int)entryHomeIndex + 1; i < _entries.Count - 1; ++i)
            {
                currentEntry = _entries[i];
                if (!currentEntry.HasFileEverExisted())
                {
                    continue;
                }

                if (currentEntry.GetPrimaryHash() != hashA || currentEntry.GetSecondaryHash() != hashB)
                {
                    continue;
                }

                if (currentEntry.DoesFileExist())
                {
                    // Found it!
                    tableEntry = currentEntry;
                    return true;
                }

                // The file might have been deleted. Store it as a possible return candidate, but keep looking.
                deletionEntry = currentEntry;
            }

            // Still nothing? Loop around and scan the start of the table as well
            for (var i = 0; i < entryHomeIndex; ++i)
            {
                currentEntry = _entries[i];
                if (!currentEntry.HasFileEverExisted())
                {
                    continue;
                }

                if (currentEntry.GetPrimaryHash() != hashA || currentEntry.GetSecondaryHash() != hashB)
                {
                    continue;
                }

                if (currentEntry.DoesFileExist())
                {
                    // Found it!
                    tableEntry = currentEntry;
                    return true;
                }

                // The file might have been deleted. Store it as a possible return candidate, but keep looking.
                deletionEntry = currentEntry;
            }

            if (deletionEntry is null)
            {
                return false;
            }

            // We found the file, but it's been deleted.
            tableEntry = deletionEntry;
            return true;
        }

        /// <summary>
        /// Serializes the current object into a byte array.
        /// </summary>
        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    foreach (var entry in _entries)
                    {
                        bw.Write(entry.Serialize());
                    }
                }

                var encryptedTable = MPQCrypt.EncryptData(ms.ToArray(), TableKey);
                return encryptedTable;
            }
        }

        /// <summary>
        /// Gets the size of the entire hash table.
        /// </summary>
        /// <returns>The size.</returns>
        [PublicAPI]
        public ulong GetSize()
        {
            return (ulong)(_entries.Count * HashTableEntry.GetSize());
        }
    }
}
