//
//  MPQ.cs
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
// A massive thanks to Justin Olbrantz (Quantam) and Jean-Francois Roy
// (BahamutZERO), whose [documentation of the MPQ
// format](http://wiki.devklog.net/index.php?title=The_MoPaQ_Archive_Format) was
// instrumental for this implementation. Although their wiki is no longer online, 
// a backup snapshot taken by the Wayback Machine saved the day. The documentation
// is available upon request.

using System;
using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Compression;
using Warcraft.MPQ.Crypto;
using Warcraft.MPQ.FileInfo;
using Warcraft.MPQ.Tables.Block;
using Warcraft.MPQ.Tables.Hash;

namespace Warcraft.MPQ
{
	public class MPQ : IDisposable
	{
		/// <summary>
		/// The header of the MPQ archive. Contains information about sizes and offsets of relational structures
		/// such as the hash and block table in the archive.
		/// </summary>
		public MPQHeader Header;

		/// <summary>
		/// The hash table. Contains hash entries for all files stored in the archive, and for any overrides.
		/// </summary>
		public HashTable ArchiveHashTable;

		/// <summary>
		/// The block table. Contains block entries for all files stored in the archive. These entries contain
		/// information about the state of the file.
		/// </summary>
		public BlockTable ArchiveBlockTable;

		/// <summary>
		/// The extended block table. Contains a list of upper bits of a ulong number, which is in later versions
		/// of the archive format merged with the offset listed in the block table. This format extension allows
		/// archives to grow larger than 4GB in size.
		/// </summary>
		public List<ushort> ExtendedBlockTable = new List<ushort>();

		/// <summary>
		/// The archive reader. A BinaryReader that exists for the lifetime of the MPQ object and handles all
		/// the file reading inside it. As archives are far too big to be loaded into memory all at once, 
		/// we seek to the desired parts and read them as we need them.
		/// </summary>
		private readonly BinaryReader ArchiveReader;

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.MPQ.MPQ"/> class.
		/// </summary>
		/// <param name="mpqStream">An open stream to data containing an MPQ archive.</param>
		public MPQ(Stream mpqStream)
		{
			ArchiveReader = new BinaryReader(mpqStream);

			this.Header = new MPQHeader(ArchiveReader.ReadBytes((int)PeekHeaderSize()));

			// Seek to the hash table and load it
			ArchiveReader.BaseStream.Position = (long)this.Header.GetHashTableOffset();

			byte[] encryptedHashTable = ArchiveReader.ReadBytes((int)this.Header.GetHashTableSize());			
			this.ArchiveHashTable = new HashTable(MPQCrypt.DecryptData(encryptedHashTable, HashTable.TableKey));

			// Seek to the block table and load it
			ArchiveReader.BaseStream.Position = (long)this.Header.GetBlockTableOffset();

			byte[] encryptedBlockTable = ArchiveReader.ReadBytes((int)this.Header.GetBlockTableSize());
			this.ArchiveBlockTable = new BlockTable(MPQCrypt.DecryptData(encryptedBlockTable, BlockTable.TableKey));

			if (this.Header.GetFormat() >= MPQFormat.Extended_v1)
			{
				// Seek to the extended block table and load it, if neccesary	
				ArchiveReader.BaseStream.Position = (long)this.Header.GetExtendedBlockTableOffset();

				for (int i = 0; i < this.Header.GetBlockTableEntryCount(); ++i)
				{
					ExtendedBlockTable.Add(ArchiveReader.ReadUInt16());
				}
			}
		}

		/// <summary>
		/// Peeks at the size of the entire MPQ header without advancing the byte position of the
		/// binary reader.
		/// </summary>
		/// <returns>The header size.</returns>
		private uint PeekHeaderSize()
		{
			long originalPosition = ArchiveReader.BaseStream.Position;

			ArchiveReader.BaseStream.Position = 4;
			uint headerSize = ArchiveReader.ReadUInt32();
			ArchiveReader.BaseStream.Position = originalPosition;

			return headerSize;
		}

		/// <summary>
		/// Determines whether this archive has a listfile.
		/// </summary>
		/// <returns><c>true</c> if this instance has a listfile; otherwise, <c>false</c>.</returns>
		public bool HasFileList()
		{
			return DoesFileExist("(listfile)");
		}

		/// <summary>
		/// Extracts the listfile from the archive and returns it as an enumerable list.
		/// </summary>
		/// <returns>The listfile.</returns>
		public List<string> GetFileList()
		{
			List<string> fileList = new List<string>();

			byte[] listfileBytes = ExtractFile("(listfile)");
			if (listfileBytes != null)
			{
				using (MemoryStream listfileStream = new MemoryStream(listfileBytes))
				{
					using (TextReader tr = new StreamReader(listfileStream))
					{
						string line;
						while ((line = tr.ReadLine()) != null)
						{
							fileList.Add(line);
						}
					}
				}
			}
			else
			{
				return null;
			}

			return fileList;
		}

