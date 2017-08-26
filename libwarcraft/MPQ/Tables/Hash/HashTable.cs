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
using Warcraft.Core.Interfaces;
using Warcraft.MPQ.Crypto;

namespace Warcraft.MPQ.Tables.Hash
{
	/// <summary>
	/// The hash table is a table containing hashed file paths for quick lookup in the archive. When stored
	/// in binary format, it can be both compressed and encrypted.
	/// </summary>
	public class HashTable : IBinarySerializable
	{
		/// <summary>
		/// The encryption key for the hash table data.
		/// </summary>
		public static readonly uint TableKey = MPQCrypt.Hash("(hash table)", HashType.FileKey);

		/// <summary>
		/// The entries contained in the hash table.
		/// </summary>
		private readonly List<HashTableEntry> Entries = new List<HashTableEntry>(65536);

		/// <summary>
		/// Initializes a new instance of the <see cref="HashTable"/> class.
		/// </summary>
		public HashTable()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HashTable"/> class from
		/// a block of data containing hash table entries.
		/// </summary>
		/// <param name="data">ExtendedData.</param>
		public HashTable(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (long i = 0; i < data.Length; i += HashTableEntry.GetSize())
					{
						byte[] entryBytes = br.ReadBytes((int)HashTableEntry.GetSize());
						HashTableEntry newEntry = new HashTableEntry(entryBytes);
						this.Entries.Add(newEntry);
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
			uint entryHomeIndex = MPQCrypt.Hash(fileName, HashType.FileHashTableOffset) & (uint) this.Entries.Count - 1;
			uint hashA = MPQCrypt.Hash(fileName, HashType.FilePathA);
			uint hashB = MPQCrypt.Hash(fileName, HashType.FilePathB);

			return FindEntry(hashA, hashB, entryHomeIndex);
		}

		/// <summary>
		/// Finds a valid entry for a given hash pair, starting at the specified offset.
		/// </summary>
		/// <returns>The entry.</returns>
		/// <param name="hashA">A hash of the filename (Algorithm A).</param>
		/// <param name="hashB">A hash of the filename (Algorithm B)</param>
		/// <param name="entryHomeIndex">The home index for the file we're searching for. Reduces lookup times.</param>
		public HashTableEntry FindEntry(uint hashA, uint hashB, uint entryHomeIndex)
		{
			// First, see if the file has ever existed. If it has and matches, return it.
			if (this.Entries[(int)entryHomeIndex].HasFileEverExisted())
			{
				if (this.Entries[(int)entryHomeIndex].GetPrimaryHash() == hashA && this.Entries[(int)entryHomeIndex].GetSecondaryHash() == hashB)
				{
					return this.Entries[(int)entryHomeIndex];
				}
			}
			else
			{
				return null;
			}

			// If that file doesn't match (but has existed, or is occupied, let's keep looking down the table.
			HashTableEntry currentEntry;
			HashTableEntry deletionEntry = null;
			for (int i = (int)entryHomeIndex + 1; i < this.Entries.Count - 1; ++i)
			{
				currentEntry = this.Entries[i];
				if (!currentEntry.HasFileEverExisted())
				{
					continue;
				}

				if (currentEntry.GetPrimaryHash() != hashA || currentEntry.GetSecondaryHash() != hashB)
				{
					continue;
				}

				if (currentEntry.DoesFileExist())
				{
					// Found it!
					return currentEntry;
				}

				// The file might have been deleted. Store it as a possible return candidate, but keep looking.
				deletionEntry = currentEntry;
			}

			// Still nothing? Loop around and scan the start of the table as well
			for (int i = 0; i < entryHomeIndex; ++i)
			{
				currentEntry = this.Entries[i];
				if (!currentEntry.HasFileEverExisted())
				{
					continue;
				}

				if (currentEntry.GetPrimaryHash() != hashA || currentEntry.GetSecondaryHash() != hashB)
				{
					continue;
				}

				if (currentEntry.DoesFileExist())
				{
					// Found it!
					return currentEntry;
				}

				// The file might have been deleted. Store it as a possible return candidate, but keep looking.
				deletionEntry = currentEntry;
			}

			// We found the file, but it's been deleted.
			return deletionEntry;
		}

		/// <summary>
		/// Serializes the current object into a byte array.
		/// </summary>
		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					foreach (HashTableEntry entry in this.Entries)
					{
						bw.Write(entry.Serialize());
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

