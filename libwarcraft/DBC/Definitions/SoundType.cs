//
//  SoundType.cs
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

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// Defines the various types a sound can be.
    /// </summary>
    public enum SoundType : uint
    {
        /// <summary>
        /// An unused sound.
        /// </summary>
        Unused = 0,

        /// <summary>
        /// A spell-related sound.
        /// </summary>
        Spell = 1,

        /// <summary>
        /// A UI sound.
        /// </summary>
        UI = 2,

        /// <summary>
        /// A footstep sound.
        /// </summary>
        Footstep1 = 3,

        /// <summary>
        /// The sound of a weapon hitting its target.
        /// </summary>
        WeaponImpact = 4,

        /// <summary>
        /// An unknown sound type.
        /// TODO: Figure out what this is.
        /// </summary>
        Unknown1 = 5,

        /// <summary>
        /// The sound of a weapon missing.
        /// </summary>
        WeaponMiss = 6,

        /// <summary>
        /// Some type of vocal greeting.
        /// </summary>
        Greeting = 7,

        /// <summary>
        /// The sound of a spell being cast.
        /// </summary>
        Cast = 8,

        /// <summary>
        /// The sound of an item being picked up or dropped.
        /// </summary>
        PickUpDown = 9,

        /// <summary>
        /// Some form of sound an NPC makes in combat.
        /// </summary>
        NPCCombat = 10,

        /// <summary>
        /// An unknown sound type.
        /// TODO: Figure out what this is.
        /// </summary>
        Unknown2 = 11,

        /// <summary>
        /// A sound indicating an error of some form.
        /// </summary>
        Error = 12,

        /// <summary>
        /// A bird-related sound.
        /// </summary>
        Bird = 13,

        /// <summary>
        /// An object-related sound.
        /// </summary>
        Object = 14,

        /// <summary>
        /// An unknown sound type.
        /// TODO: Figure out what this is.
        /// </summary>
        Unknown3 = 15,

        /// <summary>
        /// The sound of something dying.
        /// </summary>
        Death = 16,

        /// <summary>
        /// Some form of NPC greeting.
        /// </summary>
        NPCGreeting = 17,

        /// <summary>
        /// A test sound.
        /// </summary>
        Test = 18,

        /// <summary>
        /// Some type of armor-related sound.
        /// </summary>
        Armor = 19,

        /// <summary>
        /// The sound of a footstep.
        /// </summary>
        Footstep2 = 20,

        /// <summary>
        /// The sound of a character underwater.
        /// </summary>
        CharacterWater = 21,

        /// <summary>
        /// The sound of water.
        /// </summary>
        LiquidWater = 22,

        /// <summary>
        /// A tradeskill-related sound.
        /// </summary>
        Tradeskill = 23,

        /// <summary>
        /// An unknown sound type.
        /// TOOD: Figure out what this is.
        /// </summary>
        Unknown4 = 24,

        /// <summary>
        /// A doodad-related sound.
        /// </summary>
        Doodad = 25,

        /// <summary>
        /// The sound of a spell fizzling out.
        /// </summary>
        SpellFizzle = 26,

        /// <summary>
        /// Some form of looping NPC sound.
        /// </summary>
        NPCLoop = 27,

        /// <summary>
        /// A music track.
        /// </summary>
        ZoneMusic = 28,

        /// <summary>
        /// An emote-associated sound.
        /// </summary>
        Emote = 29,

        /// <summary>
        /// A music track used during narration.
        /// </summary>
        NarrationMusic = 30,

        /// <summary>
        /// A narration track.
        /// </summary>
        Narration = 31,

        // .. //

        /// <summary>
        /// Ambiance in a zone.
        /// </summary>
        ZoneAmbiance = 50
    }
}
