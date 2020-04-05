//
//  FogFlags.cs
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

using System;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Defines a set of fog flags.
    /// </summary>
    [Flags]
    public enum FogFlags : uint
    {
        /// <summary>
        /// The fog has an infinite radius.
        /// </summary>
        InfiniteRadius = 0x01,

        /// <summary>
        /// An unused value.
        /// </summary>
        Unused1 = 0x02,

        /// <summary>
        /// An unused value.
        /// </summary>
        Unused2 = 0x04,

        /// <summary>
        /// An unknown value.
        /// </summary>
        Unknown1 = 0x10,

        // Followed by 27 unused values
    }
}
