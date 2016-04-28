//
//  ExtendedAttributes.cs
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

using System;
using System.IO;
using System.Collections.Generic;

namespace Warcraft.MPQ.Attributes
{
	public class ExtendedAttributes
	{
		public const string InternalFileName = "(attributes)";

		/// <summary>
		/// The version of the attribute file format.
		/// </summary>
		public uint Version;

		/// <summary>
		/// The attributes present in the attribute file.
		/// </summary>
		public AttributeTypes AttributesPresent;

		/// <summary>
		/// The list of file attributes.
		/// </summary>
		public List<FileAttributes> FileAttributes;

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
				this.FileAttributes = null;
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
							string md5 = BitConverter.ToString(md5Data).Replace("-", "");
							MD5s.Add(md5);
						}
						else
						{
							MD5s.Add("");
						}
					}
				}

				this.FileAttributes = new List<FileAttributes>();
				for (int i = 0; i < FileBlockCount; ++i)
				{
					FileAttributes.Add(new FileAttributes(CRC32s[i], Timestamps[i], MD5s[i]));
				}
			}
		}

		public bool AreAttributesValid()
		{
			return Version == 100 && AttributesPresent > 0;
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

