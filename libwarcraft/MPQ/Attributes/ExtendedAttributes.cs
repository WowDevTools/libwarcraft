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
	/// <summary>
	/// Container class for the extended file attributes contained in an MPQ archive.
	/// </summary>
	public class ExtendedAttributes
	{
		/// <summary>
		/// The internal filename of the attributes.
		/// </summary>
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

		/// <summary>
		/// Deserializes a <see cref="ExtendedAttributes"/> object from the provided binary data, and an expected
		/// file block count.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="fileBlockCount"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public ExtendedAttributes(byte[] data, uint fileBlockCount)
		{
			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
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

						List<uint> crcHashes = new List<uint>();
						if (this.AttributesPresent.HasFlag(AttributeTypes.CRC32))
						{
							expectedDataLength += sizeof(uint) * fileBlockCount;

							for (int i = 0; i < fileBlockCount; ++i)
							{
								if (data.Length >= expectedDataLength)
								{
									crcHashes.Add(br.ReadUInt32());
								}
								else
								{
									crcHashes.Add(0);
								}
							}
						}
						else
						{
							for (int i = 0; i < fileBlockCount; ++i)
							{
								crcHashes.Add(0);
							}
						}

						List<ulong> timestamps = new List<ulong>();
						if (this.AttributesPresent.HasFlag(AttributeTypes.Timestamp))
						{
							expectedDataLength += sizeof(ulong) * fileBlockCount;

							for (int i = 0; i < fileBlockCount; ++i)
							{
								if (data.Length >= expectedDataLength)
								{
									timestamps.Add(br.ReadUInt64());
								}
								else
								{
									timestamps.Add(0);
								}
							}
						}
						else
						{
							for (int i = 0; i < fileBlockCount; ++i)
							{
								timestamps.Add(0);
							}
						}

						List<string> md5Hashes = new List<string>();
						if (this.AttributesPresent.HasFlag(AttributeTypes.MD5))
						{
							expectedDataLength += 16 * fileBlockCount;

							for (int i = 0; i < fileBlockCount; ++i)
							{
								if (data.Length >= expectedDataLength)
								{
									byte[] md5Data = br.ReadBytes(16);
									string md5 = BitConverter.ToString(md5Data).Replace("-", "");
									md5Hashes.Add(md5);
								}
								else
								{
									md5Hashes.Add("");
								}
							}
						}
						else
						{
							for (int i = 0; i < fileBlockCount; ++i)
							{
								md5Hashes.Add("");
							}
						}

						this.FileAttributes = new List<FileAttributes>();
						for (int i = 0; i < fileBlockCount; ++i)
						{
							this.FileAttributes.Add(new FileAttributes(crcHashes[i], timestamps[i], md5Hashes[i]));
						}
					}
				}
			}
		}

		/// <summary>
		/// Determines whether or not the stored attributes are valid.
		/// </summary>
		/// <returns></returns>
		public bool AreAttributesValid()
		{
			return this.Version == 100 && this.AttributesPresent > 0;
		}
	}
}

