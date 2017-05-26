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
using Warcraft.Core;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class SoundEntriesRecord : DBCRecord
	{
		public const string RecordName = "SoundEntries";

		public SoundType SoundType;
		public StringReference Name;
		public List<StringReference> SoundFiles = new List<StringReference>(10); // Up to 10 variations
		public List<uint> PlayFrequencies = new List<uint>(10);
		public StringReference DirectoryBase;
		public float Volume;
		public float Pitch;
		public float PitchVariation;
		public uint Priority;
		public uint Channel;
		public uint Flags;
		public float MinDistance;
		public float MaxDistance;
		public float DistanceCutOff;
		public uint EAXDefinition;
		
		public override void PostLoad(byte[] data)
		{
			throw new System.NotImplementedException();
		}

		public override int GetFieldCount()
		{
			throw new System.NotImplementedException();
		}

		public override int GetRecordSize()
		{
			throw new System.NotImplementedException();
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