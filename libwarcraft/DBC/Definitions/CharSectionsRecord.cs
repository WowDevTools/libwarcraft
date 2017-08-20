//
//  CharSectionsRecord.cs
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
using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Extensions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class CharSectionsRecord : DBCRecord
	{
		public const DatabaseName Database = DatabaseName.CharSections;

		public ForeignKey<uint> Race;
		public bool IsFemale;
		public CharSectionType BaseSection;
		public List<StringReference> SectionTextures;
		public CharSectionFlag Flags;
		public uint Type;
		public uint Variation;

		/*
			What follows are forwards into the SectionTextures list for ease of use.
			Which ones to use depend on the value of BaseSection.
		*/

		public StringReference SkinTexture => this.SectionTextures[0];
		public StringReference ExtraSkinTexture => this.SectionTextures[1];

		public StringReference FaceLowerTexture => this.SectionTextures[0];
		public StringReference FaceUpperTexture => this.SectionTextures[1];

		public StringReference FacialLowerTexture => this.SectionTextures[0];
		public StringReference FacialUpperTexture => this.SectionTextures[1];

		public StringReference HairTexture => this.SectionTextures[0];
		public StringReference ScalpLowerTexture => this.SectionTextures[1];
		public StringReference ScalpUpperTexture => this.SectionTextures[2];

		public StringReference PelvisTexture => this.SectionTextures[0];
		public StringReference TorsoTexture => this.SectionTextures[1];

		/// <summary>
		/// Loads and parses the provided data.
		/// </summary>
		/// <param name="data">ExtendedData.</param>
		public override void PostLoad(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					DeserializeSelf(br);
				}
			}
		}

		/// <summary>
		/// Deserializes the data of the object using the provided <see cref="BinaryReader"/>.
		/// </summary>
		/// <param name="reader"></param>
		public override void DeserializeSelf(BinaryReader reader)
		{
			base.DeserializeSelf(reader);

			this.Race = new ForeignKey<uint>(DatabaseName.ChrRaces, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.IsFemale = (reader.ReadUInt32() > 0);
			this.BaseSection = (CharSectionType)reader.ReadUInt32();
			this.SectionTextures = new List<StringReference>
			{
				reader.ReadStringReference(),
				reader.ReadStringReference(),
				reader.ReadStringReference()
			};

			this.Flags = (CharSectionFlag)reader.ReadUInt32();
			this.Type = reader.ReadUInt32();
			this.Variation = reader.ReadUInt32();

			this.HasLoadedRecordData = true;
		}

		public override IEnumerable<StringReference> GetStringReferences()
		{
			return this.SectionTextures;
		}

		public override int FieldCount => 10;

		public override int RecordSize => sizeof(uint) * this.FieldCount;
	}

	public enum CharSectionType
	{
		BaseSkin,
		Face,
		FacialHair,
		Hair,
		Underwear,
		BaseSkinHiDef,
		FaceHiDef,
		FacialHairHiDef,
		HairHiDef,
		UnderwearHiDef,
		Unknown1,
		DemonHunterTattoo
	}

	[Flags]
	public enum CharSectionFlag
	{
		CharacterCreate = 0x1,
		BarberShop      = 0x2,
		DeathKnight     = 0x4,
		NPC             = 0x8,
		Unknown         = 0x10
	}
}