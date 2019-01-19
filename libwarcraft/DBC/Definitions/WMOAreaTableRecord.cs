//
//  WMOAreaTableRecord.cs
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
    /// Defines area information for a world model object.
    /// </summary>
    [DatabaseRecord(DatabaseName.WMOAreaTable)]
    public class WMOAreaTableRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the ID of the WMO that the area information is relevant for.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint WMOID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the name set.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint NameSetID { get; set; }

        /// <summary>
        /// Gets or sets the group ID of the WMO that the area information is relevant for.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public int WMOGroupID { get; set; }

        /// <summary>
        /// Gets or sets the sound settings of the area.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundProviderPreferences, nameof(ID))]
        public ForeignKey<uint> SoundProviderPref { get; set; }

        /// <summary>
        /// Gets or sets the underwater sound settings of the area.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundProviderPreferences, nameof(ID))]
        public ForeignKey<uint> SoundProviderPrefUnderwater { get; set; }

        /// <summary>
        /// Gets or sets the ambiance in the area.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundAmbiance, nameof(ID))]
        public ForeignKey<uint> AmbianceID { get; set; }

        /// <summary>
        /// Gets or sets the music in the area.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ZoneMusic, nameof(ID))]
        public ForeignKey<uint> ZoneMusic { get; set; }

        /// <summary>
        /// Gets or sets the intro sound of the area.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ZoneIntroMusicTable, nameof(ID))]
        public ForeignKey<uint> IntroSound { get; set; }

        /// <summary>
        /// Gets or sets the flags of the area.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Flags { get; set; }

        /// <summary>
        /// Gets or sets the area table entry that contains more relevant information.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.AreaTable, nameof(ID))]
        public ForeignKey<uint> AreaTableID { get; set; }

        /// <summary>
        /// Gets or sets the name of the area.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference AreaName { get; set; }

        /*
            Cataclysm and up
        */

        /// <summary>
        /// Gets or sets the underwater intro sound.
        /// </summary>
        [RecordField(WarcraftVersion.Cataclysm)]
        [ForeignKeyInfo(DatabaseName.ZoneIntroMusicTable, nameof(ID))]
        public ForeignKey<uint> UnderwaterIntroSound { get; set; }

        /// <summary>
        /// Gets or sets the underwater zone music.
        /// </summary>
        [RecordField(WarcraftVersion.Cataclysm)]
        [ForeignKeyInfo(DatabaseName.ZoneMusic, nameof(ID))]
        public ForeignKey<uint> UnderwaterZoneMusic { get; set; }

        /// <summary>
        /// Gets or sets the ambiance underwater.
        /// </summary>
        [RecordField(WarcraftVersion.Cataclysm)]
        [ForeignKeyInfo(DatabaseName.SoundAmbiance, nameof(ID))]
        public ForeignKey<uint> UnderwaterAmbiance { get; set; }

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            return AreaName.GetReferences();
        }
    }
}
