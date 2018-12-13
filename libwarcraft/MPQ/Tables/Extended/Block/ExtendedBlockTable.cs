//
//  ExtendedBlockTable.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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
        private uint BitIndexFilePosition;
        private uint BitIndexFileSize;
        private uint BitIndexCompressedSize;
        private uint BitIndexFlagIndex;
        private uint BitIndexUnknown;
        private uint BitCountFilePosition;
        private uint BitCountFileSize;
        private uint BitCountCompressedSize;
        private uint BitCountFlagIndex;
        private uint BitCountUnknown;
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

