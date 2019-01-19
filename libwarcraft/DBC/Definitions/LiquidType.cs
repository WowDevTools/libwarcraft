//
//  LiquidType.cs
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
    /// Holds the various types of liquid.
    /// </summary>
    public enum LiquidType
    {
        /// <summary>
        /// Water. Originally, the value was 3. It is now 0.
        /// </summary>
        Water = 0,

        /// <summary>
        /// Ocean water. Originally, the value was 3. It is now 1.
        /// </summary>
        Ocean = 1,

        /// <summary>
        /// Magma. Originally, the value was 0. It is now 2.
        /// </summary>
        Magma = 2,

        /// <summary>
        /// Slime. Originally, the value was 2. It is now 3.
        /// </summary>
        Slime = 3
    }
}
