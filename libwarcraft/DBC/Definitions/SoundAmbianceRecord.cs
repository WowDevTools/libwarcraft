//
//  SoundAmbianceRecord.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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

using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// Defines ambient sounds for a zone.
    /// </summary>
    [DatabaseRecord(DatabaseName.ZoneAmbience)]
    public class SoundAmbianceRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the ambiance sound to play during the day.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
        public ForeignKey<uint> AmbianceDay { get; set; }

        /// <summary>
        /// Gets or sets the ambiance sound to play during the night.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
        public ForeignKey<uint> AmbianceNight { get; set; }
    }
}
