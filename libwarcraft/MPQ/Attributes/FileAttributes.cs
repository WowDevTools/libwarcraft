//
//  AttributeTypes.cs
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
        /// A CRC32 hash of the file.
        /// </summary>
        public uint CRC32;

        /// <summary>
        /// A last modified timestamp of the file.
        /// </summary>
        public ulong Timestamp;

        /// <summary>
        /// An MD5 hash of the file.
        /// </summary>
        public string MD5;

        /// <summary>
        /// Creates a new <see cref="FileAttributes"/> object from two given hashes and a timestamp.
        /// </summary>
        /// <param name="crc32">The CRC32 hash of the file.</param>
        /// <param name="timestamp">The last modified timestamp of the file.</param>
        /// <param name="md5">The MD5 hash of the file. </param>
        public FileAttributes(uint crc32, ulong timestamp, string md5)
        {
            this.CRC32 = crc32;
            this.Timestamp = timestamp;
            this.MD5 = md5;
        }
    }
}
