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
using Warcraft.MPQ.Attributes;

namespace Warcraft.MPQ
{
	/// <summary>
	/// The MPQ class is the superclass for all known versions of the MPQ file format, which is used to store
	/// game data for the majority of produced Blizzard Entertainment games. It acts as a loader and extraction
	/// class for reading this data, and examining the data structures inside it.
	///
	/// It should be noted that this is not a speed-oriented or lightweight implementation of the format. This
	/// implementation is designed for verbosity, clarity and ease of use at the expense of memory usage. As
	/// a side effect, file lookups may be faster than other implementations, depending on your setup.
	///
	/// Currently, the class does not support creating new archives, nor any format versions above
	/// <see cref="MPQFormat.ExtendedV1"/>. Work is ongoing to implement this.
	/// </summary>
	public sealed class MPQ : IDisposable, IPackage
	{
		/// <summary>
		/// Whether or not this instance has been disposed.
		/// </summary>
		private bool bDisposed;

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
		public readonly List<ushort> ExtendedBlockTable = new List<ushort>();

		/// <summary>
		/// A set of extended file attributes. These attributes are not guaranteed to be present in all archives,
		/// and may be empty or zeroed for some archives.
		/// </summary>
		private ExtendedAttributes FileAttributes;

		/// <summary>
		/// The archive reader. A BinaryReader that exists for the lifetime of the MPQ object and handles all
		/// the file reading inside it. As archives are far too big to be loaded into memory all at once,
		/// we seek to the desired parts and read them as we need them.
		/// </summary>
		private readonly BinaryReader ArchiveReader;

