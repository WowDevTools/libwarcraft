//
//  TextureBlobVersion.cs
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

namespace Warcraft.TEX
{
    /// <summary>
    /// Version header for a texture blob file.
    /// </summary>
    public class TextureBlobVersion : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// The RIFF chunk signature of this data chunk.
        /// </summary>
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "TXVR";

        /// <summary>
        /// The version of this texture blob object.
        /// </summary>
        public uint Version;

        /// <summary>
        /// Deserialzes the provided binary data of the object. This is the full data block which follows the data
        /// signature and data block length.
        /// </summary>
        /// <param name="inData">The binary data containing the object.</param>
        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Version = br.ReadUInt32();
                }
            }
        }

        /// <summary>
        /// Gets the static data signature of this data block type.
        /// </summary>
        /// <returns>A string representing the block signature.</returns>
        public string GetSignature()
        {
            return Signature;
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
                    bw.Write(Version);
                }

                return ms.ToArray();
            }
        }
    }
}
