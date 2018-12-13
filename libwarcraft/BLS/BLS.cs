//
//  BLS.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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
        /// The header of the BLS file.
        /// </summary>
        public BLSHeader Header;

        /// <summary>
        /// A list containing all the shader blocks in this shader container.
        /// </summary>
        public List<ShaderBlock> Shaders = new List<ShaderBlock>();

        /// <summary>
        /// Creates a new instance of the <see cref="BLS"/> class from supplied binary data.
        /// </summary>
        /// <param name="inData">The binary data containing the BLS file.</param>
        public BLS(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    this.Header = new BLSHeader(br.ReadBytes(BLSHeader.GetSize()));

                    for (int i = 0; i < this.Header.ShaderBlockCount; ++i)
                    {
                        ShaderBlock shaderBlock = new ShaderBlock(br.ReadBytes(ShaderBlock.GetSize()));
                        shaderBlock.Data = br.ReadChars((int)shaderBlock.DataSize);

                        this.Shaders.Add(shaderBlock);

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
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(this.Header.Serialize());
                    foreach (ShaderBlock shaderBlock in this.Shaders)
                    {
                        bw.Write(shaderBlock.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
