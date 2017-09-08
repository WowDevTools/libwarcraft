//
//  SoundEntriesRecord.cs
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

using System.Collections.Generic;
using System.IO;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	[DatabaseRecord(DatabaseName.SoundEntries)]
	public class SoundEntriesRecord : DBCRecord
	{
		[RecordField(WarcraftVersion.Classic)]
		public SoundType Type { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public StringReference Name { get; set; }

		[RecordFieldArray(WarcraftVersion.Classic, Count = 10)]
		public List<StringReference> SoundFiles { get; set; }

		[RecordFieldArray(WarcraftVersion.Classic, Count = 10)]
		public List<uint> PlayFrequencies { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public StringReference DirectoryBase { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float Volume { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint Flags { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float MinDistance { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float DistanceCutoff { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.SoundProviderPreferences, nameof(ID))]
		public ForeignKey<uint> EAXDefinition { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public uint SoundEntriesAdvancedID { get; set; }

		/// <inheritdoc />
		public override IEnumerable<StringReference> GetStringReferences()
		{
			yield return this.Name;
			yield return this.DirectoryBase;
			foreach (var soundFile in this.SoundFiles)
			{
				yield return soundFile;
			}
		}
	}

	public enum SoundType : uint
	{
		Unused			= 0,
		Spell			= 1,
		UI				= 2,
		Footstep1		= 3,
		WeaponImpact	= 4,
		Unknown1		= 5,
		WeaponMiss		= 6,
		Greeting		= 7,
		Cast			= 8,
		PickUpDown		= 9,
		NPCCombat		= 10,
		Unknown2		= 11,
		Error			= 12,
		Bird			= 13,
		Object			= 14,
		Unknown3		= 15,
		Death			= 16,
		NPCGreeting		= 17,
		Test			= 18,
		Armor			= 19,
		Footstep2		= 20,
		CharacterWater	= 21,
		LiquidWater		= 22,
		Tradeskill		= 23,
		Unknown4		= 24,
		Doodad			= 25,
		SpellFizzle		= 26,
		NPCLoop			= 27,
		ZoneMusic		= 28,
		Emote			= 29,
		NarrationMusic	= 30,
		Narration		= 31,

		// .. //

		ZoneAmbience	= 50

	}
}