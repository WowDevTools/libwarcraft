//
//  Interpolation.cs
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

namespace Warcraft.Core.Interpolation
{
	public static class Interpolation
	{
		public static float InterpolateFlat(float a, float b, float alpha)
		{
			return 0.0f;
		}

		public static float InterpolateLinear(float a, float b, float alpha)
		{
			return 0.0f;
		}

		public static float InterpolateHermite(float a, float tangentA, float b, float tangentB, float alpha)
		{
			return 0.0f;
		}

		public static float InterpolateBezier(float a, float tangentA, float b, float tangentB, float alpha)
		{
			return 0.0f;
		}
	}

	public enum InterpolationType : short
	{
		Flat = 0,
		Linear = 1,
		Hermite = 2,
		Bezier = 3
	}
}

