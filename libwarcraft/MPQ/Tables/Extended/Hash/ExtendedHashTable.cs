//
//  ExtendedHashTable.cs
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

namespace Warcraft.MPQ.Tables.Extended.Hash
{
	public class ExtendedHashTable
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

		public ExtendedHashTable(byte[] data)
		{
		}
	}
}

