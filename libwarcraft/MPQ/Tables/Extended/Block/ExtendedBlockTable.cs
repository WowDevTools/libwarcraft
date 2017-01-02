//
//  ExtendedBlockTable.cs
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

namespace Warcraft.MPQ.Tables.Extended.Block
{
	public class ExtendedBlockTable
	{
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

		public ExtendedBlockTable(byte[] data)
		{
		}
	}
}

