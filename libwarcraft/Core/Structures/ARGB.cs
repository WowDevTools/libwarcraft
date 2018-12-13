//
//  ARGB.cs
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
    /// A structure representing an ARGB colour value.
    /// </summary>
    public struct ARGB
    {
        /// <summary>
        /// The red component.
        /// </summary>
        public byte R;

        /// <summary>
        /// The green component.
        /// </summary>
        public byte G;

        /// <summary>
        /// The blue component.
        /// </summary>
        public byte B;

        /// <summary>
        /// The alpha component.
        /// </summary>
        public byte A;

        /// <summary>
        /// Creates a new <see cref="ARGB"/> object from a set of byte component values.
        /// </summary>
        /// <param name="inR">The input red component.</param>
        /// <param name="inG">The input blue component.</param>
        /// <param name="inB">The input green component.</param>
        /// <param name="inA">The input alpha component.</param>
        public ARGB(byte inR, byte inG, byte inB, byte inA)
        {
            this.R = inR;
            this.G = inG;
            this.B = inB;
            this.A = inA;
        }

        /// <summary>
        /// Creates a new <see cref="ARGB"/> object from a byte that fills all components.
        /// </summary>
        /// <param name="all">The input byte component.</param>
        public ARGB(byte all)
            :this(all, all, all, all)
        {

        }

        /// <summary>
        /// Creates a string representation of the current instance.
        /// </summary>
        /// <returns>A string representation of the current instance.</returns>
        public override string ToString()
        {
            return $"ARGB({this.R}, {this.G}, {this.B}, {this.A})";
        }
    }
}
