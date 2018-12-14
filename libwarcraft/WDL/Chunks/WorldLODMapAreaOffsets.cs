//
//  WorldLODMapAreaOffsets.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.WDL.Chunks
{
    public class WorldLODMapAreaOffsets : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MAOF";

        public readonly List<uint> MapAreaOffsets = new List<uint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldLODMapAreaOffsets"/> class.
        /// </summary>
        public WorldLODMapAreaOffsets()
        {
        }

        public WorldLODMapAreaOffsets(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    for (int y = 0; y < 64; ++y)
                    {
                        for (int x = 0; x < 64; ++x)
                        {
                            MapAreaOffsets.Add(br.ReadUInt32());
                        }
                    }
                }
            }
        }

        public static int GetSize()
        {
            return (64 * 64) * sizeof(uint);
        }

        public string GetSignature()
        {
            return Signature;
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    foreach (uint mapAreaOffset in MapAreaOffsets)
                    {
                        bw.Write(mapAreaOffset);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}

