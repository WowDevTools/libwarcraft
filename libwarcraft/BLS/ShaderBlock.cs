//
//  ShaderBlock.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.BLS
{
    /// <summary>
    /// A data block containing shader data in compiled or source form.
    /// </summary>
    public class ShaderBlock : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the first set of shader flags.
        /// </summary>
        public ShaderFlags1 Flags1 { get; set; }

        /// <summary>
        /// Gets or sets the second set of shader flags.
        /// </summary>
        public ShaderFlags2 Flags2 { get; set; }

        /// <summary>
        /// Gets or sets an unknown value.
        /// </summary>
        public uint Unknown { get; set; }

        /// <summary>
        /// Gets or sets the size of the data.
        /// </summary>
        public uint DataSize { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public char[] Data { get; set; } = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderBlock"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ShaderBlock(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    Flags1 = (ShaderFlags1)br.ReadUInt32();
                    Flags2 = (ShaderFlags2)br.ReadUInt32();
                    Unknown = br.ReadUInt32();

                    DataSize = br.ReadUInt32();

                    // The data is postloaded into the shader block structure outside
                    // of the constructor.
                }
            }
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write((uint)Flags1);
                    bw.Write((uint)Flags2);
                    bw.Write(Unknown);

                    bw.Write((uint)Data.Length);
                    bw.Write(Data);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Gets the static binary size of this class, that is, the absolute size in bytes of a serialized object.
        /// </summary>
        /// <returns>The size in bytes.</returns>
        public static int GetSize()
        {
            return 16;
        }
    }
}
