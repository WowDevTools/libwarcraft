//
//  BLPPixelFormat.cs
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

namespace Warcraft.BLP
{
    /// <summary>
    /// The format of the pixels stored in a <see cref="BLP"/> file.
    /// </summary>
    public enum BLPPixelFormat : uint
    {
        /// <summary>
        /// DXT1 compressed pixels.
        /// </summary>
        DXT1 = 0,

        /// <summary>
        /// DXT3 compressed pixels.
        /// </summary>
        DXT3 = 1,

        /// <summary>
        /// ARGB8888 formatted pixels.
        /// </summary>
        ARGB8888 = 2,

        /// <summary>
        /// PAL ARGB1555 formatted pixels.
        /// </summary>
        PalARGB1555DitherFloydSteinberg = 3,

        /// <summary>
        /// PAL ARGB4444 formatted pixels.
        /// </summary>
        PalARGB4444DitherFloydSteinberg = 4,

        /// <summary>
        /// PAL ARGB565 formatted pixels.
        /// </summary>
        PalARGB565DitherFloydSteinberg = 5,

        /// <summary>
        /// DXT5 compressed pixels.
        /// </summary>
        DXT5 = 7,

        /// <summary>
        /// Palettized pixels, that is, the pixels are indices into the stored colour palette.
        /// </summary>
        Palettized = 8,

        /// <summary>
        /// PAL ARGB2565 formatted pixels.
        /// </summary>
        PalARGB2565DitherFloydSteinberg = 9
    }
}
