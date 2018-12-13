//
//  AnimationDataRecord.cs
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
    /// Animation data record. This database defines the different animations models can have, and
    /// is referenced by M2 and MDX files.
    /// </summary>
    [DatabaseRecord(DatabaseName.AnimationData)]
    public class AnimationDataRecord : DBCRecord
    {
        /// <summary>
        /// The name of the animation.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference Name { get; set; }

        /// <summary>
        /// The weapon flags. This affects how the model's weapons are held during the animation.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public WeaponAnimationFlags WeaponFlags { get; set; }

        /// <summary>
        /// The body flags.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint BodyFlags { get; set; }

        /// <summary>
        /// General animation flags.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Flags { get; set; }

        /// <summary>
        /// The fallback animation that precedes this one.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.AnimationData, nameof(ID))]
        public ForeignKey<uint> FallbackAnimation { get; set; }

        /// <summary>
        /// The top-level behaviour animation that this animation is a child of.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.AnimationData, nameof(ID))]
        public ForeignKey<uint> BehaviourAnimation { get; set; }

        /// <summary>
        /// The behaviour tier of the animation. In most cases, this indicates whether or not the animation
        /// is used for flying characters.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint BehaviourTier { get; set; }

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            yield return Name;
        }
    }

    /// <summary>
    /// Weapon animation flags.
    /// </summary>
    public enum WeaponAnimationFlags : uint
    {
        /// <summary>
        /// Ignores the current state of the character's weapons.
        /// </summary>
        None = 0,

        /// <summary>
        /// Sheathes the weapons for the duration of the animation.
        /// </summary>
        Sheathe = 4,

        /// <summary>
        /// Sheathes the weapons for the duration of the animation.
        /// </summary>
        Sheathe2 = 16,

        /// <summary>
        /// Unsheathes the weapons for the duration of the animation.
        /// </summary>
        Unsheathe = 32
    }
}

