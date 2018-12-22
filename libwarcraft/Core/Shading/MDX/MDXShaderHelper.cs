//
//  MDXShaderHelper.cs
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
using System.Collections.Generic;
using Warcraft.Core.Shading.Blending;
using Warcraft.MDX;
using Warcraft.MDX.Visual;
using static Warcraft.Core.Shading.MDX.MDXFragmentShaderType;
using static Warcraft.Core.Shading.MDX.MDXVertexShaderType;

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
            new MDXShaderGroup(Combiners_Opaque_Mod2xNA_Alpha,           Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Opaque_AddAlpha,                Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Opaque_AddAlpha_Alpha,          Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Opaque_Mod2xNA_Alpha_Add,       Diffuse_T1_Env_T1,      MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 3),
            new MDXShaderGroup(Combiners_Mod_AddAlpha,                   Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(Combiners_Opaque_AddAlpha,                Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Mod_AddAlpha,                   Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(Combiners_Mod_AddAlpha_Alpha,             Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(Combiners_Opaque_Alpha_Alpha,             Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Opaque_Mod2xNA_Alpha_3s,        Diffuse_T1_Env_T1,      MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 3),
            new MDXShaderGroup(Combiners_Opaque_AddAlpha_Wgt,            Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Mod_Add_Alpha,                  Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(Combiners_Opaque_ModNA_Alpha,             Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Mod_AddAlpha_Wgt,               Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Mod_AddAlpha_Wgt,               Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Opaque_AddAlpha_Wgt,            Diffuse_T1_T2,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Opaque_Mod_Add_Wgt,             Diffuse_T1_Env,         MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Opaque_Mod2xNA_Alpha_UnshAlpha, Diffuse_T1_Env_T1,      MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 3),
            new MDXShaderGroup(Combiners_Mod_Dual_Crossfade,             Diffuse_T1_T1_T1,       MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 0),
            new MDXShaderGroup(Combiners_Mod_Depth,                      Diffuse_EdgeFade_T1,    MDXControlShaderType.T1,          MDXEvaluationShaderType.T1,          0, 0),
            new MDXShaderGroup(Combiners_Mod_AddAlpha_Alpha,             Diffuse_T1_Env_T2,      MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 3),
            new MDXShaderGroup(Combiners_Mod_Mod,                        Diffuse_EdgeFade_T1_T2, MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(Combiners_Mod_Masked_Dual_Crossfade,      Diffuse_T1_T1_T1_T2,    MDXControlShaderType.T1_T2_T3_T4, MDXEvaluationShaderType.T1_T2_T3_T4, 0, 0),
            new MDXShaderGroup(Combiners_Opaque_Alpha,                   Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 3),
            new MDXShaderGroup(Combiners_Opaque_Mod2xNA_Alpha_UnshAlpha, Diffuse_T1_Env_T2,      MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2_T3,    0, 3),
            new MDXShaderGroup(Combiners_Mod_Depth,                      Diffuse_EdgeFade_Env,   MDXControlShaderType.T1,          MDXEvaluationShaderType.T1,          0, 0),
            new MDXShaderGroup(Guild,                                    Diffuse_T1_T2_T1,       MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(Guild_NoBorder,                           Diffuse_T1_T2,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2_T3,    0, 0),
            new MDXShaderGroup(Guild_Opaque,                             Diffuse_T1_T2_T1,       MDXControlShaderType.T1_T2_T3,    MDXEvaluationShaderType.T1_T2,       0, 0),
            new MDXShaderGroup(Illum,                                    Diffuse_T1_T1,          MDXControlShaderType.T1_T2,       MDXEvaluationShaderType.T1_T2,       0, 0),
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
                    return Diffuse_Env;
                }

                if ((shaderID & 0x4000) > 0)
                {
                    return Diffuse_T2;
                }

                return Diffuse_T1;
            }

            if ((shaderID & 0x80) > 0)
            {
                if ((shaderID & 0x8) > 0)
                {
                    return Diffuse_Env_Env;
                }

                return Diffuse_Env_T1;
            }

            if ((shaderID & 0x8) > 0)
            {
                return Diffuse_T1_Env;
            }

            if ((shaderID & 0x4000) > 0)
            {
                return Diffuse_T1_T2;
            }

            return Diffuse_T1_T1;
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
        public static MDXEvaluationShaderType GetEvaluationShaderType(uint operationCount, ushort shaderID)
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
                    ? Combiners_Mod
                    : Combiners_Opaque;
            }

            uint lower = (uint)(shaderID & 7);
            if ((shaderID & 0x70) > 0)
            {
                switch (lower)
                {
                    case 0: return Combiners_Mod_Opaque;
                    case 3: return Combiners_Mod_Add;
                    case 4: return Combiners_Mod_Mod2x;
                    case 6: return Combiners_Mod_Mod2xNA;
                    case 7: return Combiners_Mod_AddNA;
                    default: return Combiners_Mod_Mod;
                }
            }

            switch (lower)
            {
                case 0: return Combiners_Opaque_Opaque;
                case 3: return Combiners_Opaque_AddAlpha;
                case 4: return Combiners_Opaque_Mod2x;
                case 6: return Combiners_Opaque_Mod2xNA;
                case 7: return Combiners_Opaque_AddAlpha;
                default: return Combiners_Opaque_Mod;
            }
        }

        /// <summary>
        /// Calculate the shader selector value based on the original shader ID, the render batch and general
        /// information from the model.
        /// </summary>
        /// <param name="baseShaderID">The original shader ID. Typically 0.</param>
        /// <param name="renderBatch">The render batch to which the ID belongs.</param>
        /// <param name="model">The model to which the batch belongs.</param>
        /// <returns>A new shader selector value.</returns>
        public static ushort GetRuntimeShaderID(ushort baseShaderID, MDXRenderBatch renderBatch, Warcraft.MDX.MDX model)
        {
            // The shader ID is already "modernized", so there's nothing to do.
            if ((baseShaderID & 0x8000) > 0)
            {
                return baseShaderID;
            }

            var operationCount = renderBatch.TextureCount;
            var material = model.Materials[renderBatch.MaterialIndex];

            ushort newShaderID = 0;

            if (!model.GlobalModelFlags.HasFlag(ModelObjectFlags.HasBlendModeOverrides))
            {
                var textureMapping = model.TextureMappingLookupTable[renderBatch.TextureMappingLookupTableIndex];
                bool isEnvMapped = textureMapping == EMDXTextureMappingType.Environment;
                bool nonOpaqueBlendingMode = material.BlendMode != BlendingMode.Opaque;

                if (nonOpaqueBlendingMode)
                {
                    newShaderID = 1;
                    if (isEnvMapped)
                    {
                        newShaderID |= 8;
                    }
                }

                newShaderID *= 16;

                if (textureMapping == EMDXTextureMappingType.T2)
                {
                    newShaderID |= 0x4000;
                }

                return newShaderID;
            }

            if (operationCount == 0)
            {
                return baseShaderID;
            }

            var v19 = new short[]{0, 0};

            for (int opIndex = 0; opIndex < operationCount; ++opIndex)
            {
                int blendingOverrideIndex = baseShaderID + opIndex;
                var blendingOverride = model.BlendMapOverrides[blendingOverrideIndex];

                if (opIndex == 0 && material.BlendMode == BlendingMode.Opaque)
                {
                    blendingOverride = BlendingMode.Opaque;
                }

                int textureMappingOverrideIndex = renderBatch.TextureMappingLookupTableIndex + opIndex;
                EMDXTextureMappingType textureSlotOverride = model.TextureMappingLookupTable[textureMappingOverrideIndex];
                bool isEnvMapped = textureSlotOverride == EMDXTextureMappingType.Environment;

                if (isEnvMapped)
                {
                    v19[opIndex] = (short)((short)blendingOverride | 8);
                }
                else
                {
                    v19[opIndex] = (short)blendingOverride;
                }

                if (textureSlotOverride == EMDXTextureMappingType.Environment && (opIndex + 1) == operationCount)
                {
                    newShaderID |= 0x4000;
                }
            }

            newShaderID |= (ushort)(v19[1] | (v19[0] * 16));

            return newShaderID;
        }
    }
}
