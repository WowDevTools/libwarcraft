using System;
using System.Collections.Generic;

using System.Drawing;
using System.IO;

using WarLib.Core;
using DotSquish;
using System.Drawing.Imaging;

namespace WarLib.BLP
{
	public class BLP
	{
		public BLPHeader Header;

		private readonly List<Bitmap> MipMaps;
		private readonly List<Color> Palette;

		private readonly List<byte[]> RawMipMaps;

		public BLP(byte[] data)
		{
			BinaryReader br = new BinaryReader(new MemoryStream(data));
			this.Header = new BLPHeader(br.ReadBytes(148));

			if (Header.compressionType == TextureCompressionType.Palettized)
			{
				Palette = new List<Color>();
				for (int i = 0; i < 256; ++i)
				{
					byte B = br.ReadByte();
					byte G = br.ReadByte();
					byte R = br.ReadByte();

					// Ignore the alpha. We'll be reading this later.
					byte A = br.ReadByte();
					Color paletteColor = Color.FromArgb(A, R, G, B);
					Palette.Add(paletteColor);
				}
			}

			RawMipMaps = new List<byte[]>();
			for (int i = 0; i < Header.GetNumMipMaps(); ++i)
			{
				br.BaseStream.Position = Header.mipMapOffsets[i];
				RawMipMaps.Add(br.ReadBytes((int)Header.mipMapSizes[i]));
			}

			MipMaps = new List<Bitmap>();
			for (int i = 0; i < RawMipMaps.Count; ++i)
			{
				byte[] rawMip = RawMipMaps[i];
				MipMaps.Add(DecompressMipMap(rawMip, (int)Math.Pow(2, i)));
			}

			br.Close();
		}

		/// <summary>
		/// Gets a bitmap representing the given mipmap level.
		/// </summary>
		/// <returns>A bitmap.</returns>
		/// <param name="level">Mipmap level.</param>
		public Bitmap GetMipMap(int level)
		{			
			return MipMaps[level];
		}

		/// <summary>
		/// Decompresses a mipmap in the file at the specified level from the specified data.
		/// </summary>
		/// <returns>The mipmap.</returns>
		/// <param name="data">Data containing the mipmap level.</param>
		/// <param name="mipLevel">The mipmap level of the data</param>
		private Bitmap DecompressMipMap(byte[] data, int mipLevel)
		{
			Bitmap map = null;	
			int XResolution = Header.resolution.X / mipLevel;
			int YResolution = Header.resolution.Y / mipLevel;

			if (data.Length > 0 && XResolution > 0 && YResolution > 0)
			{
				if (Header.compressionType == TextureCompressionType.Palettized)
				{
					map = new Bitmap(XResolution, YResolution, PixelFormat.Format32bppArgb);
					BinaryReader br = new BinaryReader(new MemoryStream(data));

					// Read colour information
					for (int y = 0; y < YResolution; ++y)
					{
						for (int x = 0; x < XResolution; ++x)
						{
							byte colorIndex = br.ReadByte();
							Color paletteColor = Palette[colorIndex];                           
							map.SetPixel(x, y, paletteColor);
						}
					}

					long bytesRead = br.BaseStream.Position;

					// Read Alpha information
					for (int y = 0; y < YResolution; ++y)
					{
						for (int x = 0; x < XResolution; ++x)
						{
							Color pixelColor = map.GetPixel(x, y);
							byte alphaValue = br.ReadByte();
							Color finalPixel = Color.FromArgb(alphaValue, pixelColor.R, pixelColor.G, pixelColor.B);

							map.SetPixel(x, y, finalPixel);
						}
					}
				}
				else if (Header.compressionType == TextureCompressionType.DXTC)
				{     
					SquishOptions squishOptions = SquishOptions.DXT1;
					if (Header.pixelFormat == BLPPixelFormat.Pixel_DXT3)
					{
						squishOptions = SquishOptions.DXT3;
					}
					else if (Header.pixelFormat == BLPPixelFormat.Pixel_DXT5)
					{
						squishOptions = SquishOptions.DXT5;
					}

					map = (Bitmap)Squish.DecompressToBitmap(data, XResolution, YResolution, squishOptions);
				}
				else if (Header.compressionType == TextureCompressionType.Uncompressed)
				{
					map = new Bitmap(XResolution, YResolution, PixelFormat.Format32bppArgb);
					BinaryReader br = new BinaryReader(new MemoryStream(data));

					for (int y = 0; y < YResolution; ++y)
					{
						for (int x = 0; x < XResolution; ++x)
						{
							byte A = br.ReadByte();
							byte R = br.ReadByte();					
							byte G = br.ReadByte();
							byte B = br.ReadByte();
																
							Color pixelColor = Color.FromArgb(A, R, G, B);
							map.SetPixel(x, y, pixelColor);
						}
					}
				}
			}		

			return map;
		}

		/// <summary>
		/// Gets the magic string that identifies this file.
		/// </summary>
		/// <returns>The magic string.</returns>
		public string GetFileType()
		{
			return Header.fileType;
		}

		/// <summary>
		/// Gets the version of the BLP file.
		/// </summary>
		/// <returns>The version of the file.</returns>
		public uint GetVersion()
		{
			return Header.version;
		}

		/// <summary>
		/// Gets the BLP pixel format. This format represents a subtype of the compression used in the file.
		/// </summary>
		/// <returns>The pixel format.</returns>
		public BLPPixelFormat GetPixelFormat()
		{
			return Header.pixelFormat;
		}

		/// <summary>
		/// Gets the resolution of the image.
		/// </summary>
		/// <returns>The resolution.</returns>
		public Resolution GetResolution()
		{
			return Header.resolution;
		}

		/// <summary>
		/// Gets the type of compression used in the image.
		/// </summary>
		/// <returns>The compression type.</returns>
		public TextureCompressionType GetCompressionType()
		{
			return Header.compressionType;
		}

		/// <summary>
		/// Gets the alpha bit depth. This value represents where the alpha value for each pixel is stored.
		/// </summary>
		/// <returns>The alpha bit depth.</returns>
		public int GetAlphaBitDepth()
		{
			return Header.alphaBitDepth;
		}

		/// <summary>
		/// Gets the number of mipmap levels in the image.
		/// </summary>
		/// <returns>The mipmap count.</returns>
		public int GetMipMapCount()
		{
			return MipMaps.Count;
		}
	}
}
