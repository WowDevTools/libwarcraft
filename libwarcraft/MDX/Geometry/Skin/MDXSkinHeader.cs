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
	public class MDXSkinHeader
	{
		public uint VertexIndexCount;
		public uint VertexIndicesOffset;

		public uint TriangleVertexCount;
		public uint TriangleVertexIndicesOffset;

		public uint VertexPropertyCount;
		public uint VertexPropertiesOffset;

		public uint SkinSectionCount;
		public uint SkinSectionOffset;

		public uint RenderBatchCount;
		public uint RenderBatchOffset;

		public uint TransitionDistance;

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

					this.TransitionDistance = br.ReadUInt32();
				}
			}
		}
	}
}

