//
//  Compression.cs
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
using NoeCompression = Noemax.Compression;
using System.IO;
using Warcraft.Core.Compression;
using Warcraft.MPQ.BlockTable;

namespace Warcraft.Core.Compression
{
	public static class Compression
	{
		static Compression()
		{

		}

		public static byte[] DecompressSector(byte[] PendingSector, BlockFlags Flags)
		{
			if (Flags.HasFlag(BlockFlags.BLF_IsCompressed))
			{
				// The sector is compressed using a combination of techniques.
				// Examine the first byte to determine the compression algorithms used
				CompressionAlgorithms compressionAlgorithms = (CompressionAlgorithms)PendingSector[0];

				// Drop the first byte
				byte[] sectorData = new byte[PendingSector.Length - 1];
				Buffer.BlockCopy(PendingSector, 1, sectorData, 0, sectorData.Length);
				PendingSector = sectorData;

				// Walk through each compression algorithm in reverse order						
				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.BZip2))
				{
					// Decompress sector using BZIP2
					PendingSector = Compression.DecompressBZip2(PendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Implode_PKWARE))
				{
					// Decompress sector using PKWARE Implode
					PendingSector = Compression.DecompressPKWAREImplode(PendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Deflate_ZLIB))
				{
					// Decompress sector using Deflate
					PendingSector = Compression.DecompressDeflate(PendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Huffman))
				{
					// Decompress sector using Huffman
					PendingSector = Compression.DecompressHuffman(PendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.IMA_ADPCM_Stereo))
				{
					// Decompress sector using ADPCM Stereo
					PendingSector = Compression.DecompressADPCMStereo(PendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.IMA_ADPCM_Mono))
				{
					// Decompress sector using ADPCM Mono
					PendingSector = Compression.DecompressADPCMMono(PendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Sparse))
				{
					// Decompress sector using Sparse
					PendingSector = Compression.DecompressSparse(PendingSector);
				}
			}
			else if (Flags.HasFlag(BlockFlags.BLF_IsImploded))
			{
				// This file or sector uses a single-pass PKWARE Implode algorithm.
				// Decompress sector using PKWARE
				PendingSector = Compression.DecompressPKWAREImplode(PendingSector);
			}

			return PendingSector;
		}

		// TODO: Implement
		public static byte[] DecompressSparse(byte[] InData)
		{
			throw new NotImplementedException("Sparse decompression has not yet been implemented.");
		}

		public static byte[] DecompressADPCMMono(byte[] InData)
		{
			return MpqWavCompression.Decompress(new MemoryStream(InData), 1);
		}

		public static byte[] DecompressADPCMStereo(byte[] InData)
		{
			return MpqWavCompression.Decompress(new MemoryStream(InData), 2);
		}

		public static byte[] DecompressHuffman(byte[] InData)
		{
			return MpqHuffman.Decompress(new MemoryStream(InData)).ToArray();
		}

		public static byte[] DecompressDeflate(byte[] InData)
		{
			return NoeCompression.CompressionFactory.Deflate.Decompress(new MemoryStream(InData));
		}

		public static byte[] DecompressPKWAREImplode(byte[] InData)
		{
			MemoryStream decompressedStream = new MemoryStream();
			new Blast.Blast(new MemoryStream(InData), decompressedStream).Decompress();

			return decompressedStream.ToArray();
		}

		public static byte[] DecompressBZip2(byte[] InData)
		{
			return NoeCompression.CompressionFactory.BZip2.Decompress(new MemoryStream(InData));
		}

		public static byte[] DecompressLZMA(byte[] InData)
		{
			return NoeCompression.CompressionFactory.Lzma.Decompress(new MemoryStream(InData));
		}
	}
}

