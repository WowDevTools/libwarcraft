//
//  MPQ.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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
using System.Collections.Generic;
using System.IO;
using System.Linq;

using JetBrains.Annotations;

using Warcraft.Core;
using Warcraft.Core.Compression;
using Warcraft.MPQ.Attributes;
using Warcraft.MPQ.Crypto;
using Warcraft.MPQ.FileInfo;
using Warcraft.MPQ.Tables.Block;
using Warcraft.MPQ.Tables.Hash;

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
    /// A massive thanks to Justin Olbrantz (Quantam) and Jean-Francois Roy
    /// (BahamutZERO), whose [documentation of the MPQ
    /// format](http://wiki.devklog.net/index.php?title=The_MoPaQ_Archive_Format) was
    /// instrumental for this implementation. Although their wiki is no longer online,
    /// a backup snapshot taken by the Wayback Machine saved the day. The documentation
    /// is available upon request.
    /// </summary>
    [PublicAPI]
    public sealed class MPQ : IDisposable, IPackage
    {
        /// <summary>
        /// Whether or not this instance has been disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Gets or sets the header of the MPQ archive. Contains information about sizes and offsets of relational structures
        /// such as the hash and block table in the archive.
        /// </summary>
        [PublicAPI, NotNull]
        public MPQHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the hash table. Contains hash entries for all files stored in the archive, and for any overrides.
        /// </summary>
        [PublicAPI, NotNull]
        public HashTable ArchiveHashTable { get; set; }

        /// <summary>
        /// Gets or sets the block table. Contains block entries for all files stored in the archive. These entries contain
        /// information about the state of the file.
        /// </summary>
        [PublicAPI, NotNull]
        public BlockTable ArchiveBlockTable { get; set; }

        /// <summary>
        /// Gets the extended block table. Contains a list of upper bits of a ulong number, which is in later versions
        /// of the archive format merged with the offset listed in the block table. This format extension allows
        /// archives to grow larger than 4GB in size.
        /// </summary>
        [PublicAPI, CanBeNull]
        public List<ushort> ExtendedBlockTable { get; }

        /// <summary>
        /// The archive reader. A BinaryReader that exists for the lifetime of the MPQ object and handles all
        /// the file reading inside it. As archives are far too big to be loaded into memory all at once,
        /// we seek to the desired parts and read them as we need them.
        /// </summary>
        [NotNull]
        private readonly BinaryReader _archiveReader;

        /// <summary>
        /// A set of extended file attributes. These attributes are not guaranteed to be present in all archives,
        /// and may be empty or zeroed for some archives.
        /// </summary>
        [CanBeNull]
        private ExtendedAttributes _fileAttributes;

        /// <summary>
        /// The external listfile. Instead of extracting the listfile from the archive, the user can provide one
        /// to be used instead. This file is prioritized over the one stored in the archive.
        /// </summary>
        [CanBeNull, ItemNotNull]
        private List<string> _externalListfile;

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.MPQ.MPQ"/> class.
        /// </summary>
        /// <param name="mpqStream">An open stream to data containing an MPQ archive.</param>
        public MPQ([NotNull] Stream mpqStream)
        {
            _archiveReader = new BinaryReader(mpqStream);

            Header = new MPQHeader(_archiveReader.ReadBytes((int)PeekHeaderSize()));

            // Seek to the hash table and load it
            _archiveReader.BaseStream.Position = (long)Header.GetHashTableOffset();

            var encryptedHashTable = _archiveReader.ReadBytes((int)Header.GetHashTableSize());
            var hashTableData = MPQCrypt.DecryptData(encryptedHashTable, HashTable.TableKey);

            ArchiveHashTable = new HashTable(hashTableData);

            // Seek to the block table and load it
            _archiveReader.BaseStream.Position = (long)Header.GetBlockTableOffset();

            var encryptedBlockTable = _archiveReader.ReadBytes((int)Header.GetBlockTableSize());
            var blockTableData = MPQCrypt.DecryptData(encryptedBlockTable, BlockTable.TableKey);

            ArchiveBlockTable = new BlockTable(blockTableData);

            if (Header.GetFormat() != MPQFormat.ExtendedV1)
            {
                return;
            }

            // Seek to the extended block table and load it, if necessary
            if (Header.GetExtendedBlockTableOffset() <= 0)
            {
                return;
            }

            _archiveReader.BaseStream.Position = (long)Header.GetExtendedBlockTableOffset();
            var extendedTable = new List<ushort>();

            for (var i = 0; i < Header.GetBlockTableEntryCount(); ++i)
            {
                extendedTable.Add(_archiveReader.ReadUInt16());
            }

            ExtendedBlockTable = extendedTable;
        }

        /// <summary>
        /// Peeks at the size of the entire MPQ header without advancing the byte position of the
        /// binary reader.
        /// </summary>
        /// <returns>The header size.</returns>
        private uint PeekHeaderSize()
        {
            var originalPosition = _archiveReader.BaseStream.Position;

            _archiveReader.BaseStream.Position = 4;
            var headerSize = _archiveReader.ReadUInt32();
            _archiveReader.BaseStream.Position = originalPosition;

            return headerSize;
        }

        /// <summary>
        /// Determines whether this archive has file attributes.
        /// </summary>
        /// <returns><c>true</c> if this archive has file attributes; otherwise, <c>false</c>.</returns>
        [PublicAPI]
        public bool HasFileAttributes()
        {
            ThrowIfDisposed();

            return ContainsFile(ExtendedAttributes.InternalFileName);
        }

        /// <summary>
        /// Gets the extended file attributes stored in the archive, if there are any.
        /// </summary>
        /// <returns>The attributes.</returns>
        [PublicAPI, CanBeNull]
        public ExtendedAttributes GetFileAttributes()
        {
            ThrowIfDisposed();

            if (!ContainsFile(ExtendedAttributes.InternalFileName))
            {
                throw new FileNotFoundException(
                    "The archive does not contain any extended attributes.",
                    ExtendedAttributes.InternalFileName
                );
            }

            if (_fileAttributes != null)
            {
                return _fileAttributes;
            }

            var attributeData = ExtractFile(ExtendedAttributes.InternalFileName);
            _fileAttributes = new ExtendedAttributes(attributeData, Header.BlockTableEntryCount);

            return _fileAttributes;
        }

        /// <summary>
        /// Gets the weak signature stored in the archive.
        /// </summary>
        /// <returns>The weak signature.</returns>
        [PublicAPI, NotNull]
        public WeakPackageSignature GetWeakSignature()
        {
            ThrowIfDisposed();

            return new WeakPackageSignature(ExtractFile(WeakPackageSignature.InternalFilename));
        }

        /// <inheritdoc />
        public bool HasFileList()
        {
            ThrowIfDisposed();

            return ContainsFile("(listfile)") || _externalListfile?.Count > 0;
        }

        /// <inheritdoc />
        public IEnumerable<string> GetFileList()
        {
            ThrowIfDisposed();

            if (_externalListfile is null)
            {
                return GetInternalFileList();
            }

            return GetExternalFileList();
        }

        /// <summary>
        /// Gets the internal file list. If no listfile is stored in the archive, this may not return anything.
        /// </summary>
        /// <returns>The internal file list.</returns>
        [PublicAPI, NotNull, ItemNotNull]
        public IEnumerable<string> GetInternalFileList()
        {
            ThrowIfDisposed();

            if (!ContainsFile("(listfile)"))
            {
                yield break;
            }

            var listfileBytes = ExtractFile("(listfile)");

            using (var listfileStream = new MemoryStream(listfileBytes))
            {
                using (TextReader tr = new StreamReader(listfileStream))
                {
                    string line;
                    while ((line = tr.ReadLine()) != null)
                    {
                        yield return line;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the external file list.
        /// </summary>
        /// <returns>The external file list.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the archive doesn't have an external file list.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the archive has been disposed.</exception>
        [PublicAPI, NotNull, ItemNotNull]
        public IEnumerable<string> GetExternalFileList()
        {
            ThrowIfDisposed();

            if (_externalListfile is null)
            {
                throw new InvalidOperationException("The archive doesn't have an external file list.");
            }

            return _externalListfile;
        }

        /// <summary>
        /// Sets the file list to the provided listfile.
        /// </summary>
        /// <param name="inExternalListfile">In external listfile.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the archive has been disposed.</exception>
        [PublicAPI]
        public void SetExternalFileList([NotNull, ItemNotNull] List<string> inExternalListfile)
        {
            ThrowIfDisposed();

            _externalListfile = inExternalListfile;
        }

        /// <summary>
        /// Resets the external file list, clearing it of any entries. The internal list file (if there is one)
        /// will be used instead.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if the archive has been disposed.</exception>
        [PublicAPI]
        public void RemoveExternalFileList()
        {
            ThrowIfDisposed();

            _externalListfile = null;
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">Thrown if the archive has been disposed.</exception>
        public bool ContainsFile(string filePath)
        {
            ThrowIfDisposed();

            try
            {
                ArchiveHashTable.FindEntry(filePath.ToUpperInvariant());
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">Thrown if the archive has been disposed.</exception>
        [PublicAPI]
        public MPQFileInfo GetFileInfo(string filePath)
        {
            ThrowIfDisposed();

            if (!ContainsFile(filePath))
            {
                throw new FileNotFoundException("The given file was not present in the archive.", filePath);
            }

            var hashEntry = ArchiveHashTable.FindEntry(filePath);
            var blockEntry = ArchiveBlockTable.GetEntry((int)hashEntry.GetBlockEntryIndex());

            if (HasFileAttributes())
            {
                return new MPQFileInfo(filePath, hashEntry, blockEntry);
            }

            return new MPQFileInfo(filePath, hashEntry, blockEntry, _fileAttributes.FileAttributes[(int)hashEntry.GetBlockEntryIndex()]);
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">Thrown if the archive has been disposed.</exception>
        /// <exception cref="FileDeletedException">Thrown if the file is deleted in the archive.</exception>
        [PublicAPI]
        public byte[] ExtractFile(string filePath)
        {
            ThrowIfDisposed();

            // Reset all positions to be safe
            _archiveReader.BaseStream.Position = 0;

            HashTableEntry fileHashEntry;
            try
            {
                fileHashEntry = ArchiveHashTable.FindEntry(filePath);
            }
            catch (FileNotFoundException fex)
            {
                throw new FileNotFoundException("No file found at the given path.", filePath, fex);
            }

            var fileBlockEntry = ArchiveBlockTable.GetEntry((int)fileHashEntry.GetBlockEntryIndex());

            // Drop out if the file has been deleted
            if (fileBlockEntry.IsDeleted())
            {
                throw new FileDeletedException("The given file is deleted.", filePath);
            }

            // Seek to the beginning of the file's sectors
            long adjustedBlockOffset;
            if (Header.GetFormat() == MPQFormat.ExtendedV1 && RequiresExtendedFormat())
            {
                var upperOffsetBits = ExtendedBlockTable[(int)fileHashEntry.GetBlockEntryIndex()];
                adjustedBlockOffset = (long)fileBlockEntry.GetExtendedBlockOffset(upperOffsetBits);
            }
            else
            {
                adjustedBlockOffset = fileBlockEntry.GetBlockOffset();
            }

            _archiveReader.BaseStream.Position = adjustedBlockOffset;

            // Calculate the decryption key if necessary
            var fileKey = MPQCrypt.CreateFileEncryptionKey
            (
                filePath,
                fileBlockEntry.ShouldEncryptionKeyBeAdjusted(),
                adjustedBlockOffset,
                fileBlockEntry.GetFileSize()
            );

            // Examine the file storage types and extract as necessary
            if (fileBlockEntry.IsSingleUnit())
            {
                return ExtractSingleUnitFile(fileBlockEntry, fileKey);
            }

            if (fileBlockEntry.IsCompressed())
            {
                return ExtractCompressedSectoredFile(fileBlockEntry, fileKey, adjustedBlockOffset);
            }

            return ExtractUncompressedSectoredFile(fileBlockEntry, fileKey);
        }

        /// <summary>
        /// Extracts a file which is divided into a set of compressed sectors.
        /// </summary>
        /// <param name="fileBlockEntry">The block entry of the file.</param>
        /// <param name="fileKey">The encryption key of the file.</param>
        /// <param name="adjustedBlockOffset">The offset to where the file sectors begin.</param>
        /// <returns>The complete file data.</returns>
        /// <exception cref="InvalidFileSectorTableException">Thrown if the sector table is found to be inconsistent in any way.</exception>
        [NotNull]
        private byte[] ExtractCompressedSectoredFile
        (
            [NotNull] BlockTableEntry fileBlockEntry,
            uint fileKey,
            long adjustedBlockOffset
        )
        {
            // This file is sectored, and is compressed. It may be encrypted.
            // Retrieve the offsets for each sector - these are relative to the beginning of the data.
            var sectorOffsets = ReadFileSectorOffsetTable(fileBlockEntry, fileKey);

            // Read all of the raw file sectors.
            var compressedSectors = new List<byte[]>();
            for (var i = 0; i < sectorOffsets.Count - 1; ++i)
            {
                var sectorStartPosition = adjustedBlockOffset + sectorOffsets[i];
                _archiveReader.BaseStream.Position = sectorStartPosition;

                var sectorLength = sectorOffsets[i + 1] - sectorOffsets[i];
                compressedSectors.Add(_archiveReader.ReadBytes((int)sectorLength));
            }

            // Begin decompressing and decrypting the sectors
            // TODO: If Checksums are present (check the flags), treat the last sector as a checksum sector
            // TODO: Check "backup.MPQ/realmlist.wtf" for a weird file with checksums that is not working correctly.
            // It has a single sector with a single checksum after it, and none of the hashing functions seem to
            // produce a valid hash. CRC32, Adler32, CRC32B, nothing.
            // Some flags (listfiles mostly) are flagged as having checksums but don't have a checksum sector.
            // Perhaps related to attributes file?
            var decompressedSectors = new List<byte[]>();

            /*
            List<uint> SectorChecksums = new List<uint>();
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
            }
             */

            uint sectorIndex = 0;
            foreach (var compressedSector in compressedSectors)
            {
                var pendingSector = compressedSector;
                if (fileBlockEntry.IsEncrypted())
                {
                    // Decrypt the block
                    pendingSector = MPQCrypt.DecryptData(compressedSector, fileKey + sectorIndex);
                }

                /*
                if (fileBlockEntry.Flags.HasFlag(BlockFlags.HasCRCChecksums))
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
                }
                 */

                // Decompress the sector if necessary
                if (pendingSector.Length < GetMaxSectorSize())
                {
                    var currentFileSize = CountBytesInSectors(decompressedSectors);
                    var canSectorCompleteFile = currentFileSize + pendingSector.Length == fileBlockEntry.GetFileSize();

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

        /// <summary>
        /// Reads the sector offset table of a file.
        /// </summary>
        /// <param name="fileBlockEntry">The block table entry of the file.</param>
        /// <param name="fileKey">The encryption key of the file. Optional, in the case that the file is not encrypted.</param>
        /// <returns>A list of sector offsets.</returns>
        /// <exception cref="InvalidFileSectorTableException">Thrown if the sector table is found to be inconsistent in any way.</exception>
        [NotNull]
        private List<uint> ReadFileSectorOffsetTable
        (
            [NotNull] BlockTableEntry fileBlockEntry,
            uint fileKey = 0
        )
        {
            var sectorOffsets = new List<uint>();
            if (fileBlockEntry.IsEncrypted())
            {
                MPQCrypt.DecryptSectorOffsetTable(_archiveReader, ref sectorOffsets, fileBlockEntry.GetBlockSize(), fileKey - 1);
            }
            else
            {
                // As protection against corrupt or maliciously zeroed blocks or corrupt blocks,
                // reading will be escaped early if the sector offset table is not consistent.
                // Should the total size as predicted by the sector offset table go beyond the total
                // block size, or if an offset is not unique, no file will be read and the function will
                // escape early.
                uint sectorOffset = 0;
                while (sectorOffset != fileBlockEntry.GetBlockSize())
                {
                    sectorOffset = _archiveReader.ReadUInt32();

                    // Should the resulting sector offset be less than the previous data, then the data is inconsistent
                    // and no table should be returned.
                    if (sectorOffsets.LastOrDefault() > sectorOffset)
                    {
                        throw new InvalidFileSectorTableException(
                            "The read offset in the sector table was less than the previous offset.");
                    }

                    // Should the resulting sector offset be greater than the total block size, then the data is
                    // inconsistent and no file should be returned.
                    if (sectorOffset > fileBlockEntry.GetBlockSize())
                    {
                        throw new InvalidFileSectorTableException(
                            "The read offset in the sector table was greater than the total size of the data block.");
                    }

                    // Should the resulting sector not be unique, something is wrong and no table should be returned.
                    if (sectorOffsets.Contains(sectorOffset))
                    {
                        throw new InvalidFileSectorTableException(
                            "The read offset in the sector table was not unique to the whole table.");
                    }

                    // The offset should be valid, so add it to the table.
                    sectorOffsets.Add(sectorOffset);
                }
            }

            return sectorOffsets;
        }

        /// <summary>
        /// Extracts a file which is divided into a set of sectors.
        /// </summary>
        /// <param name="fileBlockEntry">The block entry of the file.</param>
        /// <param name="fileKey">The encryption key of the file.</param>
        /// <returns>The complete file data.</returns>
        [NotNull]
        private byte[] ExtractUncompressedSectoredFile
        (
            [NotNull] BlockTableEntry fileBlockEntry,
            uint fileKey
        )
        {
            // This file uses sectoring, but is not compressed. It may be encrypted.
            var finalSectorSize = fileBlockEntry.GetFileSize() % GetMaxSectorSize();

            // All the even sectors you can fit into the file size
            var sectorCount = (fileBlockEntry.GetFileSize() - finalSectorSize) / GetMaxSectorSize();

            var rawSectors = new List<byte[]>();
            for (var i = 0; i < sectorCount; ++i)
            {
                // Read a normal sector (usually 4096 bytes)
                rawSectors.Add(_archiveReader.ReadBytes((int)GetMaxSectorSize()));
            }

            // And finally, if there's an uneven sector at the end, read that one too
            if (finalSectorSize > 0)
            {
                rawSectors.Add(_archiveReader.ReadBytes((int)finalSectorSize));
            }

            uint sectorIndex = 0;
            var finalSectors = new List<byte[]>();
            foreach (var rawSector in rawSectors)
            {
                var pendingSector = rawSector;
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

        /// <summary>
        /// Extracts a file which is stored as a single unit in the archive.
        /// </summary>
        /// <param name="fileBlockEntry">The block entry of the file.</param>
        /// <param name="fileKey">The encryption key of the file.</param>
        /// <returns>The complete file data.</returns>
        [NotNull]
        private byte[] ExtractSingleUnitFile
        (
            [NotNull] BlockTableEntry fileBlockEntry,
            uint fileKey
        )
        {
            // This file does not use sectoring, but may still be encrypted or compressed.
            var fileData = _archiveReader.ReadBytes((int)fileBlockEntry.GetBlockSize());

            if (fileBlockEntry.IsEncrypted())
            {
                // Decrypt the block
                fileData = MPQCrypt.DecryptData(fileData, fileKey);
            }

            // Decompress the sector if necessary
            if (fileBlockEntry.IsCompressed())
            {
                fileData = Compression.DecompressSector(fileData, fileBlockEntry.Flags);
            }

            return fileData;
        }

        /// <summary>
        /// Counts the bytes contained in a list of sectors.
        /// </summary>
        /// <returns>The number of bytes.</returns>
        /// <param name="sectors">The sectors.</param>
        private static int CountBytesInSectors([NotNull, ItemNotNull] IEnumerable<byte[]> sectors)
        {
            return sectors.Sum(sector => sector.Length);
        }

        /// <summary>
        /// Stitches together a set of file sectors into a final byte list, which can then be used for other things.
        /// </summary>
        /// <returns>A byte array representing the final file.</returns>
        /// <param name="sectors">Input file sectors.</param>
        [NotNull]
        private static byte[] StitchSectors([NotNull, ItemNotNull] IReadOnlyCollection<byte[]> sectors)
        {
            long finalSize = 0;
            foreach (var sector in sectors)
            {
                finalSize += sector.Length;
            }

            var finalBlock = new byte[finalSize];

            // Pull out your sowing kit, it's stitching time!
            var writtenBytes = 0;
            foreach (var sector in sectors)
            {
                Buffer.BlockCopy(sector, 0, finalBlock, writtenBytes, sector.Length);
                writtenBytes += sector.Length;
            }

            return finalBlock;
        }

        /// <summary>
        /// Determines whether or not the archive requires the format to be extended.
        /// ExtendedIO formats are at least <see cref="MPQFormat.ExtendedV1"/> and up.
        /// </summary>
        /// <returns><c>true</c>, if extended format is required, <c>false</c> otherwise.</returns>
        private bool RequiresExtendedFormat()
        {
            return _archiveReader.BaseStream.Length > uint.MaxValue;
        }

        /// <summary>
        /// Gets the maximum size of a file sector.
        /// </summary>
        /// <returns>The max sector size.</returns>
        private uint GetMaxSectorSize()
        {
            return (uint)(512 * Math.Pow(2, Header.GetSectorSizeExponent()));
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(ToString(), "Cannot use a disposed archive.");
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _archiveReader.Dispose();
            _isDisposed = true;
        }
    }
}
