//
//  CreatureDisplayInfoExtraRecord.cs
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
    [DatabaseRecord(DatabaseName.CreatureDisplayInfoExtra)]
    public class CreatureDisplayInfoExtraRecord : DBCRecord
    {
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ChrRaces, nameof(ID))]
        public ForeignKey<uint> Race { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade, RemovedIn = WarcraftVersion.Wrath)]
        [ForeignKeyInfo(DatabaseName.CreatureType, nameof(ID))]
        public ForeignKey<uint> CreatureType { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public bool IsFemale { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint Skin { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint Face { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.CharHairGeosets, nameof(ID))]
        public ForeignKey<uint> HairType { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.CharSections, nameof(ID))]
        public ForeignKey<uint> HairVariation { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint BeardType { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Helmet { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Shoulder { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Shirt { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Cuirass { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Belt { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Legs { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Boots { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Wrist { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Gloves { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Tabard { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Cape { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public uint Flags { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public StringReference BakedName { get; set; }

        public override IEnumerable<StringReference> GetStringReferences()
        {
            yield return this.BakedName;
        }
    }
}
