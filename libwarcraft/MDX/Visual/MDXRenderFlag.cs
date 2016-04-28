//
//  MDXRenderFlag.cs
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
	public class MDXRenderFlagPair
	{
		public MDXRenderFlag Flags;
		public MDXBlendMode BlendingMode;

		public MDXRenderFlagPair(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Flags = (MDXRenderFlag)br.ReadUInt16();
					this.BlendingMode = (MDXBlendMode)br.ReadUInt16();
				}
			}
		}
	}

	[Flags]
	public enum MDXRenderFlag : ushort
	{
		Unlit = 0x01,
		NoFog = 0x02,
		TwoSided = 0x04,
		Unknown = 0x08,
		DisableZBuffering = 0x10
	}

	[Flags]
	public enum MDXBlendMode : ushort
	{
		Opaque = 0,
		AlphaTestOnly = 1,
		AlphaBlending = 2,
		Additive = 3,
		AdditiveAlpha = 4,
		Modulate = 5,
		DeeprunTramMagic = 6
	}
}

