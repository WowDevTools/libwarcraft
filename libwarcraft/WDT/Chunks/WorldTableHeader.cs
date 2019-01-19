//
//  WorldTableHeader.cs
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

using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.WDT.Chunks
{
    /// <summary>
    /// Represents the world table header.
    /// </summary>
    public class WorldTableHeader : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MPHD";

        /// <summary>
        /// Gets or sets the world flags.
        /// </summary>
        public WorldTableFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets an unknown value.
        /// </summary>
        public uint Unknown { get; set; }

        // Six unused fields

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldTableHeader"/> class.
        /// </summary>
        public WorldTableHeader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldTableHeader"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public WorldTableHeader(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Flags = (WorldTableFlags)br.ReadUInt32();
                    Unknown = br.ReadUInt32();
                }
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <summary>
        /// Gets the size of the data contained in this chunk.
        /// </summary>
        /// <returns>The size.</returns>
        public static uint GetSize()
        {
            return 32;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((uint)Flags);
                    bw.Write(Unknown);

                    // Write the six unused fields
                    for (int i = 0; i < 6; ++i)
                    {
                        bw.Write(0U);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
