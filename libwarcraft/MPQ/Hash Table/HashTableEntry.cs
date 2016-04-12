//
//  HashTableEntry.cs
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
using System.IO;

namespace Warcraft.MPQ.HashTable
{
	public class HashTableEntry
	{
		private uint FilePathHashA;
		private uint FilePathHashB;
		private ushort Localization;
		private ushort Platform;
		private uint FileBlockIndex;

		public HashTableEntry(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.FilePathHashA = br.ReadUInt32();
					this.FilePathHashB = br.ReadUInt32();
					this.Localization = br.ReadUInt16();

					// Read the platform as an int8 and skip the next byte
					this.Platform = br.ReadByte();
					br.BaseStream.Position += 1;

					this.FileBlockIndex = br.ReadUInt32();
				}
			}
		}

		/// <summary>
		/// Determines whether this has file ever existed.
		/// </summary>
		/// <returns><c>true</c> if this file ever existed; otherwise, <c>false</c>.</returns>
		public bool HasFileEverExisted()
		{
			return FileBlockIndex != 0xFFFFFFFF;
		}

		/// <summary>
		/// Determines whether this file exists.
		/// </summary>
		/// <returns><c>true</c>, if the file exists, <c>false</c> otherwise.</returns>
		public bool DoesFileExist()
		{
			return FileBlockIndex != 0xFFFFFFFE || FileBlockIndex != 0xFFFFFFFF;
		}

		/// <summary>
		/// Gets the size of an entry.
		/// </summary>
		/// <returns>The size.</returns>
		public static long GetSize()
		{
			return 16;
		}

		/// <summary>
		/// Gets the primary hash of the file's name.
		/// </summary>
		/// <returns>The primary hash.</returns>
		public uint GetPrimaryHash()
		{
			return this.FilePathHashA;
		}

		/// <summary>
		/// Gets the secondary hash of the file's name.
		/// </summary>
		/// <returns>The secondary hash.</returns>
		public uint GetSecondaryHash()
		{
			return this.FilePathHashB;
		}

		/// <summary>
		/// Gets the index of the block entry referenced by this entry.
		/// </summary>
		/// <returns>The block entry index.</returns>
		public uint GetBlockEntryIndex()
		{
			return this.FileBlockIndex;
		}
	}
}

