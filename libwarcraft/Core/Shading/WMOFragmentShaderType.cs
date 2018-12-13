//
//  WMOFragmentShaderType.cs
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

namespace Warcraft.Core.Shading
{
    /// <summary>
    /// All of the world object shader types present in WoW.
    /// </summary>
    public enum WMOFragmentShaderType
    {
        /// <summary>
        /// Simple diffuse shading.
        /// </summary>
        Diffuse = 0,

        /// <summary>
        /// Specularity shading.
        /// </summary>
        Specular = 1,

        /// <summary>
        /// Metallic shading.
        /// </summary>
        Metal = 2,

        /// <summary>
        /// Environment mapped shading.
        /// </summary>
        Env = 3,

        /// <summary>
        /// Opaque shading.
        /// </summary>
        Opaque = 4,

        /// <summary>
        /// Environment mapped metallic shading.
        /// </summary>
        EnvMetal = 5,

        /// <summary>
        /// Two-layer diffuse shading.
        /// </summary>
        TwoLayerDiffuse = 6,

        /// <summary>
        /// Two-layer environment mapped metallic shading.
        /// </summary>
        TwolayerEnvMetal = 7,

        /// <summary>
        /// Two-layer terrain shading.
        /// </summary>
        TwoLayerTerrain = 8,

        /// <summary>
        /// Emissive diffuse shading.
        /// </summary>
        DiffuseEmissive = 9,

        /// <summary>
        /// Water window shading.
        /// </summary>
        WaterWindow = 10,

        /// <summary>
        /// Masked environment mapped metallic shading.
        /// </summary>
        MaskedEnvMetal = 11,

        /// <summary>
        /// Environment mapped emissive metallic shading.
        /// </summary>
        EnvMetalEmissive = 12,

        /// <summary>
        /// Two-layer diffuse opaque shading.
        /// </summary>
        TwoLayerDiffuseOpaque = 13,

        /// <summary>
        /// Two-layer diffuse emissive shading.
        /// </summary>
        TwoLayerDiffuseEmissive = 14,

        /// <summary>
        /// Diffuse terrain shading.
        /// </summary>
        DiffuseTerrain = 16,

        /// <summary>
        /// Additive masked environment mapped metallic shading.
        /// </summary>
        AdditiveMaskedEnvMetal = 17
    }
}

