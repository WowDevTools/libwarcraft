//
//  MDXSkinSection.cs
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

using Warcraft.Core;
using System.IO;

namespace Warcraft.MDX.Geometry
{
	public class MDXSkinSection
	{
		public uint PartID;
		public ushort StartVertexIndex;
		public ushort VertexCount;
		public ushort StartTriangleIndex;
		public ushort TriangleCount;
		public ushort BoneCount;
		public ushort StartBoneIndex;
		public ushort InfluencingBonesIndex;
		public ushort RootBoneIndex;
		public Vector3f SubmeshMedianPoint;
		public Vector3f BoundingShellMedianPoint;
		public float BoundingSphereRadius;

		public MDXSkinSection(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.PartID = br.ReadUInt32();
					this.StartVertexIndex = br.ReadUInt16();
					this.VertexCount = br.ReadUInt16();
					this.StartTriangleIndex = br.ReadUInt16();
					this.TriangleCount = br.ReadUInt16();
					this.BoneCount = br.ReadUInt16();
					this.StartBoneIndex = br.ReadUInt16();
					this.InfluencingBonesIndex = br.ReadUInt16();
					this.RootBoneIndex = br.ReadUInt16();
					this.SubmeshMedianPoint = br.ReadVector3f();

					if (br.BaseStream.Length > 32)
					{
						this.BoundingShellMedianPoint = br.ReadVector3f();
						this.BoundingSphereRadius = br.ReadSingle();
					}
				}
			}
		}
	}
}

