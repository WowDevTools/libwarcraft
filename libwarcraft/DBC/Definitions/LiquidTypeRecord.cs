//
//  LiquidTypeRecord.cs
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
    /// Defines the liquid type and properties of a liquid body.
    /// </summary>
    [DatabaseRecord(DatabaseName.LiquidType)]
    public class LiquidTypeRecord : DBCRecord
    {
        private LiquidType _internalType;

        /// <summary>
        /// Gets or sets the name of the liquid body.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference Name { get; set; }

        /// <summary>
        /// Gets or sets the flags of the liquid body.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint Flags { get; set; }

        /// <summary>
        /// Gets or sets the liquid type of the liquid body.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public LiquidType Type
        {
            get => TranslateLiquidType();
            set => _internalType = value;
        }

        /// <summary>
        /// Gets or sets the sound associated with the liquid body.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        [ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
        public ForeignKey<uint> Sound { get; set; }

        /// <summary>
        /// Gets or sets the spell effect applied to creatures in the liquid body.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.Spell, nameof(ID))]
        public ForeignKey<uint> SpellEffect { get; set; }

        /// <summary>
        /// Gets or sets the maximum screen-space depth to which underwater fog goes.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public float MaxDarkenDepth { get; set; }

        /// <summary>
        /// Gets or sets the darkening intensity of the underwater fog.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public float FogDarkenIntensity { get; set; }

        /// <summary>
        /// Gets or sets the darkening intensity of the ambient occlusion.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public float AmbientDarkenIntensity { get; set; }

        /// <summary>
        /// Gets or sets the darkening intensity of the direct darkening.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public float DirectDarkenIntensity { get; set; }

        /// <summary>
        /// Gets or sets the light ID of the liquid body.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint LightID { get; set; }

        /// <summary>
        /// Gets or sets the particle scale of the liquid body.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public float ParticleScale { get; set; }

        /// <summary>
        /// Gets or sets the particle movement speed of the liquid body.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint ParticleMovement { get; set; }

        /// <summary>
        /// Gets or sets the number of particle texture slots of the liquid body.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint ParticleTextureSlots { get; set; }

        /// <summary>
        /// Gets or sets the material ID of the liquid body.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint MaterialID { get; set; }

        /// <summary>
        /// Gets or sets the textures of the liquid body.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 6)]
        public StringReference[] Textures { get; set; }

        /// <summary>
        /// Gets or sets the colours of the liquid body.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 2)]
        public int[] Colour { get; set; }

        /// <summary>
        /// Gets or sets a set of unknown values.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 18)]
        public float[] Unknown1 { get; set; }

        /// <summary>
        /// Gets or sets another set of unknown values.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 4)]
        public int[] Unknown2 { get; set; }

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            yield return Name;
        }

        /// <summary>
        /// Translates the liquid type between the pre-cata values and the post-cata values, bringing them to a standard
        /// set of internal values.
        /// </summary>
        /// <returns>The translated type.</returns>
        public LiquidType TranslateLiquidType()
        {
            if (Version >= WarcraftVersion.Wrath)
            {
                return _internalType;
            }

            var baseValue = (int)_internalType;
            switch (baseValue)
            {
                case 0:
                {
                    return LiquidType.Magma;
                }

                case 2:
                {
                    return LiquidType.Slime;
                }

                case 3:
                {
                    return LiquidType.Water;
                }

                default:
                {
                    return LiquidType.Water;
                }
            }
        }
    }
}
