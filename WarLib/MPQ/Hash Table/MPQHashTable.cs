using System;
using System.Collections.Generic;
using System.IO;
using Warcraft.MPQ.Crypto;

namespace Warcraft.MPQ
{
	public class MPQHashTable
	{
		public static readonly uint TableKey = MPQCrypt.Hash("(hash table)", HashType.FileKey);
		private readonly List<HashTableEntry> Entries;

		public MPQHashTable(byte[] data)
		{
			MemoryStream dataStream = new MemoryStream(data);
			BinaryReader br = new BinaryReader(dataStream);

			this.Entries = new List<HashTableEntry>();

			for (long i = 0; i < data.Length; i += HashTableEntry.GetSize())
			{
				byte[] entryBytes = br.ReadBytes((int)HashTableEntry.GetSize());
				Entries.Add(new HashTableEntry(entryBytes));
			}

			br.Close();
		}

		/// <summary>
		/// Finds a valid entry for a given filename.
		/// </summary>
		/// <returns>The entry.</returns>
		/// <param name="fileName">File name.</param>
		public HashTableEntry FindEntry(string fileName)
		{
			uint HashA = MPQCrypt.Hash(fileName, HashType.FilePathA);
			uint HashB = MPQCrypt.Hash(fileName, HashType.FilePathB);

			return FindEntry(HashA, HashB);
		}

		/// <summary>
		/// Finds a valid entry for a given hash pair.
		/// </summary>
		/// <returns>The entry.</returns>
		/// <param name="HashA">Hash a.</param>
		/// <param name="HashB">Hash b.</param>
		public HashTableEntry FindEntry(uint HashA, uint HashB)
		{
			foreach (HashTableEntry Entry in Entries)
			{
				if (Entry.GetPrimaryHash() == HashA && Entry.GetSecondaryHash() == HashB)
				{
					return Entry;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the size of the entire hash table.
		/// </summary>
		/// <returns>The size.</returns>
		public ulong GetSize()
		{
			return (ulong)(Entries.Count * HashTableEntry.GetSize());
		}
	}
}

