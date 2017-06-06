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

using System;
using System.Collections.Generic;
using System.IO;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class SoundEntriesRecord : DBCRecord
	{
		public const DatabaseName Database = DatabaseName.SoundEntries;

		public SoundType Type;
		public StringReference Name;
		public List<StringReference> SoundFiles = new List<StringReference>(10); // Up to 10 variations
		public List<uint> PlayFrequencies = new List<uint>(10);
		public StringReference DirectoryBase;
		public float Volume;
		public uint Flags;
		public float MinDistance;
		public float DistanceCutoff;
		public ForeignKey<uint> EAXDefinition;

		/*
			Wrath +
		*/
		public uint SoundEntriesAdvancedID;

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

			this.Type = (SoundType) reader.ReadUInt32();
			this.Name = reader.ReadStringReference();

			for (int i = 0; i < 10; i++)
			{
				this.SoundFiles.Add(reader.ReadStringReference());
			}

			for (int i = 0; i < 10; i++)
			{
				this.PlayFrequencies.Add(reader.ReadUInt32());
			}

			this.DirectoryBase = reader.ReadStringReference();
			this.Volume = reader.ReadSingle();
			this.Flags = reader.ReadUInt32();
			this.MinDistance = reader.ReadSingle();
			this.DistanceCutoff = reader.ReadSingle();
			this.EAXDefinition = new ForeignKey<uint>(DatabaseName.SoundProviderPreferences, "ID", reader.ReadUInt32());

			this.HasLoadedRecordData = true;
		}

		public override List<StringReference> GetStringReferences()
		{
			List<StringReference> referenceList = new List<StringReference> {this.Name, this.DirectoryBase};
			referenceList.AddRange(this.SoundFiles);

			return referenceList;
		}

		public override int FieldCount
		{
			get
			{
				if (this.Version > WarcraftVersion.Wrath)
				{
					return 30;
				}

				return 29;
			}
		}

		// No distinction is made between uints and singles here since they're the same size
		public override int RecordSize => sizeof(uint) * this.FieldCount;
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