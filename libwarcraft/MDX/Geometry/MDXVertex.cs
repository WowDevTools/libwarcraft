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
using System;
using Warcraft.Core;
using System.Collections.Generic;
using System.IO;

namespace Warcraft.MDX.Geometry
{
	public class MDXVertex
	{
		public Vector3f Position;
		public List<byte> BoneWeights;
		public List<byte> BoneIndices;
		public Vector3f Normal;
		public Vector2f UVCoordinates_Channel1;
		public Vector2f UVCoordinates_Channel2;

		public MDXVertex(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Position = br.ReadVector3f();
					this.BoneWeights = new List<byte>(br.ReadBytes(4));
					this.BoneIndices = new List<byte>(br.ReadBytes(4));
					this.Normal = br.ReadVector3f();
					this.UVCoordinates_Channel1 = br.ReadVector2f();
					this.UVCoordinates_Channel2 = br.ReadVector2f();
				}
			}
		}
	}
}

