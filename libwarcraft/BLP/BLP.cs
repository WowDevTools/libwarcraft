//
//  BLP.cs
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
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;
using System.Collections.Generic;

using Squish;
using Warcraft.Core;
using Warcraft.Core.Quantization;
using System.Drawing.Drawing2D;

namespace Warcraft.BLP
{
	/// <summary>
	/// This class represents a BLP binary image and its contained data.
	/// </summary>
	public class BLP
	{
		/// <summary>
		/// The header. This header contains data about the mipmaps stored in the BLP,
		/// and storage information such as offsets and sizes.
		/// </summary>
		public BLPHeader Header;

		/// <summary>
		/// The palette of colours used in the BLP image. This is not used for DXTC-compressed
		/// textures.
		/// </summary>
		private readonly List<Color> Palette = new List<Color>();

		/// <summary>
		/// The size of the JPEG header. This is not used for palettized or DXTC-compressed
		/// textures.
		/// </summary>
		private readonly uint JPEGHeaderSize;

		/// <summary>
		/// The JPEG header. This is not used for palettized or DXTC-compressed
		/// textures.
		/// </summary>
		private readonly byte[] JPEGHeader;

		/// <summary>
		/// A list of byte arrays containing the compressed mipmaps.
		/// </summary>
		private readonly List<byte[]> RawMipMaps = new List<byte[]>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.BLP.BLP"/> class.
		/// </summary>
		/// <param name="inData">Data.</param>
		public BLP(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					byte[] fileHeaderBytes;
					if (PeekFormat(br) == BLPFormat.BLP2)
					{
						fileHeaderBytes = br.ReadBytes(148);
					}
					else
					{
						fileHeaderBytes = br.ReadBytes(156);
					}

					this.Header = new BLPHeader(fileHeaderBytes);

					if (this.Header.CompressionType == TextureCompressionType.JPEG)
					{
						this.JPEGHeaderSize = br.ReadUInt32();
						this.JPEGHeader = br.ReadBytes((int)this.JPEGHeaderSize);
					}
					else if (this.Header.CompressionType == TextureCompressionType.Palettized)
					{
						for (int i = 0; i < 256; ++i)
						{
							byte b = br.ReadByte();
							byte g = br.ReadByte();
							byte r = br.ReadByte();

							// The alpha in the palette is not used, but is stored for the sake of completion.
							byte a = br.ReadByte();

							Color paletteColor = Color.FromArgb(a, r, g, b);
							this.Palette.Add(paletteColor);
						}
					}
					else
					{
						// Fill up an empty palette - the palette is always present, but we'll be going after offsets anyway
						for (int i = 0; i < 256; ++i)
						{
							Color paletteColor = Color.FromArgb(0, 0, 0, 0);
							this.Palette.Add(paletteColor);
						}
					}

					// Read the raw mipmap data
					for (int i = 0; i < this.Header.GetNumMipMaps(); ++i)
					{
						br.BaseStream.Position = this.Header.MipMapOffsets[i];
						this.RawMipMaps.Add(br.ReadBytes((int) this.Header.MipMapSizes[i]));
					}
				}
			}
		}

		/// <summary>
		/// Peeks the format of the current file.
		/// </summary>
		/// <returns>The format.</returns>
		/// <param name="br">Br.</param>
		/// <exception cref="FileLoadException">If no format was detected, a FileLoadException will be thrown.</exception>
		private static BLPFormat PeekFormat(BinaryReader br)
		{
			long startPosition = br.BaseStream.Position;
			string dataSignature = new string(br.ReadChars(4));

			BLPFormat Format;
			if (Enum.TryParse(dataSignature, out Format))
			{
				br.BaseStream.Position = startPosition;
				return Format;
			}

			throw new FileLoadException("The provided data did not have a BLP signature.");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.BLP.BLP"/> class.
		/// This constructor creates a BLP file using the specified compression from a bitmap object.
		/// If the compression type specifed is DXTC, the default pixel format used is DXT1 for opaque textures and DXT3 for the rest.
		/// </summary>
		/// <param name="image">Image.</param>
		/// <param name="compressionType">Compression type.</param>
		public BLP(Bitmap image, TextureCompressionType compressionType)
		{
			// Set up the header
			this.Header = new BLPHeader
			{
				CompressionType = compressionType
			};

			if (compressionType == TextureCompressionType.Palettized)
			{
				this.Header.PixelFormat = BLPPixelFormat.Palettized;
				// Determine best alpha bit depth
				if (image.HasAlpha())
				{
					List<byte> alphaLevels = new List<byte>();
					for (int y = 0; y < image.Height; ++y)
					{
						for (int x = 0; x < image.Width; ++x)
						{
							Color pixel = image.GetPixel(x, y);
							if (!alphaLevels.Contains(pixel.A))
							{
								alphaLevels.Add(pixel.A);
							}

							if (alphaLevels.Count > 16)
							{
								break;
							}
						}
					}

					if (alphaLevels.Count > 16)
					{
						// More than 16? Use a full byte
						this.Header.AlphaBitDepth = 8;
					}
					else if (alphaLevels.Count > 2)
					{
						// More than 2, but less than or equal to 16? Use half a byte
						this.Header.AlphaBitDepth = 4;
					}
					else
					{
						// Just 2? Use a bit instead
						this.Header.AlphaBitDepth = 1;
					}
				}
				else
				{
					// No alpha, so a bit depth of 0.
					this.Header.AlphaBitDepth = 0;
				}
			}
			else if (compressionType == TextureCompressionType.DXTC)
			{
				this.Header.AlphaBitDepth = 8;

				// Determine best DXTC type (1, 3 or 5)
				if (image.HasAlpha())
				{
					this.Header.PixelFormat = BLPPixelFormat.DXT3;
				}
				else
				{
					// DXT1 for no alpha
					this.Header.PixelFormat = BLPPixelFormat.DXT1;
				}
			}
			else if (compressionType == TextureCompressionType.Uncompressed)
			{
				// The alpha will be stored as a straight ARGB texture, so set it to 8
				this.Header.AlphaBitDepth = 8;
				this.Header.PixelFormat = BLPPixelFormat.PalARGB1555DitherFloydSteinberg;
			}

			// What the mip type does is currently unknown, but it's usually set to 1.
			this.Header.MipMapType = 1;
			this.Header.Resolution = new Resolution((uint)image.Width, (uint)image.Height);

			// It's now time to compress the image
			this.RawMipMaps = CompressImage(image);

			// Calculate the offsets and sizes
			uint mipOffset = (uint)(this.Header.GetSize() + this.Palette.Count * 4);
			foreach (byte[] rawMipMap in this.RawMipMaps)
			{
				uint mipSize = (uint)rawMipMap.Length;

				this.Header.MipMapOffsets.Add(mipOffset);
				this.Header.MipMapSizes.Add(mipSize);

				// Push the offset ahead for the next mipmap
				mipOffset += mipSize;
			}
		}

		/// <summary>
		/// Gets the format of the BLP image.
		/// </summary>
		/// <returns>The format.</returns>
		public BLPFormat GetFormat()
		{
			return this.Header.Format;
		}

		/// <summary>
		/// Gets a list of formatted strings describing the mipmap levels.
		/// </summary>
		/// <returns>The mip map level strings.</returns>
		public List<string> GetMipMapLevelStrings()
		{
			List<string> mipStrings = new List<string>();
			for (uint i = 0; i < GetMipMapCount(); ++i)
			{
				mipStrings.Add($"{i}: {GetMipLevelResolution(i)}");
			}

			return mipStrings;
		}

		/// <summary>
		/// Gets the resolution of the specified mip level
		/// </summary>
		/// <returns>The mip level resolution.</returns>
		/// <param name="mipLevel">Mip level.</param>
		public Resolution GetMipLevelResolution(uint mipLevel)
		{
			uint targetXRes = GetLevelAdjustedResolutionValue(GetResolution().X, mipLevel);
			uint targetYRes = GetLevelAdjustedResolutionValue(GetResolution().Y, mipLevel);

			return new Resolution(targetXRes, targetYRes);
		}

		/// <summary>
		/// Gets a bitmap representing the given zero-based mipmap level. This creates a new <see cref="Bitmap"/> object
		/// from one of the raw mipmaps stored in the image.
		/// </summary>
		/// <returns>A bitmap.</returns>
		/// <param name="mipLevel">Mipmap level.</param>
		public Bitmap GetMipMap(uint mipLevel)
		{
			return DecompressMipMap(this.RawMipMaps[(int)mipLevel], mipLevel);
		}

		/// <summary>
		/// Gets the compressed data for the specified mipmap level.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">Throws if the input level is greated than the maximum stored level.</exception>
		/// <returns>A byte array containing the compressed data.</returns>
		/// <param name="mipLevel">The zero-based mipmap level.</param>
		public byte[] GetRawMipMap(uint mipLevel)
		{
			if (mipLevel > this.RawMipMaps.Count - 1)
			{
				throw new ArgumentOutOfRangeException(nameof(mipLevel), mipLevel, "The requested mip level was greater than the maximum stored level.");
			}

			return this.RawMipMaps[(int)mipLevel];
		}

		/// <summary>
		/// Gets a resolution value (height or width) adjusted by the specified mipmap level. In practice, this is
		/// a division by the level as far as possible.
		///
		/// A greater mip returns a lower resolution value, to a maximum of 1.
		/// </summary>
		/// <param name="resolutionValue">The value to adjust. This must be a power-of-two value.</param>
		/// <param name="mipLevel">The mipmap level to adjust by.</param>
		/// <returns>The adjusted value.</returns>
		private static uint GetLevelAdjustedResolutionValue(uint resolutionValue, uint mipLevel)
		{
			if ((resolutionValue != 0) && (resolutionValue & (resolutionValue - 1)) != 0)
			{
				throw new ArgumentException("The input resolution value must be a power-of-two value.", nameof(resolutionValue));
			}

			return resolutionValue / (uint)Math.Pow(2, mipLevel).Clamp(1, resolutionValue);
		}

		/// <summary>
		/// Decompresses a mipmap in the file at the specified level from the specified data.
		/// </summary>
		/// <returns>The mipmap.</returns>
		/// <param name="inData">Data containing the mipmap level.</param>
		/// <param name="mipLevel">The mipmap level of the data</param>
		private Bitmap DecompressMipMap(byte[] inData, uint mipLevel)
		{
			Bitmap map = null;
			uint targetXRes = GetLevelAdjustedResolutionValue(GetResolution().X, mipLevel);
            uint targetYRes = GetLevelAdjustedResolutionValue(GetResolution().Y, mipLevel);

			if (inData.Length > 0 && targetXRes > 0 && targetYRes > 0)
			{
				if (this.Header.CompressionType == TextureCompressionType.Palettized)
				{
					map = new Bitmap((int)targetXRes, (int)targetYRes, PixelFormat.Format32bppArgb);
					using (MemoryStream ms = new MemoryStream(inData))
					{
						using (BinaryReader br = new BinaryReader(ms))
						{
							// Read colour information
							for (int y = 0; y < targetYRes; ++y)
							{
								for (int x = 0; x < targetXRes; ++x)
								{
									byte colorIndex = br.ReadByte();
									Color paletteColor = this.Palette[colorIndex];
									map.SetPixel(x, y, paletteColor);
								}
							}

							// Read Alpha information
							List<byte> alphaValues = new List<byte>();
							if (GetAlphaBitDepth() > 0)
							{
								if (GetAlphaBitDepth() == 1)
								{
									int alphaByteCount = (int)Math.Ceiling(((double)(targetXRes * targetYRes) / 8));
									alphaValues = Decode1BitAlpha(br.ReadBytes(alphaByteCount));
								}
								else if (GetAlphaBitDepth() == 4)
								{
									int alphaByteCount = (int)Math.Ceiling(((double)(targetXRes * targetYRes) / 2));
									alphaValues = Decode4BitAlpha(br.ReadBytes(alphaByteCount));
								}
								else if (GetAlphaBitDepth() == 8)
								{
									// Directly read the alpha values
									for (int y = 0; y < targetYRes; ++y)
									{
										for (int x = 0; x < targetXRes; ++x)
										{
											byte alphaValue = br.ReadByte();
											alphaValues.Add(alphaValue);
										}
									}
								}
							}
							else
							{
								// The map is fully opaque
								for (int y = 0; y < targetYRes; ++y)
								{
									for (int x = 0; x < targetXRes; ++x)
									{
										alphaValues.Add(255);
									}
								}
							}

							// Build the final map
							for (int y = 0; y < targetYRes; ++y)
							{
								for (int x = 0; x < targetXRes; ++x)
								{
									int valueIndex = (int)(x + (targetXRes * y));
									byte alphaValue = alphaValues[valueIndex];
									Color pixelColor = map.GetPixel(x, y);
									Color finalPixel = Color.FromArgb(alphaValue, pixelColor.R, pixelColor.G, pixelColor.B);

									map.SetPixel(x, y, finalPixel);
								}
							}
						}
					}
				}
				else if (this.Header.CompressionType == TextureCompressionType.DXTC)
				{
					SquishOptions squishOptions = SquishOptions.DXT1;
					if (this.Header.PixelFormat == BLPPixelFormat.DXT3)
					{
						squishOptions = SquishOptions.DXT3;
					}
					else if (this.Header.PixelFormat == BLPPixelFormat.DXT5)
					{
						squishOptions = SquishOptions.DXT5;
					}

					map = (Bitmap)SquishCompression.DecompressToBitmap(inData, (int)targetXRes, (int)targetYRes, squishOptions);
				}
				else if (this.Header.CompressionType == TextureCompressionType.Uncompressed)
				{
					map = new Bitmap((int)targetXRes, (int)targetYRes, PixelFormat.Format32bppArgb);

					using (MemoryStream ms = new MemoryStream(inData))
					{
						using (BinaryReader br = new BinaryReader(ms))
						{
							for (int y = 0; y < targetYRes; ++y)
							{
								for (int x = 0; x < targetXRes; ++x)
								{
									byte a = br.ReadByte();
									byte r = br.ReadByte();
									byte g = br.ReadByte();
									byte b = br.ReadByte();

									Color pixelColor = Color.FromArgb(a, r, g, b);
									map.SetPixel(x, y, pixelColor);
								}
							}
						}
					}
				}
				else if (this.Header.CompressionType == TextureCompressionType.JPEG)
				{
					// Merge the JPEG header with the data in the mipmap
					byte[] jpegImage = new byte[this.JPEGHeaderSize + inData.Length];
					Buffer.BlockCopy(this.JPEGHeader, 0, jpegImage, 0, (int) this.JPEGHeaderSize);
					Buffer.BlockCopy(inData, 0, jpegImage, (int) this.JPEGHeaderSize, inData.Length);

					using (MemoryStream ms = new MemoryStream(jpegImage))
					{
						map = new Bitmap(ms).Invert();
					}
				}
			}

			return map;
		}

		/// <summary>
		/// Compresses an input bitmap into a list of mipmap using the file's compression settings.
		/// Mipmap levels which would produce an image with dimensions smaller than 1x1 will return null instead.
		/// The number of mipmaps returned will be <see cref="Warcraft.BLP.BLP.GetNumReasonableMipMapLevels"/> + 1.
		/// </summary>
		/// <returns>The compressed image data.</returns>
		/// <param name="inImage">The image to be compressed.</param>
		private List<byte[]> CompressImage(Image inImage)
		{
			List<byte[]> mipMaps = new List<byte[]>();

			// Generate a palette from the unmipped image for use with the mips
			if (this.Header.CompressionType == TextureCompressionType.Palettized)
			{
				GeneratePalette(inImage);
			}

			// Add the original image as the first mipmap
			mipMaps.Add(CompressImage(inImage, 0));

			// Then, compress the image N amount of times into mipmaps
			for (uint i = 0; i < GetNumReasonableMipMapLevels(); ++i)
			{
				mipMaps.Add(CompressImage(inImage, i));
			}

			return mipMaps;
		}

		/// <summary>
		/// Compresses in input bitmap into a single mipmap at the specified mipmap level, where a mip level is a bisection of the resolution.
		/// For instance, a mip level of 2 applied to a 64x64 image would produce an image with a resolution of 16x16.
		/// This function expects the mipmap level to be reasonable (i.e, not a level which would produce a mip smaller than 1x1)
		/// </summary>
		/// <returns>The image.</returns>
		/// <param name="inImage">Image.</param>
		/// <param name="mipLevel">Mip level.</param>
		private byte[] CompressImage(Image inImage, uint mipLevel)
		{
			uint targetXRes = GetLevelAdjustedResolutionValue(GetResolution().X, mipLevel);
			uint targetYRes = GetLevelAdjustedResolutionValue(GetResolution().Y, mipLevel);

			List<byte> colourData = new List<byte>();
			List<byte> alphaData = new List<byte>();
			using (Bitmap resizedImage = ResizeImage(inImage, (int)targetXRes, (int)targetYRes))
			{
				if (this.Header.CompressionType == TextureCompressionType.Palettized)
				{
					// Generate the colour data
					for (int y = 0; y < targetYRes; ++y)
					{
						for (int x = 0; x < targetXRes; ++x)
						{
							Color nearestColor = FindClosestMatchingColor(resizedImage.GetPixel(x, y));
							byte paletteIndex = (byte)this.Palette.IndexOf(nearestColor);

							colourData.Add(paletteIndex);
						}
					}

					// Generate the alpha data
					if (GetAlphaBitDepth() > 0)
					{
						if (GetAlphaBitDepth() == 1)
						{
							// We're going to be attempting to map 8 pixels on each X iteration
							for (int y = 0; y < targetYRes; ++y)
							{
								for (int x = 0; x < targetXRes; x += 8)
								{
									// The alpha value is stored per-bit in the byte (8 alpha values per byte)
									byte alphaByte = 0;

									for (byte i = 0; (i < 8) && (i < targetXRes); ++i)
									{
										byte pixelAlpha = resizedImage.GetPixel(x + i, y).A;
										if (pixelAlpha > 0)
										{
											pixelAlpha = 1;
										}

										// Shift the value into the correct position in the byte
										pixelAlpha = (byte)(pixelAlpha << 7 - i);
										alphaByte = (byte)(alphaByte | pixelAlpha);
									}

									alphaData.Add(alphaByte);
								}
							}
						}
						else if (GetAlphaBitDepth() == 4)
						{
							// We're going to be attempting to map 2 pixels on each X iteration
							for (int y = 0; y < targetYRes; ++y)
							{
								for (int x = 0; x < targetXRes; x += 2)
								{
									// The alpha value is stored as half a byte (2 alpha values per byte)
									// Extract these two values and map them to a byte size (4 bits can hold 0 - 15 alpha)

									byte alphaByte = 0;

									for (byte i = 0; (i < 2) && (i < targetXRes); ++i)
									{
										// Get the value from the image
										byte pixelAlpha = resizedImage.GetPixel(x + i, y).A;

										// Map the value to a 4-bit integer
										pixelAlpha = (byte)ExtensionMethods.Map(pixelAlpha, 0, 255, 0, 15);

										// Shift the value to the upper bits on the first iteration, and leave it where it is
										// on the second one
										pixelAlpha = (byte)(pixelAlpha << 4 * (1 - i));

										alphaByte = (byte)(alphaByte | pixelAlpha);
									}

									alphaData.Add(alphaByte);
								}
							}
						}
						else if (GetAlphaBitDepth() == 8)
						{
							for (int y = 0; y < targetYRes; ++y)
							{
								for (int x = 0; x < targetXRes; ++x)
								{
									// The alpha value is stored as a whole byte
									byte alphaValue = resizedImage.GetPixel(x, y).A;
									alphaData.Add(alphaValue);
								}
							}
						}
					}
					else
					{
						// The map is fully opaque
						for (int y = 0; y < targetYRes; ++y)
						{
							for (int x = 0; x < targetXRes; ++x)
							{
								alphaData.Add(255);
							}
						}
					}
				}
				else if (this.Header.CompressionType == TextureCompressionType.DXTC)
				{
					using (MemoryStream rgbaStream = new MemoryStream())
					{
						using (BinaryWriter bw = new BinaryWriter(rgbaStream))
						{
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

							SquishOptions squishOptions = SquishOptions.DXT1;
							if (this.Header.PixelFormat == BLPPixelFormat.DXT3)
							{
								squishOptions = SquishOptions.DXT3;
							}
							else if (this.Header.PixelFormat == BLPPixelFormat.DXT5)
							{
								squishOptions = SquishOptions.DXT5;
							}

							// TODO: Implement squish compression
							colourData = new List<byte>(SquishCompression.CompressImage(rgbaBytes, (int)targetXRes, (int)targetYRes, squishOptions));
						}
					}

				}
				else if (this.Header.CompressionType == TextureCompressionType.Uncompressed)
				{
					using (MemoryStream argbStream = new MemoryStream())
					{
						using (BinaryWriter bw = new BinaryWriter(argbStream))
						{
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
							colourData = new List<byte>(argbBytes);
						}
					}
				}
			}

			// After compression of the data, merge the color data and alpha data
			byte[] compressedMipMap = new byte[colourData.Count + alphaData.Count];
			Buffer.BlockCopy(colourData.ToArray(), 0, compressedMipMap, 0, colourData.ToArray().Length);
			Buffer.BlockCopy(alphaData.ToArray(), 0, compressedMipMap, colourData.ToArray().Length, alphaData.ToArray().Length);

			return compressedMipMap;
		}

		/// <summary>
		/// Resize the image to the specified width and height.
		/// Credit goes to https://stackoverflow.com/questions/1922040/resize-an-image-c-sharp (mpen)
		/// </summary>
		/// <param name="inImage">The image to resize.</param>
		/// <param name="imageWidth">The width to resize to.</param>
		/// <param name="imageHeight">The height to resize to.</param>
		/// <returns>The resized image.</returns>
		private static Bitmap ResizeImage(Image inImage, int imageWidth, int imageHeight)
		{
			Rectangle destRect = new Rectangle(0, 0, imageWidth, imageHeight);
			Bitmap destImage = new Bitmap(imageWidth, imageHeight);

			destImage.SetResolution(inImage.HorizontalResolution, inImage.VerticalResolution);

			using (Graphics graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (ImageAttributes wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(inImage, destRect, 0, 0, inImage.Width, inImage.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		/// <summary>
		/// Decodes a 1-bit alpha map into a list of byte values. The resulting list will be values of either 0 or 255.
		/// </summary>
		/// <param name="inData"></param>
		/// <returns></returns>
		private static List<byte> Decode1BitAlpha(byte[] inData)
		{
			List<byte> alphaValues = new List<byte>();

			foreach (byte dataByte in inData)
			{
				// The alpha value is stored per-bit in the byte (8 alpha values per byte)
				for (byte i = 0; i < 8; ++i)
				{
					byte alphaBit = (byte)ExtensionMethods.Map(((dataByte >> (7 - i)) & 0x01), 0, 1, 0, 255);

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

			return alphaValues;
		}

		/// <summary>
		/// Decodes a compressed 4-bit alpha map into a list of byte values.
		/// </summary>
		/// <param name="inData"></param>
		/// <returns></returns>
		private static List<byte> Decode4BitAlpha(byte[] inData)
		{
			List<byte> alphaValues = new List<byte>();

			foreach (var alphaByte in inData)
			{
				// The alpha value is stored as half a byte (2 alpha values per byte)
				// Extract these two values and map them to a byte size (4 bits can hold 0 - 15 alpha)
				byte alphaValue1 = (byte)ExtensionMethods.Map((alphaByte >> 4), 0, 15, 0, 255);
				byte alphaValue2 = (byte)ExtensionMethods.Map((alphaByte & 0x0F), 0, 15, 0, 255);
				alphaValues.Add(alphaValue1);
				alphaValues.Add(alphaValue2);
			}

			return alphaValues;
		}

		/// <summary>
		/// Encodes the alpha data of the provided image as a packed byte array of alpha values.
		/// 8 alpha values are stored in each byte as a 1 (fully opaque) or a 0 (fully transparent).
		/// </summary>
		/// <returns>The bit alpha.</returns>
		/// <param name="inMap">In map.</param>
		private List<byte> Encode1BitAlpha(Bitmap inMap)
		{
			List<byte> alphaValues = new List<byte>();
			for (int y = 0; y < inMap.Height; ++y)
			{
				for (int x = 0; x < inMap.Width; ++x)
				{
					alphaValues.Add(inMap.GetPixel(x, y).A);
				}
			}

			List<byte> packedAlphaValues = new List<byte>();
			for (int i = 0; i < alphaValues.Count; i += 8)
			{
				byte packedAlphaValue = new byte();
				for (int j = 0; j < 8; ++j)
				{
					byte alphaValue;
					if ((i + j) < alphaValues.Count)
					{
						alphaValue = alphaValues[i + j];
					}
					else
					{
						alphaValue = 0;
					}

					byte alphaMask = (byte) (1 << j);
					if (alphaValue > 0)
					{
						// Set the bit to 1 (fully opaque)
						packedAlphaValue |= alphaMask;
					}
					else
					{
						// Set the bit to 0 (fully transparent)
						packedAlphaValue &= (byte)~alphaMask;
					}

					packedAlphaValues.Add(packedAlphaValue);
				}
			}

			return packedAlphaValues;
		}

		/// <summary>
		/// Encodes the alpha data of the provided image as a packed byte array of alpha values.
		/// 2 alpha values are stored in each byte as a uint4_t integer value.
		/// </summary>
		/// <returns>The bit alpha.</returns>
		/// <param name="inMap">In map.</param>
		private List<byte> Encode4BitAlpha(Bitmap inMap)
		{
			List<byte> alphaValues = new List<byte>();
			for (int y = 0; y < inMap.Height; ++y)
			{
				for (int x = 0; x < inMap.Width; ++x)
				{
					alphaValues.Add(inMap.GetPixel(x, y).A);
				}
			}

			List<byte> packedAlphaValues = new List<byte>();
			for (int i = 0; i < alphaValues.Count; i += 2)
			{
				byte packedAlphaValue = new byte();
				for (int j = 0; j < 2; ++j)
				{
					byte alphaValue;
					if ((i + j) < alphaValues.Count)
					{
						alphaValue = alphaValues[i + j];
					}
					else
					{
						alphaValue = 0;
					}

					// Pack the alpha value into the byte
					if (j == 0)
					{
						// Pack into the first four bits
						packedAlphaValue |= (byte)(ExtensionMethods.Map((alphaValue), 0, 255, 0, 15) << 4);
					}
					else
					{
						// Pack into the last four bits
						packedAlphaValue |= (byte)(ExtensionMethods.Map((alphaValue & 0x0F), 0, 255, 0, 15));
					}
				}

				packedAlphaValues.Add(packedAlphaValue);
			}

			return packedAlphaValues;
		}

		/// <summary>
		/// Gets the number of mipmaps which can be produced in this file without producing a mipmap smaller than 1x1.
		/// </summary>
		/// <returns>The number of reasonable mip map levels.</returns>
		private uint GetNumReasonableMipMapLevels()
		{
			uint smallestXRes = GetResolution().X;
			uint smallestYRes = GetResolution().Y;

			uint mipLevels = 0;
			while (smallestXRes > 1 && smallestYRes > 1)
			{
				// Bisect the resolution using the current number of mip levels.
				smallestXRes = smallestXRes / (uint)Math.Pow(2, mipLevels);
				smallestYRes = smallestYRes / (uint)Math.Pow(2, mipLevels);

				++mipLevels;
			}

			return mipLevels.Clamp<uint>(0, 15);
		}

		/// <summary>
		/// Generates an indexed 256-color palette from the specified image and overwrites the current palette with it.
		/// Ordinarily, this would be the original mipmap.
		/// </summary>
		/// <param name="inImage">Image.</param>
		private void GeneratePalette(Image inImage)
		{
			// TODO: Replace with an algorithm that produces a better result. For now, it works.
			PaletteQuantizer quantizer = new PaletteQuantizer(new ArrayList());
			using (Bitmap quantizedMap = quantizer.Quantize(inImage))
			{
				this.Palette.Clear();
				this.Palette.AddRange(quantizedMap.Palette.Entries);
			}
		}

		/// <summary>
		/// Finds the closest matching color in the palette for the given input color.
		/// </summary>
		/// <returns>The closest matching color.</returns>
		/// <param name="inColour">Input color.</param>
		private Color FindClosestMatchingColor(Color inColour)
		{
			Color nearestColour = Color.Empty;

			// Drop out if the palette contains an exact match
			if (this.Palette.Contains(inColour))
			{
				return inColour;
			}

			double colourDistance = 250000.0;
			foreach (Color paletteColour in this.Palette)
			{
				double redTest = Math.Pow(Convert.ToDouble(paletteColour.R) - inColour.R, 2.0);
				double greenTest = Math.Pow(Convert.ToDouble(paletteColour.G) - inColour.G, 2.0);
				double blueTest = Math.Pow(Convert.ToDouble(paletteColour.B) - inColour.B, 2.0);

				double distanceResult = Math.Sqrt(blueTest + greenTest + redTest);

				if (distanceResult <= 0.0001)
				{
					nearestColour = paletteColour;
					break;
				}
				else if (distanceResult < colourDistance)
				{
					colourDistance = distanceResult;
					nearestColour = paletteColour;
				}
			}

			return nearestColour;
		}

		/// <summary>
		/// Gets the raw bytes of the palette (or an array with length 0 if there isn't a palette)
		/// </summary>
		/// <returns>The palette bytes.</returns>
		private byte[] GetPaletteBytes()
		{
			List<byte> bytes = new List<byte>();
			foreach (Color color in this.Palette)
			{
				bytes.Add(color.B);
				bytes.Add(color.G);
				bytes.Add(color.R);
				bytes.Add(color.A);
			}

			return bytes.ToArray();
		}

		/// <summary>
		/// Gets the raw, BLP-encoded mipmaps as a byte array for writing to disk.
		/// </summary>
		/// <returns>The mip map bytes.</returns>
		private byte[] GetMipMapBytes()
		{
			List<byte> mipmapBytes = new List<byte>();
			foreach (byte[] mipmap in this.RawMipMaps)
			{
				foreach (byte mipbyte in mipmap)
				{
					mipmapBytes.Add(mipbyte);
				}
			}

			return mipmapBytes.ToArray();
		}

		/// <summary>
		/// Gets the BLP image object as a byte array, which can be written to disk as a file.
		/// </summary>
		/// <returns>The bytes.</returns>
		public byte[] Serialize()
		{
			byte[] headerBytes = this.Header.Serialize();
			byte[] paletteBytes = GetPaletteBytes();
			byte[] mipBytes = GetMipMapBytes();

			byte[] imageBytes = new byte[headerBytes.Length + paletteBytes.Length + mipBytes.Length];

			Buffer.BlockCopy(headerBytes, 0, imageBytes, 0, headerBytes.Length);
			Buffer.BlockCopy(paletteBytes, 0, imageBytes, headerBytes.Length, paletteBytes.Length);
			Buffer.BlockCopy(mipBytes, 0, imageBytes, headerBytes.Length + paletteBytes.Length, mipBytes.Length);

			return imageBytes;
		}

		/// <summary>
		/// Gets the best mip map for the specified resolution, where the specified resolution
		/// is the maximum resolution for any dimension in the image.
		/// </summary>
		/// <returns>The best mip map.</returns>
		/// <param name="maxResolution">Max resolution.</param>
		public Bitmap GetBestMipMap(uint maxResolution)
		{
			// Calulcate the best mip level
			double xMip = Math.Ceiling((double)GetResolution().X / maxResolution) - 1;
			double yMip = Math.Ceiling((double)GetResolution().Y / maxResolution) - 1;

			if (xMip > yMip)
			{
				// Grab the mipmap based on the X Mip
				return GetMipMap((uint)xMip);
			}

			if (xMip < yMip)
			{
				// Grab the mipmap based on the Y Mip
				return GetMipMap((uint)yMip);
			}

			// Doesn't matter which one, just grab the X Mip
			return GetMipMap((uint)xMip);
		}

		/// <summary>
		/// Gets the magic string that identifies this file.
		/// </summary>
		/// <returns>The magic string.</returns>
		public string GetFileType()
		{
			return this.Header.Signature;
		}

		/// <summary>
		/// Gets the version of the BLP file.
		/// </summary>
		/// <returns>The version of the file.</returns>
		public uint GetVersion()
		{
			return this.Header.Version;
		}

		/// <summary>
		/// Gets the BLP pixel format. This format represents a subtype of the compression used in the file.
		/// </summary>
		/// <returns>The pixel format.</returns>
		public BLPPixelFormat GetPixelFormat()
		{
			return this.Header.PixelFormat;
		}

		/// <summary>
		/// Gets the resolution of the image.
		/// </summary>
		/// <returns>The resolution.</returns>
		public Resolution GetResolution()
		{
			return this.Header.Resolution;
		}

		/// <summary>
		/// Gets the type of compression used in the image.
		/// </summary>
		/// <returns>The compression type.</returns>
		public TextureCompressionType GetCompressionType()
		{
			return this.Header.CompressionType;
		}

		/// <summary>
		/// Gets the alpha bit depth. This value represents where the alpha value for each pixel is stored.
		/// </summary>
		/// <returns>The alpha bit depth.</returns>
		public uint GetAlphaBitDepth()
		{
			return this.Header.AlphaBitDepth;
		}

		/// <summary>
		/// Gets the number of mipmap levels in the image.
		/// </summary>
		/// <returns>The mipmap count.</returns>
		public int GetMipMapCount()
		{
			return this.RawMipMaps.Count;
		}
	}
}
