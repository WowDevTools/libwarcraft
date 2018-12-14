//
//  TerrainHeaderFlags.cs
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

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// Flags for the ADT.
    /// </summary>
    [Flags]
    public enum TerrainHeaderFlags
    {
        /// <summary>
        /// This terrain file contains a bounding box.
        /// </summary>
        HasBoundingBox = 1,

        /// <summary>
        /// Flag if the ADT is from Northrend. This flag is not always set.
        /// </summary>
        Northrend = 2,
    }
}
