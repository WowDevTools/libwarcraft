using System;
using System.Collections.Generic;

namespace Warcraft.MPQ.HETTable
{
	public class HETTable
	{
		private uint Signature;
		private uint Version;
		private uint DataSize;

		private uint TableSize;
		private uint HashTableSize;
		private uint HashEntrySize;
		private uint TotalIndexSize;
		private uint IndexSizeExtra;
		private uint IndexSize;
		private uint BlockTableSize;

		private List<byte> HashTable;
		private List<ulong> FileIndices;

		public HETTable(byte[] data)
		{
		}
	}
}

