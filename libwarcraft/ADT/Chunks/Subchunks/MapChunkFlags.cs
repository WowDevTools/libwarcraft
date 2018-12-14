//
//  MapChunkFlags.cs
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

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// Flags available for a MCNK.
    /// </summary>
    public enum MapChunkFlags : uint
    {
        /// <summary>
        /// Flags the MCNK as containing a static shadow map
        /// </summary>
        HasBakedShadows = 1,

        /// <summary>
        /// Flags the MCNK as impassible
        /// </summary>
        Impassible = 2,

        /// <summary>
        /// Flags the MCNK as a river
        /// </summary>
        IsRiver = 4,

        /// <summary>
        /// Flags the MCNK as an ocean
        /// </summary>
        IsOcean = 8,

        /// <summary>
        /// Flags the MCNK as magma
        /// </summary>
        IsMagma = 16,

        /// <summary>
        /// Flags the MCNK as slime
        /// </summary>
        IsSlime = 32,

        /// <summary>
        /// Flags the MCNK as containing an MCCV chunk
        /// </summary>
        HasVertexShading = 64,

        /// <summary>
        /// Unknown flag, but occasionally set.
        /// </summary>
        Unknown = 128,

        // 7 unused bits

        /// <summary>
        /// Disables repair of the alpha maps in this chunk.
        /// </summary>
        DoNotRepairAlphaMaps = 32768,

        /// <summary>
        /// Flags the MCNK for high-resolution holes. Introduced in WoW 5.3
        /// </summary>
        UsesHighResHoles = 65536,

        // 15 unused bits
    }
}
