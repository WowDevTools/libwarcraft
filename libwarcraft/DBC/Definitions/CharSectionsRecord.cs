//
//  CharSectionsRecord.cs
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
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// Represents a record describing textures for a given section of a character.
    /// </summary>
    [DatabaseRecord(DatabaseName.CharSections)]
    public class CharSectionsRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the ID of the race that the section belongs to.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.ChrRaces, nameof(ID))]
        public ForeignKey<uint> Race { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether the section belongs to a female character.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public bool IsFemale { get; set; }

        /// <summary>
        /// Gets or sets the base section.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public CharSectionType BaseSection { get; set; }

        /// <summary>
        /// Gets or sets the section type.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Type { get; set; }

        /// <summary>
        /// Gets or sets the variation ID.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Variation { get; set; }

        /// <summary>
        /// Gets or sets the first section texture.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference SectionTexture0 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the second section texture.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference SectionTexture1 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the third section texture.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference SectionTexture2 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the section flags.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public CharSectionAvailability Availabilities { get; set; }

        /*
            What follows are forwards into the SectionTextures list for ease of use.
            Which ones to use depend on the value of BaseSection.
        */

        /// <summary>
        /// Gets the primary skin texture.
        /// </summary>
        public StringReference SkinTexture => SectionTexture0;

        /// <summary>
        /// Gets the extra skin texture.
        /// </summary>
        public StringReference ExtraSkinTexture => SectionTexture1;

        /// <summary>
        /// Gets the lower face texture.
        /// </summary>
        public StringReference FaceLowerTexture => SectionTexture0;

        /// <summary>
        /// Gets the upper face texture.
        /// </summary>
        public StringReference FaceUpperTexture => SectionTexture1;

        /// <summary>
        /// Gets the lower facial hair texture.
        /// </summary>
        public StringReference FacialLowerTexture => SectionTexture0;

        /// <summary>
        /// Gets the upper facial hair texture.
        /// </summary>
        public StringReference FacialUpperTexture => SectionTexture1;

        /// <summary>
        /// Gets the hair texture.
        /// </summary>
        public StringReference HairTexture => SectionTexture0;

        /// <summary>
        /// Gets the lower scalp texture.
        /// </summary>
        public StringReference ScalpLowerTexture => SectionTexture1;

        /// <summary>
        /// Gets the upper scalp texture.
        /// </summary>
        public StringReference ScalpUpperTexture => SectionTexture2;

        /// <summary>
        /// Gets the pelvis texture.
        /// </summary>
        public StringReference PelvisTexture => SectionTexture0;

        /// <summary>
        /// Gets the torso texture.
        /// </summary>
        public StringReference TorsoTexture => SectionTexture1;

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            return new List<StringReference>
            {
                SectionTexture0,
                SectionTexture1,
                SectionTexture2
            };
        }
    }
}
