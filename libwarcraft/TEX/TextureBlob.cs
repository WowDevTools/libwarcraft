//
//  TextureBlob.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.TEX
{
    /// <summary>
    /// A texture blob file. This file stores a set of extremely low-resolution textures for use as
    /// placeholder textures if the real texture has not yet been streamed in or loaded.
    /// </summary>
    public class TextureBlob : IBinarySerializable
    {
        /// <summary>
        /// The version chunk of the texture blob.
        /// </summary>
        public TextureBlobVersion Version;

        /// <summary>
        /// A list of all the header chunks of the texture data chunks. These headers contain all the information neccesary to
        /// read the rest of the data contained in the file.
        /// </summary>
        public TextureBlobDataEntries BlobDataEntries;

        /// <summary>
        /// The filename chunk of the texture blob. Contains all of the filenames for which this blob provides
        /// a fallback texture.
        /// </summary>
        public TextureBlobFilenames Filenames;

        /// <summary>
        /// A list of all the texture data chunks in this texture blob.
        /// </summary>
        public readonly List<TextureBlobData> TextureData = new List<TextureBlobData>();

        /// <summary>
        /// Deserializes a <see cref="TextureBlob"/> object from binary data.
        /// </summary>
        /// <param name="inData"></param>
        public TextureBlob(byte[] inData)
        {
            if (inData == null)
            {
                throw new InvalidDataException("The input data may not be null.");
            }

            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Version = br.ReadIFFChunk<TextureBlobVersion>();
                    BlobDataEntries = br.ReadIFFChunk<TextureBlobDataEntries>();
                    Filenames = br.ReadIFFChunk<TextureBlobFilenames>();

                    long dataBlockStartingPosition = br.BaseStream.Position;
                    foreach (TextureBlobDataEntry blobDataEntry in BlobDataEntries.BlobDataEntries)
                    {
                        br.BaseStream.Position = dataBlockStartingPosition + blobDataEntry.TextureDataOffset;
                        TextureData.Add(br.ReadIFFChunk<TextureBlobData>());
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
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.WriteIFFChunk(Version);
                    bw.WriteIFFChunk(BlobDataEntries);
                    bw.WriteIFFChunk(Filenames);

                    foreach (TextureBlobData textureData in TextureData)
                    {
                        bw.WriteIFFChunk(textureData);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
