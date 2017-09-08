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
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Structures;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	[DatabaseRecord(DatabaseName.ZoneMusic)]
	public class ZoneMusicRecord : DBCRecord
	{
		[RecordField(WarcraftVersion.Classic)]
		public StringReference SetName { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint SilenceTimeDayMin { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint SilenceTimeNightMin { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint SilenceTimeDayMax { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint SilenceTimeNightMax { get; set; }

		public Range SilenceIntervalDay => new Range(this.SilenceTimeDayMin, this.SilenceTimeDayMax, rigorous:false);
		public Range SilenceIntervalNight => new Range(this.SilenceTimeNightMin, this.SilenceTimeNightMax, rigorous:false);

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
		public ForeignKey<uint> DayMusic { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
		public ForeignKey<uint> NightMusic { get; set; }

		/// <inheritdoc />
		public override IEnumerable<StringReference> GetStringReferences()
		{
			yield return this.SetName;
		}
	}
}