using System;
using System.IO;
using WarLib.Core;
using System.Collections.Generic;

namespace WarLib.BLP
{
	public class BLPHeader
	{
		public string fileType;

		public uint version;

		public TextureCompressionType compressionType;

		public int alphaBitDepth;

		public BLPPixelFormat pixelFormat;
		public int mipMapType;

		public Resolution resolution;

		public List<uint> mipMapOffsets;

		public List<uint> mipMapSizes;

		/// <summary>
		/// Initializes a new instance of the <see cref="WarLib.BLP.BLPHeader"/> class.
		/// This constructor creates a header from input data read from a BLP file.
		/// Usually, this is 148 bytes.
		/// </summary>
		/// <param name="data">Data.</param>
		public BLPHeader(byte[] data)
		{				
			BinaryReader br = new BinaryReader(new MemoryStream(data));
			this.fileType = new string(br.ReadChars(4));
			this.version = (uint)br.ReadInt32();

			this.compressionType = (TextureCompressionType)br.ReadByte();

			this.alphaBitDepth = br.ReadByte();
			this.pixelFormat = (BLPPixelFormat)br.ReadByte();
			this.mipMapType = br.ReadByte();
			this.resolution = new Resolution(br.ReadUInt32(), br.ReadUInt32());

			mipMapOffsets = new List<uint>();
			for (int i = 0; i < 16; ++i)
			{
				uint offset = br.ReadUInt32();
				if (offset > 0)
				{
					mipMapOffsets.Add(offset);
				}
			}

			mipMapSizes = new List<uint>();
			for (int i = 0; i < 16; ++i)
			{
				uint size = br.ReadUInt32();
				if (size > 0)
				{
					mipMapSizes.Add(size);
				}
			}

			br.Close();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WarLib.BLP.BLPHeader"/> class.
		/// This constructor creates a template header, ready to be filled with data about a new file.
		/// </summary>
		public BLPHeader()
		{
			this.fileType = "BLP2";
			this.version = 1;
		}

		/// <summary>
		/// Gets the number of mipmaps defined in the header.
		/// </summary>
		/// <returns>The number of mipmaps.</returns>
		public int GetNumMipMaps()
		{
			return mipMapOffsets.Count;
		}

		/// <summary>
		/// Gets the data in the header as a byte array, ready to be written to a file.
		/// </summary>
		/// <returns>The header bytes.</returns>
		public byte[] GetHeaderBytes()
		{
			MemoryStream headerStream = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(headerStream);

			bw.Write(BitConverter.GetBytes(this.fileType));
			bw.Write(this.version);
			bw.Write((byte)this.compressionType);
			bw.Write(this.alphaBitDepth);
			bw.Write((byte)this.pixelFormat);
			bw.Write(this.mipMapType);

			bw.Write(this.resolution.X);
			bw.Write(this.resolution.Y);

			for (int i = 0; i < 16; ++i)
			{
				if (i < this.mipMapOffsets.Count)
				{
					// Write a value
					bw.Write(this.mipMapOffsets[i]);
				}
				else
				{
					// Write a 0
					bw.Write((uint)0);
				}
			}

			for (int i = 0; i < 16; ++i)
			{
				if (i < this.mipMapSizes.Count)
				{
					// Write a value
					bw.Write(this.mipMapSizes[i]);
				}
				else
				{
					// Write a 0
					bw.Write((uint)0);
				}
			}

			// Finished writing data
			bw.Flush();

			byte[] headerBytes = headerStream.ToArray();
			bw.Close();
			bw.Dispose();
			headerStream.Dispose();

			return headerBytes;
		}
	}
}

