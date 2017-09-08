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
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	[DatabaseRecord(DatabaseName.CharSections)]
	public class CharSectionsRecord : DBCRecord
	{
		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.ChrRaces, nameof(ID))]
		public ForeignKey<uint> Race { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public bool IsFemale { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public CharSectionType BaseSection { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint Type { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint Variation { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public StringReference SectionTexture0 { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public StringReference SectionTexture1 { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public StringReference SectionTexture2 { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public CharSectionFlag Flags { get; set; }

		/*
			What follows are forwards into the SectionTextures list for ease of use.
			Which ones to use depend on the value of BaseSection.
		*/

		public StringReference SkinTexture => this.SectionTexture0;
		public StringReference ExtraSkinTexture => this.SectionTexture1;

		public StringReference FaceLowerTexture => this.SectionTexture0;
		public StringReference FaceUpperTexture => this.SectionTexture1;

		public StringReference FacialLowerTexture => this.SectionTexture0;
		public StringReference FacialUpperTexture => this.SectionTexture1;

		public StringReference HairTexture => this.SectionTexture0;
		public StringReference ScalpLowerTexture => this.SectionTexture1;
		public StringReference ScalpUpperTexture => this.SectionTexture2;

		public StringReference PelvisTexture => this.SectionTexture0;
		public StringReference TorsoTexture => this.SectionTexture1;

		public override IEnumerable<StringReference> GetStringReferences()
		{
			return new List<StringReference>
			{
				this.SectionTexture0,
				this.SectionTexture1,
				this.SectionTexture2
			};
		}
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