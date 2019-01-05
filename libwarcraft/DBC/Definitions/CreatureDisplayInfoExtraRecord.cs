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
    /// <summary>
    /// Defines extra information for display info record for a creature that contains a unique look - creature look,
    /// armor, weapons, etc.
    /// </summary>
    [DatabaseRecord(DatabaseName.CreatureDisplayInfoExtra)]
    public class CreatureDisplayInfoExtraRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the race the display info is valid for.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ChrRaces, nameof(ID))]
        public ForeignKey<uint> Race { get; set; }

        /// <summary>
        /// Gets or sets the creature type the display info is valid for.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade, RemovedIn = WarcraftVersion.Wrath), ForeignKeyInfo(DatabaseName.CreatureType, nameof(ID))]
        public ForeignKey<uint> CreatureType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the display info is for a female character.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public bool IsFemale { get; set; }

        /// <summary>
        /// Gets or sets the skin ID.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Skin { get; set; }

        /// <summary>
        /// Gets or sets the face ID.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Face { get; set; }

        /// <summary>
        /// Gets or sets the hair type.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.CharHairGeosets, nameof(ID))]
        public ForeignKey<uint> HairType { get; set; }

        /// <summary>
        /// Gets or sets the hair variation.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.CharSections, nameof(ID))]
        public ForeignKey<uint> HairVariation { get; set; }

        /// <summary>
        /// Gets or sets the beard type.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint BeardType { get; set; }

        /// <summary>
        /// Gets or sets the equipped helmet.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Helmet { get; set; }

        /// <summary>
        /// Gets or sets the equipped shoulder pad.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Shoulder { get; set; }

        /// <summary>
        /// Gets or sets the equipped shirt.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Shirt { get; set; }

        /// <summary>
        /// Gets or sets the equipped cuirass.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Cuirass { get; set; }

        /// <summary>
        /// Gets or sets the equipped belt.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Belt { get; set; }

        /// <summary>
        /// Gets or sets the equipped leggings.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Legs { get; set; }

        /// <summary>
        /// Gets or sets the equipped boots.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Boots { get; set; }

        /// <summary>
        /// Gets or sets the equipped wristguard.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Wrist { get; set; }

        /// <summary>
        /// Gets or sets the equipped gloves.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Gloves { get; set; }

        /// <summary>
        /// Gets or sets the equipped tabard.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Tabard { get; set; }

        /// <summary>
        /// Gets or sets the equipped cape.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath), ForeignKeyInfo(DatabaseName.ItemDisplayInfo, nameof(ID))]
        public ForeignKey<uint> Cape { get; set; }

        /// <summary>
        /// Gets or sets the display flags.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Flags { get; set; }

        /// <summary>
        /// Gets or sets the baked name of the display info.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference BakedName { get; set; }

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            yield return BakedName;
        }
    }
}
