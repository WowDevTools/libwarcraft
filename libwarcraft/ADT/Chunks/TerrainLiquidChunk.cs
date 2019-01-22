//
//  TerrainLiquidChunk.cs
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

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// Terrain liquid chunk. Contains information about water and other liquids in a map tile.
    /// </summary>
    public class TerrainLiquidChunk
    {
        /// <summary>
        /// Gets or sets the offset to the water instance.
        /// </summary>
        public uint WaterInstanceOffset { get; set; }

        /// <summary>
        /// Gets or sets the layer count.
        /// </summary>
        public uint LayerCount { get; set; }

        /// <summary>
        /// Gets or sets the offset to the liquid attributes.
        /// </summary>
        public uint AttributesOffset { get; set; }

        /// <summary>
        /// Gets or sets the liquid instances.
        /// </summary>
        public List<TerrainLiquidInstance> LiquidInstances { get; set; } = new List<TerrainLiquidInstance>();

        /// <summary>
        /// Gets or sets the liquid attributes.
        /// </summary>
        public TerrainLiquidAttributes LiquidAttributes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainLiquidChunk"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        public TerrainLiquidChunk(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    WaterInstanceOffset = br.ReadUInt32();
                    LayerCount = br.ReadUInt32();
                    AttributesOffset = br.ReadUInt32();
                }
            }
        }

        /// <summary>
        /// Gets the size of a chunk.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 12;
        }
    }
}
