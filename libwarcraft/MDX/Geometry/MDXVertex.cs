//
//  MDXVertex.cs
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
using System.Numerics;
using Warcraft.Core.Extensions;

namespace Warcraft.MDX.Geometry
{
    /// <summary>
    /// A vertex in an <see cref="MDX"/> model.
    /// </summary>
    public class MDXVertex
    {
        /// <summary>
        /// Gets or sets the position of the vertex in model space.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the weights of any affecting bones onto this vertex. Up to four bones may affect the vertex.
        /// </summary>
        public List<byte> BoneWeights { get; set; }

        /// <summary>
        /// Gets or sets the indexes of up to four bones which affect this vertex. A bone may be listed more than once, but will
        /// only affect the vertex once.
        /// </summary>
        public List<byte> BoneIndices { get; set; }

        /// <summary>
        /// Gets or sets the normal vector of this vertex.
        /// </summary>
        public Vector3 Normal { get; set; }

        /// <summary>
        /// Gets or sets uV texture coordinates for this vertex. There are two UV channels for each vertex, of which this is the
        /// first.
        /// </summary>
        public Vector2 UV1 { get; set; }

        /// <summary>
        /// Gets or sets uV texture coordinates for this vertex. There are two UV channels for each vertex, of which this is the
        /// second.
        /// </summary>
        public Vector2 UV2 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXVertex"/> class.
        /// </summary>
        /// <param name="data">The binary data in which the vertex is stored.</param>
        public MDXVertex(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    Position = br.ReadVector3();
                    BoneWeights = new List<byte>(br.ReadBytes(4));
                    BoneIndices = new List<byte>(br.ReadBytes(4));
                    Normal = br.ReadVector3();
                    UV1 = br.ReadVector2();
                    UV2 = br.ReadVector2();
                }
            }
        }

        /// <summary>
        /// Gets the absolute byte size of a serialized object.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 48;
        }
    }
}
