//
//  MPQHeader.cs
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
using System.IO;
using Warcraft.MPQ.Tables.Hash;
using Warcraft.MPQ.Tables.Block;

namespace Warcraft.MPQ
{
	public class MPQHeader
	{
		/*
			Fields present in Basic Format (v0)
		*/

		public string Signature
		{
			get
			{
				return "MPQ\x1a";
			}
		}

		public uint HeaderSize
		{
			get;
			private set;
		}

		private uint BasicArchiveSize;

		public ulong ArchiveSize
		{
			get
			{
				return GetArchiveSize();
			}
		}

		private MPQFormat Format;
		private ushort SectorSizeExponent;

		private uint HashTableOffset;
		private uint BlockTableOffset;

		public uint HashTableEntryCount
		{
			get;
			private set;
		}

		public uint BlockTableEntryCount
		{
			get;
			private set;
		}

		/*
			Fields present in Extended Format (v1)
		*/
		private ulong ExtendedBlockTableOffset;
		private ushort ExtendedFormatHashTableOffsetBits;
		private ushort ExtendedFormatBlockTableOffsetBits;

		/*
			Fields present in Extended Format (v2)
		*/
		private ulong LongArchiveSize;
		private ulong BETTableOffset;
		private ulong HETTableOffset;

		/*
			Fields present in Extended Format (v3)
		*/
		private ulong CompressedHashTableSize;
		private ulong CompressedBlockTableSize;
		private ulong CompressedExtendedBlockTableSize;
		private ulong CompressedHETTableSize;
		private ulong CompressedBETTableSize;

		private uint ChunkSizeForHashing;

		string MD5_BlockTable;
		string MD5_HashTable;
		string MD5_ExtendedBlockTable;
		string MD5_BETTable;
		string MD5_HETTable;
		string MD5_Header;
		// The MD5_Header is calculated from the start of the signature to the end of the MD5_HETTable

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.MPQ.MPQHeader"/> class.
		/// </summary>
		/// <param name="data">A byte array containing the header data of the archive.</param>
		/// <exception cref="FileLoadException">A FileLoadException may be thrown if the archive was not
		/// an MPQ file starting with the string "MPQ\x1a".</exception>
		public MPQHeader(byte[] data)
		{
			using (MemoryStream dataStream = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(dataStream))
				{
					if (this.Signature != new string(br.ReadChars(4)))
					{
						throw new FileLoadException("The provided file was not an MPQ file.");
					}

					this.HeaderSize = br.ReadUInt32();
					this.BasicArchiveSize = br.ReadUInt32();
					this.Format = (MPQFormat)br.ReadUInt16();
					this.SectorSizeExponent = br.ReadUInt16();
					this.HashTableOffset = br.ReadUInt32();
					this.BlockTableOffset = br.ReadUInt32();
					this.HashTableEntryCount = br.ReadUInt32();
					this.BlockTableEntryCount = br.ReadUInt32();

					if (this.Format >= MPQFormat.Extended_v1)
					{
						this.ExtendedBlockTableOffset = br.ReadUInt64();
						this.ExtendedFormatHashTableOffsetBits = br.ReadUInt16();
						this.ExtendedFormatBlockTableOffsetBits = br.ReadUInt16();
					}
					else
					{
						this.ExtendedBlockTableOffset = 0;
						this.ExtendedFormatHashTableOffsetBits = 0;
						this.ExtendedFormatBlockTableOffsetBits = 0;
					}

					if (this.Format >= MPQFormat.Extended_v2)
					{
						this.LongArchiveSize = br.ReadUInt64();
						this.BETTableOffset = br.ReadUInt64();
						this.HETTableOffset = br.ReadUInt64();
					}
					else
					{
						this.LongArchiveSize = 0;
						this.BETTableOffset = 0;
						this.HETTableOffset = 0;
					}

					if (this.Format >= MPQFormat.Extended_v3)
					{
						this.CompressedHashTableSize = br.ReadUInt64();
						this.CompressedBlockTableSize = br.ReadUInt64();
						this.CompressedExtendedBlockTableSize = br.ReadUInt64();
						this.CompressedHETTableSize = br.ReadUInt64();
						this.CompressedBETTableSize = br.ReadUInt64();

						this.ChunkSizeForHashing = br.ReadUInt32();

						this.MD5_BlockTable = BitConverter.ToString(br.ReadBytes(16));
						this.MD5_HashTable = BitConverter.ToString(br.ReadBytes(16));
						this.MD5_ExtendedBlockTable = BitConverter.ToString(br.ReadBytes(16));
						this.MD5_BETTable = BitConverter.ToString(br.ReadBytes(16));
						this.MD5_HETTable = BitConverter.ToString(br.ReadBytes(16));
						this.MD5_Header = BitConverter.ToString(br.ReadBytes(16));
					}
					else
					{
						this.CompressedHashTableSize = 0;
						this.CompressedBlockTableSize = 0;
						this.CompressedExtendedBlockTableSize = 0;
						this.CompressedHETTableSize = 0;
						this.CompressedBETTableSize = 0;

						this.ChunkSizeForHashing = 0;

						this.MD5_BlockTable = "";
						this.MD5_HashTable = "";
						this.MD5_ExtendedBlockTable = "";
						this.MD5_BETTable = "";
						this.MD5_HETTable = "";
						this.MD5_Header = "";
					}
				}
			}
		}

		/// <summary>
		/// Gets the size of the hash table.
		/// </summary>
		/// <returns>The hash table size.</returns>
		public ulong GetHashTableSize()
		{
			return (ulong)(this.HashTableEntryCount * HashTableEntry.GetSize());
		}

