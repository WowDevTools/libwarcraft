//
//  BlendingState.cs
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

using System.Collections.Generic;

namespace Warcraft.Core.Shading.Blending
{
    /// <summary>
    /// The <see cref="BlendingState"/> class acts as an easy way to access the intended blending state for the
    /// different blending modes that a model can have. Typically, these are used with glBlendFuncSeparate or
    /// equivalent.
    ///
    /// The names of the state enumerations are based on OpenGL, but have equivalents in DirectX. For some translation
    /// code, please refer to <a href="https://gist.github.com/Warpten/f9350f8015860671c02354312b252c4e"/>.
    /// </summary>
    public static class BlendingState
    {
        /// <summary>
        /// Whether or not blending should be enabled for a given blending mode.
        /// </summary>
        public static readonly Dictionary<BlendingMode, bool> EnableBlending = new Dictionary<BlendingMode, bool>
        {
            { BlendingMode.Opaque,                            false },
            { BlendingMode.AlphaKey,                        false },
            { BlendingMode.Alpha,                            true },
            { BlendingMode.Additive,                        true },
            { BlendingMode.Modulate,                        true },
            { BlendingMode.Modulate2x,                        true },
            { BlendingMode.ModulateAdditive,                true },
            { BlendingMode.InvertedSourceAlphaAdditive,        true },
            { BlendingMode.InvertedSourceAlphaOpaque,        true },
            { BlendingMode.SourceAlphaOpaque,                true },
            { BlendingMode.NoAlphaAdditive,                    true },
            { BlendingMode.ConstantAlpha,                    true },
            { BlendingMode.Screen,                            true },
            { BlendingMode.BlendAdditive,                    true }
        };

        /// <summary>
        /// The intended RGB source colour blending factor algorithm.
        /// </summary>
        public static readonly Dictionary<BlendingMode, ColourSource> SourceColour = new Dictionary<BlendingMode, ColourSource>
        {
            { BlendingMode.Opaque,                            ColourSource.One },
            { BlendingMode.AlphaKey,                        ColourSource.One },
            { BlendingMode.Alpha,                            ColourSource.SourceAlpha },
            { BlendingMode.Additive,                        ColourSource.SourceAlpha },
            { BlendingMode.Modulate,                        ColourSource.DestinationColour },
            { BlendingMode.Modulate2x,                        ColourSource.DestinationColour },
            { BlendingMode.ModulateAdditive,                ColourSource.DestinationColour },
            { BlendingMode.InvertedSourceAlphaAdditive,        ColourSource.OneMinusSourceAlpha },
            { BlendingMode.InvertedSourceAlphaOpaque,        ColourSource.OneMinusSourceAlpha },
            { BlendingMode.SourceAlphaOpaque,                ColourSource.SourceAlpha },
            { BlendingMode.NoAlphaAdditive,                    ColourSource.One },
            { BlendingMode.ConstantAlpha,                    ColourSource.ConstantAlpha },
            { BlendingMode.Screen,                            ColourSource.OneMinusDestionationColour },
            { BlendingMode.BlendAdditive,                    ColourSource.One }
        };

        /// <summary>
        /// The intended RGB destination colour blending factor algorithm.
        /// </summary>
        public static readonly Dictionary<BlendingMode, ColourDestination> DestinationColour = new Dictionary<BlendingMode, ColourDestination>
        {
            { BlendingMode.Opaque,                            ColourDestination.Zero },
            { BlendingMode.AlphaKey,                        ColourDestination.Zero },
            { BlendingMode.Alpha,                            ColourDestination.OneMinusSourceAlpha },
            { BlendingMode.Additive,                        ColourDestination.One },
            { BlendingMode.Modulate,                        ColourDestination.Zero },
            { BlendingMode.Modulate2x,                        ColourDestination.SourceColour },
            { BlendingMode.ModulateAdditive,                ColourDestination.One },
            { BlendingMode.InvertedSourceAlphaAdditive,        ColourDestination.One },
            { BlendingMode.InvertedSourceAlphaOpaque,        ColourDestination.Zero },
            { BlendingMode.SourceAlphaOpaque,                ColourDestination.Zero },
            { BlendingMode.NoAlphaAdditive,                    ColourDestination.One },
            { BlendingMode.ConstantAlpha,                    ColourDestination.OneMinusConstantAlpha },
            { BlendingMode.Screen,                            ColourDestination.One },
            { BlendingMode.BlendAdditive,                    ColourDestination.OneMinusSourceAlpha }
        };

        /// <summary>
        /// The intended source alpha blending factor algorithm.
        /// </summary>
        public static readonly Dictionary<BlendingMode, AlphaSource> SourceAlpha = new Dictionary<BlendingMode, AlphaSource>
        {
            { BlendingMode.Opaque,                            AlphaSource.One },
            { BlendingMode.AlphaKey,                        AlphaSource.One },
            { BlendingMode.Alpha,                            AlphaSource.One },
            { BlendingMode.Additive,                        AlphaSource.Zero },
            { BlendingMode.Modulate,                        AlphaSource.DestinationAlpha },
            { BlendingMode.Modulate2x,                        AlphaSource.DestinationAlpha },
            { BlendingMode.ModulateAdditive,                AlphaSource.DestinationAlpha },
            { BlendingMode.InvertedSourceAlphaAdditive,        AlphaSource.OneMinusSourceAlpha },
            { BlendingMode.InvertedSourceAlphaOpaque,        AlphaSource.OneMinusSourceAlpha },
            { BlendingMode.SourceAlphaOpaque,                AlphaSource.SourceAlpha },
            { BlendingMode.NoAlphaAdditive,                    AlphaSource.Zero },
            { BlendingMode.ConstantAlpha,                    AlphaSource.ConstantAlpha },
            { BlendingMode.Screen,                            AlphaSource.One },
            { BlendingMode.BlendAdditive,                    AlphaSource.One }
        };

        /// <summary>
        /// The intended destination alpha blending factor algorithm.
        /// </summary>
        public static readonly Dictionary<BlendingMode, AlphaDestination> DestinationAlpha = new Dictionary<BlendingMode, AlphaDestination>
        {
            { BlendingMode.Opaque,                            AlphaDestination.Zero },
            { BlendingMode.AlphaKey,                        AlphaDestination.Zero },
            { BlendingMode.Alpha,                            AlphaDestination.OneMinusSourceAlpha },
            { BlendingMode.Additive,                        AlphaDestination.One },
            { BlendingMode.Modulate,                        AlphaDestination.Zero },
            { BlendingMode.Modulate2x,                        AlphaDestination.SourceAlpha },
            { BlendingMode.ModulateAdditive,                AlphaDestination.One },
            { BlendingMode.InvertedSourceAlphaAdditive,        AlphaDestination.One },
            { BlendingMode.InvertedSourceAlphaOpaque,        AlphaDestination.Zero },
            { BlendingMode.SourceAlphaOpaque,                AlphaDestination.Zero },
            { BlendingMode.NoAlphaAdditive,                    AlphaDestination.One },
            { BlendingMode.ConstantAlpha,                    AlphaDestination.OneMinusConstantAlpha },
            { BlendingMode.Screen,                            AlphaDestination.Zero },
            { BlendingMode.BlendAdditive,                    AlphaDestination.OneMinusSourceAlpha }
        };
    }
}
