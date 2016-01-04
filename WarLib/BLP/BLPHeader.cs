using System;
using System.IO;
using WarLib.Core;
using System.Collections.Generic;

namespace WarLib.BLP
{
	public class BLPHeader
	{
		public string fileType;

		public UInt32 version;

		public TextureCompressionType compressionType;

		public int alphaBitDepth;

		public BLPPixelFormat pixelFormat;
		public int mipMapType;

		public Resolution resolution;

		public List<UInt32> mipMapOffsets;

		public List<UInt32> mipMapSizes;

		// Should be an array of 148 bytes.
		public BLPHeader(byte[] data)
		{				
			BinaryReader br = new BinaryReader(new MemoryStream(data));
			this.fileType = new string(br.ReadChars(4));
			this.version = (UInt32)br.ReadInt32();

			this.compressionType = (TextureCompressionType)br.ReadByte();

			this.alphaBitDepth = br.ReadByte();
			this.pixelFormat = (BLPPixelFormat)br.ReadByte();
			this.mipMapType = br.ReadChar();
			this.resolution = new Resolution(br.ReadUInt32(), br.ReadUInt32());

			mipMapOffsets = new List<UInt32>();
			for (int i = 0; i < 16; ++i)
			{
				UInt32 offset = br.ReadUInt32();
				if (offset > 0)
				{
					mipMapOffsets.Add(offset);
				}
			}

			mipMapSizes = new List<UInt32>();
			for (int i = 0; i < 16; ++i)
			{
				UInt32 size = br.ReadUInt32();
				if (size > 0)
				{
					mipMapSizes.Add(size);
				}
			}

			br.Close();
		}

		/// <summary>
		/// Gets the number of mipmaps defined in the header.
		/// </summary>
		/// <returns>The number of mipmaps.</returns>
		public int GetNumMipMaps()
		{
			return mipMapOffsets.Count;
		}
	}
}

