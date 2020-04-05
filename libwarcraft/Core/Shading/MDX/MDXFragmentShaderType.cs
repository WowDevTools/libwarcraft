//
//  MDXFragmentShaderType.cs
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
#pragma warning disable 1591, SA1602

namespace Warcraft.Core.Shading.MDX
{
    /// <summary>
    /// All of the fragment shader types for M2/MDX in WoW. See
    /// https://github.com/WowDevTools/Everlook/blob/master/Everlook/Content/Shaders/Components/Combiners/Combiners.glsl
    /// for the algorithms.
    /// </summary>
    public enum MDXFragmentShaderType
    {
        Combiners_Opaque,
        Combiners_Mod,
        Combiners_Opaque_Mod,
        Combiners_Opaque_Mod2x,
        Combiners_Opaque_Mod2xNA,
        Combiners_Opaque_Opaque,
        Combiners_Mod_Mod,
        Combiners_Mod_Mod2x,
        Combiners_Mod_Add,
        Combiners_Mod_Mod2xNA,
        Combiners_Mod_AddNA,
        Combiners_Mod_Opaque,
        Combiners_Opaque_Mod2xNA_Alpha,
        Combiners_Opaque_AddAlpha,
        Combiners_Opaque_AddAlpha_Alpha,
        Combiners_Opaque_Mod2xNA_Alpha_Add,
        Combiners_Mod_AddAlpha,
        Combiners_Mod_AddAlpha_Alpha,
        Combiners_Opaque_Alpha_Alpha,
        Combiners_Opaque_Mod2xNA_Alpha_3s,
        Combiners_Opaque_AddAlpha_Wgt,
        Combiners_Mod_Add_Alpha,
        Combiners_Opaque_ModNA_Alpha,
        Combiners_Mod_AddAlpha_Wgt,
        Combiners_Opaque_Mod_Add_Wgt,
        Combiners_Opaque_Mod2xNA_Alpha_UnshAlpha,
        Combiners_Mod_Dual_Crossfade,
        Combiners_Opaque_Mod2xNA_Alpha_Alpha,
        Combiners_Mod_Masked_Dual_Crossfade,
        Combiners_Opaque_Alpha,
        Guild,
        Guild_NoBorder,
        Guild_Opaque,
        Combiners_Mod_Depth,
        Illum
    }
}
