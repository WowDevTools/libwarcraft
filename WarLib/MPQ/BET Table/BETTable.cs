using System;
using System.Collections.Generic;

namespace WarLib.MPQ.BETTable
{
	public class BETTable
	{
		private uint Signature;
		private uint Version;
		private uint DataSize;

		private uint TableSize;
		private uint FileCount;
		private uint Unknown;
		private uint TableEntrySize;
		private uint BitIndex_FilePosition;
		private uint BitIndex_FileSize;
		private uint BitIndex_CompressedSize;
		private uint BitIndex_FlagIndex;
		private uint BitIndex_Unknown;
		private uint BitCount_FilePosition;
		private uint BitCount_FileSize;
		private uint BitCount_CompressedSize;
		private uint BitCount_FlagIndex;
		private uint BitCount_Unknown;
		private uint TotalHashSize;
		private uint HashSizeExtraBits;
		private uint EffectiveHashSize;
		private uint HashArraySize;
		private uint FlagCount;

		private List<uint> FileFlags;
		private List<byte[]> FileTable;
		private List<byte[]> HashTable;

		public BETTable(byte[] data)
		{
		}
	}
}

