//
//  Portal.cs
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

using System.IO;
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Represents a culling portal.
    /// </summary>
    public class Portal : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the base vertex index of the portal.
        /// </summary>
        public ushort BaseVertexIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of vertexes in the portal.
        /// </summary>
        public ushort VertexCount { get; set; }

        /// <summary>
        /// Gets or sets the portal's plane.
        /// </summary>
        public Plane PortalPlane { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Portal"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public Portal(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    BaseVertexIndex = br.ReadUInt16();
                    VertexCount = br.ReadUInt16();
                    PortalPlane = br.ReadPlane();
                }
            }
        }

        /// <summary>
        /// Gets the serialized size of the instance.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 20;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(BaseVertexIndex);
                    bw.Write(VertexCount);
                    bw.WritePlane(PortalPlane);
                }

                return ms.ToArray();
            }
        }
    }
}
