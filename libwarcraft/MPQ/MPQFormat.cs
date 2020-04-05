//
//  MPQFormat.cs
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

using JetBrains.Annotations;

namespace Warcraft.MPQ
{
    /// <summary>
    /// This enum contains all known versions of the MPQ archive format.
    /// </summary>
    [PublicAPI]
    public enum MPQFormat : ushort
    {
        /// <summary>
        /// The basic archive format. Used in games prior to and including Classic World of Warcraft.
        /// </summary>
        Basic = 0,

        /// <summary>
        /// The extended v1 archive format. Used in games after and including World of Warcraft: The Burning Crusade.
        /// </summary>
        ExtendedV1 = 1
    }
}
