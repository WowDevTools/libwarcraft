//
//  TextureCompressionType.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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

namespace Warcraft.BLP
{
    /// <summary>
    /// The compression type used in the <see cref="BLP"/> image.
    /// </summary>
    public enum TextureCompressionType : uint
    {
        /// <summary>
        /// The image is using JPEG compression.
        /// </summary>
        JPEG = 0,

        /// <summary>
        /// The image is using a colour palette.
        /// </summary>
        Palettized = 1,

        /// <summary>
        /// The image is compressed using the DXT algorithm.
        /// </summary>
        DXTC = 2,

        /// <summary>
        /// The image is not compressed.
        /// </summary>
        Uncompressed = 3,

        /// <summary>
        /// TODO: Unknown behaviour
        /// The image is not compressed.
        /// </summary>
        UncompressedAlternate = 4
    }
}
