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

namespace Warcraft.MPQ.Tables.Block
{
	public class BlockTableEntry
	{
		private readonly uint BlockOffset;
		private readonly uint BlockSize;
		private readonly uint FileSize;
		public BlockFlags Flags;

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

