using System;
using System.Collections.Generic;
using System.IO;
using WarLib.MPQ.Crypto;

namespace WarLib.MPQ
{
	public class MPQBlockTable
	{
		public static readonly uint TableKey = MPQCrypt.Hash("(block table)", HashType.FileKey);
		private static readonly List<BlockTableEntry> Entries = new List<BlockTableEntry>();

		public MPQBlockTable(byte[] data)
		{
			MemoryStream dataStream = new MemoryStream(data);
			BinaryReader br = new BinaryReader(dataStream);

			for (long i = 0; i < data.Length; i += BlockTableEntry.GetSize())
			{
				byte[] entryBytes = br.ReadBytes((int)BlockTableEntry.GetSize());
				Entries.Add(new BlockTableEntry(entryBytes));
			}

			br.Close();
		}

		public BlockTableEntry GetEntry(int index)
		{
			return Entries[index];
		}

		public ulong GetSize()
		{
			return (ulong)(Entries.Count * BlockTableEntry.GetSize());
		}
	}
}

