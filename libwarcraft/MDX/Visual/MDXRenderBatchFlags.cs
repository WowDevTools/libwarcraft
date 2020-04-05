//
//  MDXRenderBatchFlags.cs
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

namespace Warcraft.MDX.Visual
{
    /// <summary>
    /// Defines the render flags of a batch.
    /// </summary>
    [Flags]
    public enum MDXRenderBatchFlags : byte
    {
        /// <summary>
        /// The batch is animated.
        /// </summary>
        Animated = 0x0,

        /// <summary>
        /// The materials are inverted somehow.
        /// </summary>
        Invert = 0x1,

        /// <summary>
        /// The batch has some sort of transformation.
        /// </summary>
        Transform = 0x2,

        /// <summary>
        /// The batch is projected somehow.
        /// </summary>
        Projected = 0x4,

        /// <summary>
        /// Unknown, something batch compatible.
        /// </summary>
        Static = 0x10,

        /// <summary>
        /// Unknown, something to do with projected textures.
        /// </summary>
        Projected2 = 0x20,

        /// <summary>
        /// Uses texture weights somehow.
        /// </summary>
        Weighted = 0x40
    }
}
