//
//  CreatureDisplayInfoRecord.cs
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
    /// Defines the base display information for a creature.
    /// </summary>
    [DatabaseRecord(DatabaseName.CreatureDisplayInfo)]
    public class CreatureDisplayInfoRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the model to use.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.CreatureModelData, nameof(ID))]
        public ForeignKey<uint> Model { get; set; } = null!;

        /// <summary>
        /// Gets or sets the sound data to use.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.CreatureSoundData, nameof(ID))]
        public ForeignKey<uint> Sound { get; set; } = null!;

        /// <summary>
        /// Gets or sets eventual extra display information to use.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.CreatureDisplayInfoExtra, nameof(ID))]
        public ForeignKey<uint> ExtraDisplayInformation { get; set; } = null!;

        /// <summary>
        /// Gets or sets the scale of the model.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float Scale { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the model.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Opacity { get; set; }

        /// <summary>
        /// Gets or sets the first texture.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference TextureVariation1 { get; set; } = null!;

        /// <summary>
        /// Gets or sets second texture.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference TextureVariation2 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the third texture.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference TextureVariation3 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the portrait texture.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public StringReference PortraitTexture { get; set; } = null!;

        /// <summary>
        /// Gets or sets the size class.
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath)]
        public uint SizeClass { get; set; }

        /// <summary>
        /// Gets or sets the blood level - that is, the gore level.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath), ForeignKeyInfo(DatabaseName.UnitBloodLevels, nameof(ID))]
        public ForeignKey<uint> BloodLevel { get; set; } = null!;

        /// <summary>
        /// Gets or sets the blood splatter.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.UnitBlood, nameof(ID))]
        public ForeignKey<uint> Blood { get; set; } = null!;

        /// <summary>
        /// Gets or sets the sound pack for the NPC.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.NPCSounds, nameof(ID))]
        public ForeignKey<uint> NPCSound { get; set; } = null!;

        /// <summary>
        /// Gets or sets the base colour of particles.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint ParticleColour { get; set; }

        /// <summary>
        /// Gets or sets the geoset data.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint CreatureGeosetData { get; set; }

        /// <summary>
        /// Gets or sets the object effect package.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint ObjectEffectPackage { get; set; }

        /// <summary>
        /// Gets the texture variations.
        /// </summary>
        public IReadOnlyList<StringReference> TextureVariations =>
            new[] { TextureVariation1, TextureVariation2, TextureVariation3 };

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            var references = new List<StringReference>
            {
                TextureVariation1,
                TextureVariation2,
                TextureVariation3
            };

            if (Version >= WarcraftVersion.BurningCrusade)
            {
                references.Add(PortraitTexture);
            }

            return references;
        }
    }
}
