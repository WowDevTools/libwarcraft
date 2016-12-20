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

namespace Warcraft.MDX.Geometry
{
	public class MDXViewHeader
	{
		public uint VertexIndexCount;
		public uint VertexIndicesOffset;

		public uint TriangleVertexCount;
		public uint TriangleVertexIndicesOffset;

		public uint VertexPropertyCount;
		public uint VertexPropertiesOffset;

		public uint SubmeshCount;
		public uint SubmeshesOffset;

		public uint TextureCount;
		public uint TexturesOffset;

		public uint TransitionDistance;

		public MDXViewHeader(byte[] data)
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

					this.SubmeshCount = br.ReadUInt32();
					this.SubmeshesOffset = br.ReadUInt32();

					this.TextureCount = br.ReadUInt32();
					this.TexturesOffset = br.ReadUInt32();

					this.TransitionDistance = br.ReadUInt32();
				}
			}
		}
	}
}

