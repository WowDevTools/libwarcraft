//
//  SpellAttributeB.cs
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
    /// The second block of spell attributes.
    /// </summary>
    [Flags]
    public enum SpellAttributeB : uint
    {
        /// <summary>
        /// This spell will dismiss the current pet.
        /// </summary>
        DismissPet = 0x00000001,

        /// <summary>
        /// This spell will drain all available power.
        /// </summary>
        DrainAllPower = 0x00000002,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell is channeled.
        /// </summary>
        Channeled1 = 0x00000004,

        /// <summary>
        /// This spell cannot be redirected.
        /// </summary>
        CanNotBeRedirected = 0x00000008,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown2 = 0x00000010,

        /// <summary>
        /// This spell does not break stealth.
        /// </summary>
        DoNotBreakStealth = 0x00000020,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell is channeled.
        /// </summary>
        Channeled2 = 0x00000040,

        /// <summary>
        /// This spell cannot be reflected.
        /// </summary>
        CanNotBeReflected = 0x00000080,

        /// <summary>
        /// This spell cannot target a unit in combat.
        /// </summary>
        CanNotTargetUnitInCombat = 0x00000100,

        /// <summary>
        /// This spell will start melee combat after it has been cast.
        /// </summary>
        StartMeleeCombatAfterCast = 0x00000200,

        /// <summary>
        /// This spell does not generate threat.
        /// </summary>
        DoesNotGenerateThreat = 0x00000400,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown3 = 0x00000800,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell is a pickpocketing spell.
        /// </summary>
        IsPickpocket = 0x00001000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell is a farsight spell.
        /// </summary>
        Farsight = 0x00002000,

        /// <summary>
        /// This spell is channeled, and the caster must always face the target
        /// </summary>
        ChanneledMustFaceTarget = 0x00004000,

        /// <summary>
        /// This spell will dispel any auras on the target.
        /// </summary>
        DispelAuras = 0x00008000,

        /// <summary>
        /// This spell is unaffected by immunity to its school.
        /// </summary>
        UnaffectedBySchoolImmune = 0x00010000,

        /// <summary>
        /// Pets cannot autocast the spell.
        /// </summary>
        PetCannotAutocast = 0x00020000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown4 = 0x00040000,

        /// <summary>
        /// The caster cannot target themselves with this spell.
        /// </summary>
        CanNotTargetSelf = 0x00080000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell requires combo points.
        /// </summary>
        RequiresComboPoints1 = 0x00100000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown5 = 0x00200000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell requires combo points.
        /// </summary>
        RequiresComboPoints2 = 0x00400000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown6 = 0x00800000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell is a fishing spell.
        /// </summary>
        IsFishing = 0x01000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown7 = 0x02000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown8 = 0x04000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknonw9 = 0x08000000,

        /// <summary>
        /// This spell should not be displayed in the aura bar.
        /// </summary>
        DoNotDisplayInAuraBar = 0x10000000,

        /// <summary>
        /// The name of the spell should be displayed in the casting bar.
        /// </summary>
        DisplaySpellNameInCastBar = 0x20000000,

        /// <summary>
        /// This spell is enabled after dodging.
        /// </summary>
        EnableAfterDodge = 0x40000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown10 = 0x80000000,
    }
}
