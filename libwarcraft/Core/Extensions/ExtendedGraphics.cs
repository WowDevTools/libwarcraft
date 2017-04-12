//
//  ExtendedGraphics.cs
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
using System.Drawing;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.MDX.Geometry;

namespace Warcraft.Core.Extensions
{
	/// <summary>
	/// Extension methods used internally in the library for graphics classes.
	/// </summary>
	public static class ExtendedGraphics
	{
		/// <summary>
		/// Inverts colours of the specified bitmap.
		/// </summary>
		/// <param name="map">The bitmap to invert.</param>
		/// <param name="keepAlpha">Whether or not the keep the alpha values, or invert them as well.</param>
		/// <returns>The inverted bitmap.</returns>
		public static Bitmap Invert(this Bitmap map, bool keepAlpha = true)
		{
			Bitmap outMap = new Bitmap(map);

			for (int y = 0; y < map.Height; ++y)
			{
				for (int x = 0; x < map.Width; ++x)
				{
					Color pixel = map.GetPixel(x, y);
					byte pixelAlpha = pixel.A;
					if (!keepAlpha)
					{
						pixelAlpha = (byte)(255 - pixel.A);
					}

					Color negativePixel = Color.FromArgb(pixelAlpha, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
					//Color negativePixel = Color.FromArgb(pixelAlpha, 255 - pixel.G, 255 - pixel.B, 255 - pixel.A);

					outMap.SetPixel(x, y, negativePixel);
				}
			}

			return outMap;
		}

		/// <summary>
		/// Determines whether or not a given bitmap has any alpha values, and thus if it requires an alpha channel in
		/// other formats.
		/// </summary>
		/// <param name="map">The map to inspect.</param>
		/// <returns><value>true</value> if the bitmap has any alpha values; otherwise, <value>false</value>.</returns>
		public static bool HasAlpha(this Bitmap map)
		{
			for (int y = 0; y < map.Height; ++y)
			{
				for (int x = 0; x < map.Width; ++x)
				{
					Color pixel = map.GetPixel(x, y);
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