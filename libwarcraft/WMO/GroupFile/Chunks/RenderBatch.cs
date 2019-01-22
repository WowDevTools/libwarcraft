//
//  RenderBatch.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;

namespace Warcraft.WMO.GroupFile.Chunks
{
    /// <summary>
    /// Represents a rendering batch.
    /// </summary>
    public class RenderBatch : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the bounding box of the batch.
        /// </summary>
        public ShortBox BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the first polygon index.
        /// </summary>
        public uint FirstPolygonIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of polygons.
        /// </summary>
        public ushort PolygonIndexCount { get; set; }

        /// <summary>
        /// Gets or sets the first vertex index.
        /// </summary>
        public ushort FirstVertexIndex { get; set; }

        /// <summary>
        /// Gets or sets the last vertex index.
        /// </summary>
        public ushort LastVertexIndex { get; set; }

        /// <summary>
        /// Gets or sets an unknown flag value.
        /// </summary>
        public byte UnknownFlags { get; set; }

        /// <summary>
        /// Gets or sets the batch's material index.
        /// </summary>
        public byte MaterialIndex { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderBatch"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public RenderBatch(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    BoundingBox = br.ReadShortBox();
                    FirstPolygonIndex = br.ReadUInt32();
                    PolygonIndexCount = br.ReadUInt16();
                    FirstVertexIndex = br.ReadUInt16();
                    LastVertexIndex = br.ReadUInt16();

                    UnknownFlags = br.ReadByte();
                    MaterialIndex = br.ReadByte();
                }
            }
        }

        /// <summary>
        /// Gets the serialized size of the instance.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 24;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteShortBox(BoundingBox);

                    bw.Write(FirstPolygonIndex);
                    bw.Write(PolygonIndexCount);

                    bw.Write(FirstVertexIndex);
                    bw.Write(LastVertexIndex);

                    bw.Write(UnknownFlags);
                    bw.Write(MaterialIndex);
                }

                return ms.ToArray();
            }
        }
    }
}
