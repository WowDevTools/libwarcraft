//
//  AlphaDestination.cs
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

namespace Warcraft.Core.Shading.Blending
{
    /// <summary>
    /// Different algorithms to use for destination alpha blending factors.
    /// The given values are taken from OpenGL for ease of use.
    /// </summary>
    public enum AlphaDestination
    {
        /// <summary>
        /// A factor of zero.
        /// </summary>
        Zero = 0,

        /// <summary>
        /// A factor of one.
        /// </summary>
        One = 1,

        /// <summary>
        /// Use the source alpha as the blending factor.
        /// </summary>
        SourceAlpha = 770,

        /// <summary>
        /// Use one minus the source alpha as the blending factor.
        /// </summary>
        OneMinusSourceAlpha = 771,

        /// <summary>
        /// Use one minus the constant alpha value as the blending factor.
        /// </summary>
        OneMinusConstantAlpha = 32772
    }
}
