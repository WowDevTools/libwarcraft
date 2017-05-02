//
//  MDXViewHeader.cs
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

using System.IO;

namespace Warcraft.MDX.Geometry.Skin
{
	/// <summary>
	/// The header of an <see cref="MDXSkin"/> object.
	/// </summary>
	public class MDXSkinHeader
	{
		/// <summary>
		/// The number of vertex indices used in this skin.
		/// </summary>
		public uint VertexIndexCount;

		/// <summary>
		/// The byte offset where the indices are found.
		/// </summary>
		public uint VertexIndicesOffset;


		/// <summary>
		/// The number of triangles that compose this skin.
		/// </summary>
		public uint TriangleVertexCount;

		/// <summary>
		/// The byte offset where the triangles are found.
		/// </summary>
		public uint TriangleVertexIndicesOffset;


		/// <summary>
		/// The number of vertex properties used in this skin. Commonly the same as the number of vertices.
		/// </summary>
		public uint VertexPropertyCount;

		/// <summary>
		/// The byte offset where the vertex properties are found.
		/// </summary>
		public uint VertexPropertiesOffset;


		/// <summary>
		/// The number of sections in this skin.
		/// </summary>
		public uint SkinSectionCount;

		/// <summary>
		/// The byte offset where the skin sections are found.
		/// </summary>
		public uint SkinSectionOffset;


		/// <summary>
		/// The number of rendering batches (that is, shading information) in this skin.
		/// </summary>
		public uint RenderBatchCount;

		/// <summary>
		/// The byte offset where the rendering batches are found.
		/// </summary>
		public uint RenderBatchOffset;


		/// <summary>
		/// The maximum number of bones in each draw call.
		/// </summary>
		public uint BoneCountMax;

		/// <summary>
		/// Deserializes a new <see cref="MDXSkinHeader"/> object from binary data.
		/// </summary>
		/// <param name="data">The data which contains the header.</param>
		public MDXSkinHeader(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.VertexIndexCount = br.ReadUInt32();
					this.VertexIndicesOffset = br.ReadUInt32();

					this.TriangleVertexCount = br.ReadUInt32();
					this.TriangleVertexIndicesOffset = br.ReadUInt32();

					this.VertexPropertyCount = br.ReadUInt32();
					this.VertexPropertiesOffset = br.ReadUInt32();

					this.SkinSectionCount = br.ReadUInt32();
					this.SkinSectionOffset = br.ReadUInt32();

					this.RenderBatchCount = br.ReadUInt32();
					this.RenderBatchOffset = br.ReadUInt32();

					this.BoneCountMax = br.ReadUInt32();
				}
			}
		}
	}
}

