//
//  SpellAttributeG.cs
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
    /// The seventh block of spell attributes.
    /// </summary>
    [Flags]
    public enum SpellAttributeG : uint
    {
        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        DoNotDisplayCooldown = 0x00000001,

        /// <summary>
        /// This spell can only be cast in arenas.
        /// </summary>
        OnlyInArena = 0x00000002,

        /// <summary>
        /// This spell ignores any auras on the caster.
        /// </summary>
        IgnoreCasterAuras = 0x00000004,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown78 = 0x00000008,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown79 = 0x00000010,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown80 = 0x00000020,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown81 = 0x00000040,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown82 = 0x00000080,

        /// <summary>
        /// This spell cannot target crowd controlled units.
        /// </summary>
        CannotTargetCrowdControlled = 0x00000100,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown83 = 0x00000200,

        /// <summary>
        /// This spell can target possessed friendlies.
        /// </summary>
        CanTargetPossessedFriend = 0x00000400,

        /// <summary>
        /// This spell cannot be cast in raids.
        /// </summary>
        NotInRaidInstance = 0x00000800,

        /// <summary>
        /// This spell can be cast while on a vehicle.
        /// </summary>
        CastableWhileOnVehicle = 0x00001000,

        /// <summary>
        /// This spell can target invisible units.
        /// </summary>
        CanTargetInvisible = 0x00002000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown84 = 0x00004000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown85 = 0x00008000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown86 = 0x00010000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown87 = 0x00020000,

        /// <summary>
        /// This spell will not be allowed if the unit is not possessed, and the charmer of the caster will be the original caster.
        /// </summary>
        CastByCharmer = 0x00040000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown88 = 0x00080000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown89 = 0x00100000,

        /// <summary>
        /// A client-side attribute.
        /// </summary>
        ClientUITargetEffects = 0x00200000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown90 = 0x00400000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown91 = 0x00800000,

        /// <summary>
        /// This spell can target otherwise untargetable units.
        /// </summary>
        CanTargetUntargetable = 0x01000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown92 = 0x02000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown93 = 0x04000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown94 = 0x08000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknow95 = 0x10000000,

        /// <summary>
        /// This spell ignores percentile damage modifiers.
        /// </summary>
        IgnorePercentDamageMods = 0x20000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown96 = 0x40000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown97 = 0x80000000,
    }
}
