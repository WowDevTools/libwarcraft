//
//  SpellAttributeF.cs
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
    /// The sixth block of spell attributes.
    /// </summary>
    [Flags]
    public enum SpellAttributeF : uint
    {
        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown56 = 0x00000001,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell requires no reagents while preparing it.
        /// </summary>
        NoReagentsWhilePreparing = 0x00000002,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown57 = 0x00000004,

        /// <summary>
        /// This spell can be used while stunned.
        /// </summary>
        UsableWhileStunned = 0x00000008,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown58 = 0x00000010,

        /// <summary>
        /// This spell can only ever be applied to a single target.
        /// </summary>
        SingleTargetOnly = 0x00000020,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown59 = 0x00000040,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown60 = 0x00000080,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown61 = 0x00000100,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell starts a periodic tick when it is applied.
        /// </summary>
        StartPeriodicTickWhenApplied = 0x00000200,

        /// <summary>
        /// The duration of this spell is hidden for the client.
        /// </summary>
        HideDurationForClient = 0x00000400,

        /// <summary>
        /// The target of the target is allowed as a target for the spell.
        /// </summary>
        AllowTargetOfTargetAsTarget = 0x00000800,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown62 = 0x00001000,

        /// <summary>
        /// The caster's haste affects the duration of this spell.
        /// </summary>
        HasteAffectsDuration = 0x00002000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown63 = 0x00004000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown64 = 0x00008000,

        /// <summary>
        /// This spell will check for a special class item before casting.
        /// </summary>
        CheckForSpecialClassItem = 0x00010000,

        /// <summary>
        /// This spell can be used while feared.
        /// </summary>
        UsableWhileFeared = 0x00020000,

        /// <summary>
        /// This spell can be used while confused.
        /// </summary>
        UsableWhileConfused = 0x00040000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown65 = 0x00080000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown66 = 0x00100000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown67 = 0x00200000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown68 = 0x00400000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown69 = 0x00800000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown70 = 0x01000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown71 = 0x02000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown72 = 0x04000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown73 = 0x08000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown74 = 0x10000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown75 = 0x20000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown76 = 0x40000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown77 = 0x80000000,
    }
}
