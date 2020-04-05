//
//  RootFlags.cs
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

namespace Warcraft.WMO.RootFile
{
    /// <summary>
    /// Defines a set of flags for the root model file.
    /// </summary>
    [Flags]
    public enum RootFlags : uint
    {
        /// <summary>
        /// Vertexes should not be attenuated based on portal distance.
        /// </summary>
        DoNotAttenuateVerticesBasedOnPortalDistance = 0x01,

        /// <summary>
        /// The new rendering path should be used for this model.
        /// </summary>
        UseUnifiedRenderingPath = 0x02,

        /// <summary>
        /// Use the real liquid type from the database instead of the local one.
        /// </summary>
        UseDatabaseLiquid = 0x04,

        /// <summary>
        /// The model has outdoor groups.
        /// </summary>
        HasOutdoorGroups = 0x08,

        // Followed by 28 unused flags
    }
}
