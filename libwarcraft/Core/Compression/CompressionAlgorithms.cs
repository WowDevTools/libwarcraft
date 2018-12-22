//
//  CompressionAlgorithms.cs
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

namespace Warcraft.Core.Compression
{
    /// <summary>
    /// This enum contains all of the compression algorithms used in MPQ archives. They are sorted according to
    /// compression order, and must not be reordered.
    ///
    /// When compressing, the algorithms are applied from top to bottom. When decompressing, the inverse is true.
    /// </summary>
    [Flags]
    public enum CompressionAlgorithms : byte
    {
        /// <summary>
        /// LZMA compression.
        /// </summary>
        LZMA = 0x20,

        /// <summary>
        /// IMA ADPCM Mono Audio compression.
        /// </summary>
        ADPCMMono = 0x40,

        /// <summary>
        /// IMA ADPCM Stereo Audio compression.
        /// </summary>
        ADPCMStereo = 0x80,

        /// <summary>
        /// Huffman tree compression.
        /// </summary>
        Huffman = 0x01,

        /// <summary>
        /// ZLIB Deflate compression.
        /// </summary>
        Deflate = 0x02,

        /// <summary>
        /// PKWARE Implode compression.
        /// </summary>
        Implode = 0x08,

        /// <summary>
        /// BZip2 compression.
        /// </summary>
        BZip2 = 0x10
    }
}
