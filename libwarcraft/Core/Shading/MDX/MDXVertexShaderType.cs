//
//  MDXVertexShaderType.cs
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

// ReSharper disable InconsistentNaming

namespace Warcraft.Core.Shading.MDX
{
    /// <summary>
    /// All of the M2/MDX vertex shader types in WoW.
    /// </summary>
    public enum MDXVertexShaderType
    {
        /// <summary>
        /// Diffuse shading, one texture, the first texture coordinate.
        /// </summary>
        Diffuse_T1,

        /// <summary>
        /// Diffuse shading, one texture, environment mapping.
        /// </summary>
        Diffuse_Env,

        /// <summary>
        /// Diffuse shading, two textures, the first and second texture coordinate.
        /// </summary>
        Diffuse_T1_T2,

        /// <summary>
        /// Diffuse shading, two textures, the first texture coordinate and environment mapping.
        /// </summary>
        Diffuse_T1_Env,

        /// <summary>
        /// Diffuse shading, two textures, environment mapping and the first texture coordinates.
        /// </summary>
        Diffuse_Env_T1,

        /// <summary>
        /// Diffuse shading, two textures, environment mapping for both.
        /// </summary>
        Diffuse_Env_Env,

        /// <summary>
        /// Diffuse shading, three textures, the first texture coordinate, environment mapping, and the first coordinate
        /// again.
        /// </summary>
        Diffuse_T1_Env_T1,

        /// <summary>
        /// Diffuse shading, two textures, the first texture coordinate for both.
        /// </summary>
        Diffuse_T1_T1,

        /// <summary>
        /// Diffuse shading, three textures, the first texture coordinate for all.
        /// </summary>
        Diffuse_T1_T1_T1,

        /// <summary>
        /// Diffuse shading, two textures, edge fading and the first texture coordinate.
        /// </summary>
        Diffuse_EdgeFade_T1,

        /// <summary>
        /// Diffuse shading, one texture, the second texture coordinate.
        /// </summary>
        Diffuse_T2,

        /// <summary>
        /// Diffuse shading, three textures, the first texture coordinate, environment mapping and the second texture
        /// coordinate.
        /// </summary>
        Diffuse_T1_Env_T2,

        /// <summary>
        /// Diffuse shading, three textures, edge fading, the first and the second texture coordinates.
        /// </summary>
        Diffuse_EdgeFade_T1_T2,

        /// <summary>
        /// Diffuse shading, four textures, the first texture coordinate for the first three and the second for the
        /// final one.
        /// </summary>
        Diffuse_T1_T1_T1_T2,

        /// <summary>
        /// Diffuse shading, two textures, edge fading and environment mapping.
        /// </summary>
        Diffuse_EdgeFade_Env,

        /// <summary>
        /// Diffuse shading, three textures, the first, second, and first texture coordinates.
        /// </summary>
        Diffuse_T1_T2_T1
    }
}