		/// <summary>
		/// Gets the size of the block table.
		/// </summary>
		/// <returns>The block table size.</returns>
		public ulong GetBlockTableSize()
		{
			return (ulong)(this.BlockTableEntryCount * BlockTableEntry.GetSize());
		}

		/// <summary>
		/// Gets the size of the extended block table.
		/// </summary>
		/// <returns>The extended block table size.</returns>
		public ulong GetExtendedBlockTableSize()
		{
			return (ulong)this.BlockTableEntryCount * sizeof(ushort);
		}

		/// <summary>
		/// Gets the offset of the hash table, adjusted using the high bits if neccesary
		/// </summary>
		/// <returns>The hash table offset.</returns>
		public ulong GetHashTableOffset()
		{
			if (this.Format == MPQFormat.Basic)
			{
				return (ulong)this.HashTableOffset;
			}
			else
			{
				return MergeHighBits(GetBaseHashTableOffset(), GetExtendedHashTableOffsetBits());
			}
		}

		/// <summary>
		/// Gets the base hash table offset.
		/// </summary>
		/// <returns>The base hash table offset.</returns>
		private uint GetBaseHashTableOffset()
		{
			return this.HashTableOffset;
		}

		/// <summary>
		/// Gets the offset of the block table, adjusted using the high bits if neccesary.
		/// </summary>
		/// <returns>The block table offset.</returns>
		public ulong GetBlockTableOffset()
		{
			if (this.Format == MPQFormat.Basic)
			{
				return (ulong)this.BlockTableOffset;
			}
			else
			{
				return MergeHighBits(GetBaseBlockTableOffset(), GetExtendedBlockTableOffsetBits());
			}
		}

		/// <summary>
		/// Gets the base block table offset.
		/// </summary>
		/// <returns>The base block table offset.</returns>
		private uint GetBaseBlockTableOffset()
		{
			return this.BlockTableOffset;
		}

		/// <summary>
		/// Gets the number of entries in the hash table.
		/// </summary>
		/// <returns>The number of hash table entries.</returns>
		public uint GetHashTableEntryCount()
		{
			return this.HashTableEntryCount;
		}

		/// <summary>
		/// Gets the number of entries in the block table.
		/// </summary>
		/// <returns>The number of block table entries.</returns>
		public uint GetBlockTableEntryCount()
		{
			return this.BlockTableEntryCount;
		}

		/// <summary>
		/// Gets the format of the MPQ archive.
		/// </summary>
		/// <returns>The format of the archive.</returns>
		public MPQFormat GetFormat()
		{
			return this.Format;
		}

		/// <summary>
		/// Gets the offset to the extended block table.
		/// </summary>
		/// <returns>The extended block table offset.</returns>
		public ulong GetExtendedBlockTableOffset()
		{
			return this.ExtendedBlockTableOffset;
		}

		/// <summary>
		/// Gets the high 16 bits of the extended hash table offset.
		/// </summary>
		/// <returns>The extended hash table offset bits.</returns>
		public ushort GetExtendedHashTableOffsetBits()
		{
			return this.ExtendedFormatHashTableOffsetBits;
		}

		/// <summary>
		/// Gets the high 16 bits of the extended block table offset.
		/// </summary>
		/// <returns>The extended block table offset bits.</returns>
		public ushort GetExtendedBlockTableOffsetBits()
		{
			return this.ExtendedFormatBlockTableOffsetBits;
		}

		/// <summary>
		/// Gets the size of the archive.
		/// </summary>
		/// <returns>The archive size.</returns>
		public ulong GetArchiveSize()
		{			
			if (this.Format == MPQFormat.Basic)
			{
				return (ulong)this.BasicArchiveSize;
			}
			else if (this.Format == MPQFormat.Extended_v1)
			{
				// Calculate the size from the start of the header to the end of the 
				// hash table, block table or extended block table (whichever is furthest away)

				ulong hashTableOffset = this.GetHashTableOffset();
				ulong blockTableOffset = this.GetBlockTableOffset();
				ulong extendedBlockTableOffset = this.GetExtendedBlockTableOffset();

				ulong furthestOffset = 0;
				ulong archiveSize = 0;

				// Sort the sizes
				if (hashTableOffset > furthestOffset)
				{
					furthestOffset = hashTableOffset;

					archiveSize = furthestOffset + GetHashTableSize();
				}

				if (blockTableOffset > furthestOffset)
				{
					furthestOffset = blockTableOffset;

					archiveSize = furthestOffset + GetBlockTableSize();
				}

				if (extendedBlockTableOffset > furthestOffset)
				{
					furthestOffset = extendedBlockTableOffset;

					archiveSize = furthestOffset + GetExtendedBlockTableSize();
				}

				return archiveSize;
			}
			else
			{
				return this.LongArchiveSize;
			}
		}

		/// <summary>
		/// Gets the sector size exponent.
		/// </summary>
		/// <returns>The sector size exponent.</returns>
		public uint GetSectorSizeExponent()
		{
			return this.SectorSizeExponent;
		}

		/// <summary>
		/// Merges a base 32-bit number with a 16-bit number, forming a 64-bit number where the uppermost 16 bits are 0.
		/// </summary>
		/// <returns>The final number.</returns>
		/// <param name="baseBits">Base bits.</param>
		/// <param name="highBits">High bits.</param>
		public static ulong MergeHighBits(uint baseBits, ushort highBits)
		{
			ulong lower32Bits = (ulong)baseBits;
			ulong high16Bits = (ulong)highBits << 32 & 0x0000FFFF00000000;

			ulong mergedBits = (high16Bits + lower32Bits) & 0x0000FFFFFFFFFFFF;
			return mergedBits;
		}
	}
}

