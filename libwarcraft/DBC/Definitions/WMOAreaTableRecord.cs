//
//  WMOAreaTableRecord.cs
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
using Warcraft.Core.Extensions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class WMOAreaTableRecord : DBCRecord
	{
		public const DatabaseName Database = DatabaseName.WMOAreaTable;

		public uint WMOID;
		public uint NameSetID;
		public int WMOGroupID;

		public ForeignKey<uint> SoundProviderPref;
		public ForeignKey<uint> SoundProviderPrefUnderwater;
		public ForeignKey<uint> AmbienceID;
		public ForeignKey<uint> ZoneMusic;
		public ForeignKey<uint> IntroSound;
		public uint Flags;
		public ForeignKey<uint> AreaTableID;
		private LocalizedStringReference AreaName;

		/*
			Cataclysm and up
		*/

		public ForeignKey<uint> UnderwaterIntroSound;
		public ForeignKey<uint> UnderwaterZoneMusic;
		public ForeignKey<uint> UnderwaterAmbience;

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

			this.WMOID = reader.ReadUInt32();
			this.NameSetID = reader.ReadUInt32();
			this.WMOGroupID = reader.ReadInt32();

			this.SoundProviderPref = new ForeignKey<uint>(DatabaseName.SoundProviderPreferences, nameof(SoundProviderPreferencesRecord.ID), reader.ReadUInt32());
			this.SoundProviderPrefUnderwater = new ForeignKey<uint>(DatabaseName.SoundProviderPreferences, nameof(SoundProviderPreferencesRecord.ID), reader.ReadUInt32());
			this.AmbienceID = new ForeignKey<uint>(DatabaseName.SoundAmbience, nameof(DBCRecord.ID), reader.ReadUInt32()); // TODO: Implement SoundAmbience
			this.ZoneMusic = new ForeignKey<uint>(DatabaseName.ZoneMusic, nameof(ZoneMusicRecord.ID), reader.ReadUInt32());
			this.IntroSound = new ForeignKey<uint>(DatabaseName.ZoneIntroMusicTable, nameof(ZoneIntroMusicTableRecord.ID), reader.ReadUInt32());
			this.Flags = reader.ReadUInt32();
			this.AreaTableID = new ForeignKey<uint>(DatabaseName.AreaTable, nameof(DBCRecord.ID), reader.ReadUInt32()); // TODO: Implement AreaTable
			this.AreaName = reader.ReadLocalizedStringReference(this.Version);

			this.HasLoadedRecordData = true;
		}

		public override IEnumerable<StringReference> GetStringReferences()
		{
			return this.AreaName.GetReferences();
		}

		public override int FieldCount => 11 + LocalizedStringReference.GetFieldCount(this.Version);

		public override int RecordSize => sizeof(uint) * this.FieldCount;
	}
}