//
//  ExtendedGraphics.cs
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

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Warcraft.Core.Extensions
{
    /// <summary>
    /// Extension methods used internally in the library for graphics classes.
    /// </summary>
    public static class ExtendedGraphics
    {
        /// <summary>
        /// Determines whether or not a given bitmap has any alpha values, and thus if it requires an alpha channel in
        /// other formats.
        /// </summary>
        /// <param name="map">The map to inspect.</param>
        /// <returns><value>true</value> if the bitmap has any alpha values; otherwise, <value>false</value>.</returns>
        public static bool HasAlpha(this Image<Rgba32> map)
        {
            for (int y = 0; y < map.Height; ++y)
            {
                for (int x = 0; x < map.Width; ++x)
                {
                    var pixel = map[x, y];
                    if (pixel.A != 255)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
