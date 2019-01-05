//
//  SpellAttributeC.cs
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

using System;

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// The third block of spell attributes.
    /// </summary>
    [Flags]
    public enum SpellAttributeC : uint
    {
        /// <summary>
        /// This spell can target dead units.
        /// </summary>
        CanTargetDead = 0x00000001,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown11 = 0x00000002,

        /// <summary>
        /// This spell can target units that are not in line of sight.
        /// </summary>
        CanTargetTargetNotInSight = 0x00000004,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown12 = 0x00000008,

        /// <summary>
        /// This spell should be displayed in the stance bar.
        /// </summary>
        DisplayInStanceBar = 0x00000010,

        /// <summary>
        /// This spell repeats automatically.
        /// </summary>
        RepeatAutomatically = 0x00000020,

        /// <summary>
        /// This spell can only target tapped units.
        /// </summary>
        CanOnlyTargetTapped = 0x00000040,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown13 = 0x00000080,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown14 = 0x00000100,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown15 = 0x00000200,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown16 = 0x00000400,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell is a health funnel.
        /// </summary>
        HealthFunnel = 0x00000800,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown17 = 0x00001000,

        /// <summary>
        /// This spell is an enchantment, and it is preserved in arenas.
        /// </summary>
        PreserveEnchantInArenas = 0x00002000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown18 = 0x00004000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown19 = 0x00008000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell tames a beast.
        /// </summary>
        IsTameBeast = 0x00010000,

        /// <summary>
        /// The cooldown timer is not reset for automatic attacks from this spell.
        /// </summary>
        DoNotResetTimersForAutos = 0x00020000,

        /// <summary>
        /// The caster's pet must be dead to cast this spell.
        /// </summary>
        PetMustBeDead = 0x00040000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell does not need the caster to be shapeshifted.
        /// </summary>
        DoesNotNeedShapeshift = 0x00080000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown20 = 0x00100000,

        /// <summary>
        /// This spell reduces damage.
        /// </summary>
        ReducesDamage = 0x00200000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown21 = 0x00400000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell is arcane concentration.
        /// </summary>
        IsArcaneConcentration = 0x00800000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown22 = 0x01000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown23 = 0x02000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown24 = 0x04000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown25 = 0x08000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown26 = 0x10000000,

        /// <summary>
        /// This spell cannot score a critical hit.
        /// </summary>
        CannotCrit = 0x20000000,

        /// <summary>
        /// This spell can trigger multiple times.
        /// </summary>
        CanTriggerMultipleTimes = 0x40000000,

        /// <summary>
        /// This spell is a food buff.
        /// </summary>
        IsFoodBuff = 0x80000000,
    }
}
