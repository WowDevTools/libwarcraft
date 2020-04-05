//
//  MDXTextureMappingType.cs
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

namespace Warcraft.MDX.Visual
{
    /// <summary>
    /// Texture mapping types.
    /// </summary>
    public enum MDXTextureMappingType : short
    {
        /// <summary>
        /// Uses the first texture slot.
        /// </summary>
        T1 = 0,

        /// <summary>
        /// Uses the second texture slot.
        /// </summary>
        T2 = 1,

        /// <summary>
        /// Uses the environment mapping texture slot.
        /// </summary>
        Environment = -1
    }
}
