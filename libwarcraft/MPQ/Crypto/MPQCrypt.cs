//
//  MPQCrypt.cs
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
using Warcraft.Core.Hashing;

namespace Warcraft.MPQ.Crypto
{
    /// <summary>
    /// This class handles MPQ decryption and encryption of sections in the MPQ file.
    /// Certain parts of the crypto class was taken from this library. Thanks!
    /// https://github.com/nickaceves/nmpq/blob/master/Nmpq/Parsing/Crypto.cs.
    /// </summary>
    internal static class MPQCrypt
    {
        /// <summary>
        /// A table of int32s used as a lookup table for decryption of bytes.
        /// It is statically initialized, and is always the same.
        /// </summary>
        [NotNull]
        private static readonly uint[] EncryptionTable = new uint[0x500];

        static MPQCrypt()
        {
            InitializeEncryptionTable();
        }

        /// <summary>
        /// Initializes the encryption table.
        /// </summary>
        private static void InitializeEncryptionTable()
        {
            uint seed = 0x00100001;

            for (uint loopIndex = 0; loopIndex < 0x100; loopIndex++)
            {
                int i = 0;
                for (uint tableIndex = loopIndex; i < 5; i++, tableIndex += 0x100)
                {
                    seed = ((seed * 125) + 3) % 0x2AAAAB;
                    uint temp1 = (seed & 0xFFFF) << 0x10;

                    seed = ((seed * 125) + 3) % 0x2AAAAB;
                    uint temp2 = seed & 0xFFFF;

                    // Add to Encryption table
                    EncryptionTable[tableIndex] = temp1 | temp2;
                }
            }
        }

        /// <summary>
        /// Encrypts the given input data. Bytes outside of an even 4-byte boundary are not encrypted, and
        /// are simply appended at the end of the data block.
        /// </summary>
        /// <returns>The encrypted data.</returns>
        /// <param name="data">ExtendedData to be encrypted.</param>
        /// <param name="key">The encryption key to use.</param>
        [NotNull]
        public static byte[] EncryptData([NotNull] byte[] data, uint key)
        {
            return InternalEncryptDecrypt(data, key);
        }

        /// <summary>
        /// Decrypts the given input data. Bytes outside of an even 4-byte boundary are not decrypted, and
        /// are considered not encrypted.
        /// </summary>
        /// <returns>The decrypted data.</returns>
        /// <param name="data">ExtendedData to be decrypted.</param>
        /// <param name="key">The decryption key to use.</param>
        [NotNull]
        public static byte[] DecryptData([NotNull] byte[] data, uint key)
        {
            return InternalEncryptDecrypt(data, key);
        }

        /// <summary>
        /// Internal XOR function that encrypts and decrypts a block of data.
        /// </summary>
        /// <returns>The encrypted or decrypted data.</returns>
        /// <param name="data">ExtendedData.</param>
        /// <param name="key">Key.</param>
        [NotNull]
        private static byte[] InternalEncryptDecrypt([NotNull] byte[] data, uint key)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Input data must be populated.");
            }

            // If the input is not aligned to 4 bytes (even 32-bit numbers),
            // cut out the dangling bytes and don't XOR them

            // Find out how many dangling bytes we have
            uint danglingBytes = (uint)data.Length % 4;
            byte[] dataToBeModified = new byte[data.Length - danglingBytes];

            // Copy the aligned bytes to the new array
            Buffer.BlockCopy(data, 0, dataToBeModified, 0, (int)(data.Length - danglingBytes));

            uint encryptionSeed = 0xEEEEEEEE;
            List<byte> finalizedData = new List<byte>();

            for (int i = 0; i < dataToBeModified.Length; i += sizeof(uint))
            {
                uint encryptionTarget = BitConverter.ToUInt32(dataToBeModified, i);

                // Retrieve the decryption seed from the generated table
                encryptionSeed += EncryptionTable[0x400 + (key & 0xFF)];

                // Encrypt or decrypt the data by XORing it with the seed and key
                encryptionTarget = encryptionTarget ^ (key + encryptionSeed);

                // Modify the seed and key for the next value
                key = ((~key << 0x15) + 0x11111111) | (key >> 0x0B);
                encryptionSeed = encryptionTarget + encryptionSeed + (encryptionSeed << 5) + 3;

                byte[] encryptedBytes = BitConverter.GetBytes(encryptionTarget);
                foreach (byte encryptedByte in encryptedBytes)
                {
                    finalizedData.Add(encryptedByte);
                }
            }

