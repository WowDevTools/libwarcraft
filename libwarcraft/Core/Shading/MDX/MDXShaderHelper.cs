//
//  MDXShaderHelper.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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
using System.Collections.Generic;

namespace Warcraft.Core.Shading.MDX
{
    /// <summary>
    /// Acts as a container for shader selection functions. Algorithms are taken from the WoWDev wiki.
    /// </summary>
    public static class MDXShaderHelper
    {
        /// <summary>
        /// Internal shader lookup table.
        /// </summary>
        private static readonly List<MDXShaderGroup> ShaderTable = new List<MDXShaderGroup>
        {
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_Mod2xNA_Alpha,           MDXVertexShaderType.Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_AddAlpha,                MDXVertexShaderType.Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_AddAlpha_Alpha,          MDXVertexShaderType.Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_Mod2xNA_Alpha_Add,       MDXVertexShaderType.Diffuse_T1_Env_T1,      MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_AddAlpha,                   MDXVertexShaderType.Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_AddAlpha,                MDXVertexShaderType.Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_AddAlpha,                   MDXVertexShaderType.Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_AddAlpha_Alpha,             MDXVertexShaderType.Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_Alpha_Alpha,             MDXVertexShaderType.Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_Mod2xNA_Alpha_3s,        MDXVertexShaderType.Diffuse_T1_Env_T1,      MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_AddAlpha_Wgt,            MDXVertexShaderType.Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_Add_Alpha,                  MDXVertexShaderType.Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_ModNA_Alpha,             MDXVertexShaderType.Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_AddAlpha_Wgt,               MDXVertexShaderType.Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_AddAlpha_Wgt,               MDXVertexShaderType.Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_AddAlpha_Wgt,            MDXVertexShaderType.Diffuse_T1_T2,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_Mod_Add_Wgt,             MDXVertexShaderType.Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_Mod2xNA_Alpha_UnshAlpha, MDXVertexShaderType.Diffuse_T1_Env_T1,      MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_Dual_Crossfade,             MDXVertexShaderType.Diffuse_T1_T1_T1,       MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_Depth,                      MDXVertexShaderType.Diffuse_EdgeFade_T1,    MDXControlShaderType.T1,          MDXEvaluationShaderType.T1,          0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_AddAlpha_Alpha,             MDXVertexShaderType.Diffuse_T1_Env_T2,      MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_Mod,                        MDXVertexShaderType.Diffuse_EdgeFade_T1_T2, MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_Masked_Dual_Crossfade,      MDXVertexShaderType.Diffuse_T1_T1_T1_T2,    MDXControlShaderType.T1_T2_T3_T4, MDXEvaluationShaderType.T1_T2_T3_T4, 0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_Alpha,                   MDXVertexShaderType.Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Opaque_Mod2xNA_Alpha_UnshAlpha, MDXVertexShaderType.Diffuse_T1_Env_T2,      MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 3),
            new MDXShaderGroup(MDXFragmentShaderType.Combiners_Mod_Depth,                      MDXVertexShaderType.Diffuse_EdgeFade_Env,   MDXControlShaderType.T1,          MDXEvaluationShaderType.T1,          0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Guild,                                    MDXVertexShaderType.Diffuse_T1_T2_T1,       MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Guild_NoBorder,                           MDXVertexShaderType.Diffuse_T1_T2,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2_T3,    0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Guild_Opaque,                             MDXVertexShaderType.Diffuse_T1_T2_T1,       MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(MDXFragmentShaderType.Illum,                                    MDXVertexShaderType.Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
        };

        /// <summary>
        /// Gets the vertex shader type for a given shader ID and operation count.
        /// </summary>
        /// <param name="operationCount">The number of operations or textures in the shader.</param>
        /// <param name="shaderID">The numeric ID of the shader.</param>
        /// <returns>The vertex shader type.</returns>
        /// <exception cref="ArgumentException">Thrown if the shader ID parameter cannot be used as an indexer into the shader table.</exception>
        public static MDXVertexShaderType GetVertexShaderType(uint operationCount, ushort shaderID)
        {
            if ((shaderID & 0x8000) > 0)
            {
                ushort shaderTableIndex = (ushort)(shaderID & ~0x8000);
                if (shaderTableIndex >= ShaderTable.Count)
                {
                    throw new ArgumentException("The shader ID did not fall in an expected range.", nameof(shaderID));
                }

                return ShaderTable[shaderTableIndex].VertexShader;
            }

            if (operationCount == 1)
            {
                if ((shaderID & 0x80) > 0)
                {
                    return MDXVertexShaderType.Diffuse_Env;
                }

                if ((shaderID & 0x4000) > 0)
                {
                    return MDXVertexShaderType.Diffuse_T2;
                }

                return MDXVertexShaderType.Diffuse_T1;
            }

            if ((shaderID & 0x80) > 0)
            {
                if ((shaderID & 0x8) > 0)
                {
                    return MDXVertexShaderType.Diffuse_Env_Env;
                }

                return MDXVertexShaderType.Diffuse_Env_T1;
            }

            if ((shaderID & 0x8) > 0)
            {
                return MDXVertexShaderType.Diffuse_T1_Env;
            }

            if ((shaderID & 0x4000) > 0)
            {
                return MDXVertexShaderType.Diffuse_T1_T2;
            }

            return MDXVertexShaderType.Diffuse_T1_T1;
        }

        /// <summary>
        /// Gets the control shader type for a given shader ID and operation count.
        /// </summary>
        /// <param name="operationCount">The number of operations or textures in the shader.</param>
        /// <param name="shaderID">The numeric ID of the shader.</param>
        /// <returns>The control shader type.</returns>
        /// <exception cref="ArgumentException">Thrown if the shader ID parameter cannot be used as an indexer into the shader table.</exception>
        public static MDXControlShaderType GetControlShaderType(uint operationCount, ushort shaderID)
        {
            if ((shaderID & 0x8000) > 0)
            {
                ushort shaderTableIndex = (ushort)(shaderID & ~0x8000);
                if (shaderTableIndex >= ShaderTable.Count)
                {
                    throw new ArgumentException("The shader ID did not fall in an expected range.", nameof(shaderID));
                }

                return ShaderTable[shaderTableIndex].ControlShader;
            }

            return operationCount == 1 ? MDXControlShaderType.T1 : MDXControlShaderType.T1_T2;
        }


        /// <summary>
        /// Gets the evaluation shader type for a given shader ID and operation count.
        /// </summary>
        /// <param name="operationCount">The number of operations or textures in the shader.</param>
        /// <param name="shaderID">The numeric ID of the shader.</param>
        /// <returns>The evaluation shader type.</returns>
        /// <exception cref="ArgumentException">Thrown if the shader ID parameter cannot be used as an indexer into the shader table.</exception>
        public static MDXEvaluationShaderType GEvaluationShaderType(uint operationCount, ushort shaderID)
        {
            if ((shaderID & 0x8000) > 0)
            {
                ushort shaderTableIndex = (ushort)(shaderID & ~0x8000);
                if (shaderTableIndex >= ShaderTable.Count)
                {
                    throw new ArgumentException("The shader ID did not fall in an expected range.", nameof(shaderID));
                }

                return ShaderTable[shaderTableIndex].EvaluationShader;
            }

            return operationCount == 1 ? MDXEvaluationShaderType.T1 : MDXEvaluationShaderType.T1_T2;
        }

        /// <summary>
        /// Gets the fragment shader type for a given shader ID and operation count.
        /// </summary>
        /// <param name="operationCount">The number of operations or textures in the shader.</param>
        /// <param name="shaderID">The numeric ID of the shader.</param>
        /// <returns>The fragment shader type.</returns>
        /// <exception cref="ArgumentException">Thrown if the shader ID parameter cannot be used as an indexer into the shader table.</exception>
        public static MDXFragmentShaderType GetFragmentShaderType(uint operationCount, ushort shaderID)
        {
            if ((shaderID & 0x8000) > 0)
            {
                ushort shaderTableIndex = (ushort)(shaderID & ~0x8000);
                if (shaderTableIndex >= ShaderTable.Count)
                {
                    throw new ArgumentException("The shader ID did not fall in an expected range.", nameof(shaderID));
                }

                return ShaderTable[shaderTableIndex].FragmentShader;
            }

            if (operationCount == 1)
            {
                return (shaderID & 0x70) > 0
                    ? MDXFragmentShaderType.Combiners_Mod
                    : MDXFragmentShaderType.Combiners_Opaque;
            }

            uint lower = (uint)(shaderID & 7);
            if ((shaderID & 0x70) > 0)
            {
                switch (lower)
                {
                    case 0: return MDXFragmentShaderType.Combiners_Mod_Opaque;
                    case 3: return MDXFragmentShaderType.Combiners_Mod_Add;
                    case 4: return MDXFragmentShaderType.Combiners_Mod_Mod2x;
                    case 6: return MDXFragmentShaderType.Combiners_Mod_Mod2xNA;
                    case 7: return MDXFragmentShaderType.Combiners_Mod_AddNA;
                    default: return MDXFragmentShaderType.Combiners_Mod_Mod;
                }
            }

            switch (lower)
            {
                case 0: return MDXFragmentShaderType.Combiners_Opaque_Opaque;
                case 3: return MDXFragmentShaderType.Combiners_Opaque_AddAlpha;
                case 4: return MDXFragmentShaderType.Combiners_Opaque_Mod2x;
                case 6: return MDXFragmentShaderType.Combiners_Opaque_Mod2xNA;
                case 7: return MDXFragmentShaderType.Combiners_Opaque_AddAlpha;
                default: return MDXFragmentShaderType.Combiners_Opaque_Mod;
            }
        }
    }
}