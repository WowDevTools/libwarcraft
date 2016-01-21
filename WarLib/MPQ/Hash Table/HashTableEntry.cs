using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Warcraft.MPQ
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
			MemoryStream dataStream = new MemoryStream(data);
			BinaryReader br = new BinaryReader(dataStream);

			this.FilePathHashA = br.ReadUInt32();
			this.FilePathHashB = br.ReadUInt32();
			this.Localization = br.ReadUInt16();

			// Read the platform as an int8 and skip the next byte
			this.Platform = br.ReadByte();
			br.BaseStream.Position += 1;

			this.FileBlockIndex = br.ReadUInt32();

			br.Close();
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