            // If we did have some dangling bytes, copy them to a new array
            if (danglingBytes > 0)
            {
                byte[] decryptedDataBlock = finalizedData.ToArray();
                byte[] finalDecryptedData = new byte[decryptedDataBlock.Length + danglingBytes];

                Buffer.BlockCopy(decryptedDataBlock, 0, finalDecryptedData, 0, decryptedDataBlock.Length);
                Buffer.BlockCopy(data, (int)(data.Length - danglingBytes), finalDecryptedData, decryptedDataBlock.Length, (int)danglingBytes);

                return finalDecryptedData;
            }
            else
            {
                // Else we can just return the data as-is
                return finalizedData.ToArray();
            }
        }

        /// <summary>
        /// Hashes an input string, given a hash type. This function is case-insensitive, and
        /// treats all input as being in ALL UPPER CASE.
        /// </summary>
        /// <returns>The hash.</returns>
        /// <param name="inputString">Input string.</param>
        /// <param name="hashType">Hash type.</param>
        public static uint Hash([NotNull] string inputString, HashType hashType)
        {
            uint seed1 = 0x7FED7FED;
            var seed2 = 0xEEEEEEEE;

            foreach (var c in inputString.ToUpperInvariant())
            {
                uint ch = (byte)c;
                seed1 = EncryptionTable[((uint)hashType * 0x100) + ch] ^ (seed1 + seed2);
                seed2 = ch + seed1 + seed2 + (seed2 << 5) + 3;
            }

            return seed1;
        }

        /// <summary>
        /// Calculates the decryption key for a file sector.
        /// </summary>
        /// <returns>The sector key.</returns>
        /// <param name="fileName">The name of the file the sector belongs to.</param>
        /// <param name="isAdjusted">If set to <c>true</c>, the key is adjusted by the given block offset and file size.</param>
        /// <param name="blockOffset">The block offset of the file.</param>
        /// <param name="fileSize">The size of the file.</param>
        public static uint GetFileKey([NotNull] string fileName, bool isAdjusted = false, uint blockOffset = 0, uint fileSize = 0)
        {
            uint fileKey = Hash(fileName, HashType.FileKey);

            if (isAdjusted)
            {
                fileKey = (fileKey + blockOffset) ^ fileSize;
            }

            return fileKey;
        }

        /// <summary>
        /// Decrypts the sector offset table using rolling decryption - it starts where the input BinaryReader is, then
        /// reads and decrypts offsets until the decrypted offset equals the input block size. If the table is found to
        /// be inconsistent during the decryption (offsets are not unique, offset is less than preceding offset), then
        /// an exception will be thrown.
        /// </summary>
        /// <exception cref="InvalidFileSectorTableException">Thrown if the sector table is found to be inconsistent in any way.</exception>
        /// <param name="br">The archive's BinaryReader.</param>
        /// <param name="sectorOffsets">The output sector offsets.</param>
        /// <param name="blockSize">The size of the block to be decrypted.</param>
        /// <param name="key">The decryption key for the offset table.</param>
        public static void DecryptSectorOffsetTable
        (
            [NotNull] BinaryReader br,
            [NotNull] ref List<uint> sectorOffsets,
            uint blockSize,
            uint key
        )
        {
            uint decryptionSeed = 0xEEEEEEEE;

            uint decryptionTarget = 0;
            while (decryptionTarget != blockSize)
            {
                decryptionTarget = br.ReadUInt32();

                // Retrieve the decryption seed from the generated table
                decryptionSeed += EncryptionTable[0x400 + (key & 0xFF)];

                // Decrypt the data by XORing it with the seed and key
                decryptionTarget = decryptionTarget ^ (key + decryptionSeed);

                // Modify the seed and key for the next value
                key = ((~key << 0x15) + 0x11111111) | (key >> 0x0B);
                decryptionSeed = decryptionTarget + decryptionSeed + (decryptionSeed << 5) + 3;

                // Should the resulting sector offset be less than the previous data, then the data is inconsistent
                // and no table should be returned.
                if (sectorOffsets.LastOrDefault() > decryptionTarget)
                {
                    throw new InvalidFileSectorTableException(
                        "The read offset in the sector table was less than the previous offset.");
                }

                // Should the resulting sector offset be greater than the total block size, then the data is
                // inconsistent and no file should be returned.
                if (decryptionTarget > blockSize)
                {
                    throw new InvalidFileSectorTableException(
                        "The read offset in the sector table was greater than the total size of the data block.");
                }

                // Should the resulting sector not be unique, something is wrong and no table should be returned.
                if (sectorOffsets.Contains(decryptionTarget))
                {
                    throw new InvalidFileSectorTableException(
                        "The read offset in the sector table was not unique to the whole table.");
                }

                // The offset should be valid, so add it to the table.
                sectorOffsets.Add(decryptionTarget);
            }
        }

        /// <summary>
        /// Verifies the integrity of a file sector using an adler32 checksum.
        /// </summary>
        /// <returns><c>true</c>, if the sector integrity is not compromised, <c>false</c> otherwise.</returns>
        /// <param name="sector">Sector data.</param>
        /// <param name="checksum">Sector checksum.</param>
        public static bool VerifySectorChecksum([NotNull] byte[] sector, uint checksum)
        {
            using (MemoryStream ms = new MemoryStream(sector))
            {
                if (checksum == 0)
                {
                    return true;
                }

                uint sectorChecksum = (uint)Adler32.ComputeChecksum(ms);

                if (sectorChecksum == 0)
                {
                    // We can't handle a 0 checksum.
                    sectorChecksum = uint.MaxValue;
                }

                return sectorChecksum == checksum;
            }
        }

        /// <summary>
        /// Creates an encryption key for the specified file path.
        /// </summary>
        /// <param name="filePath">The path of the file for which the encryption key is to be created.</param>
        /// <param name="adjustKeyByOffset">Whether or not the key should be adjusted by the file offset.</param>
        /// <param name="adjustedBlockOffset">The offset into the archive where the file data begins.</param>
        /// <param name="fileSize">The absolute size of the file.</param>
        /// <returns>An encryption key for the provied file path.</returns>
        public static uint CreateFileEncryptionKey
        (
            [NotNull] string filePath,
            bool adjustKeyByOffset,
            long adjustedBlockOffset = 0,
            uint fileSize = 0
        )
        {
            uint fileKey;
            if (adjustKeyByOffset)
            {
                fileKey = GetFileKey(Path.GetFileName(filePath), true, (uint)adjustedBlockOffset, fileSize);
            }
            else
            {
                fileKey = GetFileKey(Path.GetFileName(filePath));
            }

            return fileKey;
        }
    }
}
