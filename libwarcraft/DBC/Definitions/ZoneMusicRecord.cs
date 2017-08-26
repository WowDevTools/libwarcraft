//
//  ZoneMusicRecord.cs
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
using Warcraft.Core.Structures;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class ZoneMusicRecord : DBCRecord
	{
		public const DatabaseName Database = DatabaseName.ZoneMusic;

		public StringReference SetName;
		public Range SilenceIntervalDay; // These ranges are stored as daymin/nightmin/daymax/nightmax)
		public Range SilenceIntervalNight;
		public ForeignKey<uint> DayMusic;
		public ForeignKey<uint> NightMusic;

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

			this.SetName = reader.ReadStringReference();

			uint interDayMin = reader.ReadUInt32();
			uint interNightMin = reader.ReadUInt32();
			uint interDayMax = reader.ReadUInt32();
			uint interNightMax = reader.ReadUInt32();

			// HACK: Due to some malformed data on Blizzard's part, the range error checking must be disabled

			this.SilenceIntervalDay = new Range(interDayMin, interDayMax, rigorous:false);
			this.SilenceIntervalNight = new Range(interNightMin, interNightMax, rigorous:false);

			this.DayMusic = new ForeignKey<uint>(DatabaseName.SoundEntries, nameof(SoundEntriesRecord.ID), reader.ReadUInt32());
			this.NightMusic = new ForeignKey<uint>(DatabaseName.SoundEntries, nameof(SoundEntriesRecord.ID), reader.ReadUInt32());

			this.HasLoadedRecordData = true;
		}

		public override IEnumerable<StringReference> GetStringReferences()
		{
			yield return this.SetName;
		}

		public override int FieldCount => 8;

		public override int RecordSize => sizeof(uint) * this.FieldCount;
	}
}