using System;

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// The fourth block of spell attributes.
    /// </summary>
    [Flags]
    public enum SpellAttributeD : uint
    {
        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown27 = 0x00000001,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown28 = 0x00000002,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown29 = 0x00000004,

        /// <summary>
        /// This spell can be blocked.
        /// </summary>
        CanBeBlocked = 0x00000008,

        /// <summary>
        /// This spell ignores the resurrection timer.
        /// </summary>
        IgnoreResurrectionTimer = 0x00000010,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown30 = 0x00000020,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown31 = 0x00000040,

        /// <summary>
        /// This spell tracks a separate stack for each caster.
        /// </summary>
        SeparateStackForEachCaster = 0x00000080,

        /// <summary>
        /// This spell can only target players.
        /// </summary>
        CanOnlyTargetPlayers = 0x00000100,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell can proc from an effect.
        /// </summary>
        ProcTriggerFromEffect2 = 0x00000200,

        /// <summary>
        /// This spell requires a main hand weapon.
        /// </summary>
        RequiresMainHandWeapon = 0x00000400,

        /// <summary>
        /// To cast this spell, the caster must be in a battleground.
        /// </summary>
        MustBeInBattleground = 0x00000800,

        /// <summary>
        /// This spell can only target ghosts.
        /// </summary>
        CanOnlyTargetGhost = 0x00001000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown32 = 0x00002000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// A unit under the influence of this spell is an honorless target.
        /// </summary>
        IsHonorlessTarget = 0x00004000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown33 = 0x00008000,

        /// <summary>
        /// This spell cannot trigger procs.
        /// </summary>
        CanNotTriggerProcs = 0x00010000,

        /// <summary>
        /// This spell causes no initial aggro.
        /// </summary>
        NoInitialAggro = 0x00020000,

        /// <summary>
        /// This spell ignores the hit result.
        /// </summary>
        IgnoreHitResult = 0x00040000,

        /// <summary>
        /// This spell prevents all procs for its full duration.
        /// </summary>
        DisableProcsForDuration = 0x00080000,

        /// <summary>
        /// This spell persists through death.
        /// </summary>
        PersistsThroughDeath = 0x00100000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown34 = 0x00200000,

        /// <summary>
        /// This spell requires a wand.
        /// </summary>
        RequiresWand = 0x00400000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown35 = 0x00800000,

        /// <summary>
        /// This spell requires an offhand weapon.
        /// </summary>
        RequiresOffhandWeapon = 0x01000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown36 = 0x02000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell can trigger as a proc from another proc.
        /// </summary>
        ProcTriggerFromProcTriggerEffect2 = 0x04000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell is drain soul.
        /// </summary>
        IsDrainSoul = 0x08000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell is death grip.
        /// </summary>
        IsDeathGrip = 0x10000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// This spell has no modifiers.
        /// </summary>
        NoSpellModifiers = 0x20000000,

        /// <summary>
        /// The spell range of this spell should not be displayed.
        /// </summary>
        DoNotDisplaySpellRange = 0x40000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown37 = 0x80000000,
    }
}