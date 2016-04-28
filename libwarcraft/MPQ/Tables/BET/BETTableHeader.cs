//
//  BETTableHeader.cs
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
using Warcraft.MPQ.Tables.Block;

namespace Warcraft.MPQ.Tables.BET
{
	public class BETTableHeader
	{
		public string Signature
		{
			get
			{
				return "BET\x1A";
			}
		}

		public uint Version;
		public uint DataSize;

		public uint TableSize;
		public uint FileCount;
		public uint UnknownFlag;
		public uint TableEntrySize;
		public uint BitIndexFilePosition;
		public uint BitIndexFileSize;
		public uint BitIndexCompressedSize;
		public uint BitIndexFlagIndex;
		public uint BitIndexUnknown;
		public uint BitCountFilePosition;
		public uint BitCountFileSize;
		public uint BitCountCompressedSize;
		public uint BitCountFlagIndex;
		public uint BitCountUnknown;

		public uint BETHashSizeTotal;
		public uint BETHashSizeExtra;
		public uint BETHashSize;
		public uint BETHashArraySize;
		public uint FlagCount;

		List<BlockFlags> FlagArray = new List<BlockFlags>();


		public BETTableHeader()
		{
		}
	}
}

