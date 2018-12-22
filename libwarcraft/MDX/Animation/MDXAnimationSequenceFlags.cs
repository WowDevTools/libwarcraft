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
    [Flags]
    public enum MDXAnimationSequenceFlags : uint
    {
        SetBlendAnimation = 0x01,
        Unknown1 = 0x02,
        Unknown2 = 0x04,
        Unknown3 = 0x08,
        LoadedAsLowPrioritySequence = 0x10,
        Looping = 0x20,
        IsAliasedAndHasFollowupAnimation = 0x40,
        IsBlended = 0x80,
        LocallyStoredSequence = 0x100
    }
}
