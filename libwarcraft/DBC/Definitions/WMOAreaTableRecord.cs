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
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
    [DatabaseRecord(DatabaseName.WMOAreaTable)]
    public class WMOAreaTableRecord : DBCRecord
    {
        [RecordField(WarcraftVersion.Classic)]
        public uint WMOID { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint NameSetID { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public int WMOGroupID { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundProviderPreferences, nameof(ID))]
        public ForeignKey<uint> SoundProviderPref { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundProviderPreferences, nameof(ID))]
        public ForeignKey<uint> SoundProviderPrefUnderwater { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundAmbience, nameof(ID))]
        public ForeignKey<uint> AmbienceID { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ZoneMusic, nameof(ID))]
        public ForeignKey<uint> ZoneMusic { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ZoneIntroMusicTable, nameof(ID))]
        public ForeignKey<uint> IntroSound { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint Flags { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.AreaTable, nameof(ID))]
        public ForeignKey<uint> AreaTableID { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference AreaName { get; set; }

        /*
            Cataclysm and up
        */

        [RecordField(WarcraftVersion.Cataclysm)]
        [ForeignKeyInfo(DatabaseName.ZoneIntroMusicTable, nameof(ID))]
        public ForeignKey<uint> UnderwaterIntroSound { get; set; }

        [RecordField(WarcraftVersion.Cataclysm)]
        [ForeignKeyInfo(DatabaseName.ZoneMusic, nameof(ID))]
        public ForeignKey<uint> UnderwaterZoneMusic { get; set; }

        [RecordField(WarcraftVersion.Cataclysm)]
        [ForeignKeyInfo(DatabaseName.SoundAmbience, nameof(ID))]
        public ForeignKey<uint> UnderwaterAmbience { get; set; }

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            return this.AreaName.GetReferences();
        }
    }
}
