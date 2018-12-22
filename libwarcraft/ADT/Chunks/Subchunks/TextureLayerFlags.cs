//
//  TextureLayerFlags.cs
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

using System;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// Texture layer chunk flags.
    /// </summary>
    [Flags]
    public enum TextureLayerFlags : uint
    {
        /// <summary>
        /// The texture rotates 45 degrees per tick.
        /// </summary>
        Animated45RotationPerTick = 0x001,

        /// <summary>
        /// The texture rotates 90 degrees per tick.
        /// </summary>
        Animated90RotationPerTick = 0x002,

        /// <summary>
        /// The texture rotates 180 degrees per tick.
        /// </summary>
        Animated180RotationPerTick = 0x004,

        /// <summary>
        /// The texture has an animation speed of 1.
        /// </summary>
        AnimSpeed1 = 0x008,

        /// <summary>
        /// The texture has an animation speed of 2.
        /// </summary>
        AnimSpeed2 = 0x010,

        /// <summary>
        /// The texture has an animation speed of 3.
        /// </summary>
        AnimSpeed3 = 0x020,

        /// <summary>
        /// The texture's animation is enabled.
        /// </summary>
        AnimationEnabled = 0x040,

        /// <summary>
        /// The texture is emissive.
        /// </summary>
        EmissiveLayer = 0x080,

        /// <summary>
        /// The texture uses the alpha channel.
        /// </summary>
        UseAlpha = 0x100,

        /// <summary>
        /// The texture's alpha channel is compressed.
        /// </summary>
        CompressedAlpha = 0x200,

        /// <summary>
        /// The texture uses cube mapped reflection.
        /// </summary>
        UseCubeMappedReflection = 0x400,

        // 19 unused bits
    }
}
