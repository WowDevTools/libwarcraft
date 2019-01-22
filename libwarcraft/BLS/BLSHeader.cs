//
//  BLSHeader.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.BLS
{
    /// <summary>
    /// Header class for a BLS file. This header provides information about the shaders contained in the
    /// BLS file.
    /// </summary>
    public class BLSHeader : IBinarySerializable
    {
        /// <summary>
        /// Binary data signature for a vertex shader container.
        /// </summary>
        public const string VertexShaderSignature = "GXVS";

        /// <summary>
        /// Binary data signature for a fragment shader container.
        /// </summary>
        public const string FragmentShaderSignature = "GXPS";

        /// <summary>
        /// Gets or sets the type of the container, that is, vertex or fragment.
        /// </summary>
        public ShaderContainerType ContainerType { get; set; }

        /// <summary>
        /// Gets or sets the binary format version of the BLS file.
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// Gets or sets the number of shader blocks stored in the file.
        /// </summary>
        public uint ShaderBlockCount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BLSHeader"/> class.
        /// </summary>
        /// <param name="inData">The binary data containing the object.</param>
        /// <exception cref="FileLoadException">Thrown if no matching signatures could be found in the data.</exception>
        public BLSHeader(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    string dataSignature = br.ReadBinarySignature();
                    if (dataSignature != VertexShaderSignature && dataSignature != FragmentShaderSignature)
                    {
                        throw new FileLoadException("BLS data must begin with a valid shader signature.");
                    }

                    if (dataSignature == VertexShaderSignature)
                    {
                        ContainerType = ShaderContainerType.Vertex;
                    }
                    else
                    {
                        ContainerType = ShaderContainerType.Fragment;
                    }

                    Version = br.ReadUInt32();
                    ShaderBlockCount = br.ReadUInt32();
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
                    if (ContainerType == ShaderContainerType.Vertex)
                    {
                        bw.WriteChunkSignature(VertexShaderSignature);
                    }
                    else
                    {
                        bw.WriteChunkSignature(FragmentShaderSignature);
                    }

                    bw.Write(Version);
                    bw.Write(ShaderBlockCount);
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
            return 12;
        }
    }
}
