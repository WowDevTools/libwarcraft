//
//  BLP.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using SixLabors.Primitives;

using Warcraft.Core.Compression.Squish;
using Warcraft.Core.Extensions;
using Warcraft.Core.Structures;

namespace Warcraft.BLP
{
    /// <summary>
    /// This class represents a BLP binary image and its contained data.
    /// </summary>
    public class BLP
    {
        /// <summary>
        /// Gets or sets the header. This header contains data about the mipmaps stored in the BLP,
        /// and storage information such as offsets and sizes.
        /// </summary>
        public BLPHeader Header { get; set; }

        /// <summary>
        /// The palette of colours used in the BLP image. This is not used for DXTC-compressed
        /// textures.
        /// </summary>
        private readonly List<Rgba32> _palette = new List<Rgba32>();

        /// <summary>
        /// The size of the JPEG header. This is not used for palettized or DXTC-compressed
        /// textures.
        /// </summary>
        private readonly uint _jpegHeaderSize;

        /// <summary>
        /// The JPEG header. This is not used for palettized or DXTC-compressed
        /// textures.
        /// </summary>
        private readonly byte[]? _jpegHeader;

        /// <summary>
        /// A list of byte arrays containing the compressed mipmaps.
        /// </summary>
        private readonly List<byte[]> _rawMipMaps = new List<byte[]>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.BLP.BLP"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public BLP(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            byte[] fileHeaderBytes;
            if (PeekFormat(br) == BLPFormat.BLP2)
            {
                fileHeaderBytes = br.ReadBytes(148);
            }
            else
            {
                fileHeaderBytes = br.ReadBytes(156);
            }

            Header = new BLPHeader(fileHeaderBytes);

            if (Header.CompressionType == TextureCompressionType.JPEG)
            {
                _jpegHeaderSize = br.ReadUInt32();
                _jpegHeader = br.ReadBytes((int)_jpegHeaderSize);
            }
            else if (Header.CompressionType == TextureCompressionType.Palettized)
            {
                for (var i = 0; i < 256; ++i)
                {
                    var b = br.ReadByte();
                    var g = br.ReadByte();
                    var r = br.ReadByte();

                    // The alpha in the palette is not used, but is stored for the sake of completion.
                    var a = br.ReadByte();

                    var paletteColor = new Rgba32(r, g, b, a);
                    _palette.Add(paletteColor);
                }
            }
            else
            {
                // Fill up an empty palette - the palette is always present, but we'll be going after offsets anyway
                for (var i = 0; i < 256; ++i)
                {
                    var paletteColor = default(Rgba32);
                    _palette.Add(paletteColor);
                }
            }

