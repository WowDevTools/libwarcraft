//
//  TerrainLiquid.cs
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

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MH2O Chunk - Contains liquid information about the ADT file, superseding the older MCLQ chunk.
    /// </summary>
    public class TerrainLiquid : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MH2O";

        /// <summary>
        /// Gets or sets the liquid chunks in this map tile.
        /// </summary>
        public List<TerrainLiquidChunk> LiquidChunks { get; set; } = new List<TerrainLiquidChunk>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainLiquid"/> class.
        /// </summary>
        public TerrainLiquid()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainLiquid"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public TerrainLiquid(byte[] inData)
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
                    for (int i = 0; i < 256; ++i)
                    {
                        LiquidChunks.Add(new TerrainLiquidChunk(br.ReadBytes(TerrainLiquidChunk.GetSize())));
                    }

                    foreach (TerrainLiquidChunk liquidChunk in LiquidChunks)
                    {
                        br.BaseStream.Position = liquidChunk.WaterInstanceOffset;
                        for (int i = 0; i < liquidChunk.LayerCount; ++i)
                        {
                            byte[] instanceData = br.ReadBytes(TerrainLiquidInstance.GetSize());
                            liquidChunk.LiquidInstances.Add(new TerrainLiquidInstance(instanceData));
                        }

                        br.BaseStream.Position = liquidChunk.AttributesOffset;
                        if (liquidChunk.LayerCount > 0)
                        {
                            byte[] attributeData = br.ReadBytes(TerrainLiquidAttributes.GetSize());
                            liquidChunk.LiquidAttributes = new TerrainLiquidAttributes(attributeData);
                        }
                        else
                        {
                            liquidChunk.LiquidAttributes = new TerrainLiquidAttributes();
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
