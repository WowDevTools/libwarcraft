//
//  BLS.cs
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

using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.BLS
{
    /// <summary>
    /// The BLS (assumed to be shorthand for Blizzard Shader) class represents a set of shaders, stored in binary
    /// format. Each shader has a shader block with some information and data, and each BLS file can contain multiple
    /// shaders for different architectures and configurations.
    /// </summary>
    public class BLS : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the header of the BLS file.
        /// </summary>
        public BLSHeader Header { get; set; }

        /// <summary>
        /// Gets or sets a list containing all the shader blocks in this shader container.
        /// </summary>
        public List<ShaderBlock> Shaders { get; set; } = new List<ShaderBlock>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BLS"/> class.
        /// </summary>
        /// <param name="inData">The binary data containing the BLS file.</param>
        public BLS(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    Header = new BLSHeader(br.ReadBytes(BLSHeader.GetSize()));

                    for (int i = 0; i < Header.ShaderBlockCount; ++i)
                    {
                        var shaderBlock = new ShaderBlock(br.ReadBytes(ShaderBlock.GetSize()));
                        shaderBlock.Data = br.ReadChars((int)shaderBlock.DataSize);

                        Shaders.Add(shaderBlock);

                        if ((ms.Position % 4) == 0)
                        {
                            continue;
                        }

                        long padCount = 4 - (ms.Position % 4);
                        ms.Position += padCount;
                    }
                }
            }
        }

        /// <summary>
        /// Serializes the current object into a byte array.
        /// </summary>
        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Header.Serialize());
                    foreach (var shaderBlock in Shaders)
                    {
                        bw.Write(shaderBlock.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
