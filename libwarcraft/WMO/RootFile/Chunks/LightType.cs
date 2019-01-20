//
//  LightType.cs
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

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Defines a set of light types.
    /// </summary>
    public enum LightType : byte
    {
        /// <summary>
        /// A point light source, like a lightbulb.
        /// </summary>
        Point = 0,

        /// <summary>
        /// A directional spotlight, like a, well, spotlight.
        /// </summary>
        Spot = 1,

        /// <summary>
        /// A directional global light, like the sun.
        /// </summary>
        Directional = 2,

        /// <summary>
        /// A general ambient light level.
        /// </summary>
        Ambient = 3
    }
}
