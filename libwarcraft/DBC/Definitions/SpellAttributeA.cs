//
//  SpellAttributeA.cs
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
    /// The first block of spell attributes.
    /// </summary>
    [Flags]
    public enum SpellAttributeA : uint
    {
        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown0 = 0x00000001,

        /// <summary>
        /// This spell requires some form of ammunition.
        /// </summary>
        RequiresAmmunition = 0x00000002,

        /// <summary>
        /// This spell will trigger on the next swing.
        /// </summary>
        TriggersOnNextSwing = 0x00000004,

        /// <summary>
        /// This spell is a form of replenishment.
        /// </summary>
        IsReplenishment = 0x00000008,

        /// <summary>
        /// This spell is an ability.
        /// </summary>
        Ability = 0x00000010,

        /// <summary>
        /// This spell is a tradeskill spell, that is, it is a recipe that creates something.
        /// </summary>
        TradeskillSpell = 0x00000020,

        /// <summary>
        /// This spell is passive.
        /// </summary>
        Passive = 0x00000040,

        /// <summary>
        /// This spell is hidden for the client, and should not be displayed in the UI.
        /// </summary>
        HiddenClientside = 0x00000080,

        /// <summary>
        /// This spell is hidden in combat logs.
        /// </summary>
        HideFromCombatLogs = 0x00000100,

        /// <summary>
        /// This spell always targets the item equipped in the main hand.
        /// </summary>
        AlwaysTargetMainHandItem = 0x00000200,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell triggers on the next swing.
        /// </summary>
        TriggersOnNextSwing2 = 0x00000400,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown1 = 0x00000800,

        /// <summary>
        /// This spell can only be cast during daytime.
        /// </summary>
        DaytimeOnly = 0x00001000,

        /// <summary>
        /// This spell can only be cast during nighttime.
        /// </summary>
        NighttimeOnly = 0x00002000,

        /// <summary>
        /// This spell can only be cast indoors.
        /// </summary>
        IndoorsOnly = 0x00004000,

        /// <summary>
        /// This spell can only be cast outdoors.
        /// </summary>
        OutdoorsOnly = 0x00008000,

        /// <summary>
        /// This spell cannot be used while shapeshifted.
        /// </summary>
        NotUseableWhileShapeshifted = 0x00010000,

        /// <summary>
        /// This spell can only be cast while in stealth.
        /// </summary>
        MustBeInStealth = 0x00020000,

        /// <summary>
        /// This spell does not affect the sheath state of the caster's weapons.
        /// </summary>
        DoNotAffectWeaponSheathState = 0x00040000,

        /// <summary>
        /// The damage of this spell scales by the caster's level.
        /// </summary>
        UseLevelScaledDamage = 0x00080000,

        /// <summary>
        /// When cast, this spell causes the caster to stop attacking.
        /// </summary>
        StopAttackingWhenUsed = 0x00100000,

        /// <summary>
        /// This spell cannot be dodged, blocked or parried.
        /// </summary>
        ImpossibleToDodgeBlockParry = 0x00200000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        AlwaysFaceTarget = 0x00400000,

        /// <summary>
        /// This spell can be cast while dead.
        /// </summary>
        CastableWhileDead = 0x00800000,

        /// <summary>
        /// This spell can be cast while mounted.
        /// </summary>
        CastableWhileMounted = 0x01000000,

        /// <summary>
        /// The cooldown of this spell will not start ticking down before the spell has finished.
        /// </summary>
        CooldownDisabledWhileActive = 0x02000000,

        /// <summary>
        /// This spell is some form of detrimental effect.
        /// </summary>
        DebuffOrNegativeSpell = 0x04000000,

        /// <summary>
        /// This spell can be cast while sitting.
        /// </summary>
        CastableWhileSitting = 0x08000000,

        /// <summary>
        /// This spell cannot be used while in combat.
        /// </summary>
        CanNotUseInCombat = 0x10000000,

        /// <summary>
        /// This spell bypasses invulnerability.
        /// </summary>
        UnaffectedByInvulnerability = 0x20000000,

        /// <summary>
        /// This spell can be interrupted by damage.
        /// </summary>
        DamageCanInterrupt = 0x40000000,

        /// <summary>
        /// This spell cannot be cancelled once cast.
        /// </summary>
        CanNotCancelOnceCast = 0x80000000
    }
}
