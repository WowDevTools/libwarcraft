//
//  AnimationDataRecord.cs
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
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class SpellRecord : DBCRecord
	{
		public const string RecordName = "Spell";

		public UInt32ForeignKey School;
		public UInt32ForeignKey Category;
		public uint CastUI;
		public UInt32ForeignKey DispelType;
		public UInt32ForeignKey Mechanic;
		public SpellAttributeA AttributesA;
		public SpellAttributeB AttributesB;
		public SpellAttributeC AttributesC;
		public SpellAttributeD AttributesD;
		public SpellAttributeE AttributesE;
		public SpellAttributeF AttributesF;
		public SpellAttributeG AttributesG;


		public override void LoadData(byte[] data)
		{
			throw new NotImplementedException();
		}

		public override int GetFieldCount()
		{
			throw new NotImplementedException();
		}

		public override int GetRecordSize()
		{
			throw new NotImplementedException();
		}
	}

	[Flags]
	public enum SpellAttributeA : uint
	{
		Unknown0					= 0x00000001,
		RequiresAmmunition			= 0x00000002,
		TriggersOnNextSwing			= 0x00000004,
		IsReplenishment				= 0x00000008,
		Ability						= 0x00000010,
		TradeskillSpell				= 0x00000020,
		Passive						= 0x00000040,
		HiddenClientside			= 0x00000080,
		HideFromCombatLogs			= 0x00000100,
		AlwaysTargetMainHandItem	= 0x00000200,
		TriggersOnNextSwing2		= 0x00000400,
		Unknown1					= 0x00000800,
		DaytimeOnly					= 0x00001000,
		NighttimeOnly				= 0x00002000,
		IndoorsOnly					= 0x00004000,
		OutdoorsOnly				= 0x00008000,
		NotUseableWhileShapeshifted = 0x00010000,
		MustBeInStealth				= 0x00020000,
		DoNotAffectWeaponSheathState= 0x00040000,
		UseLevelScaledDamage		= 0x00080000,
		StopAttackingWhenUsed		= 0x00100000,
		ImpossibleToDodgeBlockParry	= 0x00200000,
		AlwaysFaceTarget			= 0x00400000,
		CastableWhileDead			= 0x00800000,
		CastableWhileMounted		= 0x01000000,
		CooldownDisabledWhileActive	= 0x02000000,
		DebuffOrNegativeSpell		= 0x04000000,
		CastableWhileSitting		= 0x08000000,
		CanNotUseInCombat			= 0x10000000,
		UnaffectedByInvulnerability	= 0x20000000,
		DamageCanInterrupt			= 0x40000000,
		CanNotCancelOnceCast		= 0x80000000
	}

	[Flags]
	public enum SpellAttributeB : uint
	{
		DismissPet					= 0x00000001,
		DrainAllPower				= 0x00000002,
		Channeled1					= 0x00000004,
		CanNotBeRedirected 			= 0x00000008,
		Unknown2					= 0x00000010,
		DoNotBreakStealth			= 0x00000020,
		Channeled2					= 0x00000040,
		CanNotBeReflected			= 0x00000080,
		CanNotTargetUnitInCombat	= 0x00000100,
		StartMeleeCombatAfterCast	= 0x00000200,
		DoesNotGenerateThreat		= 0x00000400,
		Unknown3					= 0x00000800,
		IsPickpocket				= 0x00001000,
		Farsight					= 0x00002000,
		ChanneledMustFaceTarget		= 0x00004000,
		DispelAuras					= 0x00008000,
		UnaffectedBySchoolImmune	= 0x00010000,
		PetCannotAutocast			= 0x00020000,
		Unknown4					= 0x00040000,
		CanNotTargetSelf			= 0x00080000,
		RequiresComboPoints1		= 0x00100000,
		Unknown5					= 0x00200000,
		RequiresComboPoints2		= 0x00400000,
		Unknown6					= 0x00800000,
		IsFishing					= 0x01000000,
		Unknown7					= 0x02000000,
		Unknown8					= 0x04000000,
		Unknonw9					= 0x08000000,
		DoNotDisplayInAuraBar		= 0x10000000,
		DisplaySpellNameInCastBar	= 0x20000000,
		EnableAfterDodge			= 0x40000000,
		Unknown10					= 0x80000000,
	}


	[Flags]
	public enum SpellAttributeC : uint
	{
		CanTargetDead				= 0x00000001,
		Unknown11					= 0x00000002,
		CanTargetTargetNotInSight	= 0x00000004,
		Unknown12					= 0x00000008,
		DisplayInStanceBar			= 0x00000010,
		RepeatAutomatically			= 0x00000020,
		CanOnlyTargetTapped			= 0x00000040,
		Unknown13					= 0x00000080,
		Unknown14					= 0x00000100,
		Unknown15					= 0x00000200,
		Unknown16					= 0x00000400,
		HealthFunnel				= 0x00000800,
		Unknown17					= 0x00001000,
		PreserveEnchantInArenas		= 0x00002000,
		Unknown18					= 0x00004000,
		Unknown19					= 0x00008000,
		IsTameBeast					= 0x00010000,
		DoNotResetTimersForAutos	= 0x00020000,
		PetMustBeDead				= 0x00040000,
		DoesNotNeedShapeshift		= 0x00080000,
		Unknown20					= 0x00100000,
		ReducesDamage				= 0x00200000,
		Unknown21					= 0x00400000,
		IsArcaneConcentration		= 0x00800000,
		Unknown22					= 0x01000000,
		Unknown23					= 0x02000000,
		Unknown24					= 0x04000000,
		Unknown25					= 0x08000000,
		Unknown26					= 0x10000000,
		CannotCrit					= 0x20000000,
		CanTriggerMultipleTimes		= 0x40000000,
		IsFoodBuff					= 0x80000000,
	}

	[Flags]
	public enum SpellAttributeD : uint
	{
		Unknown27					= 0x00000001,
		Unknown28					= 0x00000002,
		Unknown29					= 0x00000004,
		CanBeBlocked				= 0x00000008,
		IgnoreResurrectionTimer		= 0x00000010,
		Unknown30					= 0x00000020,
		Unknown31					= 0x00000040,
		SeparateStackForEachCaster	= 0x00000080,
		CanOnlyTargetPlayers		= 0x00000100,
		ProcTriggerFromEffect2	= 0x00000200,
		RequiresMainHandWeapon		= 0x00000400,
		MustBeInBattleground		= 0x00000800,
		CanOnlyTargetGhost			= 0x00001000,
		Unknown32					= 0x00002000,
		IsHonorlessTarget			= 0x00004000,
		Unknown33					= 0x00008000,
		CanNotTriggerProcs			= 0x00010000,
		NoInitialAggro				= 0x00020000,
		IgnoreHitResult				= 0x00040000,
		DisableProcsForDuration		= 0x00080000,
		PersistsThroughDeath		= 0x00100000,
		Unknown34					= 0x00200000,
		RequiresWand				= 0x00400000,
		Unknown35					= 0x00800000,
		RequiresOffhandWeapon		= 0x01000000,
		Unknown36					= 0x02000000,
		ProcTriggerFromProcTriggerEffect2	= 0x04000000,
		IsDrainSoul					= 0x08000000,
		IsDeathGrip					= 0x10000000,
		NoSpellModifiers			= 0x20000000,
		DoNotDisplaySpellRange		= 0x40000000,
		Unknown37					= 0x80000000,
	}

	[Flags]
	public enum SpellAttributeE : uint
	{
		IgnoreResistances			= 0x00000001,
		ProcTriggerOnlyOnCaster		= 0x00000002,
		Unknown38					= 0x00000004,
		Unknown39					= 0x00000008,
		Unknown40					= 0x00000010,
		Unknown41					= 0x00000020,
		CanNotBeStolen				= 0x00000040,
		ForceTriggered				= 0x00000080,
		BypassArmor					= 0x00000100,
		InitiallyDisabled			= 0x00000200,
		ExtendCost					= 0x00000400,
		Unknown42					= 0x00000800,
		Unknown43					= 0x00001000,
		Unknown44					= 0x00002000,
		DamageDoesNotBreakAuras		= 0x00004000,
		Unknown45					= 0x00008000,
		NotUsableInArenas			= 0x00010000,
		UsableInArenas				= 0x00020000,
		AffectTargetsAsChain		= 0x00040000,
		Unknown46					= 0x00080000,
		DoNotCheckForMorePowerful	= 0x00100000,
		Unknown47					= 0x00200000,
		Unknown48					= 0x00400000,
		Unknown49					= 0x00800000,
		Unknown50					= 0x01000000,
		IsPetScaling				= 0x02000000,
		CanOnlyBeUsedInOutland		= 0x04000000,
		Unknown51					= 0x08000000,
		Unknown52					= 0x10000000,
		Unknown53					= 0x20000000,
		Unknown54					= 0x40000000,
		Unknown55					= 0x80000000,
	}

	[Flags]
	public enum SpellAttributeF : uint
	{
		Unknown56					= 0x00000001,
		NoReagentsWhilePreparing	= 0x00000002,
		Unknown57					= 0x00000004,
		UsableWhileStunned			= 0x00000008,
		Unknown58					= 0x00000010,
		SingleTargetOnly			= 0x00000020,
		Unknown59					= 0x00000040,
		Unknown60					= 0x00000080,
		Unknown61					= 0x00000100,
		StartPeriodicTickWhenApplied= 0x00000200,
		HideDurationForClient		= 0x00000400,
		AllowTargetOfTargetAsTarget	= 0x00000800,
		Unknown62					= 0x00001000,
		HasteAffectsDuration		= 0x00002000,
		Unknown63					= 0x00004000,
		Unknown64					= 0x00008000,
		CheckForSpecialClassItem	= 0x00010000,
		UsableWhileFeared			= 0x00020000,
		UsableWhileConfused			= 0x00040000,
		Unknown65					= 0x00080000,
		Unknown66					= 0x00100000,
		Unknown67					= 0x00200000,
		Unknown68					= 0x00400000,
		Unknown69					= 0x00800000,
		Unknown70					= 0x01000000,
		Unknown71					= 0x02000000,
		Unknown72					= 0x04000000,
		Unknown73					= 0x08000000,
		Unknown74					= 0x10000000,
		Unknown75					= 0x20000000,
		Unknown76					= 0x40000000,
		Unknown77					= 0x80000000,
	}

	[Flags]
	public enum SpellAttributeG : uint
	{
		Unknown78					= 0x00000001,
		Unknown79					= 0x00000002,
		ReactivateOnResurrection	= 0x00000004,
		IsCheat						= 0x00000008,
		Unknown80					= 0x00000010,
		SummonPlayerTotem			= 0x00000020,
		Unknown81					= 0x00000040,
		Unknown82					= 0x00000080,
		HordeOnly					= 0x00000100,
		AllianceOnly				= 0x00000200,
		DispelCharge				= 0x00000400,
		OnlyInterruptNonPlayer		= 0x00000800,
		Unknown83					= 0x00001000,
		Unknown84					= 0x00002000,
		Unknown85					= 0x00004000,
		Unknown86					= 0x00008000,
		Unknown87					= 0x00010000,
		Unknown88					= 0x00020000,
		HasChargeEffect				= 0x00040000,
		ZoneTeleport				= 0x00080000,
		Unknown89					= 0x00100000,
		Unknown90					= 0x00200000,
		Unknown91					= 0x00400000,
		Unknown92					= 0x00800000,
		Unknown93					= 0x01000000,
		Unknown94					= 0x02000000,
		Unknown95					= 0x04000000,
		Unknown96					= 0x08000000,
		Unknown97					= 0x10000000,
		Unknown98					= 0x20000000,
		Unknown99					= 0x40000000,
		Unknown100					= 0x80000000,
	}
}