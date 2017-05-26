//
//  ExtendedHashTableHeader.cs
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
	public class ExtendedHashTableHeader
	{
		public string Signature
		{
			get
			{
				return "HET\x1A";
			}
		}

		public uint Version;
		public uint DataSize;

		public uint TableSize;
		public uint MaxFileCount;
		public uint HashTableSize;
		public uint HashEntrySize;
		public uint IndexSizeExtra;
		public uint IndexSize;
		public uint BlockTableSize;

		List<byte> TableData = new List<byte>();

		public ExtendedHashTableHeader()
		{
		}
	}
}

