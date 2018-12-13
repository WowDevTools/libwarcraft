//
//  TextureBlobDataEntries.cs
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

namespace Warcraft.TEX
{
    /// <summary>
    /// Holds a set of <see cref="TextureBlobDataEntry"/> objects. This acts as a reference table.
    /// </summary>
    public class TextureBlobDataEntries : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// The RIFF chunk signature of this data chunk.
        /// </summary>
        public const string Signature = "TXBT";

        /// <summary>
        /// A list of all blob data headers contained in the texture blob.
        /// </summary>
        public readonly List<TextureBlobDataEntry> BlobDataEntries = new List<TextureBlobDataEntry>();

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
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        this.BlobDataEntries.Add(new TextureBlobDataEntry(br.ReadBytes(TextureBlobDataEntry.GetSize())));
                    }
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
                    foreach (TextureBlobDataEntry blobDataEntry in this.BlobDataEntries)
                    {
                        bw.Write(blobDataEntry.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
