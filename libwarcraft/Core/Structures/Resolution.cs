//
//  Resolution.cs
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

namespace Warcraft.Core.Structures
{
    /// <summary>
    /// A structure representing a graphical resolution, consisting of two uint values.
    /// </summary>
    public struct Resolution
    {
        /// <summary>
        /// The horizontal resolution (or X resolution)
        /// </summary>
        public uint X;

        /// <summary>
        /// The vertical resolution (or Y resolution)
        /// </summary>
        public uint Y;

        /// <summary>
        /// Creates a new <see cref="Resolution"/> object from a height and a width.
        /// </summary>
        /// <param name="inX">The input width component.</param>
        /// <param name="inY">The input height component.</param>
        public Resolution(uint inX, uint inY)
        {
            X = inX;
            Y = inY;
        }

        /// <summary>
        /// Creates a new <see cref="Resolution"/> object from a single input uint, filling all components.
        /// </summary>
        /// <param name="all">The input component.</param>
        public Resolution(uint all)
            :this(all, all)
        {

        }

        /// <summary>
        /// Creates a string representation of the current object.
        /// </summary>
        /// <returns>A string representation of the current object.</returns>
        public override string ToString()
        {
            return $"{X}x{Y}";
        }
    }
}
