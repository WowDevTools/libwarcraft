//
//  SpellRecord.cs
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
        /// The school of the spell (fire, destruction, etc). This is a reference to a row in another
        /// database.
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.BurningCrusade)]
        public uint School { get; set; }

        /// <summary>
        /// The category of the spell (tradeskill, passive, active, etc). This is a reference to a row in another
        /// database.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SpellCategory, nameof(ID))]
        public ForeignKey<uint> Category { get; set; }

        /// <summary>
        /// The UI type to use when casting.
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath)]
        public uint CastUI { get; set; }

        /// <summary>
        /// The dispel type of the spell. This is a reference to a row in another
        /// database.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SpellDispelType, nameof(ID))]
        public ForeignKey<uint> DispelType { get; set; }

        /// <summary>
        /// The mechanic type of the spell. This is a reference to a row in another
        /// database.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SpellMechanic, nameof(ID))]
        public ForeignKey<uint> Mechanic { get; set; }

        /// <summary>
        /// The first block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeA"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SpellAttributeA AttributesA { get; set; }

        /// <summary>
        /// The second block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeB"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SpellAttributeB AttributesB { get; set; }

        /// <summary>
        /// The third block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeC"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SpellAttributeC AttributesC { get; set; }

        /// <summary>
        /// The fourth block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeD"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SpellAttributeD AttributesD { get; set; }

        /// <summary>
        /// The fifth block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeE"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SpellAttributeE AttributesE { get; set; }

        /// <summary>
        /// The sixth block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeF"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public SpellAttributeF AttributesF { get; set; }

        /// <summary>
        /// The seventh block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeG"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public SpellAttributeG AttributesG { get; set; }

        /// <summary>
        /// The eighth block of spell attributes. This is a set of flags, defining different behaviour for the spell
        /// under different circumstances. See <see cref="SpellAttributeH"/> for specifics.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public SpellAttributeH AttributesH { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint StanceWhitelistA { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public uint StanceWhitelistB { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint StanceBlacklistA { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public uint StanceBlacklistB { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint Targets { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint TargetCreatureType { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint SpellFocusObject { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint FacingCasterFlags { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint CasterAuraState { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint TargetAuraState { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint ExcludeCasterAuraState { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint ExcludeTargetAuraState { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public uint CasterAuraSpell { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public uint TargetAuraSpell { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public uint ExcludeCasterAuraSpell { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public uint ExcludeTargetAuraSpell { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint CastingTimeIndex { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint RecoveryTime { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint CategoryRecoveryTime { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint InterruptFlags { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint AuraInterruptFlags { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ChannelInterruptFlags { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ProcTypeMask { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ProcChance { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ProcCharges { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint MaxLevel { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint BaseLevel { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint SpellLevel { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint DurationIndex { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint PowerType { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ManaCost { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ManaCostPerLevel { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ManaCostPerSecond { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ManaCostPerSecondPerLevel { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint RangeIndex { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public float Speed { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ModalNextSpell { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint StackAmount { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 2)]
        public uint[] Totem { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 8)]
        public uint[] Reagent { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 8)]
        public uint[] ReagentCount { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint EquippedItemClass { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint EquippedItemSubclass { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint EquippedItemInvType { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] Effect { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectDieSides { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath, Count = 3)]
        public uint[] EffectBaseDice { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath, Count = 3)]
        public float[] EffectDicePerLevel { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public float[] EffectRealPointsPerLevel { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public float[] EffectBasePoints { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectMechanic { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] ImplicitTargetA { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] ImplicitTargetB { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectRadiusIndex { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectAura { get; set; }

        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public uint[] EffectAuraPeriod { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath, Count = 3)]
        public float[] EffectAmplitude { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public float[] EffectMultipleValue { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectChainTarget { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectItemType { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectMiscValue { get; set; }

        [RecordFieldArray(WarcraftVersion.BurningCrusade, Count = 3)]
        public uint[] EffectMiscValueB { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public uint[] EffectTriggerSpell { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public float[] EffectPointsPerCombo { get; set; }

        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public uint[] EffectClassMaskA { get; set; }

        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public uint[] EffectClassMaskB { get; set; }

        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public uint[] EffectClassMaskC { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 2)]
        public uint[] SpellVisualID { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint SpellIconID { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ActiveIconID { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint SpellPriority { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference Name { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference Subtext { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference Description { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference Tooltip { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint ManaCostPercentage { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint StartRecoveryCategory { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint StartRecoveryTime { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint MaxTargetLevel { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint SpellClassSet { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 2)]
        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public uint[] SpellClassMask { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint MaxTargets { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint DefenseType { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint PreventionType { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint StanceBarOrder { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 3)]
        public float[] DamageMultiplier { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint MinFactionID { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint MinReputation { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint RequiredAuraVision { get; set; }

        [RecordFieldArray(WarcraftVersion.BurningCrusade, Count = 2)]
        public uint[] RequiredTotemCategoryID { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint RequiredAreaID { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint SchoolMask { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public uint RuneCostID { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public uint SpellMissileID { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public int PowerDisplayID { get; set; }

        [RecordFieldArray(WarcraftVersion.Wrath, Count = 3)]
        public float[] EffectBonusMultiplier { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public uint DescriptionVariableID { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public uint DifficultyID { get; set; }
    }
}
