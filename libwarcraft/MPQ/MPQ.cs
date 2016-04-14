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
using System.IO;
using System.Collections.Generic;
using Warcraft.Core.Compression;
using Warcraft.MPQ.Crypto;

using Warcraft.MPQ.Tables.Hash;
using Warcraft.MPQ.Tables.Block;
using Warcraft.MPQ.FileInfo;

namespace Warcraft.MPQ
{
	public class MPQ : IDisposable
	{
		public MPQHeader Header;
		public MPQHashTable HashTable;
		public MPQBlockTable BlockTable;
		public ushort[] ExtendedBlockTable;

		private readonly BinaryReader mpqReader;

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.MPQ.MPQ"/> class.
		/// </summary>
		/// <param name="mpqStream">An open stream to data containing an MPQ archive.</param>
		public MPQ(Stream mpqStream)
		{
			mpqReader = new BinaryReader(mpqStream);

			this.Header = new MPQHeader(mpqReader.ReadBytes((int)PeekHeaderSize()));

			if (this.Header.GetFormat() >= MPQFormat.Extended_v1)
			{
				// Seek to the extended block table and load it, if neccesary			
				mpqReader.BaseStream.Position = (long)this.Header.GetExtendedBlockTableOffset();
				long extendedBlockTableBytesToRead = sizeof(ushort) * this.Header.GetNumBlockTableEntries();			
				byte[] extendedBlockTableBytes = mpqReader.ReadBytes((int)extendedBlockTableBytesToRead);

				List<ushort> extendedBlockTable = new List<ushort>();
				for (int i = 0; i < extendedBlockTableBytes.Length; i += 2)
				{
					extendedBlockTable.Add(BitConverter.ToUInt16(extendedBlockTableBytes, i));
				}
				this.ExtendedBlockTable = extendedBlockTable.ToArray();
			}

			// Seek to the hash table and load it
			mpqReader.BaseStream.Position = (long)this.Header.GetHashTableOffset();
			long hashTableBytesToRead = HashTableEntry.GetSize() * this.Header.GetNumHashTableEntries();

			byte[] encryptedHashTable = mpqReader.ReadBytes((int)hashTableBytesToRead);			
			this.HashTable = new MPQHashTable(MPQCrypt.DecryptData(encryptedHashTable, MPQHashTable.TableKey));

			// Seek to the block table and load it
			mpqReader.BaseStream.Position = (long)this.Header.GetBlockTableOffset();
			long blockTableBytesToRead = BlockTableEntry.GetSize() * this.Header.GetNumBlockTableEntries();

			byte[] encryptedBlockTable = mpqReader.ReadBytes((int)blockTableBytesToRead);
			this.BlockTable = new MPQBlockTable(MPQCrypt.DecryptData(encryptedBlockTable, MPQBlockTable.TableKey));
		}

		/// <summary>
		/// Peeks at the format of the MPQ archive without advancing the byte position of the 
		/// binary reader.
		/// </summary>
		/// <returns>The format version of the MPQ archive.</returns>
		private MPQFormat PeekFormat()
		{
			long originalPosition = mpqReader.BaseStream.Position;

			mpqReader.BaseStream.Position = 12;
			MPQFormat format = (MPQFormat)mpqReader.ReadUInt16();
			mpqReader.BaseStream.Position = originalPosition;

			return format;
		}

		/// <summary>
		/// Peeks at the size of the entire MPQ header without advancing the byte position of the
		/// binary reader.
		/// </summary>
		/// <returns>The header size.</returns>
		private uint PeekHeaderSize()
		{
			long originalPosition = mpqReader.BaseStream.Position;

			mpqReader.BaseStream.Position = 4;
			uint headerSize = mpqReader.ReadUInt32();
			mpqReader.BaseStream.Position = originalPosition;

			return headerSize;
		}

