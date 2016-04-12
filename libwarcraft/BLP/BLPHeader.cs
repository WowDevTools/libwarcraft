//
//  BLPHeader.cs
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

using System.IO;
using Warcraft.Core;
using System.Collections.Generic;

namespace Warcraft.BLP
{
	/// <summary>
	/// This class represents a file header for a binary BLP image. Its primary function is to 
	/// map the rest of the data in a meaningful way, and describe the image format the BLP image
	/// is stored as.
	/// </summary>
	public class BLPHeader
	{
		/// <summary>
		/// The binary signature of a BLP file.
		/// </summary>
		public string Signature;

		/// <summary>
		/// The version of the BLP file.
		/// </summary>
		public uint Version;

		/// <summary>
		/// The type of the compression used for the mipmaps stored in the BLP file.
		/// </summary>
		public TextureCompressionType CompressionType;

		/// <summary>
		/// The alpha bit depth of the mipmaps. Depends on the compression type.
		/// </summary>
		public int AlphaBitDepth;

		/// <summary>
		/// The pixel format of the compressed textures.
		/// </summary>
		public BLPPixelFormat PixelFormat;

		/// <summary>
		/// The type of the mip map stored in the image.
		/// </summary>
		public int MipMapType;

		/// <summary>
		/// The resolution of the image.
		/// </summary>
		public Resolution Resolution;

		/// <summary>
		/// A list of offsets to the start of each mipmap.
		/// </summary>
		public List<uint> MipMapOffsets = new List<uint>();

		/// <summary>
		/// A list of sizes for each mipmap.
		/// </summary>
		public List<uint> MipMapSizes = new List<uint>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.BLP.BLPHeader"/> class.
		/// This constructor creates a header from input data read from a BLP file.
		/// Usually, this is 148 bytes.
		/// </summary>
		/// <param name="InData">Data.</param>
		public BLPHeader(byte[] InData)
		{	
			using (MemoryStream ms = new MemoryStream(InData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Signature = new string(br.ReadChars(4));

					if (Signature != "BLP2")
					{
						throw new FileLoadException("The provided data did not have a BLP signature.");
					}

					this.Version = (uint)br.ReadInt32();

					this.CompressionType = (TextureCompressionType)br.ReadByte();

					this.AlphaBitDepth = br.ReadByte();
					this.PixelFormat = (BLPPixelFormat)br.ReadByte();
					this.MipMapType = br.ReadByte();
					this.Resolution = new Resolution(br.ReadUInt32(), br.ReadUInt32());

					this.MipMapOffsets = new List<uint>();
					for (int i = 0; i < 16; ++i)
					{
						uint offset = br.ReadUInt32();
						if (offset > 0)
						{
							this.MipMapOffsets.Add(offset);
						}
					}

					this.MipMapSizes = new List<uint>();
					for (int i = 0; i < 16; ++i)
					{
						uint size = br.ReadUInt32();
						if (size > 0)
						{
							this.MipMapSizes.Add(size);
						}
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
			this.Signature = "BLP2";
			this.Version = 1;
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
		public byte[] ToByteArray()
		{
			byte[] headerBytes = null;

			using (MemoryStream headerStream = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(headerStream))
				{
					bw.Write(this.Signature.ToCharArray());
					bw.Write(this.Version);
					bw.Write((byte)this.CompressionType);
					bw.Write((byte)this.AlphaBitDepth);
					bw.Write((byte)this.PixelFormat);
					bw.Write((byte)this.MipMapType);

					bw.Write(this.Resolution.X);
					bw.Write(this.Resolution.Y);

					for (int i = 0; i < 16; ++i)
					{
						if (i < this.MipMapOffsets.Count)
						{
							// Write a value
							bw.Write(this.MipMapOffsets[i]);
						}
						else
						{
							// Write a 0
							bw.Write((uint)0);
						}
					}

					for (int i = 0; i < 16; ++i)
					{
						if (i < this.MipMapSizes.Count)
						{
							// Write a value
							bw.Write(this.MipMapSizes[i]);
						}
						else
						{
							// Write a 0
							bw.Write((uint)0);
						}
					}

					// Finished writing data
					bw.Flush();

					headerBytes = headerStream.ToArray();
				}
			}

			return headerBytes;
		}

		/// <summary>
		/// Gets the size of a complete header. This is always 148 bytes, since the offset and size lists are padded.
		/// </summary>
		/// <returns>The size.</returns>
		public uint GetSize()
		{
			return 148;
		}
	}
}

