//
//  Box.cs
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

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.Core.Structures
{
	/// <summary>
	/// A structure representing an axis-aligned bounding box, comprised of two <see cref="Vector3f"/> objects
	/// defining the bottom and top corners of the box.
	/// </summary>
	public struct Box : IFlattenableData<float>
	{
		/// <summary>
		/// The bottom corner of the bounding box.
		/// </summary>
		public Vector3 BottomCorner;

		/// <summary>
		/// The top corner of the bounding box.
		/// </summary>
		public Vector3 TopCorner;

		/// <summary>
		/// Creates a new <see cref="Box"/> object from a top and bottom corner.
		/// </summary>
		/// <param name="inBottomCorner">The bottom corner of the box.</param>
		/// <param name="inTopCorner">The top corner of the box.</param>
		/// <returns>A new <see cref="Box"/> object.</returns>
		public Box(Vector3 inBottomCorner, Vector3 inTopCorner)
		{
			this.BottomCorner = inBottomCorner;
			this.TopCorner = inTopCorner;
		}

		/// <summary>
		/// Gets the coordinates of the center of the box.
		/// </summary>
		/// <returns>A vector with the coordinates of the center of the box.</returns>
		public Vector3 GetCenterCoordinates()
		{
			return (this.BottomCorner + this.TopCorner) / 2;
		}

		public IReadOnlyCollection<float> Flatten()
		{
			return this.TopCorner.Flatten().Concat(this.BottomCorner.Flatten()).ToArray();
		}
	}
}
