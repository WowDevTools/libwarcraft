//
//  HashTableEntry.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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
using System.IO;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Locale;

namespace Warcraft.MPQ.Tables.Hash
{
    /// <summary>
    /// An entry in the hash table of an MPQ archive, containing data about where to
    /// find the block entry for information about the file data.
    /// </summary>
    public class HashTableEntry : IBinarySerializable
    {
        /// <summary>
        /// The hashed file path, using algorithm A.
        /// </summary>
        private readonly uint _filePathHashA;

        /// <summary>
        /// The hashed file path, using algorithm B.
        /// </summary>
        private readonly uint _filePathHashB;

        /// <summary>
        /// The localization (language) of the file referred to by this entry. There may exist multiple
        /// hash table entries for the same file, but in different languages.
        /// </summary>
        private readonly LocaleID _localization;

        /// <summary>
        /// The platform of the file referred to by this entry. There may exist multiple
        /// hash table entries for the same file, but for different platforms.
        /// </summary>
        private readonly ushort _platform;

        /// <summary>
        /// The index of the block entry which holds the offset data for the file referred to by
        /// this entry.
        /// </summary>
        private readonly uint _fileBlockIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashTableEntry"/> class.
        /// </summary>
        /// <param name="data">The serialized hash table data.</param>
        public HashTableEntry(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    _filePathHashA = br.ReadUInt32();
                    _filePathHashB = br.ReadUInt32();
                    _localization = (LocaleID)br.ReadUInt16();

                    // Read the platform as an int8 and skip the next byte
                    _platform = br.ReadByte();
                    br.BaseStream.Position += 1;

                    _fileBlockIndex = br.ReadUInt32();
                }
            }
        }

        /// <summary>
        /// Determines whether this has file ever existed.
        /// </summary>
        /// <returns><c>true</c> if this file ever existed; otherwise, <c>false</c>.</returns>
        public bool HasFileEverExisted()
        {
            return _fileBlockIndex != 0xFFFFFFFF;
        }

        /// <summary>
        /// Determines whether this file exists in the archive. If this returns false for a valid hash
        /// table entry, it is most likely a deletion marker.
        /// </summary>
        /// <returns><c>true</c>, if the file exists, <c>false</c> otherwise.</returns>
        public bool DoesFileExist()
        {
            return _fileBlockIndex != 0xFFFFFFFE || _fileBlockIndex != 0xFFFFFFFF;
        }

        /// <summary>
        /// Gets the primary hash of the file's name.
        /// </summary>
        /// <returns>The primary hash.</returns>
        public uint GetPrimaryHash()
        {
            return _filePathHashA;
        }

        /// <summary>
        /// Gets the secondary hash of the file's name.
        /// </summary>
        /// <returns>The secondary hash.</returns>
        public uint GetSecondaryHash()
        {
            return _filePathHashB;
        }

        /// <summary>
        /// Gets the localization ID.
        /// </summary>
        /// <returns>The localization ID.</returns>
        public LocaleID GetLocalizationID()
        {
            return _localization;
        }

        /// <summary>
        /// Gets the platform ID.
        /// </summary>
        /// <returns>The platform ID.</returns>
        public ushort GetPlatformID()
        {
            return _platform;
        }

        /// <summary>
        /// Gets the index of the block entry referenced by this entry.
        /// </summary>
        /// <returns>The block entry index.</returns>
        public uint GetBlockEntryIndex()
        {
            return _fileBlockIndex;
        }

        /// <summary>
        /// Serializes the current object into a byte array.
        /// </summary>
        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(_filePathHashA);
                    bw.Write(_filePathHashB);
                    bw.Write((ushort)_localization);
                    bw.Write(_platform);
                    bw.Write(_fileBlockIndex);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Gets the size of an entry.
        /// </summary>
        /// <returns>The size.</returns>
        public static long GetSize()
        {
            return 16;
        }
    }
}