		/// <summary>
		/// Determines whether this archive has a listfile.
		/// </summary>
		/// <returns><c>true</c> if this instance has a listfile; otherwise, <c>false</c>.</returns>
		public bool HasFileList()
		{
			HashTableEntry fileHashEntry = HashTable.FindEntry("(listfile)");
			return fileHashEntry != null;
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
				MemoryStream listfileStream = new MemoryStream(listfileBytes);
				StreamReader sr = new StreamReader(listfileStream);

				string fileContent = sr.ReadToEnd();

				string[] semiSplit = fileContent.Split(';');
				foreach (string line in semiSplit)
				{
					string[] special1Split = line.Split('\x0D');
					foreach (string special1line in special1Split)
					{
						string[] special2Split = special1line.Split('\x0A');
						foreach (string special2line in special2Split)
						{
							if (!String.IsNullOrWhiteSpace(special2line))
							{
								fileList.Add(special2line);
							}
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

		public bool DoesFileExist(string filePath)
		{
			HashTableEntry fileHashEntry = HashTable.FindEntry(filePath);
			return fileHashEntry != null;
		}

		public MPQFileInfo GetFileInfo(string filePath)
		{
			if (DoesFileExist(filePath))
			{
				HashTableEntry hashEntry = HashTable.FindEntry(filePath);
				BlockTableEntry blockEntry = BlockTable.GetEntry((int)hashEntry.GetBlockEntryIndex());

				return new MPQFileInfo(filePath, hashEntry, blockEntry);
			}
			else
			{
				return null;
			}
		}

		// TODO: Filter files based on language and platform
		public byte[] ExtractFile(string filePath)
		{
			// Reset all positions to be safe
			mpqReader.BaseStream.Position = 0;

			HashTableEntry fileHashEntry = HashTable.FindEntry(filePath);
			if (fileHashEntry != null)
			{
				BlockTableEntry fileBlockEntry = BlockTable.GetEntry((int)fileHashEntry.GetBlockEntryIndex());

				// Drop out if the file is not actually a file
				if (!fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_IsFile))
				{
					return null;
				}

				// Seek to the beginning of the file's sectors
				long adjustedBlockOffset;
				if (this.Header.GetFormat() == MPQFormat.Extended_v1 && RequiresExtendedFormat())
				{
					adjustedBlockOffset = (long)fileBlockEntry.GetExtendedBlockOffset(this.ExtendedBlockTable[fileHashEntry.GetBlockEntryIndex()]);
				}
				else
				{
					adjustedBlockOffset = fileBlockEntry.GetBlockOffset();
				}			
				mpqReader.BaseStream.Position = adjustedBlockOffset;

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
					byte[] fileData = mpqReader.ReadBytes((int)fileBlockEntry.GetBlockSize());

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
						MPQCrypt.DecryptSectorOffsetTable(mpqReader, ref sectorOffsets, fileBlockEntry.GetBlockSize(), fileKey - 1);
					}
					else
					{
						uint dataBlock = 0;
						while (dataBlock != fileBlockEntry.GetBlockSize())
						{
							dataBlock = mpqReader.ReadUInt32();
							sectorOffsets.Add(dataBlock);
						}
					}					

					// Read all of the raw file sectors.
					List<byte[]> compressedSectors = new List<byte[]>();
					for (int i = 0; i < sectorOffsets.Count - 1; ++i)
					{
						long sectorStartPosition = adjustedBlockOffset + sectorOffsets[i];
						mpqReader.BaseStream.Position = sectorStartPosition;

						uint sectorLength = sectorOffsets[i + 1] - sectorOffsets[i];
						compressedSectors.Add(mpqReader.ReadBytes((int)sectorLength));
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
						rawSectors.Add(mpqReader.ReadBytes((int)GetMaxSectorSize()));
					}

					// And finally, if there's an uneven sector at the end, read that one too
					if (finalSectorSize > 0)
					{
						rawSectors.Add(mpqReader.ReadBytes((int)finalSectorSize));
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
		/// Determines whether or not the archive requires the format to be extended (at least <see cref="Warcraft.MPQ.MPQFormat.Extended_v1"/>)
		/// </summary>
		/// <returns><c>true</c>, if extended format is required, <c>false</c> otherwise.</returns>
		private bool RequiresExtendedFormat()
		{
			return this.mpqReader.BaseStream.Length > UInt32.MaxValue;
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
			HashTable = null;
			BlockTable = null;
			ExtendedBlockTable = null;

			if (mpqReader != null)
			{
				mpqReader.Close();
				mpqReader.Dispose();
			}		
		}
	}
}

