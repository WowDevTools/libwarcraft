//
//  MapChunkLiquids.cs
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

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// MCLQ chunk - holds liquid vertex data.
    /// </summary>
    public class MapChunkLiquids : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCLQ";

        /// <summary>
        /// Gets or sets the minimum liquid level.
        /// </summary>
        public float MinimumLiquidLevel { get; set; }

        /// <summary>
        /// Gets or sets the maximum liquid level.
        /// </summary>
        public float MaxiumLiquidLevel { get; set; }

        /// <summary>
        /// Gets or sets the liquid vertices in the chunk.
        /// </summary>
        public List<LiquidVertex> LiquidVertices { get; set; } = new List<LiquidVertex>();

        /// <summary>
        /// Gets or sets the flags for each liquid tile.
        /// </summary>
        public List<LiquidFlags> LiquidTileFlags { get; set; } = new List<LiquidFlags>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkLiquids"/> class.
        /// </summary>
        public MapChunkLiquids()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkLiquids"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public MapChunkLiquids(byte[] inData)
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
                    MinimumLiquidLevel = br.ReadSingle();
                    MaxiumLiquidLevel = br.ReadSingle();

                    // Future note: New information suggests there may be more than one liquid layer here, based on
                    // the chunk flags (i.e, one layer for river, one layer for ocean, etc)
                    for (int y = 0; y < 9; ++y)
                    {
                        for (int x = 0; x < 9; ++x)
                        {
                            LiquidVertices.Add(new LiquidVertex(br.ReadBytes(LiquidVertex.GetSize())));
                        }
                    }

                    for (int y = 0; y < 8; ++y)
                    {
                        for (int x = 0; x < 8; ++x)
                        {
                            LiquidTileFlags.Add((LiquidFlags)br.ReadByte());
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
    }
}
