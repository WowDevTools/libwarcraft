using System;
using System.Collections.Generic;


// A massive thanks to Justin Olbrantz (Quantam) and Jean-Francois Roy
// (BahamutZERO), whose [documentation of the MPQ
// format](http://wiki.devklog.net/index.php?title=The_MoPaQ_Archive_Format) was
// instrumental for this implementation.

// Certain parts of the crypto class was taken from this library. Thanks!
// https://github.com/nickaceves/nmpq/blob/master/Nmpq/Parsing/Crypto.cs
using System.IO;
using WarLib.Core.Hashing;

namespace WarLib.MPQ.Crypto
{
	static class MPQCrypt
	{
		/// <summary>
		/// A table of int32s used as a lookup table for decryption of bytes. 
		/// It is statically initialized, and is always the same.
		/// </summary>
		private static readonly uint[] encryptionTable = new uint[0x500];

		static MPQCrypt()
		{
			InitializeEncryptionTable();
		}

		/// <summary>
		/// Initializes the encryption table.
		/// </summary>
		static void InitializeEncryptionTable()
		{
			uint seed = 0x00100001;

			for (uint loopIndex = 0; loopIndex < 0x100; loopIndex++)
			{
				int i = 0;
				for (uint tableIndex = loopIndex; i < 5; i++, tableIndex += 0x100)
				{
					seed = (seed * 125 + 3) % 0x2AAAAB;
					uint temp1 = (seed & 0xFFFF) << 0x10;

					seed = (seed * 125 + 3) % 0x2AAAAB;
					uint temp2 = (seed & 0xFFFF);

					// Add to Encryption table
					encryptionTable[tableIndex] = (temp1 | temp2);
				}
			}
		}

		/// <summary>
		/// Encrypts the given input data. Bytes outside of an even 4-byte boundary (even int32s) are not encrypted.
		/// </summary>
		/// <returns>The encrypted data.</returns>
		/// <param name="data">Data to be encrypted.</param>
		/// <param name="key">The encryption key to use.</param>
		public static byte[] EncryptData(byte[] data, uint key)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}

			// If the input is not aligned to 4 bytes (even 32-bit numbers),
			// cut out the dangling bytes and don't encrypt them

			// Find out how many dangling bytes we have
			uint danglingBytes = (uint)data.Length % 4;
			byte[] dataToBeEncrypted = new byte[data.Length - danglingBytes];

			// Copy the aligned bytes to the new array
			Buffer.BlockCopy(data, 0, dataToBeEncrypted, 0, (int)(data.Length - danglingBytes));


			uint encryptionSeed = 0xEEEEEEEE;
			List<byte> encryptedData = new List<byte>();

			for (int i = 0; i < dataToBeEncrypted.Length; i += sizeof(uint))
			{
				uint encryptionTarget = BitConverter.ToUInt32(dataToBeEncrypted, i);
				uint encryptedValue;

				// Retrieve the decryption seed from the generated table
				encryptionSeed += encryptionTable[0x400 + (key & 0xFF)];

				// Encrypt the data by XORing it with the seed and key
				encryptedValue = encryptionTarget ^ (key + encryptionSeed);

				// Modify the seed and key for the next value
				key = ((~key << 0x15) + 0x11111111) | (key >> 0x0B);
				encryptionSeed = encryptionTarget + encryptionSeed + (encryptionSeed << 5) + 3;

				byte[] encryptedBytes = BitConverter.GetBytes(encryptedValue);
				foreach (byte encryptedByte in encryptedBytes)
				{
					encryptedData.Add(encryptedByte);
				}
			}

