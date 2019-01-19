//
//  WorldTableFlags.cs
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

namespace Warcraft.WDT.Chunks
{
    /// <summary>
    /// Defines a set of world table flags.
    /// </summary>
    [Flags]
    public enum WorldTableFlags : uint
    {
        /// <summary>
        /// This world uses global models.
        /// </summary>
        UsesGlobalModels = 0x01,

        /// <summary>
        /// This world uses vertex shading.
        /// </summary>
        UsesVertexShading = 0x02,

        /// <summary>
        /// This world uses environment mapping.
        /// </summary>
        UsesEnvironmentMapping = 0x04,

        /// <summary>
        /// This world disables some unknown rendering flag.
        /// </summary>
        DisableUnknownRenderingFlag = 0x08,

        /// <summary>
        /// This world uses vertex lighting.
        /// </summary>
        UsesVertexLighting = 0x10,

        /// <summary>
        /// Ground normals should be flipped in this world.
        /// </summary>
        FlipGroundNormals = 0x20,

        /// <summary>
        /// An unknown flag.
        /// </summary>
        Unknown = 0x40,

        /// <summary>
        /// This world uses hard alpha falloff.
        /// </summary>
        UsesHardAlphaFalloff = 0x80,

        /// <summary>
        /// Unknown. Alpha related.
        /// </summary>
        UnknownHardAlphaRelated = 0x100,

        /// <summary>
        /// Unknown.
        /// </summary>
        UnknownContinentRelated = 0x8000
    }
}
