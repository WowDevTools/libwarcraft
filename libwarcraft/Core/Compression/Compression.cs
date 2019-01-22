//
//  Compression.cs
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
using System.IO;
using SharpCompress.Compressors;
using SharpCompress.Compressors.BZip2;
using SharpCompress.Compressors.Deflate;
using SharpCompress.Compressors.LZMA;
using Warcraft.Core.Compression.Huffman;
using Warcraft.MPQ.Tables.Block;

namespace Warcraft.Core.Compression
{
    /// <summary>
    /// Compression handling for all compression algorithms used in MPQ archives.
    /// ExtendedData can be decompressed manually or by using one of the helper methods
    /// for when multiple algorithms have been used.
    /// </summary>
    public static class Compression
    {
        /// <summary>
        /// Decompresses a sector of a file stored in an MPQ archive.
        /// </summary>
        /// <param name="pendingSector">The compressed sector data.</param>
        /// <param name="blockFlags">The flags of the file block, taken from the block table.</param>
        /// <returns>The decompressed sector data.</returns>
        public static byte[] DecompressSector(byte[] pendingSector, BlockFlags blockFlags)
        {
            if (blockFlags.HasFlag(BlockFlags.IsCompressedMultiple))
            {
                return DecompressData(pendingSector);
            }

            if (blockFlags.HasFlag(BlockFlags.IsImploded))
            {
                // This file or sector uses a single-pass PKWARE Implode algorithm.
                // Decompress sector using PKWARE
                pendingSector = DecompressPKWAREImplode(pendingSector);
            }

            return pendingSector;
        }

        /// <summary>
        /// Decompressed a block of data. It is assumed that this data is compressed with one
        /// or more algorithms, and that it is prepended by a single byte describing the algorithms
        /// used. See <see cref="CompressionAlgorithms"/> for valid algorithms.
        /// </summary>
        /// <param name="pendingSector">The compressed data.</param>
        /// <returns>The decompressed data.</returns>
        public static byte[] DecompressData(byte[] pendingSector)
        {
            // The sector is compressed using a combination of techniques.
            // Examine the first byte to determine the compression algorithms used
            var compressionAlgorithms = (CompressionAlgorithms)pendingSector[0];

            // Drop the first byte
            var sectorData = new byte[pendingSector.Length - 1];
            Buffer.BlockCopy(pendingSector, 1, sectorData, 0, sectorData.Length);
            pendingSector = sectorData;

            // Walk through each compression algorithm in reverse order
            if (compressionAlgorithms.HasFlag(CompressionAlgorithms.BZip2))
            {
                // Decompress sector using BZIP2
                pendingSector = DecompressBZip2(pendingSector);
            }

            if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Implode))
            {
                // Decompress sector using PKWARE Implode
                pendingSector = DecompressPKWAREImplode(pendingSector);
            }

            if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Deflate))
            {
                // Decompress sector using Deflate
                pendingSector = DecompressDeflate(pendingSector);
            }

            if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Huffman))
            {
                // Decompress sector using Huffman
                pendingSector = DecompressHuffman(pendingSector);
            }

            if (compressionAlgorithms.HasFlag(CompressionAlgorithms.ADPCMStereo))
            {
                // Decompress sector using ADPCM Stereo
                pendingSector = DecompressADPCMStereo(pendingSector);
            }

            if (compressionAlgorithms.HasFlag(CompressionAlgorithms.ADPCMMono))
            {
                // Decompress sector using ADPCM Mono
                pendingSector = DecompressADPCMMono(pendingSector);
            }

            if (compressionAlgorithms.HasFlag(CompressionAlgorithms.LZMA))
            {
                // Decompress sector using LZMA
                pendingSector = DecompressLZMA(pendingSector);
            }

            return pendingSector;
        }

        /// <summary>
        /// Decompresssed a block of data using the ADPCM-Mono algorithm.
        /// </summary>
        /// <param name="inData">The compressed data block.</param>
        /// <returns>The decompressed data.</returns>
        public static byte[] DecompressADPCMMono(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                return ADPCM.ADPCM.Decompress(ms, 1);
            }
        }

        /// <summary>
        /// Decompresssed a block of data using the ADPCM-Stereo algorithm.
        /// </summary>
        /// <param name="inData">The compressed data block.</param>
        /// <returns>The decompressed data.</returns>
        public static byte[] DecompressADPCMStereo(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                return ADPCM.ADPCM.Decompress(ms, 2);
            }
        }

        /// <summary>
        /// Decompresssed a block of data using the Huffman algorithm.
        /// </summary>
        /// <param name="inData">The compressed data block.</param>
        /// <returns>The decompressed data.</returns>
        public static byte[] DecompressHuffman(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                return MpqHuffman.Decompress(ms).ToArray();
            }
        }

        /// <summary>
        /// Decompresssed a block of data using the ZLib Deflate algorithm.
        /// </summary>
        /// <param name="inData">The compressed data block.</param>
        /// <returns>The decompressed data.</returns>
        public static byte[] DecompressDeflate(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var ds = new ZlibStream(ms, CompressionMode.Decompress))
                {
                    using (var output = new MemoryStream())
                    {
                        ds.CopyTo(output);
                        return output.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Decompresssed a block of data using the IMPLODE algorithm.
        /// </summary>
        /// <param name="inData">The compressed data block.</param>
        /// <returns>The decompressed data.</returns>
        public static byte[] DecompressPKWAREImplode(byte[] inData)
        {
            using (var decompressedStream = new MemoryStream())
            {
                using (var compressedStream = new MemoryStream(inData))
                {
                    new Blast.Blast(compressedStream, decompressedStream).Decompress();
                    return decompressedStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Decompresssed a block of data using the BZip2 algorithm.
        /// </summary>
        /// <param name="inData">The compressed data block.</param>
        /// <returns>The decompressed data.</returns>
        public static byte[] DecompressBZip2(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var input = new BZip2Stream(ms, CompressionMode.Decompress, false))
                {
                    using (var decompressedData = new MemoryStream())
                    {
                        input.CopyTo(decompressedData);
                        return decompressedData.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Decompresssed a block of data using the LZMA algorithm.
        /// </summary>
        /// <param name="inData">The compressed data block.</param>
        /// <returns>The decompressed data.</returns>
        public static byte[] DecompressLZMA(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var input = new LZipStream(ms, CompressionMode.Decompress))
                {
                    using (var decompressedData = new MemoryStream())
                    {
                        input.CopyTo(decompressedData);
                        return decompressedData.ToArray();
                    }
                }
            }
        }
    }
}
