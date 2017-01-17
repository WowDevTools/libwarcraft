//
//  TextureBlobHeader.cs
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

using System;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.TEX
{
	/// <summary>
	/// Header class for a texture blob object.
	/// </summary>
	public class TextureBlobHeader : IRIFFChunk, IBinarySerializable
	{
		/// <summary>
		/// The RIFF chunk signature of this data chunk.
		/// </summary>
		public const string Signature = "TXBT";

		/// <summary>
		/// The absolute offset of the filename chunk in the texture blob.
		/// </summary>
		public uint FilenameOffset;

		/// <summary>
		/// The absolute offset of the start of the texture data array in the texture blob.
		/// </summary>
		public uint TextureDataOffset;

		/// <summary>
		/// The width in pixels of the texture data.
		/// </summary>
		public byte TextureWidth;

		/// <summary>
		/// The height in pixels of the texture data.
		/// </summary>
		public byte TextureHeight;

		private byte _mipMapCount;
		/// <summary>
		/// The number of mipmaps for each texture.
		/// </summary>
		public byte MipMapCount
		{
			get
			{
				return this._mipMapCount;
			}
			set
			{
				if (value > 127)
				{
					throw new ArgumentOutOfRangeException(nameof(value),
						"The mip level count must be below 127, since it is packed into a single byte when serialized.");
				}

				this._mipMapCount = value;
			}
		}

		/// <summary>
		/// Whether or not this object has been loaded. Only set in the client.
		/// </summary>
		public bool IsLoaded;

		/// <summary>
		/// The compression used for the texture data.
		/// </summary>
		public TextureBlobCompressionType CompressionType;

		/// <summary>
		/// Flags which may modify the compression algorithms.
		/// </summary>
		public TextureBlobFlags Flags;

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
					this.FilenameOffset = br.ReadUInt32();
					this.TextureDataOffset = br.ReadUInt32();
					this.TextureWidth = br.ReadByte();
					this.TextureHeight = br.ReadByte();

					// Explode the first packed byte into mip levels and whether or not the blob is loaded
					byte mipsAndIsLoadedCombined = br.ReadByte();
					this.MipMapCount = (byte)(mipsAndIsLoadedCombined & 0b1111_1110);
					this.IsLoaded = (mipsAndIsLoadedCombined & 0b0000_0001) > 0;

					// Exploede the next packed byte into the compression type and the flags.
					byte compressionAndFlagsCombined = br.ReadByte();
					this.CompressionType = (TextureBlobCompressionType) (compressionAndFlagsCombined & 0b1111_0000);
					this.Flags = (TextureBlobFlags) (compressionAndFlagsCombined & 0b0000_1111);
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
					bw.Write(this.FilenameOffset);
					bw.Write(this.TextureDataOffset);
					bw.Write(this.TextureWidth);
					bw.Write(this.TextureHeight);

					// Pack the mip count and isLoaded state
					byte mipCount = (byte)(this.MipMapCount << 1);
					byte packedMipCountAndState = (byte)(mipCount & (this.IsLoaded ? 1 : 0));
					bw.Write(packedMipCountAndState);

					// Pack the compression type and flags
					byte packedCompressionAndFlags = (byte) ((byte)this.CompressionType & (byte)this.Flags);
					bw.Write(packedCompressionAndFlags);
				}

				return ms.ToArray();
			}
		}
	}
}