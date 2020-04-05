//
//  BlendingMode.cs
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

namespace Warcraft.Core.Shading.Blending
{
    /// <summary>
    /// Defines the blending mode used.
    /// </summary>
    public enum BlendingMode : ushort
    {
        /// <summary>
        /// Opaque blending mode.
        /// </summary>
        Opaque = 0,

        /// <summary>
        /// Keyed alpha blending mode.
        /// </summary>
        AlphaKey = 1,

        /// <summary>
        /// Standard alpha blending mode.
        /// </summary>
        Alpha = 2,

        /// <summary>
        /// Additive blending mode.
        /// </summary>
        Additive = 3,

        /// <summary>
        /// Modulative blending mode.
        /// </summary>
        Modulate = 4,

        /// <summary>
        /// Modulative blending mode with a multiplication factor of two.
        /// </summary>
        Modulate2x = 5,

        /// <summary>
        /// Additive modulative blending mode.
        /// </summary>
        ModulateAdditive = 6,

        /// <summary>
        /// Inverted source alpha with additive blending mode.
        /// </summary>
        InvertedSourceAlphaAdditive = 7,

        /// <summary>
        /// Inverted source alpha with opaque blending mode.
        /// </summary>
        InvertedSourceAlphaOpaque = 8,

        /// <summary>
        /// Source alpha with opaque blending mode.
        /// </summary>
        SourceAlphaOpaque = 9,

        /// <summary>
        /// Alphaless additive blending mode.
        /// </summary>
        NoAlphaAdditive = 10,

        /// <summary>
        /// Constant alpha blending mode.
        /// </summary>
        ConstantAlpha = 11,

        /// <summary>
        /// Screen-space blending mode.
        /// </summary>
        Screen = 12,

        /// <summary>
        /// Blended additive blending mode.
        /// </summary>
        BlendAdditive = 13
    }
}
