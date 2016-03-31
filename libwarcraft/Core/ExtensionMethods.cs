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
		/// Reads a 12-byte Vector3f structure from the data stream	and advances the position of the stream by
		/// 12 bytes.
		/// </summary>
		/// <returns>The vector3f.</returns>
		/// <param name="br">Br.</param>
		public static Vector3f ReadVector3f(this BinaryReader br)
		{
			return new Vector3f(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
		}

		/// <summary>
		/// Reads a 24-byte Box structure from the data stream and advances the position of the stream by
		// 24 bytes.
		/// </summary>
		/// <returns>The box.</returns>
		/// <param name="br">Br.</param>
		public static Box ReadBox(this BinaryReader br)
		{
			return new Box(br.ReadVector3f(), br.ReadVector3f());
		}
	}
}