//
//  AreaInfoEntry.cs
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
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.WDT.Chunks
{
    /// <summary>
    /// Represents an area information entry.
    /// </summary>
    public class AreaInfoEntry : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the area info flags.
        /// </summary>
        public AreaInfoFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the area ID.
        /// </summary>
        public uint AreaID { get; set; }

        /*
            The following fields are not serialized, and are provided as
            helper fields for programmers.
        */

        /// <summary>
        /// Gets the X coordinate of the tile.
        /// </summary>
        [field: NonSerialized]
        public uint TileX { get; }

        /// <summary>
        /// Gets the Y coordinate of the tile.
        /// </summary>
        [field: NonSerialized]
        public uint TileY { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaInfoEntry"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        /// <param name="inTileX">The tile's X coordinate.</param>
        /// <param name="inTileY">The tile's Y coordinate.</param>
        public AreaInfoEntry(byte[] data, uint inTileX, uint inTileY)
        {
            TileX = inTileX;
            TileY = inTileY;
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    Flags = (AreaInfoFlags)br.ReadUInt32();
                    AreaID = br.ReadUInt32();
                }
            }
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream(8))
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write((uint)Flags);
                    bw.Write(AreaID);

                    bw.Flush();
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Gets the serialized size of the instance.
        /// </summary>
        /// <returns>The size.</returns>
        public static uint GetSize()
        {
            return 8;
        }
    }
}
