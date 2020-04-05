//
//  CreatureModelDataRecord.cs
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
    /// Defines model data for a creature.
    /// </summary>
    [DatabaseRecord(DatabaseName.CreatureModelData)]
    public class CreatureModelDataRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the model flags.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Flags { get; set; }

        /// <summary>
        /// Gets or sets the path to the model.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference ModelPath { get; set; } = null!;

        /// <summary>
        /// Gets or sets the size class of the model.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint SizeClass { get; set; }

        /// <summary>
        /// Gets or sets the scale of the model.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float ModelScale { get; set; }

        /// <summary>
        /// Gets or sets the blood level - that is, the gore level.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.UnitBloodLevels, nameof(ID))]
        public ForeignKey<uint> BloodLevel { get; set; } = null!;

        /// <summary>
        /// Gets or sets the footprint decal texture.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.FootprintTextures, nameof(ID))]
        public ForeignKey<uint> FootprintDecal { get; set; } = null!;

        /// <summary>
        /// Gets or sets the length of the footprint.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float FootprintDecalLength { get; set; }

        /// <summary>
        /// Gets or sets the width of the footprint.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float FootprintDecalWidth { get; set; }

        /// <summary>
        /// Gets or sets the footprint's particle scale.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float FootprintDecalParticleScale { get; set; }

        /// <summary>
        /// Gets or sets the material ID.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint FoleyMaterialID { get; set; }

        /// <summary>
        /// Gets or sets the footstep shake.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.CameraShakes, nameof(ID))]
        public ForeignKey<uint> FootstepShakeSize { get; set; } = null!;

        /// <summary>
        /// Gets or sets the death thud shake.
        /// </summary>
        [RecordField(WarcraftVersion.Classic), ForeignKeyInfo(DatabaseName.CameraShakes, nameof(ID))]
        public ForeignKey<uint> DeathThudShakeSize { get; set; } = null!;

        /// <summary>
        /// Gets or sets the sound pack.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade), ForeignKeyInfo(DatabaseName.CreatureSoundData, nameof(ID))]
        public ForeignKey<uint> Sound { get; set; } = null!;

        /// <summary>
        /// Gets or sets the model's collision width.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float CollisionWidth { get; set; }

        /// <summary>
        /// Gets or sets the model's collision height.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float CollisionHeight { get; set; }

        /// <summary>
        /// Gets or sets the mount height.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float MountHeight { get; set; }

        /// <summary>
        /// Gets or sets the bounding box.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public Box BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the world effect scale.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public float WorldEffectScale { get; set; }

        /// <summary>
        /// Gets or sets the attached effect scale.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public float AttachedEffectScale { get; set; }

        /// <summary>
        /// Gets or sets the missile collision radius.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public float MissileCollisionRadius { get; set; }

        /// <summary>
        /// Gets or sets the missile collision push strength.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public float MissileCollisionPush { get; set; }

        /// <summary>
        /// Gets or sets the missile collision raise strength.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public float MissileCollisionRaise { get; set; }

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            yield return ModelPath;
        }
    }
}
