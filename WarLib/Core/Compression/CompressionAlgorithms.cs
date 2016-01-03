using System;

namespace WarLib.Core.Compression
{
	// It is important that these algorithms are not reordered.
	// If multiple algorithms are used, the order of algorithms are from top to bottom
	// when compressing, and the inverse when decompressing.
	[Flags]
	public enum CompressionAlgorithms : byte
	{
		Sparse = 0x20,
		IMA_ADPCM_Mono = 0x40,
		IMA_ADPCM_Stereo = 0x80,
		Huffman = 0x01,
		Deflate_ZLIB = 0x02,
		Implode_PKWARE = 0x08,
		BZip2 = 0x10
	}
}

