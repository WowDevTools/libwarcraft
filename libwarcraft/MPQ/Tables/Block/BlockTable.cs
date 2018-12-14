//
//  MPQBlockTable.cs
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

using System.Collections.Generic;
using System.IO;
using Warcraft.MPQ.Crypto;

namespace Warcraft.MPQ.Tables.Block
{
    public class BlockTable
    {
        public static readonly uint TableKey = MPQCrypt.Hash("(block table)", HashType.FileKey);
        private readonly List<BlockTableEntry> Entries = new List<BlockTableEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTable"/> class.
        /// </summary>
        public BlockTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTable"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        public BlockTable(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    for (long i = 0; i < data.Length; i += BlockTableEntry.GetSize())
                    {
                        byte[] entryBytes = br.ReadBytes((int)BlockTableEntry.GetSize());
                        Entries.Add(new BlockTableEntry(entryBytes));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    foreach (BlockTableEntry entry in Entries)
                    {
                        bw.Write(entry.Serialize());
                    }
                }

                byte[] encryptedTable = MPQCrypt.EncryptData(ms.ToArray(), TableKey);
                return encryptedTable;
            }
        }

        public BlockTableEntry GetEntry(int index)
        {
            return Entries[index];
        }

        public ulong GetSize()
        {
            return (ulong)(Entries.Count * BlockTableEntry.GetSize());
        }
    }
}

