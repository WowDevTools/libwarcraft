//
//  LiquidVertex.cs
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

using System;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// Represents a vertex in a liquid chunk.
    /// </summary>
    public class LiquidVertex : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets texture coordinates for this vertex.
        /// </summary>
        public Tuple<ushort, ushort> TextureCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the height of the liquid vertex.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiquidVertex"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        public LiquidVertex(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            TextureCoordinates = new Tuple<ushort, ushort>(br.ReadUInt16(), br.ReadUInt16());
            Height = br.ReadSingle();
        }

        /// <summary>
        /// Gets the serialized size of the vertex.
        /// </summary>
        /// <returns>The size in bytes.</returns>
        public static int GetSize()
        {
            return 8;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(TextureCoordinates.Item1);
                bw.Write(TextureCoordinates.Item2);

                bw.Write(Height);
            }

            return ms.ToArray();
        }
    }
}
