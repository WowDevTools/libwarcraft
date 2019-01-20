//
//  MaterialFlags.cs
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

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Defines a set of model material flags.
    /// </summary>
    [Flags]
    public enum MaterialFlags : uint
    {
        /// <summary>
        /// The material is unlit.
        /// </summary>
        Unlit = 0x1,

        /// <summary>
        /// The material ignores fog.
        /// </summary>
        Unfogged = 0x2,

        /// <summary>
        /// The material is two-sided.
        /// </summary>
        TwoSided = 0x4,

        /// <summary>
        /// The material should only use exterior lighting.
        /// </summary>
        ExteriorLighting = 0x8,

        /// <summary>
        /// Disables lighting and shading during night. Used for windows and lamps which glow at night.
        /// </summary>
        UnlitDuringNight = 0x10,

        /// <summary>
        /// The material is a window.
        /// </summary>
        Window = 0x20,

        /// <summary>
        /// The textures should wrap with clamping on the S axis.
        /// </summary>
        TextureWrappingClampS = 0x40,

        /// <summary>
        /// The textures should wrap with clamping on the T axis.
        /// </summary>
        TextureWrappingClampT = 0x80,

        /// <summary>
        /// An unknwon value.
        /// </summary>
        Unknown = 0x100

        // Followed by 23 unused flags
    }
}
