//
//  CharSectionAvailability.cs
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
    /// Defines flag values for the section.
    /// </summary>
    [Flags]
    public enum CharSectionAvailability
    {
        /// <summary>
        /// Available during character creation.
        /// </summary>
        CharacterCreate = 0x1,

        /// <summary>
        /// Available in barber shops.
        /// </summary>
        BarberShop = 0x2,

        /// <summary>
        /// Available for death knights.
        /// </summary>
        DeathKnight = 0x4,

        /// <summary>
        /// Available for NPCs.
        /// </summary>
        NPC = 0x8,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0x10
    }
}
