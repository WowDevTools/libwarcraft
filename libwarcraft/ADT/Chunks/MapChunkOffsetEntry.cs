//
//  MapChunkOffsetEntry.cs
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

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// A struct containing information about the referenced MCNK.
    /// </summary>
    public class MapChunkOffsetEntry
    {
        /// <summary>
        /// Gets or sets the absolute offset of the MCNK.
        /// </summary>
        public int MapChunkOffset { get; set; }

        /// <summary>
        /// Gets or sets the size of the MCNK.
        /// </summary>
        public int MapChunkSize { get; set; }

        /// <summary>
        /// Gets or sets the flags of the MCNK. This is only set on the client, and is as such always 0.
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// Gets or sets the loading ID of the MCNK. This is only set on the client, and is as such always 0.
        /// </summary>
        public int AsynchronousLoadingID { get; set; }
    }
}
