//
//  RGB.cs
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

using System.Numerics;

namespace Warcraft.Core.Structures
{
    /// <summary>
    /// A structure representing an RGB colour value.
    /// </summary>
    public struct RGB
    {
        /// <summary>
        /// The values in the structure.
        /// </summary>
        private Vector3 Values;

        /// <summary>
        /// The red component.
        /// </summary>
        public float R
        {
            get => Values.X;
            set => Values.X = value;
        }

        /// <summary>
        /// The green component.
        /// </summary>
        public float G
        {
            get => Values.Y;
            set => Values.Y = value;
        }

        /// <summary>
        /// The blue component.
        /// </summary>
        public float B
        {
            get => Values.Z;
            set => Values.Z = value;
        }

        /// <summary>
        /// Creates a new <see cref="RGB"/> object from a set of floating point colour component
        /// values.
        /// </summary>
        /// <param name="inR">The input red component.</param>
        /// <param name="inG">The input blue component.</param>
        /// <param name="inB">The input green component.</param>
        public RGB(float inR, float inG, float inB)
        {
            Values = new Vector3(inR, inG, inB);
        }

        /// <summary>
        /// Creates a new <see cref="RGB"/> object from a <see cref="Vector3"/> colour vector.
        /// </summary>
        /// <param name="inVector">The input colour vector.</param>
        public RGB(Vector3 inVector)
        {
            Values = inVector;
        }

        /// <summary>
        /// Creates a string representation of the current instance.
        /// </summary>
        /// <returns>A string representation of the current instance.</returns>
        public override string ToString()
        {
            return $"rgb({R}, {G}, {B})";
        }
    }
}
