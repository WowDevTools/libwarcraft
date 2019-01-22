//
//  BlockTableEntry.cs
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

using System.IO;
using JetBrains.Annotations;
using Warcraft.Core.Interfaces;

namespace Warcraft.MPQ.Tables.Block
{
    /// <summary>
    /// Represents a table entry in the block table, which holds information about how a file is stored in the
    /// archive, its size, and where it is.
    /// </summary>
    [PublicAPI]
    public class BlockTableEntry : IBinarySerializable
    {
        /// <summary>
        /// The offset into the archive file where the file data begins.
        /// </summary>
        private readonly uint _blockOffset;

        /// <summary>
        /// The absolute size in bytes of the stored file data.
        /// </summary>
        private readonly uint _blockSize;

        /// <summary>
        /// The absolute original file size in bytes.
        /// </summary>
        private readonly uint _fileSize;

        /// <summary>
        /// Gets or sets a set of flags which holds some information about how the file is stored.
        /// </summary>
        public BlockFlags Flags { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockTableEntry"/> class.
        /// Deserializes a <see cref="BlockTableEntry"/> from provided binary data.
        /// </summary>
        /// <param name="data">The serialized data.</param>
        [PublicAPI]
        public BlockTableEntry([NotNull] byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    _blockOffset = br.ReadUInt32();
                    _blockSize = br.ReadUInt32();
                    _fileSize = br.ReadUInt32();
                    Flags = (BlockFlags)br.ReadUInt32();
                }
            }
        }

        /// <summary>
        /// Gets the offset of the block with file data.
        /// </summary>
        /// <returns>The block offset.</returns>
        [PublicAPI]
        public uint GetBlockOffset()
        {
            return _blockOffset;
        }

        /// <summary>
        /// Gets the block offset, merging the provided high bits with
        /// the offset in the block table entry.
        /// </summary>
        /// <param name="highBits">The bits to merge in.</param>
        /// <returns>The final offset.</returns>
        [PublicAPI]
        public ulong GetExtendedBlockOffset(ushort highBits)
        {
            return MPQHeader.MergeHighBits(_blockOffset, highBits);
        }

        /// <summary>
        /// Gets the size of the data block.
        /// </summary>
        /// <returns>The block size.</returns>
        [PublicAPI]
        public uint GetBlockSize()
        {
            return _blockSize;
        }

        /// <summary>
        /// Gets the flags.
        /// </summary>
        /// <returns>The flags.</returns>
        [PublicAPI]
        public BlockFlags GetFlags()
        {
            return Flags;
        }

        /// <summary>
        /// Gets the size of the decompressed and decrypted file.
        /// </summary>
        /// <returns>The file size.</returns>
        [PublicAPI]
        public uint GetFileSize()
        {
            return _fileSize;
        }

        /// <summary>
        /// Determines whether this data block is empty.
        /// </summary>
        /// <returns><c>true</c> if this instance is block empty; otherwise, <c>false</c>.</returns>
        [PublicAPI]
        public bool IsBlockEmpty()
        {
            return (_blockOffset != 0) && (_blockSize != 0) && (_fileSize == 0) && (Flags == 0);
        }

        /// <summary>
        /// Determines whether this data block is unused.
        /// </summary>
        /// <returns><c>true</c> if this instance is block unused; otherwise, <c>false</c>.</returns>
        [PublicAPI]
        public bool IsBlockUnused()
        {
            return (_blockOffset == 0) && (_blockSize == 0) && (_fileSize == 0) && (Flags == 0);
        }

        /// <summary>
        /// Determines whether or not the file data contained in this block is compressed or not.
        /// Oddly enough, on occasion the block will be marked as compressed but will not actually be compressed.
        /// These sorts of blocks can be detected by comparing the block size with the actual file size, which
        /// is done by this method.
        /// </summary>
        /// <returns><value>true</value> if the block is compressed; otherwise, <value>false</value>.</returns>
        [PublicAPI]
        public bool IsCompressed()
        {
            bool isCompressedInAnyWay = Flags.HasFlag(BlockFlags.IsCompressed) ||
                                        Flags.HasFlag(BlockFlags.IsCompressedMultiple) ||
                                        Flags.HasFlag(BlockFlags.IsImploded);

            // Oddly enough, on occasion the block will be marked as compressed but will not actually be compressed.
            // These sorts of blocks can be detected by comparing the block size with the actual file size.
            return (_blockSize != _fileSize) && isCompressedInAnyWay;
        }

        /// <summary>
        /// Determines whether or not the file data contained in this block is encrypted or not.
        /// </summary>
        /// <returns><value>true</value> if the block is encrypted; otherwise, <value>false</value>.</returns>
        [PublicAPI]
        public bool IsEncrypted()
        {
            return Flags.HasFlag(BlockFlags.IsEncrypted);
        }

        /// <summary>
        /// Determines whether or not the file data is stored as a set of sectors. If not, it's in a single unit.
        /// </summary>
        /// <returns><value>true</value> if the block is stored in sectors; otherwise, <value>false</value>.</returns>
        [PublicAPI]
        public bool HasSectors()
        {
            return !Flags.HasFlag(BlockFlags.IsSingleUnit);
        }

        /// <summary>
        /// Determines whether or not this block entry is pointing to any file data at all.
        /// </summary>
        /// <returns><value>true</value> if the block points to any data; otherwise, <value>false</value>.</returns>
        [PublicAPI]
        public bool HasData()
        {
            return Flags.HasFlag(BlockFlags.Exists) && !Flags.HasFlag(BlockFlags.IsDeletionMarker);
        }

        /// <summary>
        /// Determines whether or not this block entry is pointing to a deleted file.
        /// </summary>
        /// <returns>true if the file is deleted; otherwise, false.</returns>
        [PublicAPI]
        public bool IsDeleted()
        {
            return Flags.HasFlag(BlockFlags.IsDeletionMarker);
        }

        /// <summary>
        /// Determines whether or not the file data is stored as a single unit. If not, it's in sectors.
        /// </summary>
        /// <returns><value>true</value> if the block is stored as a single unit; otherwise, <value>false</value>.</returns>
        [PublicAPI]
        public bool IsSingleUnit()
        {
            return Flags.HasFlag(BlockFlags.IsSingleUnit);
        }

        /// <summary>
        /// Determined whether or not the encryption key should be adjusted by the block offset.
        /// </summary>
        /// <returns><value>true</value> if the key should be adjusted; otherwise, <value>false</value>.</returns>
        [PublicAPI]
        public bool ShouldEncryptionKeyBeAdjusted()
        {
            return Flags.HasFlag(BlockFlags.HasAdjustedEncryptionKey);
        }

        /// <summary>
        /// Serializes the current object into a byte array.
        /// </summary>
        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(_blockOffset);
                    bw.Write(_blockSize);
                    bw.Write(_fileSize);
                    bw.Write((uint)Flags);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Gets the size of a block table entry.
        /// </summary>
        /// <returns>The size.</returns>
        [PublicAPI]
        public static long GetSize()
        {
            return 16;
        }
    }
}
