//
//  WorldLODMapAreaOffsets.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.WDL.Chunks
{
    /// <summary>
    /// Represents the offsets of a map area.
    /// </summary>
    public class WorldLODMapAreaOffsets : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MAOF";

        /// <summary>
        /// Gets the map area offsets.
        /// </summary>
        public List<uint> MapAreaOffsets { get; } = new List<uint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldLODMapAreaOffsets"/> class.
        /// </summary>
        public WorldLODMapAreaOffsets()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldLODMapAreaOffsets"/> class.
        /// </summary>
        /// <param name="inData">The input data.</param>
        public WorldLODMapAreaOffsets(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    for (var y = 0; y < 64; ++y)
                    {
                        for (var x = 0; x < 64; ++x)
                        {
                            MapAreaOffsets.Add(br.ReadUInt32());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the binary size of the instance.
        /// </summary>
        /// <returns>The size in bytes.</returns>
        public static int GetSize()
        {
            return (64 * 64) * sizeof(uint);
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    foreach (var mapAreaOffset in MapAreaOffsets)
                    {
                        bw.Write(mapAreaOffset);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