			// If we did have some dangling bytes, copy them to a new array
			if (danglingBytes > 0)
			{
				byte[] decryptedDataBlock = encryptedData.ToArray();
				byte[] finalDecryptedData = new byte[decryptedDataBlock.Length + danglingBytes];

				Buffer.BlockCopy(decryptedDataBlock, 0, finalDecryptedData, 0, decryptedDataBlock.Length);
				Buffer.BlockCopy(data, (int)(data.Length - danglingBytes), finalDecryptedData, decryptedDataBlock.Length, (int)danglingBytes);

				return finalDecryptedData;
			}
			else
			{
				// Else we can just return the data as-is
				return encryptedData.ToArray();
			}
		}

		/// <summary>
		/// Decrypts the given input data. Bytes outside of an even 4-byte boundary (even int32s) are not decrypted.
		/// </summary>
		/// <returns>The decrypted data.</returns>
		/// <param name="data">Data to be decrypted.</param>
		/// <param name="key">The decryption key to use.</param>
		public static byte[] DecryptData(byte[] data, uint key)
		{

			if (data == null)
			{
				throw new ArgumentNullException("data");
			}

			// If the input is not aligned to 4 bytes (even 32-bit numbers),
			// cut out the dangling bytes and don't decrypt them

			// Find out how many dangling bytes we have
			uint danglingBytes = (uint)data.Length % 4;
			byte[] dataToBeDecrypted = new byte[data.Length - danglingBytes];

			// Copy the aligned bytes to the new array
			Buffer.BlockCopy(data, 0, dataToBeDecrypted, 0, (int)(data.Length - danglingBytes));


			uint decryptionSeed = 0xEEEEEEEE;		
			List<byte> decryptedData = new List<byte>();

			for (int i = 0; i < dataToBeDecrypted.Length; i += sizeof(uint))
			{
				var decryptionTarget = BitConverter.ToUInt32(dataToBeDecrypted, i);

				// Retrieve the decryption seed from the generated table
				decryptionSeed += encryptionTable[0x400 + (key & 0xFF)];

				// Decrypt the data by XORing it with the seed and key
				decryptionTarget = decryptionTarget ^ (key + decryptionSeed);

				// Modify the seed and key for the next value
				key = ((~key << 0x15) + 0x11111111) | (key >> 0x0B);
				decryptionSeed = decryptionTarget + decryptionSeed + (decryptionSeed << 5) + 3;

				var decryptedBytes = BitConverter.GetBytes(decryptionTarget);
				foreach (byte decryptedByte in decryptedBytes)
				{
					decryptedData.Add(decryptedByte);
				}
			}

			// If we did have some dangling bytes, copy them to a new array
			if (danglingBytes > 0)
			{
				byte[] decryptedDataBlock = decryptedData.ToArray();
				byte[] finalDecryptedData = new byte[decryptedDataBlock.Length + danglingBytes];

				Buffer.BlockCopy(decryptedDataBlock, 0, finalDecryptedData, 0, decryptedDataBlock.Length);
				Buffer.BlockCopy(data, (int)(data.Length - danglingBytes), finalDecryptedData, decryptedDataBlock.Length, (int)danglingBytes);

				return finalDecryptedData;
			}
			else
			{
				// Else we can just return the data as-is
				return decryptedData.ToArray();
			}
		}

		/// <summary>
		/// Hashes an input string, given a hash type.
		/// </summary>
		/// <returns>The hash.</returns>
		/// <param name="inputString">Input string.</param>
		/// <param name="hashType">Hash type.</param>
		public static uint Hash(string inputString, HashType hashType)
		{
			uint seed1 = 0x7FED7FED;
			var seed2 = 0xEEEEEEEE;

			foreach (var c in inputString.ToUpperInvariant())
			{
				uint ch = (byte)c;
				seed1 = encryptionTable[((uint)hashType * 0x100) + ch] ^ (seed1 + seed2);
				seed2 = ch + seed1 + seed2 + (seed2 << 5) + 3;
			}

			return seed1;
		}

		/// <summary>
		/// Calculates the decryption key for a file sector.
		/// </summary>
		/// <returns>The sector key.</returns>
		/// <param name="fileName">The name of the file the sector belongs to</param>
		/// <param name="isAdjusted">If set to <c>true</c>, the key is adjusted by the given block offset and file size.</param>
		/// <param name="blockOffset">The block offset of the file.</param>
		/// <param name="fileSize">The size of the file.</param>
		public static uint GetFileKey(string fileName, bool isAdjusted = false, uint blockOffset = 0, uint fileSize = 0)
		{
			uint fileKey = Hash(fileName, HashType.FileKey);

			if (isAdjusted)
			{
				fileKey = (uint)(fileKey + blockOffset) ^ fileSize;
			}

			return fileKey;
		}

		/// <summary>
		/// Decrypts the sector offset table using rolling decryption - it starts where the input BinaryReader is, reads 
		/// and decrypts offsets until the decrypted offset equals the input block size.
		/// </summary>
		/// <param name="br">The archive's BinaryReader</param>
		/// <param name="sectorOffsets">The output sector offsets.</param>
		/// <param name="blockSize">The size of the block to be decrypted.</param>
		/// <param name="key">The decryption key for the offset table.</param>
		public static void DecryptSectorOffsetTable(BinaryReader br, ref List<uint> sectorOffsets, uint blockSize, uint key)
		{
			uint decryptionSeed = 0xEEEEEEEE;		

			uint decryptionTarget = 0;
			while (decryptionTarget != blockSize)
			{
				decryptionTarget = br.ReadUInt32();

				// Retrieve the decryption seed from the generated table
				decryptionSeed += encryptionTable[0x400 + (key & 0xFF)];

				// Decrypt the data by XORing it with the seed and key
				decryptionTarget = decryptionTarget ^ (key + decryptionSeed);

				// Modify the seed and key for the next value
				key = ((~key << 0x15) + 0x11111111) | (key >> 0x0B);
				decryptionSeed = decryptionTarget + decryptionSeed + (decryptionSeed << 5) + 3;

				sectorOffsets.Add(decryptionTarget);
			}
		}

		/// <summary>
		/// Verifies the integrity of a file sector using an adler32 checksum.
		/// </summary>
		/// <returns><c>true</c>, if the sector integrity is not compromised, <c>false</c> otherwise.</returns>
		/// <param name="sector">Sector data.</param>
		/// <param name="checksum">Sector checksum.</param>
		public static bool VerifySectorChecksum(byte[] sector, uint checksum)
		{
			if (checksum == 0)
			{
				return true;
			}

			uint sectorChecksum = (uint)Adler32.ComputeChecksum(new MemoryStream(sector));

			if (sectorChecksum == 0)
			{
				// We can't handle a 0 checksum.
				return false;
			}

			return sectorChecksum == checksum;
		}
	}

	/// <summary>
	/// Different types of hashes that can be produced by the hashing function.
	/// </summary>
	public enum HashType : uint
	{
		FileHashTableOffset = 0,
		FilePathA = 1,
		FilePathB = 2,
		FileKey = 3,
	}
}

