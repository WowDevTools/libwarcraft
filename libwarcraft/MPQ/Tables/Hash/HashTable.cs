//
//  MPQHashTable.cs
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

using System.Collections.Generic;
using System.IO;
using Warcraft.MPQ.Crypto;

namespace Warcraft.MPQ.Tables.Hash
{
	public class HashTable
	{
		public static readonly uint TableKey = MPQCrypt.Hash("(hash table)", HashType.FileKey);
		private readonly List<HashTableEntry> Entries = new List<HashTableEntry>(65536);

		public HashTable()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.MPQ.Tables.Hash.HashTable"/> class from
		/// a block of data containing hash table entries.
		/// </summary>
		/// <param name="data">Data.</param>
		public HashTable(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (long i = 0; i < data.Length; i += HashTableEntry.GetSize())
					{
						byte[] entryBytes = br.ReadBytes((int)HashTableEntry.GetSize());
						this.Entries.Add(new HashTableEntry(entryBytes));
					}
				}
			}
		}

		/// <summary>
		/// Finds a valid entry for a given filename.
		/// </summary>
		/// <returns>The entry.</returns>
		/// <param name="fileName">File name.</param>
		public HashTableEntry FindEntry(string fileName)
		{
			uint EntryHomeIndex = MPQCrypt.Hash(fileName, HashType.FileHashTableOffset) & (uint) this.Entries.Count - 1;
			uint HashA = MPQCrypt.Hash(fileName, HashType.FilePathA);
			uint HashB = MPQCrypt.Hash(fileName, HashType.FilePathB);

			return FindEntry(HashA, HashB, EntryHomeIndex);
		}

		/// <summary>
		/// Finds a valid entry for a given hash pair, starting at the specified offset.
		/// </summary>
		/// <returns>The entry.</returns>
		/// <param name="HashA">A hash of the filename (Algorithm A).</param>
		/// <param name="HashB">A hash of the filename (Algorithm B)</param>
		/// <param name="EntryHomeIndex">The home index for the file we're searching for. Reduces lookup times.</param>
		public HashTableEntry FindEntry(uint HashA, uint HashB, uint EntryHomeIndex)
		{
			// First, see if the file has ever existed. If it has and matches, return it.
			if (this.Entries[(int)EntryHomeIndex].HasFileEverExisted())
			{
				if (this.Entries[(int)EntryHomeIndex].GetPrimaryHash() == HashA && this.Entries[(int)EntryHomeIndex].GetSecondaryHash() == HashB)
				{
					return this.Entries[(int)EntryHomeIndex];
				}
			}
			else
			{
				return null;
			}

			// If that file doesn't match (but has existed, or is occupied, let's keep looking down the table.
			HashTableEntry currentEntry = null;
			HashTableEntry deletionEntry = null;
			for (int i = (int)EntryHomeIndex + 1; i < this.Entries.Count - 1; ++i)
			{
				currentEntry = this.Entries[i];
				if (currentEntry.HasFileEverExisted())
				{
					if (currentEntry.GetPrimaryHash() == HashA && currentEntry.GetSecondaryHash() == HashB)
					{
						if (currentEntry.DoesFileExist())
						{
							// Found it!
							return currentEntry;
						}
						else
						{
							// The file might have been deleted. Store it as a possible return candidate, but keep looking.
							deletionEntry = currentEntry;
						}
					}
				}
			}

			// Still nothing? Loop around and scan the start of the table as well
			for (int i = 0; i < EntryHomeIndex; ++i)
			{
				currentEntry = this.Entries[i];
				if (currentEntry.HasFileEverExisted())
				{
					if (currentEntry.GetPrimaryHash() == HashA && currentEntry.GetSecondaryHash() == HashB)
					{
						if (currentEntry.DoesFileExist())
						{
							// Found it!
							return currentEntry;
						}
						else
						{
							// The file might have been deleted. Store it as a possible return candidate, but keep looking.
							deletionEntry = currentEntry;
						}
					}
				}
			}

			// We found the file, but it's been deleted.
			return deletionEntry;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					foreach (HashTableEntry Entry in this.Entries)
					{
						bw.Write(Entry.Serialize());
					}
				}

				byte[] encryptedTable = MPQCrypt.EncryptData(ms.ToArray(), TableKey);
				return encryptedTable;
			}
		}

		/// <summary>
		/// Gets the size of the entire hash table.
		/// </summary>
		/// <returns>The size.</returns>
		public ulong GetSize()
		{
			return (ulong)(this.Entries.Count * HashTableEntry.GetSize());
		}
	}
}

