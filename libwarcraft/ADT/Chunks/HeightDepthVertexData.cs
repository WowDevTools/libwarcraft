//
//  HeightDepthVertexData.cs
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
    /// Represents a liquid vertex block with height and depth.
    /// </summary>
    public sealed class HeightDepthVertexData : LiquidVertexData
    {
        /// <summary>
        /// Gets or sets the height map in the block.
        /// </summary>
        public List<float> Heightmap { get; set; } = new List<float>();

        /// <summary>
        /// Gets or sets the depth map in the block.
        /// </summary>
        public List<byte> Depthmap { get; set; } = new List<byte>();

        /// <summary>
        /// Initializes a new instance of the <see cref="HeightDepthVertexData"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        /// <param name="width">The width of the vertex block.</param>
        /// <param name="height">The height of the vertex block.</param>
        public HeightDepthVertexData(byte[] data, byte width, byte height)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    var arrayEntryCount = (width + 1) * (height + 1);
                    for (var i = 0; i < arrayEntryCount; ++i)
                    {
                        Heightmap.Add(br.ReadSingle());
                    }

                    for (var i = 0; i < arrayEntryCount; ++i)
                    {
                        Depthmap.Add(br.ReadByte());
                    }
                }
            }
        }

        /// <summary>
        /// Gets the byte size of the vertex block.
        /// </summary>
        /// <param name="width">The width of the vertex block.</param>
        /// <param name="height">The height of the vertex block.</param>
        /// <returns>The byte size of the vertex block.</returns>
        public static int GetBlockSize(byte width, byte height)
        {
            var arrayEntryCount = (width + 1) * (height + 1);

            return (sizeof(float) + sizeof(byte)) * arrayEntryCount;
        }
    }
}
