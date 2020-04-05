//
//  TerrainLiquidAttributes.cs
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

using System.IO;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// Holds attributes for liquid tiles.
    /// </summary>
    public class TerrainLiquidAttributes
    {
        /// <summary>
        /// Gets or sets a boolean bitfield representing which liquid chunks are fishable.
        /// </summary>
        public ulong Fishable { get; set; }

        /// <summary>
        /// Gets or sets a boolean bitfield representing which liquid chunks are considered deep.
        /// </summary>
        public ulong Deep { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainLiquidAttributes"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        public TerrainLiquidAttributes(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    Fishable = br.ReadUInt64();
                    Deep = br.ReadUInt64();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainLiquidAttributes"/> class.
        /// </summary>
        public TerrainLiquidAttributes()
        {
            Fishable = 0;
            Deep = 0;
        }

        /// <summary>
        /// Gets the serialized size of the attributes.
        /// </summary>
        /// <returns>The size in bytes.</returns>
        public static int GetSize()
        {
            return 16;
        }
    }
}
