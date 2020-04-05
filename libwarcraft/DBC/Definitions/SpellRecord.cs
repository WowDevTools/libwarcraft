//
//  SpellRecord.cs
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

using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

// ReSharper disable UnusedMember.Global
namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// A database record defining a spell.
    /// </summary>
    [DatabaseRecord(DatabaseName.Spell)]
    public class SpellRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the school of the spell (fire, destruction, etc). This is a reference to a row in another
        /// database.
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.BurningCrusade)]
        public uint School { get; set; }

        /// <summary>
        /// Gets or sets the category of the spell (tradeskill, passive, active, etc). This is a reference to a row in another
        /// database.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SpellCategory, nameof(ID))]
        public ForeignKey<uint> Category { get; set; } = null!;

        /// <summary>
        /// Gets or sets the UI type to use when casting.
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath)]
        public uint CastUI { get; set; }

        /// <summary>
        /// Gets or sets the dispel type of the spell. This is a reference to a row in another
        /// database.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SpellDispelType, nameof(ID))]
        public ForeignKey<uint> DispelType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the mechanic type of the spell. This is a reference to a row in another
        /// database.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SpellMechanic, nameof(ID))]
        public ForeignKey<uint> Mechanic { get; set; } = null!;

        /// <summary>
        /// Gets or sets the first block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeA"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SpellAttributeA AttributesA { get; set; }

        /// <summary>
        /// Gets or sets the second block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeB"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SpellAttributeB AttributesB { get; set; }

        /// <summary>
        /// Gets or sets the third block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeC"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SpellAttributeC AttributesC { get; set; }

        /// <summary>
        /// Gets or sets the fourth block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeD"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SpellAttributeD AttributesD { get; set; }

        /// <summary>
        /// Gets or sets the fifth block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeE"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SpellAttributeE AttributesE { get; set; }

        /// <summary>
        /// Gets or sets the sixth block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeF"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public SpellAttributeF AttributesF { get; set; }

        /// <summary>
        /// Gets or sets the seventh block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeG"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public SpellAttributeG AttributesG { get; set; }

        /// <summary>
        /// Gets or sets the eighth block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeH"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public SpellAttributeH AttributesH { get; set; }

        /// <summary>
        /// Gets or sets the stances the spell may be used in.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint StanceWhitelistA { get; set; }

        /// <summary>
        /// Gets or sets the stances the spell may be used in.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint StanceWhitelistB { get; set; }

        /// <summary>
        /// Gets or sets the stances the spell may not be used in.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint StanceBlacklistA { get; set; }

        /// <summary>
        /// Gets or sets the stances the spell may not be used in.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint StanceBlacklistB { get; set; }

        /// <summary>
        /// Gets or sets the valid targets of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Targets { get; set; }

        /// <summary>
        /// Gets or sets the valid target creatures types of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint TargetCreatureType { get; set; }

        /// <summary>
        /// Gets or sets the valid spell focus objects.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint SpellFocusObject { get; set; }

        /// <summary>
        /// Gets or sets a set of flags controlling how the caster must be facing.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint FacingCasterFlags { get; set; }

        /// <summary>
        /// Gets or sets the state the caster's aura must be in.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint CasterAuraState { get; set; }

        /// <summary>
        /// Gets or sets the state the target's aura must be in.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint TargetAuraState { get; set; }

        /// <summary>
        /// Gets or sets the state the caster's aura must not be in.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint ExcludeCasterAuraState { get; set; }

        /// <summary>
        /// Gets or sets the state the target's aura must not be in.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint ExcludeTargetAuraState { get; set; }

        /// <summary>
        /// Gets or sets the aura spell the caster must have.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint CasterAuraSpell { get; set; }

        /// <summary>
        /// Gets or sets the aura spell the target must have.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint TargetAuraSpell { get; set; }

        /// <summary>
        /// Gets or sets the aura spell the caster must not have.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint ExcludeCasterAuraSpell { get; set; }

        /// <summary>
        /// Gets or sets the aura spell the target must not have.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint ExcludeTargetAuraSpell { get; set; }

        /// <summary>
        /// Gets or sets the casting time.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint CastingTimeIndex { get; set; }

        /// <summary>
        /// Gets or sets the cooldown of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint RecoveryTime { get; set; }

        /// <summary>
        /// Gets or sets the category cooldown of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint CategoryRecoveryTime { get; set; }

        /// <summary>
        /// Gets or sets the interruption conditions.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint InterruptFlags { get; set; }

        /// <summary>
        /// Gets or sets the aura interruption conditions.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint AuraInterruptFlags { get; set; }

        /// <summary>
        /// Gets or sets the channeling interruption conditions.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ChannelInterruptFlags { get; set; }

        /// <summary>
        /// Gets or sets a set of proc conditions.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ProcTypeMask { get; set; }

        /// <summary>
        /// Gets or sets the proc chance of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ProcChance { get; set; }

        /// <summary>
        /// Gets or sets the number of charges that a proc has.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ProcCharges { get; set; }

        /// <summary>
        /// Gets or sets the maximum level of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint MaxLevel { get; set; }

        /// <summary>
        /// Gets or sets the base level of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint BaseLevel { get; set; }

        /// <summary>
        /// Gets or sets the spell's level.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint SpellLevel { get; set; }

        /// <summary>
        /// Gets or sets the duration of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint DurationIndex { get; set; }

        /// <summary>
        /// Gets or sets the power type of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint PowerType { get; set; }

        /// <summary>
        /// Gets or sets the cost of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ManaCost { get; set; }

        /// <summary>
        /// Gets or sets the cost per level of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ManaCostPerLevel { get; set; }

        /// <summary>
        /// Gets or sets the cost per second of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ManaCostPerSecond { get; set; }

        /// <summary>
        /// Gets or sets the cost per second per level of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ManaCostPerSecondPerLevel { get; set; }

        /// <summary>
        /// Gets or sets the range of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint RangeIndex { get; set; }

        /// <summary>
        /// Gets or sets the speed of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the next spell that must be cast.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ModalNextSpell { get; set; }

        /// <summary>
        /// Gets or sets the number of stacks the spell has.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint StackAmount { get; set; }

        /// <summary>
        /// Gets or sets the totems required.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 2)]
        public uint[] Totem { get; set; } = null!;

        /// <summary>
        /// Gets or sets the required reagents.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 8)]
        public uint[] Reagent { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of each reagent required.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 8)]
        public uint[] ReagentCount { get; set; } = null!;

        /// <summary>
        /// Gets or sets the item class that the spell requires a player to have equipped.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint EquippedItemClass { get; set; }

        /// <summary>
        /// Gets or sets the item subclass that the spell requires a player to have equipped.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint EquippedItemSubclass { get; set; }

        /// <summary>
        /// Gets or sets the item type that the spell requires a player to have in their inventory.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint EquippedItemInvType { get; set; }

        /// <summary>
        /// Gets or sets the random effects the spell has.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] Effect { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of sides on each random effect's die.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectDieSides { get; set; } = null!;

        /// <summary>
        /// Gets or sets the base number of dice of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath, Count = 3)]
        public uint[] EffectBaseDice { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of dice per level.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath, Count = 3)]
        public float[] EffectDicePerLevel { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of real points per level.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public float[] EffectRealPointsPerLevel { get; set; } = null!;

        /// <summary>
        /// Gets or sets the base points per level.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public float[] EffectBasePoints { get; set; } = null!;

        /// <summary>
        /// Gets or sets the mechanic of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectMechanic { get; set; } = null!;

        /// <summary>
        /// Gets or sets the implicit targets of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] ImplicitTargetA { get; set; } = null!;

        /// <summary>
        /// Gets or sets the implicit targets of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] ImplicitTargetB { get; set; } = null!;

        /// <summary>
        /// Gets or sets the radius of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectRadiusIndex { get; set; } = null!;

        /// <summary>
        /// Gets or sets the aura of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectAura { get; set; } = null!;

        /// <summary>
        /// Gets or sets the aura period of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public uint[] EffectAuraPeriod { get; set; } = null!;

        /// <summary>
        /// Gets or sets the amplitude of each ranomd effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath, Count = 3)]
        public float[] EffectAmplitude { get; set; } = null!;

        /// <summary>
        /// Gets or sets the multiplier of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public float[] EffectMultipleValue { get; set; } = null!;

        /// <summary>
        /// Gets or sets the chain target of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectChainTarget { get; set; } = null!;

        /// <summary>
        /// Gets or sets the required item type of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectItemType { get; set; } = null!;

        /// <summary>
        /// Gets or sets an unknown value of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectMiscValue { get; set; } = null!;

        /// <summary>
        /// Gets or sets an unknown value of each random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.BurningCrusade, Count = 3)]
        public uint[] EffectMiscValueB { get; set; } = null!;

        /// <summary>
        /// Gets or sets the spell to be triggered by the random effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectTriggerSpell { get; set; } = null!;

        /// <summary>
        /// Gets or sets the number of required points per combo per effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public float[] EffectPointsPerCombo { get; set; } = null!;

        /// <summary>
        /// Gets or sets the classes of the effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public uint[] EffectClassMaskA { get; set; } = null!;

        /// <summary>
        /// Gets or sets the classes of the effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public uint[] EffectClassMaskB { get; set; } = null!;

        /// <summary>
        /// Gets or sets the classes of the effect.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public uint[] EffectClassMaskC { get; set; } = null!;

        /// <summary>
        /// Gets or sets the visual effects of the spell.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 2)]
        public uint[] SpellVisualID { get; set; } = null!;

        /// <summary>
        /// Gets or sets the spell icon.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint SpellIconID { get; set; }

        /// <summary>
        /// Gets or sets the icon that is shown when the spell is active.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ActiveIconID { get; set; }

        /// <summary>
        /// Gets or sets the priority of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint SpellPriority { get; set; }

        /// <summary>
        /// Gets or sets the name of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the subtext of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference Subtext { get; set; } = null!;

        /// <summary>
        /// Gets or sets the full description of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the quick tooltip of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference Tooltip { get; set; } = null!;

        /// <summary>
        /// Gets or sets the percentile mana cost of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint ManaCostPercentage { get; set; }

        /// <summary>
        /// Gets or sets the category of the spell's recovery.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint StartRecoveryCategory { get; set; }

        /// <summary>
        /// Gets or sets the spell's recovery time.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint StartRecoveryTime { get; set; }

        /// <summary>
        /// Gets or sets the maximum target level of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint MaxTargetLevel { get; set; }

        /// <summary>
        /// Gets or sets the spell's class set.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint SpellClassSet { get; set; }

        /// <summary>
        /// Gets or sets the class mask of the spell.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 2)]
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public uint[] SpellClassMask { get; set; } = null!;

        /// <summary>
        /// Gets or sets the maximum number of targets the spell can have.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint MaxTargets { get; set; }

        /// <summary>
        /// Gets or sets the defense type of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint DefenseType { get; set; }

        /// <summary>
        /// Gets or sets the prevention type of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint PreventionType { get; set; }

        /// <summary>
        /// Gets or sets the order of the spell in the stance bar.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint StanceBarOrder { get; set; }

        /// <summary>
        /// Gets or sets the damage multipliers of the spell.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public float[] DamageMultiplier { get; set; } = null!;

        /// <summary>
        /// Gets or sets the minimum faction ID of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint MinFactionID { get; set; }

        /// <summary>
        /// Gets or sets the minimum reputation required to use the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint MinReputation { get; set; }

        /// <summary>
        /// Gets or sets the required aura vision.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint RequiredAuraVision { get; set; }

        /// <summary>
        /// Gets or sets the required totem categories.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.BurningCrusade, Count = 2)]
        public uint[] RequiredTotemCategoryID { get; set; } = null!;

        /// <summary>
        /// Gets or sets the ID of the area a player has to be in to use the spell.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint RequiredAreaID { get; set; }

        /// <summary>
        /// Gets or sets the schools the spell belongs to.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint SchoolMask { get; set; }

        /// <summary>
        /// Gets or sets the type of rune the spell uses.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint RuneCostID { get; set; }

        /// <summary>
        /// Gets or sets the missile the spell uses.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint SpellMissileID { get; set; }

        /// <summary>
        /// Gets or sets the power display of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public int PowerDisplayID { get; set; }

        /// <summary>
        /// Gets or sets the bonus multipliers of the spell.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public float[] EffectBonusMultiplier { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description variable.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint DescriptionVariableID { get; set; }

        /// <summary>
        /// Gets or sets the difficulty of the spell.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint DifficultyID { get; set; }
    }
}
