//
//  SpellAttributeE.cs
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

using System;

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// The fifth block of spell attributes.
    /// </summary>
    [Flags]
    public enum SpellAttributeE : uint
    {
        /// <summary>
        /// This spell ignores resistances against its school.
        /// </summary>
        IgnoreResistances = 0x00000001,

        /// <summary>
        /// This spell proc only triggers on the caster.
        /// </summary>
        ProcTriggerOnlyOnCaster = 0x00000002,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown38 = 0x00000004,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown39 = 0x00000008,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown40 = 0x00000010,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown41 = 0x00000020,

        /// <summary>
        /// This spell cannot be stolen.
        /// </summary>
        CanNotBeStolen = 0x00000040,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell is triggered by force.
        /// </summary>
        ForceTriggered = 0x00000080,

        /// <summary>
        /// This spell bypasses armor.
        /// </summary>
        BypassArmor = 0x00000100,

        /// <summary>
        /// This spell is initially disabled.
        /// </summary>
        InitiallyDisabled = 0x00000200,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        ExtendCost = 0x00000400,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown42 = 0x00000800,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown43 = 0x00001000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown44 = 0x00002000,

        /// <summary>
        /// Damage from this spell does not break auras.
        /// </summary>
        DamageDoesNotBreakAuras = 0x00004000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown45 = 0x00008000,

        /// <summary>
        /// This spell cannot be used in arenas.
        /// </summary>
        NotUsableInArenas = 0x00010000,

        /// <summary>
        /// This spell is usable in arenas.
        /// </summary>
        UsableInArenas = 0x00020000,

        /// <summary>
        /// This spell will chain between targets.
        /// </summary>
        AffectTargetsAsChain = 0x00040000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown46 = 0x00080000,

        /// <summary>
        /// When applied, this spell will not check for a more powerful version of itself.
        /// </summary>
        DoNotCheckForMorePowerful = 0x00100000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown47 = 0x00200000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown48 = 0x00400000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown49 = 0x00800000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown50 = 0x01000000,

        /// <summary>
        /// This spell scales for the caster's pet.
        /// </summary>
        IsPetScaling = 0x02000000,

        /// <summary>
        /// This spell can only be used in Outland.
        /// </summary>
        CanOnlyBeUsedInOutland = 0x04000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown51 = 0x08000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown52 = 0x10000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown53 = 0x20000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown54 = 0x40000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown55 = 0x80000000,
    }
}
