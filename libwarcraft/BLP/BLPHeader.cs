//
//  BLPHeader.cs
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
using System.Collections.Generic;
using System.IO;

using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;

namespace Warcraft.BLP
{
    /// <summary>
    /// This class represents a file header for a binary BLP image. Its primary function is to
    /// map the rest of the data in a meaningful way, and describe the image format the BLP image
    /// is stored as.
    /// </summary>
    public class BLPHeader : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the the binary signature of a BLP file.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the the format of the BLP file.
        /// </summary>
        public BLPFormat Format { get; set; }

        /// <summary>
        /// Gets or sets the the version of the BLP file.
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// Gets or sets the the type of the compression used for the mipmaps stored in the BLP file.
        /// </summary>
        public TextureCompressionType CompressionType { get; set; }

        /// <summary>
        /// Gets or sets the the alpha bit depth of the mipmaps. Depends on the compression type.
        /// </summary>
        public uint AlphaBitDepth { get; set; }

        /// <summary>
        /// Gets or sets the the pixel format of the compressed textures.
        /// </summary>
        public BLPPixelFormat PixelFormat { get; set; }

        /// <summary>
        /// Gets or sets the the type of the mip map stored in the image.
        /// </summary>
        public uint MipMapType { get; set; }

        /// <summary>
        /// Gets or sets the the resolution of the image.
        /// </summary>
        public Resolution Resolution { get; set; }

        /// <summary>
        /// Gets or sets the a list of offsets to the start of each mipmap.
        /// </summary>
        public List<uint> MipMapOffsets { get; set; } = new List<uint>();

        /// <summary>
        /// Gets or sets the a list of sizes for each mipmap.
        /// </summary>
        public List<uint> MipMapSizes { get; set; } = new List<uint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.BLP.BLPHeader"/> class.
        /// This constructor creates a header from input data read from a BLP file.
        /// Usually, this is 148 bytes.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public BLPHeader(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Signature = new string(br.ReadChars(4));

                    if (Enum.TryParse(Signature, out BLPFormat format))
                    {
                        Format = format;

                        if (Format == BLPFormat.BLP2)
                        {
                            Version = (uint)br.ReadInt32();
                        }

                        if (Format == BLPFormat.BLP2)
                        {
                            CompressionType = (TextureCompressionType)br.ReadByte();
                        }
                        else
                        {
                            CompressionType = (TextureCompressionType)br.ReadUInt32();
                        }

                        if (Format == BLPFormat.BLP2)
                        {
                            AlphaBitDepth = br.ReadByte();
                        }
                        else
                        {
                            AlphaBitDepth = br.ReadUInt32();
                        }

                        // BLP0 & BLP1 stores the resolution here
                        if (Format < BLPFormat.BLP2)
                        {
                            Resolution = new Resolution(br.ReadUInt32(), br.ReadUInt32());
                        }

                        if (Format == BLPFormat.BLP2)
                        {
                            PixelFormat = (BLPPixelFormat)br.ReadByte();
                        }
                        else
                        {
                            PixelFormat = (BLPPixelFormat)br.ReadUInt32();
                        }

                        if (Format == BLPFormat.BLP2)
                        {
                            MipMapType = br.ReadByte();
                        }
                        else
                        {
                            MipMapType = br.ReadUInt32();
                        }

                        // BLP2 stores the resolution here
                        if (Format == BLPFormat.BLP2)
                        {
                            Resolution = new Resolution(br.ReadUInt32(), br.ReadUInt32());
                        }

                        MipMapOffsets = new List<uint>();
                        for (int i = 0; i < 16; ++i)
                        {
                            uint offset = br.ReadUInt32();
                            if (offset > 0)
                            {
                                MipMapOffsets.Add(offset);
                            }
                        }

                        MipMapSizes = new List<uint>();
                        for (int i = 0; i < 16; ++i)
                        {
                            uint size = br.ReadUInt32();
                            if (size > 0)
                            {
                                MipMapSizes.Add(size);
                            }
                        }
                    }
                    else
                    {
                        throw new FileLoadException("The provided data did not have a BLP signature.");
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.BLP.BLPHeader"/> class.
        /// This constructor creates a template header, ready to be filled with data about a new file.
        /// </summary>
        public BLPHeader()
        {
            Signature = "BLP2";
            Version = 1;
        }

        /// <summary>
        /// Gets the number of mipmaps defined in the header.
        /// </summary>
        /// <returns>The number of mipmaps.</returns>
        public int GetNumMipMaps()
        {
            return MipMapOffsets.Count;
        }

        /// <summary>
        /// Gets the data in the header as a byte array, ready to be written to a file.
        /// </summary>
        /// <returns>The header bytes.</returns>
        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (MemoryStream headerStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(headerStream))
                {
                    bw.Write(Signature.ToCharArray());
                    bw.Write(Version);
                    bw.Write((byte)CompressionType);
                    bw.Write((byte)AlphaBitDepth);
                    bw.Write((byte)PixelFormat);
                    bw.Write((byte)MipMapType);

                    bw.Write(Resolution.X);
                    bw.Write(Resolution.Y);

                    for (int i = 0; i < 16; ++i)
                    {
                        if (i < MipMapOffsets.Count)
                        {
                            // Write a value
                            bw.Write(MipMapOffsets[i]);
                        }
                        else
                        {
                            // Write a 0
                            bw.Write(0U);
                        }
                    }

                    for (int i = 0; i < 16; ++i)
                    {
                        if (i < MipMapSizes.Count)
                        {
                            // Write a value
                            bw.Write(MipMapSizes[i]);
                        }
                        else
                        {
                            // Write a 0
                            bw.Write(0U);
                        }
                    }

                    // Finished writing data
                    bw.Flush();

                    return headerStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Gets the size of a complete header. This is always 148 bytes, since the offset and size lists are padded.
        /// </summary>
        /// <returns>The size.</returns>
        public uint GetSize()
        {
            if (Signature == "BLP2")
            {
                return 148;
            }
            else if (Signature == "BLP1")
            {
                return 156;
            }
            else
            {
                return 0;
            }
        }
    }
}
