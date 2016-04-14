//
//  ExtensionMethods.cs
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

namespace Warcraft.Core
{
	static class ExtensionMethods
	{
		public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0)
				return min;
			else if (val.CompareTo(max) > 0)
				return max;
			else
				return val;
		}

		// Taken from the Arduino reference (https://www.arduino.cc/en/Reference/Map)
		public static int Map(int val, int in_min, int in_max, int out_min, int out_max)
		{
			return (val - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		}

		public static Bitmap Invert(this Bitmap map, bool KeepAlpha = true)
		{
			Bitmap outMap = new Bitmap(map);

			for (int y = 0; y < map.Height; ++y)
			{
				for (int x = 0; x < map.Width; ++x)
				{
					Color pixel = map.GetPixel(x, y);
					byte pixelAlpha = pixel.A;
					if (!KeepAlpha)
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

		/*
			Binary Reader Extensions for standard typess
		*/

		/// <summary>
		/// Reads a 16-byte 32-bit quaternion structure from the data stream, and advances the position of the stream by
		/// 16 bytes.
		/// </summary>
		/// <returns>The quaternion.</returns>
		/// <param name="Reader">Binary reader.</param>
		public static Quaternion ReadQuaternion32(this BinaryReader Reader)
		{
			return new Quaternion(Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
		}

		/// <summary>
		/// Reads a 8-byte 16-bit quaternion structure from the data stream, and advances the position of the stream by
		/// 8 bytes.
		/// </summary>
		/// <returns>The quaternion.</returns>
		/// <param name="Reader">Binary reader.</param>
		public static Quaternion ReadQuaternion16(this BinaryReader Reader)
		{
			short x = Reader.ReadInt16();
			short y = Reader.ReadInt16();
			short z = Reader.ReadInt16();
			short w = Reader.ReadInt16();
			return new Quaternion(ShortQuatValueToFloat(x), ShortQuatValueToFloat(y), ShortQuatValueToFloat(z), ShortQuatValueToFloat(w));
		}

		private static float ShortQuatValueToFloat(short InShort)
		{
			return (float)(InShort < 0 ? InShort + 32768 : InShort - 32768) / 32767.0f;
		}

		/// <summary>
		/// Reads a 12-byte Vector3f structure from the data stream	and advances the position of the stream by
		/// 12 bytes.
		/// </summary>
		/// <returns>The vector3f.</returns>
		/// <param name="Reader">Binary reader.</param>
		public static Vector3f ReadVector3f(this BinaryReader Reader)
		{
			return new Vector3f(Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
		}

		/// <summary>
		/// Reads an 8-byte Vector2f structure from the data stream and advances the position of the stream by
		/// 8 bytes.
		/// </summary>
		/// <returns>The vector2f.</returns>
		/// <param name="Reader">Binary reader.</param>
		public static Vector2f ReadVector2f(this BinaryReader Reader)
		{
			return new Vector2f(Reader.ReadSingle(), Reader.ReadSingle());
		}

		/// <summary>
		/// Reads a 24-byte Box structure from the data stream and advances the position of the stream by
		// 24 bytes.
		/// </summary>
		/// <returns>The box.</returns>
		/// <param name="Reader">Binary reader.</param>
		public static Box ReadBox(this BinaryReader Reader)
		{
			return new Box(Reader.ReadVector3f(), Reader.ReadVector3f());
		}
	}
}