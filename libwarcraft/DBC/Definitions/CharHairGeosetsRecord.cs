//
//  CharHairGeosetsRecord.cs
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
    /// Represents information about a character's hair geoset.
    /// </summary>
    [DatabaseRecord(DatabaseName.CharHairGeosets)]
    public class CharHairGeosetsRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the ID of the race that the geoset belongs to.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ChrRaces, nameof(ID))]
        public ForeignKey<uint> Race { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the geoset belongs to a female character.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public bool IsFemale { get; set; }

        /// <summary>
        /// Gets or sets the variation ID of the geoset.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint VariationID { get; set; }

        /// <summary>
        /// Gets or sets the geoset ID.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint GeosetID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the character's scalp should be shown.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public bool ShowScalp { get; set; }
    }
}
