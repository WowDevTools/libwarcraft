namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// Weapon animation flags.
    /// </summary>
    public enum WeaponAnimationFlags : uint
    {
        /// <summary>
        /// Ignores the current state of the character's weapons.
        /// </summary>
        None = 0,

        /// <summary>
        /// Sheathes the weapons for the duration of the animation.
        /// </summary>
        Sheathe = 4,

        /// <summary>
        /// Sheathes the weapons for the duration of the animation.
        /// </summary>
        Sheathe2 = 16,

        /// <summary>
        /// Unsheathes the weapons for the duration of the animation.
        /// </summary>
        Unsheathe = 32
    }
}