//
//  MDXRenderBatch.cs
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
using System.IO;

namespace Warcraft.MDX.Visual
{
	public class MDXRenderBatch
	{
		public ERenderBatchFlags Flags;
		public sbyte RenderOrder;

		public ushort SubmeshIndex;
		public ushort GeosetIndex;
		public short ColorIndex;
		public ushort RenderFlagsIndex;
		public ushort Layer; // Also known as MaterialIndex
		public ushort OPCount; // Also known as MaterialLayerCount

		public ushort TextureLookupTableIndex;
		public ushort TextureUnitLookupTableIndex;
		public ushort TransparencyLookupTableIndex;
		public ushort UVAnimationLookupTableIndex;

		public MDXRenderBatch(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Flags = (ERenderBatchFlags)br.ReadByte();
					this.RenderOrder = br.ReadSByte();

					this.SubmeshIndex = br.ReadUInt16();
					this.GeosetIndex = br.ReadUInt16();
					this.ColorIndex = br.ReadInt16();
					this.RenderFlagsIndex = br.ReadUInt16();
					this.Layer = br.ReadUInt16();
					this.OPCount = br.ReadUInt16();

					this.TextureLookupTableIndex = br.ReadUInt16();
					this.TextureUnitLookupTableIndex = br.ReadUInt16();
					this.TransparencyLookupTableIndex = br.ReadUInt16();
					this.UVAnimationLookupTableIndex = br.ReadUInt16();
				}
			}
		}
	}

	// TODO: This is incorrect
	[Flags]
	public enum ERenderBatchFlags : byte
	{
		Static = 0x10,
		Animated = 0x00
	}
}