            // Read the raw mipmap data
            for (var i = 0; i < Header.GetNumMipMaps(); ++i)
            {
                br.BaseStream.Position = Header.MipMapOffsets[i];
                _rawMipMaps.Add(br.ReadBytes((int)Header.MipMapSizes[i]));
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
            var startPosition = br.BaseStream.Position;
            var dataSignature = new string(br.ReadChars(4));

            BLPFormat format;
            if (!Enum.TryParse(dataSignature, out format))
            {
                throw new FileLoadException("The provided data did not have a BLP signature.");
            }

            br.BaseStream.Position = startPosition;
            return format;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.BLP.BLP"/> class.
        /// This constructor creates a BLP file using the specified compression from a bitmap object.
        /// If the compression type specifed is DXTC, the default pixel format used is DXT1 for opaque textures and DXT3 for the rest.
        /// </summary>
        /// <param name="image">Image.</param>
        /// <param name="compressionType">Compression type.</param>
        public BLP(Image<Rgba32> image, TextureCompressionType compressionType)
        {
            // Set up the header
            Header = new BLPHeader
            {
                CompressionType = compressionType
            };

            if (compressionType == TextureCompressionType.Palettized)
            {
                Header.PixelFormat = BLPPixelFormat.Palettized;

                // Determine best alpha bit depth
                if (image.HasAlpha())
                {
                    var alphaLevels = new List<byte>();
                    for (var y = 0; y < image.Height; ++y)
                    {
                        for (var x = 0; x < image.Width; ++x)
                        {
                            var pixel = image[x, y];
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
                        Header.AlphaBitDepth = 8;
                    }
                    else if (alphaLevels.Count > 2)
                    {
                        // More than 2, but less than or equal to 16? Use half a byte
                        Header.AlphaBitDepth = 4;
                    }
                    else
                    {
                        // Just 2? Use a bit instead
                        Header.AlphaBitDepth = 1;
                    }
                }
                else
                {
                    // No alpha, so a bit depth of 0.
                    Header.AlphaBitDepth = 0;
                }
            }
            else if (compressionType == TextureCompressionType.DXTC)
            {
                Header.AlphaBitDepth = 8;

                // Determine best DXTC type (1, 3 or 5)
                if (image.HasAlpha())
                {
                    Header.PixelFormat = BLPPixelFormat.DXT3;
                }
                else
                {
                    // DXT1 for no alpha
                    Header.PixelFormat = BLPPixelFormat.DXT1;
                }
            }
            else if (compressionType == TextureCompressionType.Uncompressed)
            {
                // The alpha will be stored as a straight ARGB texture, so set it to 8
                Header.AlphaBitDepth = 8;
                Header.PixelFormat = BLPPixelFormat.PalARGB1555DitherFloydSteinberg;
            }

            // What the mip type does is currently unknown, but it's usually set to 1.
            Header.MipMapType = 1;
            Header.Resolution = new Resolution((uint)image.Width, (uint)image.Height);

            // It's now time to compress the image
            _rawMipMaps = CompressImage(image);

            // Calculate the offsets and sizes
            var mipOffset = (uint)(Header.GetSize() + (_palette.Count * 4));
            foreach (var rawMipMap in _rawMipMaps)
            {
                var mipSize = (uint)rawMipMap.Length;

                Header.MipMapOffsets.Add(mipOffset);
                Header.MipMapSizes.Add(mipSize);

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
            return Header.Format;
        }

        /// <summary>
        /// Gets a list of formatted strings describing the mipmap levels.
        /// </summary>
        /// <returns>The mip map level strings.</returns>
        public IEnumerable<string> GetMipMapLevelStrings()
        {
            for (uint i = 0; i < GetMipMapCount(); ++i)
            {
                yield return $"{i}: {GetMipLevelResolution(i)}";
            }
        }

        /// <summary>
        /// Gets the resolution of the specified mip level.
        /// </summary>
        /// <returns>The mip level resolution.</returns>
        /// <param name="mipLevel">Mip level.</param>
        public Resolution GetMipLevelResolution(uint mipLevel)
        {
            var targetXRes = GetLevelAdjustedResolutionValue(GetResolution().X, mipLevel);
            var targetYRes = GetLevelAdjustedResolutionValue(GetResolution().Y, mipLevel);

            return new Resolution(targetXRes, targetYRes);
        }

        /// <summary>
        /// Gets a bitmap representing the given zero-based mipmap level. This creates a new <see cref="Image{Rgba32}"/> object
        /// from one of the raw mipmaps stored in the image.
        /// </summary>
        /// <returns>A bitmap.</returns>
        /// <param name="mipLevel">Mipmap level.</param>
        public Image<Rgba32> GetMipMap(uint mipLevel)
        {
            return DecompressMipMap(_rawMipMaps[(int)mipLevel], mipLevel);
        }

        /// <summary>
        /// Gets the compressed data for the specified mipmap level.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if the input level is greated than the maximum stored level.
        /// </exception>
        /// <returns>A byte array containing the compressed data.</returns>
        /// <param name="mipLevel">The zero-based mipmap level.</param>
        public byte[] GetRawMipMap(uint mipLevel)
        {
            if (mipLevel > _rawMipMaps.Count - 1)
            {
                throw new ArgumentOutOfRangeException
                (
                    nameof(mipLevel),
                    mipLevel,
                    "The requested mip level was greater than the maximum stored level."
                );
            }

            return _rawMipMaps[(int)mipLevel];
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
                throw new ArgumentException
                (
                    "The input resolution value must be a power-of-two value.",
                    nameof(resolutionValue)
                );
            }

            return resolutionValue / (uint)Math.Pow(2, mipLevel).Clamp(1, resolutionValue);
        }

        /// <summary>
        /// Decompresses a mipmap in the file at the specified level from the specified data.
        /// </summary>
        /// <returns>The mipmap.</returns>
        /// <param name="inData">ExtendedData containing the mipmap level.</param>
        /// <param name="mipLevel">The mipmap level of the data.</param>
        private Image<Rgba32> DecompressMipMap(byte[] inData, uint mipLevel)
        {
            if (inData == null || inData.Length <= 0)
            {
                throw new ArgumentException("No image data provided.", nameof(inData));
            }

            Image<Rgba32>? map = null;
            var targetXRes = GetLevelAdjustedResolutionValue(GetResolution().X, mipLevel);
            var targetYRes = GetLevelAdjustedResolutionValue(GetResolution().Y, mipLevel);

            if (targetXRes <= 0 || targetYRes <= 0)
            {
                throw new ArgumentException
                (
                    $"The input mipmap level produced an invalid resolution: {mipLevel}",
                    nameof(mipLevel)
                );
            }

            switch (Header.CompressionType)
            {
                case TextureCompressionType.Palettized:
                {
                    map = new Image<Rgba32>((int)targetXRes, (int)targetYRes);
                    using var ms = new MemoryStream(inData);
                    using var br = new BinaryReader(ms);
                    // Read colour information
                    for (var y = 0; y < targetYRes; ++y)
                    {
                        for (var x = 0; x < targetXRes; ++x)
                        {
                            var colorIndex = br.ReadByte();
                            var paletteColor = _palette[colorIndex];
                            map[x, y] = paletteColor;
                        }
                    }

                    // Read Alpha information
                    var alphaValues = new List<byte>();
                    if (GetAlphaBitDepth() > 0)
                    {
                        if (GetAlphaBitDepth() == 1)
                        {
                            var alphaByteCount = (int)Math.Ceiling((double)(targetXRes * targetYRes) / 8);
                            alphaValues = Decode1BitAlpha(br.ReadBytes(alphaByteCount));
                        }
                        else if (GetAlphaBitDepth() == 4)
                        {
                            var alphaByteCount = (int)Math.Ceiling((double)(targetXRes * targetYRes) / 2);
                            alphaValues = Decode4BitAlpha(br.ReadBytes(alphaByteCount));
                        }
                        else if (GetAlphaBitDepth() == 8)
                        {
                            // Directly read the alpha values
                            for (var y = 0; y < targetYRes; ++y)
                            {
                                for (var x = 0; x < targetXRes; ++x)
                                {
                                    var alphaValue = br.ReadByte();
                                    alphaValues.Add(alphaValue);
                                }
                            }
                        }
                    }
                    else
                    {
                        // The map is fully opaque
                        for (var y = 0; y < targetYRes; ++y)
                        {
                            for (var x = 0; x < targetXRes; ++x)
                            {
                                alphaValues.Add(255);
                            }
                        }
                    }

                    // Build the final map
                    for (var y = 0; y < targetYRes; ++y)
                    {
                        for (var x = 0; x < targetXRes; ++x)
                        {
                            var valueIndex = (int)(x + (targetXRes * y));
                            var alphaValue = alphaValues[valueIndex];

                            var pixelColor = map[x, y];
                            var finalPixel = new Rgba32(pixelColor.R, pixelColor.G, pixelColor.B, alphaValue);

                            map[x, y] = finalPixel;
                        }
                    }

                    break;
                }

                case TextureCompressionType.DXTC:
                {
                    var squishOptions = SquishOptions.DXT1;
                    if (Header.PixelFormat == BLPPixelFormat.DXT3)
                    {
                        squishOptions = SquishOptions.DXT3;
                    }
                    else if (Header.PixelFormat == BLPPixelFormat.DXT5)
                    {
                        squishOptions = SquishOptions.DXT5;
                    }

                    map = SquishCompression.DecompressToImage(inData, (int)targetXRes, (int)targetYRes, squishOptions);
                    break;
                }

                case TextureCompressionType.Uncompressed:
                {
                    map = new Image<Rgba32>((int)targetXRes, (int)targetYRes);

                    using var ms = new MemoryStream(inData);
                    using var br = new BinaryReader(ms);
                    for (var y = 0; y < targetYRes; ++y)
                    {
                        for (var x = 0; x < targetXRes; ++x)
                        {
                            var a = br.ReadByte();
                            var r = br.ReadByte();
                            var g = br.ReadByte();
                            var b = br.ReadByte();

                            var pixelColor = new Rgba32(r, g, b, a);
                            map[x, y] = pixelColor;
                        }
                    }

                    break;
                }

                case TextureCompressionType.JPEG:
                {
                    // Merge the JPEG header with the data in the mipmap
                    if (_jpegHeader is null)
                    {
                        // TODO: generate header?
                        throw new InvalidOperationException();
                    }

                    var jpegImage = new byte[_jpegHeaderSize + inData.Length];
                    Buffer.BlockCopy(_jpegHeader, 0, jpegImage, 0, (int)_jpegHeaderSize);
                    Buffer.BlockCopy(inData, 0, jpegImage, (int)_jpegHeaderSize, inData.Length);

                    using var ms = new MemoryStream(jpegImage);
                    map = Image.Load<Rgba32>(ms).Clone(cx => cx.Invert());

                    break;
                }
            }

            return map ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Compresses an input bitmap into a list of mipmap using the file's compression settings.
        /// Mipmap levels which would produce an image with dimensions smaller than 1x1 will return null instead.
        /// The number of mipmaps returned will be <see cref="Warcraft.BLP.BLP.GetNumReasonableMipMapLevels"/> + 1.
        /// </summary>
        /// <returns>The compressed image data.</returns>
        /// <param name="inImage">The image to be compressed.</param>
        private List<byte[]> CompressImage(Image<Rgba32> inImage)
        {
            var mipMaps = new List<byte[]>();

            // Generate a palette from the unmipped image for use with the mips
            if (Header.CompressionType == TextureCompressionType.Palettized)
            {
                _palette.Clear();
                _palette.AddRange(GeneratePalette(inImage));
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
        /// Compresses in input bitmap into a single mipmap at the specified mipmap level, where a mip level is a
        /// bisection of the resolution.
        /// For instance, a mip level of 2 applied to a 64x64 image would produce an image with a resolution of 16x16.
        /// This function expects the mipmap level to be reasonable (i.e, not a level which would produce a mip smaller
        /// than 1x1).
        /// </summary>
        /// <returns>The image.</returns>
        /// <param name="inImage">Image.</param>
        /// <param name="mipLevel">Mip level.</param>
        private byte[] CompressImage(Image<Rgba32> inImage, uint mipLevel)
        {
            var targetXRes = GetLevelAdjustedResolutionValue(GetResolution().X, mipLevel);
            var targetYRes = GetLevelAdjustedResolutionValue(GetResolution().Y, mipLevel);

            var colourData = new List<byte>();
            var alphaData = new List<byte>();
            using (var resizedImage = ResizeImage(inImage, (int)targetXRes, (int)targetYRes))
            {
                if (Header.CompressionType == TextureCompressionType.Palettized)
                {
                    // Generate the colour data
                    for (var y = 0; y < targetYRes; ++y)
                    {
                        for (var x = 0; x < targetXRes; ++x)
                        {
                            var nearestColor = FindClosestMatchingColor(resizedImage[x, y]);
                            var paletteIndex = (byte)_palette.IndexOf(nearestColor);

                            colourData.Add(paletteIndex);
                        }
                    }

                    // Generate the alpha data
                    if (GetAlphaBitDepth() > 0)
                    {
                        if (GetAlphaBitDepth() == 1)
                        {
                            // We're going to be attempting to map 8 pixels on each X iteration
                            for (var y = 0; y < targetYRes; ++y)
                            {
                                for (var x = 0; x < targetXRes; x += 8)
                                {
                                    // The alpha value is stored per-bit in the byte (8 alpha values per byte)
                                    byte alphaByte = 0;

                                    for (byte i = 0; (i < 8) && (i < targetXRes); ++i)
                                    {
                                        var pixelAlpha = resizedImage[x + i, y].A;
                                        if (pixelAlpha > 0)
                                        {
                                            pixelAlpha = 1;
                                        }

                                        // Shift the value into the correct position in the byte
                                        pixelAlpha = (byte)(pixelAlpha << (7 - i));
                                        alphaByte = (byte)(alphaByte | pixelAlpha);
                                    }

                                    alphaData.Add(alphaByte);
                                }
                            }
                        }
                        else if (GetAlphaBitDepth() == 4)
                        {
                            // We're going to be attempting to map 2 pixels on each X iteration
                            for (var y = 0; y < targetYRes; ++y)
                            {
                                for (var x = 0; x < targetXRes; x += 2)
                                {
                                    // The alpha value is stored as half a byte (2 alpha values per byte)
                                    // Extract these two values and map them to a byte size (4 bits can hold 0 - 15
                                    // alpha)
                                    byte alphaByte = 0;

                                    for (byte i = 0; (i < 2) && (i < targetXRes); ++i)
                                    {
                                        // Get the value from the image
                                        var pixelAlpha = resizedImage[x + i, y].A;

                                        // Map the value to a 4-bit integer
                                        pixelAlpha = (byte)ExtendedMath.Map(pixelAlpha, 0, 255, 0, 15);

                                        // Shift the value to the upper bits on the first iteration, and leave it where
                                        // it is on the second one
                                        pixelAlpha = (byte)(pixelAlpha << (4 * (1 - i)));

                                        alphaByte = (byte)(alphaByte | pixelAlpha);
                                    }

                                    alphaData.Add(alphaByte);
                                }
                            }
                        }
                        else if (GetAlphaBitDepth() == 8)
                        {
                            for (var y = 0; y < targetYRes; ++y)
                            {
                                for (var x = 0; x < targetXRes; ++x)
                                {
                                    // The alpha value is stored as a whole byte
                                    var alphaValue = resizedImage[x, y].A;
                                    alphaData.Add(alphaValue);
                                }
                            }
                        }
                    }
                    else
                    {
                        // The map is fully opaque
                        for (var y = 0; y < targetYRes; ++y)
                        {
                            for (var x = 0; x < targetXRes; ++x)
                            {
                                alphaData.Add(255);
                            }
                        }
                    }
                }
                else if (Header.CompressionType == TextureCompressionType.DXTC)
                {
                    using var rgbaStream = new MemoryStream();
                    using var bw = new BinaryWriter(rgbaStream);
                    for (var y = 0; y < targetYRes; ++y)
                    {
                        for (var x = 0; x < targetXRes; ++x)
                        {
                            bw.Write(resizedImage[x, y].R);
                            bw.Write(resizedImage[x, y].G);
                            bw.Write(resizedImage[x, y].B);
                            bw.Write(resizedImage[x, y].A);
                        }
                    }

                    // Finish writing the data
                    bw.Flush();

                    var rgbaBytes = rgbaStream.ToArray();

                    var squishOptions = SquishOptions.DXT1;
                    if (Header.PixelFormat == BLPPixelFormat.DXT3)
                    {
                        squishOptions = SquishOptions.DXT3;
                    }
                    else if (Header.PixelFormat == BLPPixelFormat.DXT5)
                    {
                        squishOptions = SquishOptions.DXT5;
                    }

                    // TODO: Implement squish compression
                    colourData = new List<byte>
                    (
                        SquishCompression.CompressImage
                        (
                            rgbaBytes,
                            (int)targetXRes,
                            (int)targetYRes,
                            squishOptions
                        )
                    );
                }
                else if (Header.CompressionType == TextureCompressionType.Uncompressed)
                {
                    using var argbStream = new MemoryStream();
                    using var bw = new BinaryWriter(argbStream);
                    for (var y = 0; y < targetYRes; ++y)
                    {
                        for (var x = 0; x < targetXRes; ++x)
                        {
                            bw.Write(resizedImage[x, y].A);
                            bw.Write(resizedImage[x, y].R);
                            bw.Write(resizedImage[x, y].G);
                            bw.Write(resizedImage[x, y].B);
                        }
                    }

                    // Finish writing the data
                    bw.Flush();

                    var argbBytes = argbStream.ToArray();
                    colourData = new List<byte>(argbBytes);
                }
            }

            // After compression of the data, merge the color data and alpha data
            var compressedMipMap = new byte[colourData.Count + alphaData.Count];
            Buffer.BlockCopy(colourData.ToArray(), 0, compressedMipMap, 0, colourData.ToArray().Length);
            Buffer.BlockCopy
            (
                alphaData.ToArray(),
                0,
                compressedMipMap,
                colourData.ToArray().Length,
                alphaData.ToArray().Length
            );

            return compressedMipMap;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// Credit goes to https://stackoverflow.com/questions/1922040/resize-an-image-c-sharp (mpen).
        /// </summary>
        /// <param name="inImage">The image to resize.</param>
        /// <param name="imageWidth">The width to resize to.</param>
        /// <param name="imageHeight">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private static Image<Rgba32> ResizeImage(Image<Rgba32> inImage, int imageWidth, int imageHeight)
        {
            var resizeOptions = new ResizeOptions
            {
                Mode = ResizeMode.Min,
                Sampler = new BicubicResampler(),
                Size = new Size(imageHeight, imageWidth),
            };

            var resizedImage = inImage.Clone(cx => cx.Resize(resizeOptions));
            return resizedImage;
        }

        /// <summary>
        /// Finds the closest matching color in the palette for the given input color.
        /// </summary>
        /// <returns>The closest matching color.</returns>
        /// <param name="inColour">Input color.</param>
        private Rgba32 FindClosestMatchingColor(Rgba32 inColour)
        {
            var nearestColour = Rgba32.Black;

            // Drop out if the palette contains an exact match
            if (_palette.Contains(inColour))
            {
                return inColour;
            }

            var colourDistance = 250000.0;
            foreach (var paletteColour in _palette)
            {
                var redTest = Math.Pow(Convert.ToDouble(paletteColour.R) - inColour.R, 2.0);
                var greenTest = Math.Pow(Convert.ToDouble(paletteColour.G) - inColour.G, 2.0);
                var blueTest = Math.Pow(Convert.ToDouble(paletteColour.B) - inColour.B, 2.0);

                var distanceResult = Math.Sqrt(blueTest + greenTest + redTest);

                if (distanceResult <= 0.0001)
                {
                    nearestColour = paletteColour;
                    break;
                }

                if (!(distanceResult < colourDistance))
                {
                    continue;
                }

                colourDistance = distanceResult;
                nearestColour = paletteColour;
            }

            return nearestColour;
        }

        /// <summary>
        /// Decodes a 1-bit alpha map into a list of byte values. The resulting list will be values of either 0 or 255.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        /// <returns>The decoded map.</returns>
        private static List<byte> Decode1BitAlpha(byte[] inData)
        {
            var alphaValues = new List<byte>();

            foreach (var dataByte in inData)
            {
                // The alpha value is stored per-bit in the byte (8 alpha values per byte)
                for (byte i = 0; i < 8; ++i)
                {
                    var alphaBit = (byte)ExtendedMath.Map((dataByte >> (7 - i)) & 0x01, 0, 1, 0, 255);

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
        /// <param name="inData">The binary data.</param>
        /// <returns>The decoded map.</returns>
        private static List<byte> Decode4BitAlpha(byte[] inData)
        {
            var alphaValues = new List<byte>();

            foreach (var alphaByte in inData)
            {
                // The alpha value is stored as half a byte (2 alpha values per byte)
                // Extract these two values and map them to a byte size (4 bits can hold 0 - 15 alpha)
                var alphaValue1 = (byte)ExtendedMath.Map(alphaByte >> 4, 0, 15, 0, 255);
                var alphaValue2 = (byte)ExtendedMath.Map(alphaByte & 0x0F, 0, 15, 0, 255);
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
        private List<byte> Encode1BitAlpha(Image<Rgba32> inMap)
        {
            var alphaValues = new List<byte>();
            for (var y = 0; y < inMap.Height; ++y)
            {
                for (var x = 0; x < inMap.Width; ++x)
                {
                    alphaValues.Add(inMap[x, y].A);
                }
            }

            var packedAlphaValues = new List<byte>();
            for (var i = 0; i < alphaValues.Count; i += 8)
            {
                var packedAlphaValue = default(byte);
                for (var j = 0; j < 8; ++j)
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

                    var alphaMask = (byte)(1 << j);
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
        private List<byte> Encode4BitAlpha(Image<Rgba32> inMap)
        {
            var alphaValues = new List<byte>();
            for (var y = 0; y < inMap.Height; ++y)
            {
                for (var x = 0; x < inMap.Width; ++x)
                {
                    alphaValues.Add(inMap[x, y].A);
                }
            }

            var packedAlphaValues = new List<byte>();
            for (var i = 0; i < alphaValues.Count; i += 2)
            {
                var packedAlphaValue = default(byte);
                for (var j = 0; j < 2; ++j)
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
                        packedAlphaValue |= (byte)(ExtendedMath.Map(alphaValue, 0, 255, 0, 15) << 4);
                    }
                    else
                    {
                        // Pack into the last four bits
                        packedAlphaValue |= (byte)ExtendedMath.Map(alphaValue & 0x0F, 0, 255, 0, 15);
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
            var smallestXRes = GetResolution().X;
            var smallestYRes = GetResolution().Y;

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
        private IEnumerable<Rgba32> GeneratePalette(Image<Rgba32> inImage)
        {
            var knownColours = new List<Rgba32>();

            using (var quantizedImage = inImage.Clone(cx => cx.Quantize()))
            {
                var pixels = quantizedImage.GetPixelSpan();

                for (var i = 0; i < pixels.Length; ++i)
                {
                    var pixelColour = pixels[i];
                    if (knownColours.Contains(pixelColour))
                    {
                        continue;
                    }

                    knownColours.Add(pixelColour);
                }
            }

            return knownColours;
        }

        /// <summary>
        /// Gets the raw bytes of the palette (or an array with length 0 if there isn't a palette).
        /// </summary>
        /// <returns>The palette bytes.</returns>
        private byte[] GetPaletteBytes(IEnumerable<Rgba32> palette)
        {
            var bytes = new List<byte>();
            foreach (var color in palette)
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
            var mipmapBytes = new List<byte>();
            foreach (var mipmap in _rawMipMaps)
            {
                foreach (var mipbyte in mipmap)
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
            var headerBytes = Header.Serialize();
            var paletteBytes = GetPaletteBytes(_palette);
            var mipBytes = GetMipMapBytes();

            var imageBytes = new byte[headerBytes.Length + paletteBytes.Length + mipBytes.Length];

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
        public Image<Rgba32> GetBestMipMap(uint maxResolution)
        {
            // Calulcate the best mip level
            var xMip = Math.Ceiling((double)GetResolution().X / maxResolution) - 1;
            var yMip = Math.Ceiling((double)GetResolution().Y / maxResolution) - 1;

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
            return Header.Signature;
        }

        /// <summary>
        /// Gets the version of the BLP file.
        /// </summary>
        /// <returns>The version of the file.</returns>
        public uint GetVersion()
        {
            return Header.Version;
        }

        /// <summary>
        /// Gets the BLP pixel format. This format represents a subtype of the compression used in the file.
        /// </summary>
        /// <returns>The pixel format.</returns>
        public BLPPixelFormat GetPixelFormat()
        {
            return Header.PixelFormat;
        }

        /// <summary>
        /// Gets the resolution of the image.
        /// </summary>
        /// <returns>The resolution.</returns>
        public Resolution GetResolution()
        {
            return Header.Resolution;
        }

        /// <summary>
        /// Gets the type of compression used in the image.
        /// </summary>
        /// <returns>The compression type.</returns>
        public TextureCompressionType GetCompressionType()
        {
            return Header.CompressionType;
        }

        /// <summary>
        /// Gets the alpha bit depth. This value represents where the alpha value for each pixel is stored.
        /// </summary>
        /// <returns>The alpha bit depth.</returns>
        public uint GetAlphaBitDepth()
        {
            return Header.AlphaBitDepth;
        }

        /// <summary>
        /// Gets the number of mipmap levels in the image.
        /// </summary>
        /// <returns>The mipmap count.</returns>
        public int GetMipMapCount()
        {
            return _rawMipMaps.Count;
        }
    }
}
