//
//  ExtendedAttributes.cs
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
using JetBrains.Annotations;

namespace Warcraft.MPQ.Attributes
{
    /// <summary>
    /// Container class for the extended file attributes contained in an MPQ archive.
    /// </summary>
    [PublicAPI]
    public class ExtendedAttributes
    {
        /// <summary>
        /// The internal filename of the attributes.
        /// </summary>
        [PublicAPI, NotNull]
        public const string InternalFileName = "(attributes)";

        /// <summary>
        /// Gets or sets the version of the attribute file format.
        /// </summary>
        [PublicAPI]
        public uint Version { get; set; }

        /// <summary>
        /// Gets or sets the attributes present in the attribute file.
        /// </summary>
        [PublicAPI]
        public AttributeTypes AttributesPresent { get; set; }

        /// <summary>
        /// Gets or sets the list of file attributes.
        /// </summary>
        [PublicAPI, NotNull, ItemNotNull]
        public List<FileAttributes> FileAttributes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedAttributes"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fileBlockCount">The expected file block count.</param>
        /// <exception cref="ArgumentNullException">Thrown if the data is null.</exception>
        [PublicAPI]
        public ExtendedAttributes([NotNull] byte[] data, uint fileBlockCount)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    // Initial length (without any attributes) should be at least 8 bytes
                    uint expectedDataLength = 8;
                    if (data.Length < expectedDataLength)
                    {
                        Version = 0;
                        AttributesPresent = 0;
                        FileAttributes = new List<FileAttributes>();
                    }
                    else
                    {
                        Version = br.ReadUInt32();
                        AttributesPresent = (AttributeTypes)br.ReadUInt32();

                        var crcHashes = new List<uint>();
                        if (AttributesPresent.HasFlag(AttributeTypes.CRC32))
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

                        var timestamps = new List<ulong>();
                        if (AttributesPresent.HasFlag(AttributeTypes.Timestamp))
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

                        var md5Hashes = new List<string>();
                        if (AttributesPresent.HasFlag(AttributeTypes.MD5))
                        {
                            expectedDataLength += 16 * fileBlockCount;

                            for (int i = 0; i < fileBlockCount; ++i)
                            {
                                if (data.Length >= expectedDataLength)
                                {
                                    var md5Data = br.ReadBytes(16);
                                    string md5 = BitConverter.ToString(md5Data).Replace("-", string.Empty);
                                    md5Hashes.Add(md5);
                                }
                                else
                                {
                                    md5Hashes.Add(string.Empty);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < fileBlockCount; ++i)
                            {
                                md5Hashes.Add(string.Empty);
                            }
                        }

                        FileAttributes = new List<FileAttributes>();
                        for (int i = 0; i < fileBlockCount; ++i)
                        {
                            FileAttributes.Add(new FileAttributes(crcHashes[i], timestamps[i], md5Hashes[i]));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether or not the stored attributes are valid.
        /// </summary>
        /// <returns>true if the attributes are valid; otherwise, false.</returns>
        [PublicAPI]
        public bool AreAttributesValid()
        {
            return Version == 100 && AttributesPresent > 0;
        }
    }
}