		/// <summary>
		/// The external listfile. Instead of extracting the listfile from the archive, the user can provide one
		/// to be used instead. This file is prioritized over the one stored in the archive.
		/// </summary>
		private List<string> ExternalListfile = new List<string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.MPQ.MPQ"/> class.
		/// This constructor creates an empty archive.
		/// </summary>
		/// <param name="inFormat">In format.</param>
		public MPQ(MPQFormat inFormat)
		{
			if (inFormat > MPQFormat.ExtendedV1)
			{
				throw new NotImplementedException();
			}

			this.Header = new MPQHeader(inFormat);
			this.ArchiveHashTable = new HashTable();
			this.ArchiveBlockTable = new BlockTable();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.MPQ.MPQ"/> class.
		/// </summary>
		/// <param name="mpqStream">An open stream to data containing an MPQ archive.</param>
		public MPQ(Stream mpqStream)
		{
			this.ArchiveReader = new BinaryReader(mpqStream);

			this.Header = new MPQHeader(this.ArchiveReader.ReadBytes((int)PeekHeaderSize()));

			if (this.Header.GetFormat() > MPQFormat.ExtendedV1)
			{
				//throw new NotImplementedException();
			}

			// Seek to the hash table and load it
			this.ArchiveReader.BaseStream.Position = (long)this.Header.GetHashTableOffset();

			byte[] hashTableData;
			if (this.Header.IsHashTableCompressed())
			{
				byte[] encryptedData = this.ArchiveReader.ReadBytes((int)this.Header.GetCompressedHashTableSize());
				byte[] decryptedData = MPQCrypt.DecryptData(encryptedData, HashTable.TableKey);

				BlockFlags tableFlags = BlockFlags.IsCompressedMultiple;
				hashTableData = Compression.DecompressSector(decryptedData, tableFlags);
			}
			else
			{
				byte[] encryptedData = this.ArchiveReader.ReadBytes((int)this.Header.GetHashTableSize());
				hashTableData = MPQCrypt.DecryptData(encryptedData, HashTable.TableKey);
			}

			this.ArchiveHashTable = new HashTable(hashTableData);

			// Seek to the block table and load it
			this.ArchiveReader.BaseStream.Position = (long)this.Header.GetBlockTableOffset();

			byte[] blockTableData;
			if (this.Header.IsBlockTableCompressed())
			{
				byte[] encryptedData = this.ArchiveReader.ReadBytes((int)this.Header.GetCompressedBlockTableSize());
				byte[] decryptedData = MPQCrypt.DecryptData(encryptedData, BlockTable.TableKey);

				BlockFlags tableFlags = BlockFlags.IsCompressedMultiple;
				blockTableData = Compression.DecompressSector(decryptedData, tableFlags);
			}
			else
			{
				byte[] encryptedData = this.ArchiveReader.ReadBytes((int)this.Header.GetBlockTableSize());
				blockTableData = MPQCrypt.DecryptData(encryptedData, BlockTable.TableKey);
			}

			this.ArchiveBlockTable = new BlockTable(blockTableData);

			if (this.Header.GetFormat() >= MPQFormat.ExtendedV1)
			{
				// Seek to the extended block table and load it, if neccesary
				if (this.Header.GetExtendedBlockTableOffset() > 0)
				{
					this.ArchiveReader.BaseStream.Position = (long)this.Header.GetExtendedBlockTableOffset();

					for (int i = 0; i < this.Header.GetBlockTableEntryCount(); ++i)
					{
						this.ExtendedBlockTable.Add(this.ArchiveReader.ReadUInt16());
					}
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
			long originalPosition = this.ArchiveReader.BaseStream.Position;

			this.ArchiveReader.BaseStream.Position = 4;
			uint headerSize = this.ArchiveReader.ReadUInt32();
			this.ArchiveReader.BaseStream.Position = originalPosition;

			return headerSize;
		}

		/// <summary>
		/// Determines whether this archive has file attributes.
		/// </summary>
		/// <returns><c>true</c> if this archive has file attributes; otherwise, <c>false</c>.</returns>
		public bool HasFileAttributes()
		{
			if (this.bDisposed)
			{
				throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
			}

			return ContainsFile(ExtendedAttributes.InternalFileName);
		}

		/// <summary>
		/// Gets the extended file attributes stored in the archive, if there are any.
		/// </summary>
		public ExtendedAttributes GetFileAttributes()
		{
			if (this.FileAttributes == null)
			{
				if (ContainsFile(ExtendedAttributes.InternalFileName))
				{
					byte[] attributeData = ExtractFile(ExtendedAttributes.InternalFileName);
					this.FileAttributes = new ExtendedAttributes(attributeData, this.Header.BlockTableEntryCount);
				}
			}

			return this.FileAttributes;
		}

		/// <summary>
		/// Gets the weak signature stored in the archive.
		/// </summary>
		/// <returns>The weak signature.</returns>
		public WeakPackageSignature GetWeakSignature()
		{
			if (ContainsFile(WeakPackageSignature.InternalFilename))
			{
				return new WeakPackageSignature(ExtractFile(WeakPackageSignature.InternalFilename));
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Determines whether this archive has a listfile.
		/// </summary>
		/// <returns><c>true</c> if this archive has a listfile; otherwise, <c>false</c>.</returns>
		public bool HasFileList()
		{
			if (this.bDisposed)
			{
				throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
			}

			return ContainsFile("(listfile)") || this.ExternalListfile.Count > 0;
		}

		/// <summary>
		/// Gets the best available listfile from the archive. If an external listfile has been provided,
		/// that one is prioritized over the one stored in the archive.
		/// </summary>
		/// <returns>The listfile.</returns>
		public List<string> GetFileList()
		{
			if (this.bDisposed)
			{
				throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
			}

			if (this.ExternalListfile.Count > 0)
			{
				return GetExternalFileList();
			}
			else
			{
				return GetInternalFileList();
			}
		}

		/// <summary>
		/// Gets the internal file list. If no listfile is stored in the archive, this may
		/// return null.
		/// </summary>
		/// <returns>The internal file list.</returns>
		public List<string> GetInternalFileList()
		{
			if (this.bDisposed)
			{
				throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
			}

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
		/// Gets the external file list. If no list has been provided to the archive, this may
		/// return an empty list.
		/// </summary>
		/// <returns>The external file list.</returns>
		public List<string> GetExternalFileList()
		{
			if (this.bDisposed)
			{
				throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
			}

			return this.ExternalListfile;
		}

		/// <summary>
		/// Sets the file list to the provided listfile.
		/// </summary>
		/// <param name="inExternalListfile">In external listfile.</param>
		public void SetFileList(List<string> inExternalListfile)
		{
			if (this.bDisposed)
			{
				throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
			}

			this.ExternalListfile = inExternalListfile;
		}

		/// <summary>
		/// Resets the external file list, clearing it of any entries. The internal list file (if there is one)
		/// will be used instead.
		/// </summary>
		public void ResetExternalFileList()
		{
			if (this.bDisposed)
			{
				throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
			}

			this.ExternalListfile.Clear();
		}

		/// <summary>
		/// Checks if the specified file path exists in the archive.
		/// </summary>
		/// <returns><c>true</c>, if the file exists, <c>false</c> otherwise.</returns>
		/// <param name="filePath">File path.</param>
		public bool ContainsFile(string filePath)
		{
			if (this.bDisposed)
			{
				throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
			}

			HashTableEntry fileHashEntry = this.ArchiveHashTable.FindEntry(filePath.ToUpperInvariant());
			return fileHashEntry != null;
		}

		/// <summary>
		/// Gets the file info of the provided path.
		/// </summary>
		/// <returns>The file info, or null if the file doesn't exist in the archive.</returns>
		/// <param name="filePath">File path.</param>
		public MPQFileInfo GetFileInfo(string filePath)
		{
			if (this.bDisposed)
			{
				throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
			}

			if (ContainsFile(filePath))
			{
				HashTableEntry hashEntry = this.ArchiveHashTable.FindEntry(filePath);
				BlockTableEntry blockEntry = this.ArchiveBlockTable.GetEntry((int)hashEntry.GetBlockEntryIndex());

				if (HasFileAttributes())
				{
					return new MPQFileInfo(filePath, hashEntry, blockEntry);
				}
				else
				{
					return new MPQFileInfo(filePath, hashEntry, blockEntry, this.FileAttributes.FileAttributes[(int)hashEntry.GetBlockEntryIndex()]);
				}
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
			if (this.bDisposed)
			{
				throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
			}

			// Reset all positions to be safe
			this.ArchiveReader.BaseStream.Position = 0;

			HashTableEntry fileHashEntry = this.ArchiveHashTable.FindEntry(filePath);
			if (fileHashEntry != null)
			{
				BlockTableEntry fileBlockEntry = this.ArchiveBlockTable.GetEntry((int)fileHashEntry.GetBlockEntryIndex());

				// Drop out if the file is not actually a file
				if (!fileBlockEntry.HasData())
				{
					return null;
				}

				// Seek to the beginning of the file's sectors
				long adjustedBlockOffset;
				if (this.Header.GetFormat() == MPQFormat.ExtendedV1 && RequiresExtendedFormat())
				{
					ushort upperOffsetBits = this.ExtendedBlockTable[(int)fileHashEntry.GetBlockEntryIndex()];
					adjustedBlockOffset = (long)fileBlockEntry.GetExtendedBlockOffset(upperOffsetBits);
				}
				else
				{
					adjustedBlockOffset = fileBlockEntry.GetBlockOffset();
				}
				this.ArchiveReader.BaseStream.Position = adjustedBlockOffset;

				// Calculate the decryption key if neccesary
				uint fileKey = 0;
				if (fileBlockEntry.IsEncrypted())
				{
					if (fileBlockEntry.Flags.HasFlag(BlockFlags.HasAdjustedEncryptionKey))
					{
						fileKey = MPQCrypt.GetFileKey(Path.GetFileName(filePath), true, (uint)adjustedBlockOffset, fileBlockEntry.GetFileSize());
					}
					else
					{
						fileKey = MPQCrypt.GetFileKey(Path.GetFileName(filePath));
					}
				}


				// Examine the file storage types and extract as neccesary
				if (fileBlockEntry.IsSingleUnit())
				{
					// This file does not use sectoring, but may still be encrypted or compressed.
					byte[] fileData = this.ArchiveReader.ReadBytes((int)fileBlockEntry.GetBlockSize());

					if (fileBlockEntry.Flags.HasFlag(BlockFlags.IsEncrypted))
					{
						// Decrypt the block
						fileData = MPQCrypt.DecryptData(fileData, fileKey);
					}

					// Decompress the sector if neccesary
					if (fileBlockEntry.IsCompressed())
					{
						fileData = Compression.DecompressSector(fileData, fileBlockEntry.Flags);
					}

					return fileData;
				}

				if (fileBlockEntry.IsCompressed())
				{
					// This file uses sectoring, and is compressed. It may be encrypted.
					//Retrieve the offsets for each sector - these are relative to the beginning of the data.
					List<uint> sectorOffsets = new List<uint>();
					if (fileBlockEntry.IsEncrypted())
					{
						MPQCrypt.DecryptSectorOffsetTable(this.ArchiveReader, ref sectorOffsets, fileBlockEntry.GetBlockSize(), fileKey - 1);
					}
					else
					{
						uint dataBlock = 0;
						while (dataBlock != fileBlockEntry.GetBlockSize())
						{
							dataBlock = this.ArchiveReader.ReadUInt32();
							sectorOffsets.Add(dataBlock);
						}
					}

					// Read all of the raw file sectors.
					List<byte[]> compressedSectors = new List<byte[]>();
					for (int i = 0; i < sectorOffsets.Count - 1; ++i)
					{
						long sectorStartPosition = adjustedBlockOffset + sectorOffsets[i];
						this.ArchiveReader.BaseStream.Position = sectorStartPosition;

						uint sectorLength = sectorOffsets[i + 1] - sectorOffsets[i];
						compressedSectors.Add(this.ArchiveReader.ReadBytes((int)sectorLength));
					}

					// Begin decompressing and decrypting the sectors
					// TODO: If Checksums are present (check the flags), treat the last sector as a checksum sector
					// TODO: Check "backup.MPQ/realmlist.wtf" for a weird file with checksums that is not working correctly.
					// It has a single sector with a single checksum after it, and none of the hashing functions seem to
					// produce a valid hash. CRC32, Adler32, CRC32B, nothing.
					// Some flags (listfiles mostly) are flagged as having checksums but don't have a checksum sector.
					// Perhaps related to attributes file?
					List<byte[]> decompressedSectors = new List<byte[]>();

					/*	List<uint> SectorChecksums = new List<uint>();
					if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_HasChecksums))
					{
						byte[] compressedChecksums = compressedSectors.Last();
						byte[] decompressedChecksums = Compression.DecompressSector(compressedChecksums, fileBlockEntry.Flags);

						// Lift out the last sector and treat it as a checksum sector
						using (MemoryStream ms = new MemoryStream(decompressedChecksums))
						{
							using (BinaryReader br = new BinaryReader(ms))
							{
								// Drop the checksum sector from the file sectors
								compressedSectors.RemoveAt(compressedSectors.Count - 1);

								for (int i = 0; i < compressedSectors.Count; ++i)
								{
									SectorChecksums.Add(br.ReadUInt32());
								}
							}
						}
					}*/

					uint sectorIndex = 0;
					foreach (byte[] compressedSector in compressedSectors)
					{
						byte[] pendingSector = compressedSector;
						if (fileBlockEntry.IsEncrypted())
						{
							// Decrypt the block
							pendingSector = MPQCrypt.DecryptData(compressedSector, fileKey + sectorIndex);
						}

						/*if (fileBlockEntry.Flags.HasFlag(BlockFlags.BLF_HasChecksums))
						{
							// Verify the sector
							bool isSectorIntact = MPQCrypt.VerifySectorChecksum(pendingSector, SectorChecksums[(int)sectorIndex]);
							if (!isSectorIntact)
							{
								using (MemoryStream ms = new MemoryStream(pendingSector))
								{
									//DEBUG

									uint sectorChecksum = (uint)Adler32.ComputeChecksum(ms);

									string exceptionMessage = String.Format("The decrypted sector failed its integrity checking. \n" +
										                          "The sector had a checksum of \"{0}\", and the expected one was \"{1}\".",
										                          sectorChecksum, SectorChecksums[(int)sectorIndex]);

									throw new InvalidDataException(exceptionMessage);
								}
							}
						}*/

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
						rawSectors.Add(this.ArchiveReader.ReadBytes((int)GetMaxSectorSize()));
					}

					// And finally, if there's an uneven sector at the end, read that one too
					if (finalSectorSize > 0)
					{
						rawSectors.Add(this.ArchiveReader.ReadBytes((int)finalSectorSize));
					}

					uint sectorIndex = 0;
					List<byte[]> finalSectors = new List<byte[]>();
					foreach (byte[] rawSector in rawSectors)
					{
						byte[] pendingSector = rawSector;
						if (fileBlockEntry.IsEncrypted())
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
		private static int CountBytesInSectors(IEnumerable<byte[]> sectors)
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
		private static byte[] StitchSectors(IEnumerable<byte[]> sectors)
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
		/// Extended formats are at least <see cref="MPQFormat.ExtendedV1"/> and up.
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
			this.Header = null;
			this.ArchiveHashTable = null;
			this.ArchiveBlockTable = null;

			if (this.ExtendedBlockTable.Count > 0)
			{
				this.ExtendedBlockTable.Clear();
			}

			if (this.ExternalListfile.Count > 0)
			{
				this.ExternalListfile.Clear();
			}

			if (this.ArchiveReader != null)
			{
				this.ArchiveReader.Close();
				this.ArchiveReader.Dispose();
			}

			this.bDisposed = true;
		}
	}
}

