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

using System;
using System.IO;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Structures;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class ZoneMusicRecord : DBCRecord
	{
		public const DatabaseName Database = DatabaseName.ZoneMusic;

		public StringReference MusicFileDay;
		public StringReference MusicFileNight;
		public Range SilenceIntervalDay; // These ranges are stored as daymin/nightmin/daymax/nightmax)
		public Range SilenceIntervalNight;
		public uint SegmentLengthDay;
		public uint SegmentLengthNight;
		public Range SegmentPlayCountDay;
		public Range SegmentPlayCountNight;
		public ForeignKey<uint> SoundsDay;
		public ForeignKey<uint> SoundsNight;

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

			this.MusicFileDay = reader.ReadStringReference();
			this.MusicFileNight = reader.ReadStringReference();

			uint interDayMin = reader.ReadUInt32();
			uint interNightMin = reader.ReadUInt32();
			uint interDayMax = reader.ReadUInt32();
			uint interNightMax = reader.ReadUInt32();

			this.SilenceIntervalDay = new Range(interDayMin, interDayMax);
			this.SilenceIntervalNight = new Range(interNightMin, interNightMax);

			this.SegmentLengthDay = reader.ReadUInt32();
			this.SegmentLengthNight = reader.ReadUInt32();

			uint playDayMin = reader.ReadUInt32();
			uint playNightMin = reader.ReadUInt32();
			uint playDayMax = reader.ReadUInt32();
			uint playNightMax = reader.ReadUInt32();

			this.SegmentPlayCountDay = new Range(playDayMin, playDayMax);
			this.SegmentPlayCountNight = new Range(playNightMin, playNightMax);

			this.SoundsDay = new ForeignKey<uint>(DatabaseName.SoundEntries, "ID", reader.ReadUInt32());
			this.SoundsNight = new ForeignKey<uint>(DatabaseName.SoundEntries, "ID", reader.ReadUInt32());
		}

		public override int FieldCount => 15;

		public override int RecordSize => sizeof(uint) * this.FieldCount;
	}
}