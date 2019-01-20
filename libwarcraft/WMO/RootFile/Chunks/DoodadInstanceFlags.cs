//
//  DoodadInstanceFlags.cs
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

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// A set of flags which can affect the way a doodad instance is rendered.
    /// </summary>
    [Flags]
    public enum DoodadInstanceFlags : byte
    {
        /// <summary>
        /// Accepts a projected texture.
        /// </summary>
        AcceptProjectedTexture = 0x1,

        /// <summary>
        /// Unknown. Related to lighting.
        /// </summary>
        Unknown1 = 0x2,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown2 = 0x4,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown3 = 0x8
    }
}
