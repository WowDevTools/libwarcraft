//
//  InterpolationType.cs
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

namespace Warcraft.Core.Interpolation
{
    /// <summary>
    /// The type of interpolation used.
    /// </summary>
    public enum InterpolationType : ushort
    {
        /// <summary>
        /// No interpolation.
        /// </summary>
        None = 0,

        /// <summary>
        /// Linear interpolation.
        /// </summary>
        Linear = 1,

        /// <summary>
        /// Hermite interpolation.
        /// </summary>
        Hermite = 2,

        /// <summary>
        /// Bezier interpolation.
        /// </summary>
        Bezier = 3
    }
}
