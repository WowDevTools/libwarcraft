//
//  ZoneIntroMusicTableRecord.cs
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

using System.Collections.Generic;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// Defines the intro music of a zone.
    /// </summary>
    [DatabaseRecord(DatabaseName.ZoneIntroMusicTable)]
    public class ZoneIntroMusicTableRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the name of the intro.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference Name { get; set; }

        /// <summary>
        /// Gets or sets the sound track to use.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
        public ForeignKey<uint> Sound { get; set; }

        /// <summary>
        /// Gets or sets the priority of the sound.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Priority { get; set; }

        /// <summary>
        /// Gets or sets the minimum delay in minutes before the sound plays.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint MinDelayMinutes { get; set; }

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            yield return Name;
        }
    }
}
