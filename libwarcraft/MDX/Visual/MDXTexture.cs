//
//  Texture.cs
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
using System.Linq;
using Warcraft.Core.Extensions;

namespace Warcraft.MDX.Visual
{
	public class MDXTexture
	{
		public EMDXTextureType TextureType;
		public EMDXTextureFlags Flags;

		public string Filename;

		public MDXTexture(BinaryReader br)
		{
			this.TextureType = (EMDXTextureType)br.ReadUInt32();
			this.Flags = (EMDXTextureFlags)br.ReadUInt32();

			// This points off to a null-terminated string, so we'll pop off the null byte when deserializing it
			this.Filename = new string(br.ReadMDXArray<char>().ToArray()).TrimEnd('\0');
		}
	}
}

