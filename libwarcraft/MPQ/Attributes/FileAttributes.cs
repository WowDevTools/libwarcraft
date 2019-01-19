//
//  FileAttributes.cs
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

namespace Warcraft.MPQ.Attributes
{
    /// <summary>
    /// A set of file attributes for a file in an MPQ archive.
    /// </summary>
    public class FileAttributes
    {
        /// <summary>
        /// Gets or sets the CRC32 hash of the file.
        /// </summary>
        public uint CRC32 { get; set; }

        /// <summary>
        /// Gets or sets the last modified timestamp of the file.
        /// </summary>
        public ulong Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the MD5 hash of the file.
        /// </summary>
        public string MD5 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAttributes"/> class.
        /// </summary>
        /// <param name="crc32">The CRC32 hash of the file.</param>
        /// <param name="timestamp">The last modified timestamp of the file.</param>
        /// <param name="md5">The MD5 hash of the file. </param>
        public FileAttributes(uint crc32, ulong timestamp, string md5)
        {
            CRC32 = crc32;
            Timestamp = timestamp;
            MD5 = md5;
        }
    }
}
