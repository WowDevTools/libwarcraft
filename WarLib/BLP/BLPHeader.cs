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

		public List<uint> mipMapOffsets = new List<uint>();

		public List<uint> mipMapSizes = new List<uint>();

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

			this.mipMapOffsets = new List<uint>();
			for (int i = 0; i < 16; ++i)
			{
				uint offset = br.ReadUInt32();
				if (offset > 0)
				{
					this.mipMapOffsets.Add(offset);
				}
			}

			this.mipMapSizes = new List<uint>();
			for (int i = 0; i < 16; ++i)
			{
				uint size = br.ReadUInt32();
				if (size > 0)
				{
					this.mipMapSizes.Add(size);
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
		public byte[] GetBytes()
		{
			MemoryStream headerStream = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(headerStream);

			bw.Write(this.fileType.ToCharArray());
			bw.Write(this.version);
			bw.Write((byte)this.compressionType);
			bw.Write((byte)this.alphaBitDepth);
			bw.Write((byte)this.pixelFormat);
			bw.Write((byte)this.mipMapType);

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

