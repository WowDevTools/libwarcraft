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
using System;
using System.IO;

namespace Warcraft.MDX.Visual
{
	public class MDXTexture
	{
		public ETextureType TextureType;
		public ETextureFlags Flags;
		public uint FilenameLength;
		public uint FilenameOffset;

		public MDXTexture(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.TextureType = (ETextureType)br.ReadUInt32();
					this.Flags = (ETextureFlags)br.ReadUInt32();
					this.FilenameLength = br.ReadUInt32();
					this.FilenameOffset = br.ReadUInt32();
				}
			}
		}
	}

	public enum ETextureType : uint
	{
		Regular = 0,
		CharacterSkin = 1,
		EquipmentSkin = 2,
		WeaponBladeReflection = 3,
		WeaponHandle = 4,
		[Obsolete]
		Environment = 5,
		Hair = 6,
		[Obsolete]
		FacialHair = 7,
		SkinExtra = 8,
		UIModelSkin = 9,
		[Obsolete]
		TaurenMane = 10,
		MonsterSkin1 = 11,
		MonsterSkin2 = 12,
		MonsterSkin3 = 13,
		ItemIcon = 14,
		GuildBackgroundColor = 15,
		GuildEmblemColor = 16,
		GuildBorderColor = 17,
		GuildEmblem = 18
	}

	public enum ETextureFlags : uint
	{
		TextureWrapX = 1,
		TextureWrapY = 2
	}
}

