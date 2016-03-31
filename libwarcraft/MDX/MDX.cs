//
//  MDX.cs
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

namespace Warcraft.MDX
{
	public class MDX
	{
		public MDXHeader Header;

		public MDX(Stream MDXStream)
		{
			using (BinaryReader br = new BinaryReader(MDXStream))
			{
				EMDXFlags Flags = PeekFlags(br);
				if (Flags.HasFlag(EMDXFlags.HasBlendModeOverrides))
				{
					this.Header = new MDXHeader(br.ReadBytes(308));
				}
				else
				{
					this.Header = new MDXHeader(br.ReadBytes(312));
				}
			}
		}

		private EMDXFlags PeekFlags(BinaryReader br)
		{
			long initialPosition = br.BaseStream.Position;

			// Skip ahead to the flag block
			br.BaseStream.Position += 16;

			EMDXFlags flags = (EMDXFlags)br.ReadUInt32();

			// Seek back to the initial position
			br.BaseStream.Position = initialPosition;

			return flags;
		}
	}
}

