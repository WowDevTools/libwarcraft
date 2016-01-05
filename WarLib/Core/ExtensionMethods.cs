using System;

namespace WarLib.Core
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
	}
}