		/// <summary>
		/// Checks if the specified file path exists in the archive.
		/// </summary>
		/// <returns><c>true</c>, if the file exists, <c>false</c> otherwise.</returns>
		/// <param name="filePath">File path.</param>
		public bool DoesFileExist(string filePath)
		{
			HashTableEntry fileHashEntry = ArchiveHashTable.FindEntry(filePath);
			return fileHashEntry != null;
		}

		/// <summary>
		/// Gets the file info of the provided path.
		/// </summary>
		/// <returns>The file info, or null if the file doesn't exist in the archive.</returns>
		/// <param name="filePath">File path.</param>
		public MPQFileInfo GetFileInfo(string filePath)
		{
			if (DoesFileExist(filePath))
			{
				HashTableEntry hashEntry = ArchiveHashTable.FindEntry(filePath);
				BlockTableEntry blockEntry = ArchiveBlockTable.GetEntry((int)hashEntry.GetBlockEntryIndex());

				return new MPQFileInfo(filePath, hashEntry, blockEntry);
			}
			else
			{
				return null;
			}
		}

		// TODO: Filter files based on language and platform
		/// <summary>
		/// Extract the file at <paramref name="filePath"/> from the archive.
		/// </summary>
		/// <returns>The file as a byte array, or null if the file could not be found.</returns>
		/// <param name="filePath">Path to the file in the archive.</param>
		public byte[] ExtractFile(string filePath)
		{
			// Reset all positions to be safe
			ArchiveReader.BaseStream.Position = 0;

			HashTableEntry fileHashEntry = ArchiveHashTable.FindEntry(filePath);
			if (fileHashEntry != null)
			{
				BlockTableEntry fileBlockEntry = ArchiveBlockTable.GetEntry((int)fileHashEntry.GetBlockEntryIndex());

				// Drop out if the file is not actually a file
				if (!fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsFile))
				{
					return null;
				}

				// Seek to the beginning of the file's sectors
				long adjustedBlockOffset;
				if (this.Header.GetFormat() == MPQFormat.Extended_v1 && RequiresExtendedFormat())
				{
					ushort upperOffsetBits = this.ExtendedBlockTable[(int)fileHashEntry.GetBlockEntryIndex()];
					adjustedBlockOffset = (long)fileBlockEntry.GetExtendedBlockOffset(upperOffsetBits);
				}
				else
				{
					adjustedBlockOffset = fileBlockEntry.GetBlockOffset();
				}			
				ArchiveReader.BaseStream.Position = adjustedBlockOffset;

				// Calculate the decryption key if neccesary
				uint fileKey = 0;
				if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsEncrypted))
				{						
					if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_HasAdjustedEncryptionKey))
					{
						fileKey = MPQCrypt.GetFileKey(Path.GetFileName(filePath), true, (uint)adjustedBlockOffset, fileBlockEntry.GetFileSize());
					}
					else
					{
						fileKey = MPQCrypt.GetFileKey(Path.GetFileName(filePath));
					}
				}


				// Examine the file storage types and extract as neccesary
				if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsSingleUnit))
				{
					// This file does not use sectoring, but may still be encrypted or compressed.
					byte[] fileData = ArchiveReader.ReadBytes((int)fileBlockEntry.GetBlockSize());

					if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsEncrypted))
					{
						// Decrypt the block
						fileData = MPQCrypt.DecryptData(fileData, fileKey);
					}

					// Decompress the sector if neccesary
					if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsCompressed) || fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsImploded))
					{
						fileData = Compression.DecompressSector(fileData, fileBlockEntry.Flags);						
					}

					return fileData;
				}
				else if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsCompressed) || fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsImploded))
				{						
					// This file uses sectoring, and is compressed. It may be encrypted.		
					//Retrieve the offsets for each sector - these are relative to the beginning of the data.
					List<uint> sectorOffsets = new List<uint>();
					if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsEncrypted))
					{
						MPQCrypt.DecryptSectorOffsetTable(ArchiveReader, ref sectorOffsets, fileBlockEntry.GetBlockSize(), fileKey - 1);
					}
					else
					{
						uint dataBlock = 0;
						while (dataBlock != fileBlockEntry.GetBlockSize())
						{
							dataBlock = ArchiveReader.ReadUInt32();
							sectorOffsets.Add(dataBlock);
						}
					}					

					// Read all of the raw file sectors.
					List<byte[]> compressedSectors = new List<byte[]>();
					for (int i = 0; i < sectorOffsets.Count - 1; ++i)
					{
						long sectorStartPosition = adjustedBlockOffset + sectorOffsets[i];
						ArchiveReader.BaseStream.Position = sectorStartPosition;

						uint sectorLength = sectorOffsets[i + 1] - sectorOffsets[i];
						compressedSectors.Add(ArchiveReader.ReadBytes((int)sectorLength));
					}

					// Begin decompressing and decrypting the sectors
					// TODO: If Checksums are present (check the flags), treat the last sector as a checksum sector
					// TODO: Check "backup.MPQ/realmlist.wtf" for a weird file with checksums that is not working correctly
					List<byte[]> decompressedSectors = new List<byte[]>();
					uint sectorIndex = 0;
					foreach (byte[] compressedSector in compressedSectors)
					{						
						byte[] pendingSector = compressedSector;
						if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsEncrypted))
						{
							// Decrypt the block
							pendingSector = MPQCrypt.DecryptData(compressedSector, fileKey + sectorIndex);
						}

						// Decompress the sector if neccesary
						if (pendingSector.Length < GetMaxSectorSize())
						{
							int currentFileSize = CountBytesInSectors(decompressedSectors);
							bool canSectorCompleteFile = currentFileSize + pendingSector.Length == fileBlockEntry.GetFileSize();

							if (!canSectorCompleteFile && currentFileSize != fileBlockEntry.GetFileSize())
							{
								pendingSector = Compression.DecompressSector(pendingSector, fileBlockEntry.Flags);

							}
						}

						decompressedSectors.Add(pendingSector);
						++sectorIndex;
					}

					return StitchSectors(decompressedSectors);
				}
				else
				{
					// This file uses sectoring, but is not compressed. It may be encrypted.
					uint finalSectorSize = fileBlockEntry.GetFileSize() % GetMaxSectorSize();

					// All the even sectors you can fit into the file size
					uint sectorCount = ((fileBlockEntry.GetFileSize() - finalSectorSize) / GetMaxSectorSize());

					List<byte[]> rawSectors = new List<byte[]>();
					for (int i = 0; i < sectorCount; ++i)
					{
						// Read a normal sector (usually 4096 bytes)
						rawSectors.Add(ArchiveReader.ReadBytes((int)GetMaxSectorSize()));
					}

					// And finally, if there's an uneven sector at the end, read that one too
					if (finalSectorSize > 0)
					{
						rawSectors.Add(ArchiveReader.ReadBytes((int)finalSectorSize));
					}

					uint sectorIndex = 0;
					List<byte[]> finalSectors = new List<byte[]>();
					foreach (byte[] rawSector in rawSectors)
					{						
						byte[] pendingSector = rawSector;
						if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsEncrypted))
						{
							// Decrypt the block
							pendingSector = MPQCrypt.DecryptData(rawSector, fileKey + sectorIndex);
						}

						finalSectors.Add(pendingSector);
						++sectorIndex;
					}

					return StitchSectors(finalSectors);
				}
			}

			return null;
		}

		/// <summary>
		/// Counts the bytes contained in a list of sectors.
		/// </summary>
		/// <returns>The number of bytes.</returns>
		/// <param name="sectors">The sectors.</param>
		private int CountBytesInSectors(List<byte[]> sectors)
		{
			int bytes = 0;

			foreach (byte[] sector in sectors)
			{
				bytes += sector.Length;
			}

			return bytes;
		}

		/// <summary>
		/// Stitches together a set of file sectors into a final byte list, which can then be used for other things.
		/// </summary>
		/// <returns>A byte array representing the final file.</returns>
		/// <param name="sectors">Input file sectors.</param>
		private byte[] StitchSectors(List<byte[]> sectors)
		{
			// Pull out your sowing kit, it's stitching time!
			List<byte> stitchedSectors = new List<byte>();
			foreach (byte[] finalSector in sectors)
			{
				foreach (byte sectorByte in finalSector)
				{
					stitchedSectors.Add(sectorByte);
				}
			}

			return stitchedSectors.ToArray();
		}

		/// <summary>
		/// Determines whether or not the archive requires the format to be extended.
		/// Extended formats are at least <see cref="Warcraft.MPQ.MPQFormat.Extended_v1"/> and up.
		/// </summary>
		/// <returns><c>true</c>, if extended format is required, <c>false</c> otherwise.</returns>
		private bool RequiresExtendedFormat()
		{
			return this.ArchiveReader.BaseStream.Length > UInt32.MaxValue;
		}

		/// <summary>
		/// Gets the maximum size of a file sector.
		/// </summary>
		/// <returns>The max sector size.</returns>
		private uint GetMaxSectorSize()
		{
			return (uint)(512 * Math.Pow(2, this.Header.GetSectorSizeExponent()));
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Warcraft.MPQ.MPQ"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Warcraft.MPQ.MPQ"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="Warcraft.MPQ.MPQ"/> in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="Warcraft.MPQ.MPQ"/> so the garbage collector can reclaim the memory that
		/// the <see cref="Warcraft.MPQ.MPQ"/> was occupying.</remarks>
		public void Dispose()
		{
			Header = null;
			ArchiveHashTable = null;
			ArchiveBlockTable = null;
			ExtendedBlockTable = null;

			if (ArchiveReader != null)
			{
				ArchiveReader.Close();
				ArchiveReader.Dispose();
			}		
		}
	}
}

