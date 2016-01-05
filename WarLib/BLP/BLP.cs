using System;
using System.Collections.Generic;

using System.Drawing;
using System.IO;

using WarLib.Core;
using DotSquish;
using System.Drawing.Imaging;
using nQuant;

namespace WarLib.BLP
{
	public class BLP
	{
		public BLPHeader Header;

		private List<Bitmap> MipMaps;
		private List<Color> Palette;

		private List<byte[]> RawMipMaps;

		/// <summary>
		/// Initializes a new instance of the <see cref="WarLib.BLP.BLP"/> class.
		/// This constructor reads a binary BLP file from disk.
		/// </summary>
		/// <param name="data">Data.</param>
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
				MipMaps.Add(DecompressMipMap(rawMip, (uint)Math.Pow(2, i)));
			}

			br.Close();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WarLib.BLP.BLP"/> class.
		/// This constructor creates a BLP file using the specified compression from a bitmap object.
		/// If the compression type specifed is DXTC, the default pixel format used is DXT1.
		/// </summary>
		/// <param name="Image">Image.</param>
		/// <param name="CompressionType">Compression type.</param>
		/// <param name="PixelFormat">Pixel format.</param>
		public BLP(Bitmap Image, TextureCompressionType CompressionType, BLPPixelFormat PixelFormat = BLPPixelFormat.Pixel_DXT1)
		{
			
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
		private Bitmap DecompressMipMap(byte[] data, uint mipLevel)
		{
			Bitmap map = null;	
			uint XResolution = Header.resolution.X / mipLevel;
			uint YResolution = Header.resolution.Y / mipLevel;

			if (data.Length > 0 && XResolution > 0 && YResolution > 0)
			{
				if (Header.compressionType == TextureCompressionType.Palettized)
				{
					map = new Bitmap((int)XResolution, (int)YResolution, PixelFormat.Format32bppArgb);
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
					// TODO: Split this into three loops, adjusted for the data density
					List<byte> alphaValues = new List<byte>();
					if (this.GetAlphaBitDepth() > 0)
					{
						if (this.GetAlphaBitDepth() == 1)
						{
							int alphaByteCount = (int)Math.Ceiling(((double)(XResolution * YResolution) / 8));
							for (int i = 0; i < alphaByteCount; ++i)
							{
								// The alpha value is stored per-bit in the byte (8 alpha values per byte)
								byte alphaByte = br.ReadByte();

								for (byte j = 7; j > 0; --j)
								{
									byte alphaBit = (byte)ExtensionMethods.Map((byte)((alphaByte >> i) & 0x01), 0, 1, 0, 255);

									// At this point, alphaBit will be either 0 or 1. Map this to 0 or 255.
									if (alphaBit > 0)
									{
										alphaValues.Add(255);
									}
									else
									{
										alphaValues.Add(0);
									}
								}
							}
						}
						else if (this.GetAlphaBitDepth() == 4)
						{
							int alphaByteCount = (int)Math.Ceiling(((double)(XResolution * YResolution) / 2));
							for (int i = 0; i < alphaByteCount; ++i)
							{
								// The alpha value is stored as half a byte (2 alpha values per byte)
								// Extract these two values and map them to a byte size (4 bits can hold 0 - 15 alpha)
								byte alphaByte = br.ReadByte();
										
								byte alphaValue1 = (byte)ExtensionMethods.Map((byte)(alphaByte >> 4), 0, 15, 0, 255);
								byte alphaValue2 = (byte)ExtensionMethods.Map((byte)(alphaByte & 0x0F), 0, 15, 0, 255);

								alphaValues.Add(alphaValue1);
								alphaValues.Add(alphaValue2);
							}
						}
						else if (this.GetAlphaBitDepth() == 8)
						{
							for (int i = 0; i < YResolution * XResolution; ++i)
							{
								// The alpha value is stored as a whole byte
								byte alphaValue = br.ReadByte();
								alphaValues.Add(alphaValue);
							}
							
						}
					}
					else
					{
						alphaValues.Add(255);
					}

					// Build the final map
					for (int y = 0; y < YResolution; ++y)
					{
						for (int x = 0; x < XResolution; ++x)
						{
							byte alphaValue = alphaValues[x + y];
							Color pixelColor = map.GetPixel(x, y);
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

					map = (Bitmap)Squish.DecompressToBitmap(data, (int)XResolution, (int)YResolution, squishOptions);
				}
				else if (Header.compressionType == TextureCompressionType.Uncompressed)
				{
					map = new Bitmap((int)XResolution, (int)YResolution, PixelFormat.Format32bppArgb);
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
		/// Compresses an input bitmap into a list of mipmap using the file's compression settings. 
		/// Mipmap levels which would produce an image with dimensions smaller than 1x1 will return null instead.
		/// The number of mipmaps returned will be <see cref="GetNumReasonableMipLevels"/> + 1. 
		/// </summary>
		/// <returns>The compressed image data.</returns>
		/// <param name="Image">The image to be compressed.</param>
		/// <param name="MipMapLevels">All of the compressed mipmap levels.</param>
		private List<byte[]> CompressImage(Bitmap Image)
		{
			List<byte[]> mipMaps = new List<byte[]>();

			// Generate a palette from the unmipped image for use with the mips
			if (Header.compressionType == TextureCompressionType.Palettized)
			{
				this.Palette = GeneratePalette(Image);
			}

			for (int i = 0; i <= GetNumReasonableMipMapLevels(); ++i)
			{
				mipMaps.Add(CompressImage(Image, i));
			}

			return mipMaps;
		}

		/// <summary>
		/// Compresses in input bitmap into a single mipmap at the specified mipmap level, where a mip level is a bisection of the resolution.
		/// For instance, a mip level of 2 applied to a 64x64 image would produce an image with a resolution of 16x16.	
		/// This function expects the mipmap level to be reasonable (i.e, not a level which would produce a mip smaller than 1x1)
		/// </summary>
		/// <returns>The image.</returns>
		/// <param name="Image">Image.</param>
		/// <param name="MipLevel">Mip level.</param>
		private byte[] CompressImage(Bitmap Image, int MipLevel)
		{
			// TODO: Stub function
			uint targetXRes = this.GetResolution().X / (uint)Math.Pow(2, MipLevel);
			uint targetYRes = this.GetResolution().Y / (uint)Math.Pow(2, MipLevel);

			Bitmap resizedImage = new Bitmap(Image, (int)targetXRes, (int)targetYRes);

			byte[] compressedMipMap = null;
			if (Header.compressionType == TextureCompressionType.Palettized)
			{
				byte[] paletteIndices = null;
				byte[] alphaValues = null;
				for (int y = 0; y < targetYRes; ++y)
				{
					for (int x = 0; x < targetXRes; ++x)
					{
						Color nearestColor = FindClosestMatchingColor(resizedImage.GetPixel(x, y));
						int paletteIndex = this.Palette.IndexOf(nearestColor);

						if (this.GetAlphaBitDepth() > 0)
						{
							byte pixelAlpha = resizedImage.GetPixel(x, y).A;
						}

					}
				}
			}
			else if (Header.compressionType == TextureCompressionType.DXTC)
			{
				MemoryStream rgbaStream = new MemoryStream();
				BinaryWriter bw = new BinaryWriter(rgbaStream);
				for (int y = 0; y < targetYRes; ++y)
				{
					for (int x = 0; x < targetXRes; ++x)
					{
						bw.Write(resizedImage.GetPixel(x, y).R);
						bw.Write(resizedImage.GetPixel(x, y).G);
						bw.Write(resizedImage.GetPixel(x, y).B);
						bw.Write(resizedImage.GetPixel(x, y).A);
					}
				}		

				// Finish writing the data
				bw.Flush();

				byte[] rgbaBytes = rgbaStream.ToArray();

				bw.Close();
				bw.Dispose();

				SquishOptions squishOptions = SquishOptions.DXT1;
				if (Header.pixelFormat == BLPPixelFormat.Pixel_DXT3)
				{
					squishOptions = SquishOptions.DXT3;
				}
				else if (Header.pixelFormat == BLPPixelFormat.Pixel_DXT5)
				{
					squishOptions = SquishOptions.DXT5;
				}

				compressedMipMap = Squish.CompressImage(rgbaBytes, (int)targetXRes, (int)targetYRes, squishOptions);
			}
			else if (Header.compressionType == TextureCompressionType.Uncompressed)
			{
				MemoryStream argbStream = new MemoryStream();
				BinaryWriter bw = new BinaryWriter(argbStream);
				for (int y = 0; y < targetYRes; ++y)
				{
					for (int x = 0; x < targetXRes; ++x)
					{
						bw.Write(resizedImage.GetPixel(x, y).A);
						bw.Write(resizedImage.GetPixel(x, y).R);
						bw.Write(resizedImage.GetPixel(x, y).G);
						bw.Write(resizedImage.GetPixel(x, y).B);
					}
				}		

				// Finish writing the data
				bw.Flush();

				byte[] argbBytes = argbStream.ToArray();

				bw.Close();
				bw.Dispose();

				compressedMipMap = argbBytes;
			}			

			return compressedMipMap;
		}

		/// <summary>
		/// Gets the number of mipmaps which can be produced in this file without producing a mipmap smaller than 1x1.
		/// </summary>
		/// <returns>The number of reasonable mip map levels.</returns>
		private uint GetNumReasonableMipMapLevels()
		{
			uint smallestXRes = this.GetResolution().X;
			uint smallestYRes = this.GetResolution().Y;

			uint mipLevels = 1;
			while (smallestXRes > 1 && smallestYRes > 1)
			{
				// Bisect the resolution using the current number of mip levels.
				smallestXRes = smallestXRes / (uint)Math.Pow(2, mipLevels);
				smallestYRes = smallestYRes / (uint)Math.Pow(2, mipLevels);

				++mipLevels;
			}

			return mipLevels;
		}

		/// <summary>
		/// Generates an indexed 256-color palette from the specified image.
		/// Ordinarily, this would be the original mipmap.
		/// </summary>
		/// <returns>The palette.</returns>
		/// <param name="Image">Image.</param>
		private List<Color> GeneratePalette(Bitmap Image)
		{
			WuQuantizer quantizer = new WuQuantizer();
			Bitmap quantized = (Bitmap)quantizer.QuantizeImage(Image);

			return new List<Color>(quantized.Palette.Entries);
		}

		/// <summary>
		/// Finds the closest matching color in the palette for the given input color.
		/// </summary>
		/// <returns>The closest matching color.</returns>
		/// <param name="InColor">Input color.</param>
		private Color FindClosestMatchingColor(Color InColor)
		{
			Color NearestColor = Color.Empty;

			double ColorDistance = 250000.0;
			foreach (Color PaletteColor in Palette)
			{				
				double TestRed = Math.Pow(Convert.ToDouble(PaletteColor.R) - InColor.R, 2.0);
				double TestGreen = Math.Pow(Convert.ToDouble(PaletteColor.G) - InColor.G, 2.0);
				double TestBlue = Math.Pow(Convert.ToDouble(PaletteColor.B) - InColor.B, 2.0);

				double DistanceResult = Math.Sqrt(TestBlue + TestGreen + TestRed);			

				if (DistanceResult == 0.0)
				{
					NearestColor = PaletteColor;
					break;
				}
				else if (DistanceResult < ColorDistance)
				{
					ColorDistance = DistanceResult;
					NearestColor = PaletteColor;
				}
			}

			return NearestColor;
		}

		/// <summary>
		/// Writes the image to disk as a BLP file.
		/// To write a "normal" image format to disk, retrieve a mipmap (<see cref="WarLib.BLP.BLP.GetMipMap"/>) instead.
		/// </summary>
		/// <param name="path">Path.</param>
		private void WriteImageToDisk(string path)
		{
			
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
