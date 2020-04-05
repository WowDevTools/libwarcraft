//
//  PolygonMaterialFlags.cs
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
    /// Gets the material flags of the polygon.
    /// </summary>
    public enum PolygonMaterialFlags : byte
    {
        /// <summary>
        /// An unknown value.
        /// </summary>
        Unknown1 = 0x01,

        /// <summary>
        /// Collision is disabled between this polygon and the camera.
        /// </summary>
        NoCameraCollide = 0x02,

        /// <summary>
        /// This polygon is a small detail.
        /// </summary>
        Detail = 0x04,

        /// <summary>
        /// This polygon has detail.
        /// </summary>
        HasCollision = 0x08,

        /// <summary>
        /// This polygon serves as hinting information for something.
        /// </summary>
        Hint = 0x10,

        /// <summary>
        /// This polygon should be rendered.
        /// </summary>
        Render = 0x20,

        /// <summary>
        /// An unknown value.
        /// </summary>
        Unknown2 = 0x40,

        /// <summary>
        /// This polygon has collision.
        /// </summary>
        CollideHit = 0x80
    }
}
