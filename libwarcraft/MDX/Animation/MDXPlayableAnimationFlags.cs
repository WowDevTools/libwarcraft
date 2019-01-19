//
//  MDXPlayableAnimationFlags.cs
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

namespace Warcraft.MDX.Animation
{
    /// <summary>
    /// Flags for how an animation should be played.
    /// </summary>
    [Flags]
    public enum MDXPlayableAnimationFlags : short
    {
        /// <summary>
        /// Normal playback.
        /// </summary>
        PlayNormally = 0,

        /// <summary>
        /// Reversed playback.
        /// </summary>
        PlayReversed = 1,

        /// <summary>
        /// The animation is frozen (typically at time index 0).
        /// </summary>
        Freeze = 3
    }
}
