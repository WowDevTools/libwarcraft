//
//  BlockTable.cs
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
using System.IO;
using JetBrains.Annotations;
using Warcraft.MPQ.Crypto;

namespace Warcraft.MPQ.Tables.Block
{
    /// <summary>
    /// Represents the block table in an archive.
    /// </summary>
    [PublicAPI]
    public class BlockTable
    {
        /// <summary>
        /// Holds the table encryption key.
        /// </summary>
        [PublicAPI]
        public static readonly uint TableKey = MPQCrypt.Hash("(block table)", HashType.FileKey);

        private readonly List<BlockTableEntry> _entries = new List<BlockTableEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTable"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        [PublicAPI]
        public BlockTable([NotNull] byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            for (long i = 0; i < data.Length; i += BlockTableEntry.GetSize())
            {
                var entryBytes = br.ReadBytes((int)BlockTableEntry.GetSize());
                _entries.Add(new BlockTableEntry(entryBytes));
            }
        }

        /// <summary>
        /// Serializes the table.
        /// </summary>
        /// <returns>The serialized bytes.</returns>
        public byte[] Serialize()
        {
            using var ms = new MemoryStream();
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

        /// <summary>
        /// Gets the entry at the given index.
        /// </summary>
        /// <param name="index">The element index.</param>
        /// <returns>The entry at the index.</returns>
        [PublicAPI]
        public BlockTableEntry GetEntry(int index)
        {
            return _entries[index];
        }

        /// <summary>
        /// Gets the size in bytes of the table.
        /// </summary>
        /// <returns>The size.</returns>
        public ulong GetSize()
        {
            return (ulong)(_entries.Count * BlockTableEntry.GetSize());
        }
    }
}
