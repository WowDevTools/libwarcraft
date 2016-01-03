using System;
using NoeCompression = Noemax.Compression;
using System.IO;
using Blast;
using WarLib.Core.Compression;
using WarLib.MPQ;

namespace Warlib.Core.Compression
{
	public static class Compression
	{
		static Compression()
		{
					
		}

		public static byte[] DecompressSector(byte[] pendingSector, BlockFlags flags)
		{
			if (flags.HasFlag(BlockFlags.BLF_IsCompressed))
			{
				// The sector is compressed using a combination of techniques
				// TODO: switch decompression chain based on MPQ version (SCII doesn't use arbitrary combinations)

				// Examine the first byte to determine the compression algorithms used
				CompressionAlgorithms compressionAlgorithms = (CompressionAlgorithms)pendingSector[0];

				// Drop the first byte
				byte[] sectorData = new byte[pendingSector.Length - 1];
				Buffer.BlockCopy(pendingSector, 1, sectorData, 0, sectorData.Length);
				pendingSector = sectorData;

				// Walk through each compression algorithm in reverse order						
				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.BZip2))
				{
					// Decompress sector using BZIP2
					pendingSector = Compression.DecompressBZip2(pendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Implode_PKWARE))
				{
					// Decompress sector using PKWARE Implode
					pendingSector = Compression.DecompressPKWAREImplode(pendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Deflate_ZLIB))
				{
					// Decompress sector using Deflate
					pendingSector = Compression.DecompressDeflate(pendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Huffman))
				{
					// Decompress sector using Huffman
					pendingSector = Compression.DecompressHuffman(pendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.IMA_ADPCM_Stereo))
				{
					// Decompress sector using ADPCM Stereo
					pendingSector = Compression.DecompressADPCMStereo(pendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.IMA_ADPCM_Mono))
				{
					// Decompress sector using ADPCM Mono
					pendingSector = Compression.DecompressADPCMMono(pendingSector);
				}

				if (compressionAlgorithms.HasFlag(CompressionAlgorithms.Sparse))
				{
					// Decompress sector using Sparse
					//pendingSector = Compression.DecompressSparse(pendingSector);
				}
			}
			else if (flags.HasFlag(BlockFlags.BLF_IsImploded))
			{
				// Uses single-pass PKWARE Implode algorithm, otherwise normal sectoring
				// Decompress sector using PKWARE
				pendingSector = Compression.DecompressPKWAREImplode(pendingSector);
			}

			return pendingSector;
		}

		// TODO: Implement
		public static byte[] DecompressSparse(byte[] data)
		{
			return null;
		}

		public static byte[] DecompressADPCMMono(byte[] data)
		{
			return MpqWavCompression.Decompress(new MemoryStream(data), 1);
		}

		public static byte[] DecompressADPCMStereo(byte[] data)
		{
			return MpqWavCompression.Decompress(new MemoryStream(data), 2);
		}

		public static byte[] DecompressHuffman(byte[] data)
		{
			return MpqHuffman.Decompress(new MemoryStream(data)).ToArray();
		}

		public static byte[] DecompressDeflate(byte[] data)
		{
			return NoeCompression.DeflateCompression.Deflate.Decompress(new MemoryStream(data));
		}

		public static byte[] DecompressPKWAREImplode(byte[] data)
		{
			MemoryStream decompressedStream = new MemoryStream();
			new Blast.Blast(new MemoryStream(data), decompressedStream).Decompress();

			return decompressedStream.ToArray();
		}

		public static byte[] DecompressBZip2(byte[] data)
		{
			return NoeCompression.BZip2Compression.BZip2.Decompress(new MemoryStream(data));
		}

		public static byte[] DecompressLZMA(byte[] data)
		{
			return NoeCompression.LzmaCompression.Lzma.Decompress(new MemoryStream(data));
		}
	}
}

