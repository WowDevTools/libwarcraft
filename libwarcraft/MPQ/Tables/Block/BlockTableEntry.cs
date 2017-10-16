//
//  BlockTableEntry.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.MPQ.Tables.Block
{
	/// <summary>
	/// Represents a table entry in the block table, which holds information about how a file is stored in the
	/// archive, its size, and where it is.
	/// </summary>
	public class BlockTableEntry : IBinarySerializable
	{
		/// <summary>
		/// The offset into the archive file where the file data begins.
		/// </summary>
		private readonly uint BlockOffset;

		/// <summary>
		/// The absolute size in bytes of the stored file data.
		/// </summary>
		private readonly uint BlockSize;

		/// <summary>
		/// The absolute original file size in bytes.
		/// </summary>
		private readonly uint FileSize;

		/// <summary>
		/// A set of flags which holds some information about how the file is stored.
		/// </summary>
		public BlockFlags Flags;

		/// <summary>
		/// Deserializes a <see cref="BlockTableEntry"/> from provided binary data.
		/// </summary>
		/// <param name="data">The serialized data.</param>
		public BlockTableEntry(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.BlockOffset = br.ReadUInt32();
					this.BlockSize = br.ReadUInt32();
					this.FileSize = br.ReadUInt32();
					this.Flags = (BlockFlags)br.ReadUInt32();
				}
			}
		}

		/// <summary>
		/// Gets the offset of the block with file data.
		/// </summary>
		/// <returns>The block offset.</returns>
		public uint GetBlockOffset()
		{
			return this.BlockOffset;
		}

		/// <summary>
		/// Gets the block offset, merging the provided high bits with
		/// the offset in the block table entry.
		/// </summary>
		/// <param name="highBits">The bits to merge in.</param>
		/// <returns>The final offset.</returns>
		public ulong GetExtendedBlockOffset(ushort highBits)
		{
			return MPQHeader.MergeHighBits(this.BlockOffset, highBits);
		}

		/// <summary>
		/// Gets the size of the data block.
		/// </summary>
		/// <returns>The block size.</returns>
		public uint GetBlockSize()
		{
			return this.BlockSize;
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <returns>The flags.</returns>
		public BlockFlags GetFlags()
		{
			return this.Flags;
		}

		/// <summary>
		/// Gets the size of the decompressed and decrypted file.
		/// </summary>
		/// <returns>The file size.</returns>
		public uint GetFileSize()
		{
			return this.FileSize;
		}

		/// <summary>
		/// Determines whether this data block is empty.
		/// </summary>
		/// <returns><c>true</c> if this instance is block empty; otherwise, <c>false</c>.</returns>
		public bool IsBlockEmpty()
		{
			return (this.BlockOffset != 0) && (this.BlockSize != 0) && (this.FileSize == 0) && (this.Flags == 0);
		}

		/// <summary>
		/// Determines whether this data block is unused.
		/// </summary>
		/// <returns><c>true</c> if this instance is block unused; otherwise, <c>false</c>.</returns>
		public bool IsBlockUnused()
		{
			return (this.BlockOffset == 0) && (this.BlockSize == 0) && (this.FileSize == 0) && (this.Flags == 0);
		}

		/// <summary>
		/// Determines whether or not the file data contained in this block is compressed or not.
		/// Oddly enough, on occasion the block will be marked as compressed but will not actually be compressed.
		/// These sorts of blocks can be detected by comparing the block size with the actual file size, which
		/// is done by this method.
		/// </summary>
		/// <returns><value>true</value> if the block is compressed; otherwise, <value>false</value>.</returns>
		public bool IsCompressed()
		{
			bool isCompressedInAnyWay = this.Flags.HasFlag(BlockFlags.IsCompressed) ||
			                            this.Flags.HasFlag(BlockFlags.IsCompressedMultiple) ||
										this.Flags.HasFlag(BlockFlags.IsImploded);

			// Oddly enough, on occasion the block will be marked as compressed but will not actually be compressed.
			// These sorts of blocks can be detected by comparing the block size with the actual file size.
			return (this.BlockSize != this.FileSize) && isCompressedInAnyWay;
		}

		/// <summary>
		/// Determines whether or not the file data contained in this block is encrypted or not.
		/// </summary>
		/// <returns><value>true</value> if the block is encrypted; otherwise, <value>false</value>.</returns>
		public bool IsEncrypted()
		{
			return this.Flags.HasFlag(BlockFlags.IsEncrypted);
		}

		/// <summary>
		/// Determines whether or not the file data is stored as a set of sectors. If not, it's in a single unit.
		/// </summary>
		/// <returns><value>true</value> if the block is stored in sectors; otherwise, <value>false</value>.</returns>
		public bool HasSectors()
		{
			return !this.Flags.HasFlag(BlockFlags.IsSingleUnit);
		}

		/// <summary>
		/// Determines whether or not this block entry is pointing to any file data at all.
		/// </summary>
		/// <returns><value>true</value> if the block points to any data; otherwise, <value>false</value>.</returns>
		public bool HasData()
		{
			return this.Flags.HasFlag(BlockFlags.Exists) && !this.Flags.HasFlag(BlockFlags.IsDeletionMarker);
		}

		/// <summary>
		/// Determines whether or not this block entry is pointing to a deleted file.
		/// </summary>
		/// <returns></returns>
		public bool IsDeleted()
		{
			return this.Flags.HasFlag(BlockFlags.IsDeletionMarker);
		}

		/// <summary>
		/// Determines whether or not the file data is stored as a single unit. If not, it's in sectors.
		/// </summary>
		/// <returns><value>true</value> if the block is stored as a single unit; otherwise, <value>false</value>.</returns>
		public bool IsSingleUnit()
		{
			return this.Flags.HasFlag(BlockFlags.IsSingleUnit);
		}

		/// <summary>
		/// Determined whether or not the encryption key should be adjusted by the block offset.
		/// </summary>
		/// <returns><value>true</value> if the key should be adjusted; otherwise, <value>false</value>.</returns>
		public bool ShouldEncryptionKeyBeAdjusted()
		{
			return this.Flags.HasFlag(BlockFlags.HasAdjustedEncryptionKey);
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
					bw.Write(this.BlockOffset);
					bw.Write(this.BlockSize);
					bw.Write(this.FileSize);
					bw.Write((uint)this.Flags);
				}

				return ms.ToArray();
			}
		}

		/// <summary>
		/// Gets the size of a block table entry.
		/// </summary>
		/// <returns>The size.</returns>
		public static long GetSize()
		{
			return 16;
		}
	}
}

