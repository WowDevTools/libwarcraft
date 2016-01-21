using System;
using System.IO;
using System.Security.Cryptography;

namespace Warcraft.MPQ
{
	public class MPQHeader
	{
		// Basic format, v0 (World of Warcraft and earlier)
		private string Filetype;
		private uint HeaderSize;
		private uint ArchiveSize;
		private MPQFormat Format;
		private ushort SectorSizeExponent;
		private uint HashTableOffset;
		private uint BlockTableOffset;
		private uint HashTableEntries;
		private uint BlockTableEntries;

		// Extended format, v1 (Burning Crusade and newer)
		private ulong ExtendedBlockTableOffset;
		private ushort ExtendedFormatHashTableOffsetBits;
		private ushort ExtendedFormatBlockTableOffsetBits;

		// Extended format, v2 (Cataclysm Beta and newer)
		private ulong LongArchiveSize;
		private ulong BETTableOffset;
		private ulong HETTableOffset;

		/// Extended format, v3 (Cataclysm Beta and newer)
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
		/// Initializes a new instance of the <see cref="WarLib.MPQ.MPQHeader"/> class.
		/// </summary>
		/// <param name="data">A byte array containing the header data of the archive.</param>
		public MPQHeader(byte[] data)
		{
			MemoryStream dataStream = new MemoryStream(data);
			BinaryReader br = new BinaryReader(dataStream);

			this.Filetype = new string(br.ReadChars(4));
			this.HeaderSize = br.ReadUInt32();
			this.ArchiveSize = br.ReadUInt32();
			this.Format = (MPQFormat)br.ReadUInt16();
			this.SectorSizeExponent = br.ReadUInt16();
			this.HashTableOffset = br.ReadUInt32();
			this.BlockTableOffset = br.ReadUInt32();
			this.HashTableEntries = br.ReadUInt32();
			this.BlockTableEntries = br.ReadUInt32();

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

			br.Close();
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
				return (ulong)MergeHighBits(GetBaseHashTableOffset(), GetExtendedHashTableOffsetBits());
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
				return (ulong)MergeHighBits(GetBaseBlockTableOffset(), GetExtendedBlockTableOffsetBits());
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
		public uint GetNumHashTableEntries()
		{
			return this.HashTableEntries;
		}

		/// <summary>
		/// Gets the number of entries in the block table.
		/// </summary>
		/// <returns>The number of block table entries.</returns>
		public uint GetNumBlockTableEntries()
		{
			return this.BlockTableEntries;
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
		/// <param name="hashTableSize">Hash table size.</param>
		/// <param name="blockTableSize">Block table size.</param>
		/// <param name="extendedBlockTableSize">Extended block table size.</param>
		public ulong GetArchiveSize(ulong hashTableSize, ulong blockTableSize, ulong extendedBlockTableSize)
		{
			if (this.Format == MPQFormat.Basic)
			{
				return (ulong)this.ArchiveSize;
			}
			else
			{
				// Calculate the size from the start of the header to the end of the 
				// hash table, block table or extended block table (whichever is furthest away)

				ulong archiveStart = 0;
				ulong archiveEnd = 0;

				ulong hashTableOffset = this.GetHashTableOffset();
				ulong blockTableOffset = this.GetBlockTableOffset();
				ulong extendedBlockTableOffset = this.GetExtendedBlockTableOffset();

				ulong furthestOffset = 0;
				ulong archiveSize = 0;
				// Sort the sizes
				if (hashTableOffset > furthestOffset)
				{
					furthestOffset = hashTableOffset;

					archiveEnd = furthestOffset + hashTableSize;
				}

				if (blockTableOffset > furthestOffset)
				{
					furthestOffset = blockTableOffset;

					archiveEnd = furthestOffset + blockTableSize;
				}

				if (extendedBlockTableOffset > furthestOffset)
				{
					furthestOffset = extendedBlockTableOffset;

					archiveEnd = furthestOffset + extendedBlockTableSize;
				}

				archiveSize = archiveEnd - archiveStart;
				return archiveSize;
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
		/// <param name="baseOffset">Base bits.</param>
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

