//
//  MDXVertex.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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
		/// The position of the vertex in model space.
		/// </summary>
		public Vector3 Position;

		/// <summary>
		/// The weights of any affecting bones onto this vertex. Up to four bones may affect the vertex.
		/// </summary>
		public List<byte> BoneWeights;

		/// <summary>
		/// The indexes of up to four bones which affect this vertex. A bone may be listed more than once, but will
		/// only affect the vertex once.
		/// </summary>
		public List<byte> BoneIndices;

		/// <summary>
		/// The normal vector of this vertex.
		/// </summary>
		public Vector3 Normal;

		/// <summary>
		/// UV texture coordinates for this vertex. There are two UV channels for each vertex, of which this is the
		/// first.
		/// </summary>
		public Vector2 UVCoordinatesChannel1;

		/// <summary>
		/// UV texture coordinates for this vertex. There are two UV channels for each vertex, of which this is the
		/// second.
		/// </summary>
		public Vector2 UVCoordinatesChannel2;

		/// <summary>
		/// Deserializes an <see cref="MDXVertex"/> from binary data.
		/// </summary>
		/// <param name="data">The binary data in which the vertex is stored.</param>
		public MDXVertex(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Position = br.ReadVector3();
					this.BoneWeights = new List<byte>(br.ReadBytes(4));
					this.BoneIndices = new List<byte>(br.ReadBytes(4));
					this.Normal = br.ReadVector3();
					this.UVCoordinatesChannel1 = br.ReadVector2();
					this.UVCoordinatesChannel2 = br.ReadVector2();
				}
			}
		}

		public static int GetSize()
		{
			return 48;
		}
	}
}

