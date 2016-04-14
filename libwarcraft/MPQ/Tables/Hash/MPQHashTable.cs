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

using System;
using System.Collections.Generic;
using System.IO;
using Warcraft.MPQ.Crypto;

namespace Warcraft.MPQ.Tables.Hash
{
	public class MPQHashTable
	{
		public static readonly uint TableKey = MPQCrypt.Hash("(hash table)", HashType.FileKey);
		private readonly List<HashTableEntry> Entries;

		public MPQHashTable(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Entries = new List<HashTableEntry>();

					for (long i = 0; i < data.Length; i += HashTableEntry.GetSize())
					{
						byte[] entryBytes = br.ReadBytes((int)HashTableEntry.GetSize());
						Entries.Add(new HashTableEntry(entryBytes));
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

