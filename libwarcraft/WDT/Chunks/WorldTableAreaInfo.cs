//
//  WorldTableAreaInfo.cs
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

using System;
using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.WDT.Chunks
{
    /// <summary>
    /// Represents area information in a world table.
    /// </summary>
    public class WorldTableAreaInfo : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MAIN";

        /// <summary>
        /// Gets the entries in the area information table.
        /// </summary>
        public List<AreaInfoEntry> Entries { get; } = new List<AreaInfoEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldTableAreaInfo"/> class.
        /// </summary>
        public WorldTableAreaInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldTableAreaInfo"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public WorldTableAreaInfo(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    for (uint y = 0; y < 64; ++y)
                    {
                        for (uint x = 0; x < 64; ++x)
                        {
                            Entries.Add(new AreaInfoEntry(br.ReadBytes((int)AreaInfoEntry.GetSize()), x, y));
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <summary>
        /// Retrieves the area information at the given coordinates.
        /// </summary>
        /// <param name="inTileX">The tile's X coordinate.</param>
        /// <param name="inTileY">The tile's Y coordinate.</param>
        /// <returns>The tile.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either of the coordinates is more than 63.</exception>
        public AreaInfoEntry GetAreaInfo(uint inTileX, uint inTileY)
        {
            if (inTileX > 63)
            {
                throw new ArgumentOutOfRangeException(nameof(inTileX), "The tile coordinate may not be more than 63 in either dimension.");
            }

            if (inTileY > 63)
            {
                throw new ArgumentOutOfRangeException(nameof(inTileY), "The tile coordinate may not be more than 63 in either dimension.");
            }

            int tileIndex = (int)((inTileY * 64) + inTileX);
            return Entries[tileIndex];
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    foreach (AreaInfoEntry entry in Entries)
                    {
                        bw.Write(entry.Serialize());
                    }

                    bw.Flush();
                }

                return ms.ToArray();
            }
        }
    }
}
