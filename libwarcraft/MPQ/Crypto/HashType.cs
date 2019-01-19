//
//  HashType.cs
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

namespace Warcraft.MPQ.Crypto
{
    /// <summary>
    /// Different types of hashes that can be produced by the hashing function.
    /// </summary>
    public enum HashType : uint
    {
        /// <summary>
        /// The hash algorithm used for determining the home entry of a file in the hash table.
        /// </summary>
        FileHashTableOffset = 0,

        /// <summary>
        /// One of the two algorithms used to generate hashes for filenames.
        /// </summary>
        FilePathA = 1,

        /// <summary>
        /// One of the two algorithms used to generate hashes for filenames.
        /// </summary>
        FilePathB = 2,

        /// <summary>
        /// The hash algorithm used for generating encryption keys.
        /// </summary>
        FileKey = 3
    }
}
