//
//  MDXAnimationSequenceFlags.cs
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
    /// Defines modifying flags for an animation sequence.
    /// </summary>
    [Flags]
    public enum MDXAnimationSequenceFlags : uint
    {
        /// <summary>
        /// The blend animation should be set.
        /// </summary>
        SetBlendAnimation = 0x01,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown1 = 0x02,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown2 = 0x04,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown3 = 0x08,

        /// <summary>
        /// This animation was loaded as a low-priority sequence.
        /// </summary>
        LoadedAsLowPrioritySequence = 0x10,

        /// <summary>
        /// This animation is looping.
        /// </summary>
        Looping = 0x20,

        /// <summary>
        /// This animation is aliased, and has another animation that will follow it.
        /// </summary>
        IsAliasedAndHasFollowupAnimation = 0x40,

        /// <summary>
        /// This animation is blended.
        /// </summary>
        IsBlended = 0x80,

        /// <summary>
        /// The sequence is locally stored.
        /// </summary>
        LocallyStoredSequence = 0x100
    }
}
