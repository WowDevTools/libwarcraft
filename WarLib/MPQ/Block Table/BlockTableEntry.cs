using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WarLib.MPQ
{
	public class BlockTableEntry
	{
		private uint BlockOffset;
		private uint BlockSize;
		private uint FileSize;
		public BlockFlags Flags;

		public BlockTableEntry(byte[] data)
		{
			MemoryStream dataStream = new MemoryStream(data);
			BinaryReader br = new BinaryReader(dataStream);

			this.BlockOffset = br.ReadUInt32();
			this.BlockSize = br.ReadUInt32();
			this.FileSize = br.ReadUInt32();
			this.Flags = (BlockFlags)br.ReadUInt32();

			br.Close();
		}

		/// <summary>
		/// Gets the size of a block table entry.
		/// </summary>
		/// <returns>The size.</returns>
		public static long GetSize()
		{
			return 16;
		}

		/// <summary>
		/// Gets the offset of the block with file data.
		/// </summary>
		/// <returns>The block offset.</returns>
		public uint GetBlockOffset()
		{
			return this.BlockOffset;
		}

		public ulong GetExtendedBlockOffset(ushort highBits)
		{
			return MPQHeader.MergeHighBits(BlockOffset, highBits);
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
			return (BlockOffset != 0) && (BlockSize != 0) && (FileSize == 0) && (Flags == 0);
		}

		/// <summary>
		/// Determines whether this data block is unused.
		/// </summary>
		/// <returns><c>true</c> if this instance is block unused; otherwise, <c>false</c>.</returns>
		public bool IsBlockUnused()
		{
			return (BlockOffset == 0) && (BlockSize == 0) && (FileSize == 0) && (Flags == 0);
		}
	}
}

