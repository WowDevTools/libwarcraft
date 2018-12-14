//
//  MapChunkAlphaMaps.cs
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

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// Holds flags for the liquid chunk.
    /// </summary>
    [Flags]
    public enum LiquidFlags : byte
    {
        /// <summary>
        /// The liquid is present, but hidden.
        /// </summary>
        Hidden = 0x08,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown1 = 0x10,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown2 = 0x20,

        /// <summary>
        /// The liquid is fishable.
        /// </summary>
        Fishable = 0x40,

        /// <summary>
        /// The liquid is shared.
        /// </summary>
        Shared = 0x80
    }
}
