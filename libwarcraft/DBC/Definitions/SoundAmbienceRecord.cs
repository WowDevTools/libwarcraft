//
//  SoundAmbienceRecord.cs
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
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	[DatabaseRecord(DatabaseName.ZoneAmbience)]
	public class SoundAmbienceRecord : DBCRecord
	{
		/// <summary>
		/// The ambience sound to play during the day.
		/// </summary>
		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
		public ForeignKey<uint> AmbienceDay { get; set; }

		/// <summary>
		/// The ambience sound to play during the night.
		/// </summary>
		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
		public ForeignKey<uint> AmbienceNight { get; set; }
	}
}