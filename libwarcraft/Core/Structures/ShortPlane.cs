//
//  ShortPlane.cs
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
using System.Collections.Generic;

namespace Warcraft.Core.Structures
{
	/// <summary>
	/// A structure representing a world Z-aligned plane with nine coordinates.
	/// </summary>
	public struct ShortPlane
	{
		/// <summary>
		/// The 3x3 grid of coordinates in the plane.
		/// </summary>
		public List<List<short>> Coordinates;

		/// <summary>
		/// Creates a new <see cref="ShortPlane"/> from a jagged list of coordinates.
		/// </summary>
		/// <param name="inCoordinates">A list of coordinates.</param>
		/// <exception cref="ArgumentException">
		/// An <see cref="ArgumentException"/> will be thrown if the input list is not a 3x3 jagged list of coordinates.
		/// </exception>
		public ShortPlane(List<List<short>> inCoordinates)
		{
			if (inCoordinates.Count != 3)
			{
				throw new ArgumentException("The input coordinate list must be a 3x3 grid of coordinates.", nameof(inCoordinates));
			}

			for (int i = 0; i < 3; ++i)
			{
				if (inCoordinates[i].Count != 3)
				{
					throw new ArgumentException("The input coordinate list must be a 3x3 grid of coordinates.", nameof(inCoordinates));
				}
			}

			this.Coordinates = inCoordinates;
		}

		/// <summary>
		/// Creates a new <see cref="ShortPlane"/> from a single short value, which is applied to all nine coordinates.
		/// </summary>
		/// <param name="inAllCoordinates">The short to use for all coordinates.</param>
		public ShortPlane(short inAllCoordinates)
		{
			this.Coordinates = new List<List<short>>();

			for (int y = 0; y < 3; ++y)
			{
				List<short> coordinateRow = new List<short>();
				for (int x = 0; x < 3; ++x)
				{
					coordinateRow.Add(inAllCoordinates);
				}
				this.Coordinates.Add(coordinateRow);
			}
		}
	}
}
