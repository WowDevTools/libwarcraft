using System;

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// The eighth block of spell attributes.
    /// </summary>
    [Flags]
    public enum SpellAttributeH : uint
    {
        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown98 = 0x00000001,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown99 = 0x00000002,

        /// <summary>
        /// This spell will reactive when the caster is resurrected.
        /// </summary>
        ReactivateOnResurrection = 0x00000004,

        /// <summary>
        /// This spell is a cheat spell.
        /// </summary>
        IsCheat = 0x00000008,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown100 = 0x00000010,

        /// <summary>
        /// This spell will summon a player-controlled totem.
        /// </summary>
        SummonPlayerTotem = 0x00000020,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown101 = 0x00000040,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown102 = 0x00000080,

        /// <summary>
        /// This spell is horde only.
        /// </summary>
        HordeOnly = 0x00000100,

        /// <summary>
        /// This spell is alliance only.
        /// </summary>
        AllianceOnly = 0x00000200,

        /// <summary>
        /// This spell will dispel a single charge.
        /// </summary>
        DispelCharge = 0x00000400,

        /// <summary>
        /// This spell can only interrupt NPCs.
        /// </summary>
        OnlyInterruptNonPlayer = 0x00000800,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown103 = 0x00001000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown104 = 0x00002000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown105 = 0x00004000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown106 = 0x00008000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown107 = 0x00010000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown108 = 0x00020000,

        /// <summary>
        /// This spell has a charge effect.
        /// </summary>
        HasChargeEffect = 0x00040000,

        /// <summary>
        /// This spell is a zone teleport.
        /// </summary>
        ZoneTeleport = 0x00080000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown109 = 0x00100000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown110 = 0x00200000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown111 = 0x00400000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown112 = 0x00800000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown113 = 0x01000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown114 = 0x02000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown115 = 0x04000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown116 = 0x08000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown117 = 0x10000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown118 = 0x20000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown119 = 0x40000000,

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        Unknown120 = 0x80000000,
    }
}