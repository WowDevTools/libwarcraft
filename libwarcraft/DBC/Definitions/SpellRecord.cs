//
//  SpellRecord.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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

	/// <summary>
	/// The first block of spell attributes.
	/// </summary>
	[Flags]
	public enum SpellAttributeA : uint
	{
		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown0					= 0x00000001,

		/// <summary>
		/// This spell requires some form of ammunition.
		/// </summary>
		RequiresAmmunition			= 0x00000002,

		/// <summary>
		/// This spell will trigger on the next swing.
		/// </summary>
		TriggersOnNextSwing			= 0x00000004,

		/// <summary>
		/// This spell is a form of replenishment.
		/// </summary>
		IsReplenishment				= 0x00000008,

		/// <summary>
		/// This spell is an ability.
		/// </summary>
		Ability						= 0x00000010,

		/// <summary>
		/// This spell is a tradeskill spell, that is, it is a recipe that creates something.
		/// </summary>
		TradeskillSpell				= 0x00000020,

		/// <summary>
		/// This spell is passive.
		/// </summary>
		Passive						= 0x00000040,

		/// <summary>
		/// This spell is hidden for the client, and should not be displayed in the UI.
		/// </summary>
		HiddenClientside			= 0x00000080,

		/// <summary>
		/// This spell is hidden in combat logs.
		/// </summary>
		HideFromCombatLogs			= 0x00000100,

		/// <summary>
		/// This spell always targets the item equipped in the main hand.
		/// </summary>
		AlwaysTargetMainHandItem	= 0x00000200,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell triggers on the next swing.
		/// </summary>
		TriggersOnNextSwing2		= 0x00000400,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown1					= 0x00000800,

		/// <summary>
		/// This spell can only be cast during daytime.
		/// </summary>
		DaytimeOnly					= 0x00001000,

		/// <summary>
		/// This spell can only be cast during nighttime.
		/// </summary>
		NighttimeOnly				= 0x00002000,

		/// <summary>
		/// This spell can only be cast indoors.
		/// </summary>
		IndoorsOnly					= 0x00004000,

		/// <summary>
		/// This spell can only be cast outdoors.
		/// </summary>
		OutdoorsOnly				= 0x00008000,

		/// <summary>
		/// This spell cannot be used while shapeshifted.
		/// </summary>
		NotUseableWhileShapeshifted = 0x00010000,

		/// <summary>
		/// This spell can only be cast while in stealth.
		/// </summary>
		MustBeInStealth				= 0x00020000,

		/// <summary>
		/// This spell does not affect the sheath state of the caster's weapons.
		/// </summary>
		DoNotAffectWeaponSheathState= 0x00040000,

		/// <summary>
		/// The damage of this spell scales by the caster's level.
		/// </summary>
		UseLevelScaledDamage		= 0x00080000,

		/// <summary>
		/// When cast, this spell causes the caster to stop attacking.
		/// </summary>
		StopAttackingWhenUsed		= 0x00100000,

		/// <summary>
		/// This spell cannot be dodged, blocked or parried.
		/// </summary>
		ImpossibleToDodgeBlockParry	= 0x00200000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		AlwaysFaceTarget			= 0x00400000,

		/// <summary>
		/// This spell can be cast while dead.
		/// </summary>
		CastableWhileDead			= 0x00800000,

		/// <summary>
		/// This spell can be cast while mounted.
		/// </summary>
		CastableWhileMounted		= 0x01000000,

		/// <summary>
		/// The cooldown of this spell will not start ticking down before the spell has finished.
		/// </summary>
		CooldownDisabledWhileActive	= 0x02000000,

		/// <summary>
		/// This spell is some form of detrimental effect.
		/// </summary>
		DebuffOrNegativeSpell		= 0x04000000,

		/// <summary>
		/// This spell can be cast while sitting.
		/// </summary>
		CastableWhileSitting		= 0x08000000,

		/// <summary>
		/// This spell cannot be used while in combat.
		/// </summary>
		CanNotUseInCombat			= 0x10000000,

		/// <summary>
		/// This spell bypasses invulnerability.
		/// </summary>
		UnaffectedByInvulnerability	= 0x20000000,

		/// <summary>
		/// This spell can be interrupted by damage.
		/// </summary>
		DamageCanInterrupt			= 0x40000000,

		/// <summary>
		/// This spell cannot be cancelled once cast.
		/// </summary>
		CanNotCancelOnceCast		= 0x80000000
	}

	/// <summary>
	/// The second block of spell attributes.
	/// </summary>
	[Flags]
	public enum SpellAttributeB : uint
	{
		/// <summary>
		/// This spell will dismiss the current pet.
		/// </summary>
		DismissPet					= 0x00000001,

		/// <summary>
		/// This spell will drain all available power.
		/// </summary>
		DrainAllPower				= 0x00000002,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell is channeled.
		/// </summary>
		Channeled1					= 0x00000004,

		/// <summary>
		/// This spell cannot be redirected.
		/// </summary>
		CanNotBeRedirected 			= 0x00000008,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown2					= 0x00000010,

		/// <summary>
		/// This spell does not break stealth.
		/// </summary>
		DoNotBreakStealth			= 0x00000020,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell is channeled.
		/// </summary>
		Channeled2					= 0x00000040,

		/// <summary>
		/// This spell cannot be reflected.
		/// </summary>
		CanNotBeReflected			= 0x00000080,

		/// <summary>
		/// This spell cannot target a unit in combat.
		/// </summary>
		CanNotTargetUnitInCombat	= 0x00000100,

		/// <summary>
		/// This spell will start melee combat after it has been cast.
		/// </summary>
		StartMeleeCombatAfterCast	= 0x00000200,

		/// <summary>
		/// This spell does not generate threat.
		/// </summary>
		DoesNotGenerateThreat		= 0x00000400,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown3					= 0x00000800,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell is a pickpocketing spell.
		/// </summary>
		IsPickpocket				= 0x00001000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell is a farsight spell.
		/// </summary>
		Farsight					= 0x00002000,

		/// <summary>
		/// This spell is channeled, and the caster must always face the target
		/// </summary>
		ChanneledMustFaceTarget		= 0x00004000,

		/// <summary>
		/// This spell will dispel any auras on the target.
		/// </summary>
		DispelAuras					= 0x00008000,

		/// <summary>
		/// This spell is unaffected by immunity to its school.
		/// </summary>
		UnaffectedBySchoolImmune	= 0x00010000,

		/// <summary>
		/// Pets cannot autocast the spell.
		/// </summary>
		PetCannotAutocast			= 0x00020000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown4					= 0x00040000,

		/// <summary>
		/// The caster cannot target themselves with this spell.
		/// </summary>
		CanNotTargetSelf			= 0x00080000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell requires combo points.
		/// </summary>
		RequiresComboPoints1		= 0x00100000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown5					= 0x00200000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell requires combo points.
		/// </summary>
		RequiresComboPoints2		= 0x00400000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown6					= 0x00800000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell is a fishing spell.
		/// </summary>
		IsFishing					= 0x01000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown7					= 0x02000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown8					= 0x04000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknonw9					= 0x08000000,

		/// <summary>
		/// This spell should not be displayed in the aura bar.
		/// </summary>
		DoNotDisplayInAuraBar		= 0x10000000,

		/// <summary>
		/// The name of the spell should be displayed in the casting bar.
		/// </summary>
		DisplaySpellNameInCastBar	= 0x20000000,

		/// <summary>
		/// This spell is enabled after dodging.
		/// </summary>
		EnableAfterDodge			= 0x40000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown10					= 0x80000000,
	}

	/// <summary>
	/// The third block of spell attributes.
	/// </summary>
	[Flags]
	public enum SpellAttributeC : uint
	{
		/// <summary>
		/// This spell can target dead units.
		/// </summary>
		CanTargetDead				= 0x00000001,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown11					= 0x00000002,

		/// <summary>
		/// This spell can target units that are not in line of sight.
		/// </summary>
		CanTargetTargetNotInSight	= 0x00000004,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown12					= 0x00000008,

		/// <summary>
		/// This spell should be displayed in the stance bar.
		/// </summary>
		DisplayInStanceBar			= 0x00000010,

		/// <summary>
		/// This spell repeats automatically.
		/// </summary>
		RepeatAutomatically			= 0x00000020,

		/// <summary>
		/// This spell can only target tapped units.
		/// </summary>
		CanOnlyTargetTapped			= 0x00000040,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown13					= 0x00000080,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown14					= 0x00000100,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown15					= 0x00000200,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown16					= 0x00000400,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell is a health funnel.
		/// </summary>
		HealthFunnel				= 0x00000800,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown17					= 0x00001000,

		/// <summary>
		/// This spell is an enchantment, and it is preserved in arenas.
		/// </summary>
		PreserveEnchantInArenas		= 0x00002000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown18					= 0x00004000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown19					= 0x00008000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell tames a beast.
		/// </summary>
		IsTameBeast					= 0x00010000,

		/// <summary>
		/// The cooldown timer is not reset for automatic attacks from this spell.
		/// </summary>
		DoNotResetTimersForAutos	= 0x00020000,

		/// <summary>
		/// The caster's pet must be dead to cast this spell.
		/// </summary>
		PetMustBeDead				= 0x00040000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell does not need the caster to be shapeshifted.
		/// </summary>
		DoesNotNeedShapeshift		= 0x00080000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown20					= 0x00100000,

		/// <summary>
		/// This spell reduces damage.
		/// </summary>
		ReducesDamage				= 0x00200000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown21					= 0x00400000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell is arcane concentration.
		/// </summary>
		IsArcaneConcentration		= 0x00800000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown22					= 0x01000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown23					= 0x02000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown24					= 0x04000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown25					= 0x08000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown26					= 0x10000000,

		/// <summary>
		/// This spell cannot score a critical hit.
		/// </summary>
		CannotCrit					= 0x20000000,

		/// <summary>
		/// This spell can trigger multiple times.
		/// </summary>
		CanTriggerMultipleTimes		= 0x40000000,

		/// <summary>
		/// This spell is a food buff.
		/// </summary>
		IsFoodBuff					= 0x80000000,
	}

	/// <summary>
	/// The fourth block of spell attributes.
	/// </summary>
	[Flags]
	public enum SpellAttributeD : uint
	{
		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown27					= 0x00000001,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown28					= 0x00000002,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown29					= 0x00000004,

		/// <summary>
		/// This spell can be blocked.
		/// </summary>
		CanBeBlocked				= 0x00000008,

		/// <summary>
		/// This spell ignores the resurrection timer.
		/// </summary>
		IgnoreResurrectionTimer		= 0x00000010,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown30					= 0x00000020,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown31					= 0x00000040,

		/// <summary>
		/// This spell tracks a separate stack for each caster.
		/// </summary>
		SeparateStackForEachCaster	= 0x00000080,

		/// <summary>
		/// This spell can only target players.
		/// </summary>
		CanOnlyTargetPlayers		= 0x00000100,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell can proc from an effect.
		/// </summary>
		ProcTriggerFromEffect2		= 0x00000200,

		/// <summary>
		/// This spell requires a main hand weapon.
		/// </summary>
		RequiresMainHandWeapon		= 0x00000400,

		/// <summary>
		/// To cast this spell, the caster must be in a battleground.
		/// </summary>
		MustBeInBattleground		= 0x00000800,

		/// <summary>
		/// This spell can only target ghosts.
		/// </summary>
		CanOnlyTargetGhost			= 0x00001000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown32					= 0x00002000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// A unit under the influence of this spell is an honorless target.
		/// </summary>
		IsHonorlessTarget			= 0x00004000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown33					= 0x00008000,

		/// <summary>
		/// This spell cannot trigger procs.
		/// </summary>
		CanNotTriggerProcs			= 0x00010000,

		/// <summary>
		/// This spell causes no initial aggro.
		/// </summary>
		NoInitialAggro				= 0x00020000,

		/// <summary>
		/// This spell ignores the hit result.
		/// </summary>
		IgnoreHitResult				= 0x00040000,

		/// <summary>
		/// This spell prevents all procs for its full duration.
		/// </summary>
		DisableProcsForDuration		= 0x00080000,

		/// <summary>
		/// This spell persists through death.
		/// </summary>
		PersistsThroughDeath		= 0x00100000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown34					= 0x00200000,

		/// <summary>
		/// This spell requires a wand.
		/// </summary>
		RequiresWand				= 0x00400000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown35					= 0x00800000,

		/// <summary>
		/// This spell requires an offhand weapon.
		/// </summary>
		RequiresOffhandWeapon		= 0x01000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown36					= 0x02000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell can trigger as a proc from another proc.
		/// </summary>
		ProcTriggerFromProcTriggerEffect2	= 0x04000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell is drain soul.
		/// </summary>
		IsDrainSoul					= 0x08000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell is death grip.
		/// </summary>
		IsDeathGrip					= 0x10000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell has no modifiers.
		/// </summary>
		NoSpellModifiers			= 0x20000000,

		/// <summary>
		/// The spell range of this spell should not be displayed.
		/// </summary>
		DoNotDisplaySpellRange		= 0x40000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown37					= 0x80000000,
	}

	/// <summary>
	/// The fifth block of spell attributes.
	/// </summary>
	[Flags]
	public enum SpellAttributeE : uint
	{
		/// <summary>
		/// This spell ignores resistances against its school.
		/// </summary>
		IgnoreResistances			= 0x00000001,

		/// <summary>
		/// This spell proc only triggers on the caster.
		/// </summary>
		ProcTriggerOnlyOnCaster		= 0x00000002,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown38					= 0x00000004,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown39					= 0x00000008,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown40					= 0x00000010,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown41					= 0x00000020,

		/// <summary>
		/// This spell cannot be stolen.
		/// </summary>
		CanNotBeStolen				= 0x00000040,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell is triggered by force.
		/// </summary>
		ForceTriggered				= 0x00000080,

		/// <summary>
		/// This spell bypasses armor.
		/// </summary>
		BypassArmor					= 0x00000100,

		/// <summary>
		/// This spell is initially disabled.
		/// </summary>
		InitiallyDisabled			= 0x00000200,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		ExtendCost					= 0x00000400,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown42					= 0x00000800,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown43					= 0x00001000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown44					= 0x00002000,

		/// <summary>
		/// Damage from this spell does not break auras.
		/// </summary>
		DamageDoesNotBreakAuras		= 0x00004000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown45					= 0x00008000,

		/// <summary>
		/// This spell cannot be used in arenas.
		/// </summary>
		NotUsableInArenas			= 0x00010000,

		/// <summary>
		/// This spell is usable in arenas.
		/// </summary>
		UsableInArenas				= 0x00020000,

		/// <summary>
		/// This spell will chain between targets.
		/// </summary>
		AffectTargetsAsChain		= 0x00040000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown46					= 0x00080000,

		/// <summary>
		/// When applied, this spell will not check for a more powerful version of itself.
		/// </summary>
		DoNotCheckForMorePowerful	= 0x00100000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown47					= 0x00200000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown48					= 0x00400000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown49					= 0x00800000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown50					= 0x01000000,

		/// <summary>
		/// This spell scales for the caster's pet.
		/// </summary>
		IsPetScaling				= 0x02000000,

		/// <summary>
		/// This spell can only be used in Outland.
		/// </summary>
		CanOnlyBeUsedInOutland		= 0x04000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown51					= 0x08000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown52					= 0x10000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown53					= 0x20000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown54					= 0x40000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown55					= 0x80000000,
	}

	/// <summary>
	/// The sixth block of spell attributes.
	/// </summary>
	[Flags]
	public enum SpellAttributeF : uint
	{
		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown56					= 0x00000001,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell requires no reagents while preparing it.
		/// </summary>
		NoReagentsWhilePreparing	= 0x00000002,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown57					= 0x00000004,

		/// <summary>
		/// This spell can be used while stunned.
		/// </summary>
		UsableWhileStunned			= 0x00000008,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown58					= 0x00000010,

		/// <summary>
		/// This spell can only ever be applied to a single target.
		/// </summary>
		SingleTargetOnly			= 0x00000020,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown59					= 0x00000040,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown60					= 0x00000080,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown61					= 0x00000100,

		/// <summary>
		/// TODO: Unknown behaviour
		/// This spell starts a periodic tick when it is applied.
		/// </summary>
		StartPeriodicTickWhenApplied= 0x00000200,

		/// <summary>
		/// The duration of this spell is hidden for the client.
		/// </summary>
		HideDurationForClient		= 0x00000400,

		/// <summary>
		/// The target of the target is allowed as a target for the spell.
		/// </summary>
		AllowTargetOfTargetAsTarget	= 0x00000800,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown62					= 0x00001000,

		/// <summary>
		/// The caster's haste affects the duration of this spell.
		/// </summary>
		HasteAffectsDuration		= 0x00002000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown63					= 0x00004000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown64					= 0x00008000,

		/// <summary>
		/// This spell will check for a special class item before casting.
		/// </summary>
		CheckForSpecialClassItem	= 0x00010000,

		/// <summary>
		/// This spell can be used while feared.
		/// </summary>
		UsableWhileFeared			= 0x00020000,

		/// <summary>
		/// This spell can be used while confused.
		/// </summary>
		UsableWhileConfused			= 0x00040000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown65					= 0x00080000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown66					= 0x00100000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown67					= 0x00200000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown68					= 0x00400000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown69					= 0x00800000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown70					= 0x01000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown71					= 0x02000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown72					= 0x04000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown73					= 0x08000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown74					= 0x10000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown75					= 0x20000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown76					= 0x40000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown77					= 0x80000000,
	}

	/// <summary>
	/// The seventh block of spell attributes.
	/// </summary>
	[Flags]
	public enum SpellAttributeG : uint
	{
		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		DoNotDisplayCooldown		= 0x00000001,

		/// <summary>
		/// This spell can only be cast in arenas.
		/// </summary>
		OnlyInArena					= 0x00000002,

		/// <summary>
		/// This spell ignores any auras on the caster.
		/// </summary>
		IgnoreCasterAuras			= 0x00000004,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown78					= 0x00000008,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown79					= 0x00000010,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown80					= 0x00000020,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown81					= 0x00000040,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown82					= 0x00000080,

		/// <summary>
		/// This spell cannot target crowd controlled units.
		/// </summary>
		CannotTargetCrowdControlled = 0x00000100,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown83					= 0x00000200,

		/// <summary>
		/// This spell can target possessed friendlies.
		/// </summary>
		CanTargetPossessedFriend	= 0x00000400,

		/// <summary>
		/// This spell cannot be cast in raids.
		/// </summary>
		NotInRaidInstance			= 0x00000800,

		/// <summary>
		/// This spell can be cast while on a vehicle.
		/// </summary>
		CastableWhileOnVehicle		= 0x00001000,

		/// <summary>
		/// This spell can target invisible units.
		/// </summary>
		CanTargetInvisible			= 0x00002000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown84					= 0x00004000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown85					= 0x00008000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown86					= 0x00010000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown87					= 0x00020000,

		/// <summary>
		/// This spell will not be allowed if the unit is not possessed, and the charmer of the caster will be the original caster.
		/// </summary>
		CastByCharmer				= 0x00040000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown88					= 0x00080000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown89					= 0x00100000,

		/// <summary>
		/// A client-side attribute.
		/// </summary>
		ClientUITargetEffects		= 0x00200000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown90					= 0x00400000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown91					= 0x00800000,

		/// <summary>
		/// This spell can target otherwise untargetable units.
		/// </summary>
		CanTargetUntargetable		= 0x01000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown92					= 0x02000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown93					= 0x04000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown94					= 0x08000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknow95					= 0x10000000,

		/// <summary>
		/// This spell ignores percentile damage modifiers.
		/// </summary>
		IgnorePercentDamageMods		= 0x20000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown96					= 0x40000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown97					= 0x80000000,
	}

	/// <summary>
	/// The eighth block of spell attributes.
	/// </summary>
	[Flags]
	public enum SpellAttributeH : uint
	{
		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown98					= 0x00000001,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown99					= 0x00000002,

		/// <summary>
		/// This spell will reactive when the caster is resurrected.
		/// </summary>
		ReactivateOnResurrection	= 0x00000004,

		/// <summary>
		/// This spell is a cheat spell.
		/// </summary>
		IsCheat						= 0x00000008,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown100					= 0x00000010,

		/// <summary>
		/// This spell will summon a player-controlled totem.
		/// </summary>
		SummonPlayerTotem			= 0x00000020,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown101					= 0x00000040,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown102					= 0x00000080,

		/// <summary>
		/// This spell is horde only.
		/// </summary>
		HordeOnly					= 0x00000100,

		/// <summary>
		/// This spell is alliance only.
		/// </summary>
		AllianceOnly				= 0x00000200,

		/// <summary>
		/// This spell will dispel a single charge.
		/// </summary>
		DispelCharge				= 0x00000400,

		/// <summary>
		/// This spell can only interrupt NPCs.
		/// </summary>
		OnlyInterruptNonPlayer		= 0x00000800,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown103					= 0x00001000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown104					= 0x00002000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown105					= 0x00004000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown106					= 0x00008000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown107					= 0x00010000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown108					= 0x00020000,

		/// <summary>
		/// This spell has a charge effect.
		/// </summary>
		HasChargeEffect				= 0x00040000,

		/// <summary>
		/// This spell is a zone teleport.
		/// </summary>
		ZoneTeleport				= 0x00080000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown109					= 0x00100000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown110					= 0x00200000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown111					= 0x00400000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown112					= 0x00800000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown113					= 0x01000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown114					= 0x02000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown115					= 0x04000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown116					= 0x08000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown117					= 0x10000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown118					= 0x20000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown119					= 0x40000000,

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		Unknown120					= 0x80000000,
	}
}