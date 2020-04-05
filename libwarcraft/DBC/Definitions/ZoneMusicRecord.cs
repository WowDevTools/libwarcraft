//
//  ZoneMusicRecord.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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
using Warcraft.Core.Reflection.DBC;
using Warcraft.Core.Structures;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// Defines music in a zone.
    /// </summary>
    [DatabaseRecord(DatabaseName.ZoneMusic)]
    public class ZoneMusicRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the name of the music set.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference SetName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the minimum silence between tracks during the day.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint SilenceTimeDayMin { get; set; }

        /// <summary>
        /// Gets or sets the minimum silence between tracks during the night.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint SilenceTimeNightMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum silence between tracks during the day.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint SilenceTimeDayMax { get; set; }

        /// <summary>
        /// Gets or sets the maximum silence between tracks during the night.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint SilenceTimeNightMax { get; set; }

        /// <summary>
        /// Gets the silence interval during the day.
        /// </summary>
        public Range SilenceIntervalDay => new Range(SilenceTimeDayMin, SilenceTimeDayMax, rigorous: false);

        /// <summary>
        /// Gets the silence interval during the night.
        /// </summary>
        public Range SilenceIntervalNight => new Range(SilenceTimeNightMin, SilenceTimeNightMax, rigorous: false);

        /// <summary>
        /// Gets or sets the music to use during the day.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
        public ForeignKey<uint> DayMusic { get; set; } = null!;

        /// <summary>
        /// Gets or sets the music to use during the night.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
        public ForeignKey<uint> NightMusic { get; set; } = null!;

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            yield return SetName;
        }
    }
}
