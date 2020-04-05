//
//  BSPPlaneType.cs
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

namespace Warcraft.WMO.GroupFile.Chunks
{
    /// <summary>
    /// Gets the plane type of a BSP node.
    /// </summary>
    public enum BSPPlaneType : ushort
    {
        /// <summary>
        /// Divides in the YZ direction.
        /// </summary>
        YZ = 0,

        /// <summary>
        /// Divides in the XZ direction.
        /// </summary>
        XZ = 1,

        /// <summary>
        /// Divides in the XY direction.
        /// </summary>
        XY = 2,

        /// <summary>
        /// A leaf node.
        /// </summary>
        Leaf = 4
    }
}
