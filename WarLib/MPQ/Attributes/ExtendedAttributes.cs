using System;
using System.IO;
using System.Collections.Generic;

namespace WarLib.MPQ.Attributes
{
	public class ExtendedAttributes
	{
		private uint Version;
		private AttributeTypes AttributesPresent;
		private List<FileAttributes> BlockFileAttributes;

		public ExtendedAttributes(byte[] data, uint FileBlockCount)
		{			
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			
			MemoryStream dataStream = new MemoryStream(data);
			BinaryReader br = new BinaryReader(dataStream, System.Text.Encoding.UTF8);

			// Initial length (without any attributes) should be at least 8 bytes
			uint expectedDataLength = 8;
			if (data.Length < expectedDataLength)
			{
				this.Version = 0;
				this.AttributesPresent = 0;
				this.BlockFileAttributes = null;
			}
			else
			{
				this.Version = br.ReadUInt32();
				this.AttributesPresent = (AttributeTypes)br.ReadUInt32();

				List<uint> CRC32s = new List<uint>();
				if (AttributesPresent.HasFlag(AttributeTypes.CRC32))
				{
					expectedDataLength += sizeof(uint) * FileBlockCount;

					for (int i = 0; i < FileBlockCount; ++i)
					{
						if (data.Length >= expectedDataLength)
						{
							CRC32s.Add(br.ReadUInt32());
						}
						else
						{
							CRC32s.Add(0);
						}
					}
				}

				List<ulong> Timestamps = new List<ulong>();
				if (AttributesPresent.HasFlag(AttributeTypes.Timestamp))
				{
					expectedDataLength += sizeof(ulong) * FileBlockCount;

					for (int i = 0; i < FileBlockCount; ++i)
					{
						if (data.Length >= expectedDataLength)
						{
							Timestamps.Add(br.ReadUInt64());
						}
						else
						{
							Timestamps.Add(0);
						}
					}
				}

				List<string> MD5s = new List<string>();
				if (AttributesPresent.HasFlag(AttributeTypes.MD5))
				{
					expectedDataLength += 16 * FileBlockCount;

					for (int i = 0; i < FileBlockCount; ++i)
					{
						if (data.Length >= expectedDataLength)
						{
							byte[] md5Data = br.ReadBytes(16);
							string md5 = BitConverter.ToString(md5Data).Replace("-", "").ToLower();
							MD5s.Add(md5);
						}
						else
						{
							MD5s.Add("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
						}
					}
				}

				this.BlockFileAttributes = new List<FileAttributes>();
				for (int i = 0; i < FileBlockCount; ++i)
				{
					BlockFileAttributes.Add(new FileAttributes(CRC32s[i], Timestamps[i], MD5s[i]));
				}
			}
		}

		public bool AreAttributesValid()
		{
			return Version > 0 && AttributesPresent > 0;
		}
	}

	public class FileAttributes
	{
		public uint CRC32;
		public ulong Timestamp;
		public string MD5;

		public FileAttributes(uint CRC32, ulong Timestamp, string MD5)
		{
			this.CRC32 = CRC32;
			this.Timestamp = Timestamp;
			this.MD5 = MD5;
		}
	}

	[Flags]
	public enum AttributeTypes : uint
	{
		CRC32 = 0x00000001,
		Timestamp = 0x00000002,
		MD5 = 0x00000004
	}
}

