//
//  Vector4f.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.Core.Structures
{
	/// <summary>
	/// A structure representing a four-dimensional floating point vector.
	/// </summary>
	public struct Vector4f : IFlattenableData<float>
	{
		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public float X;

		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		public float Y;

		/// <summary>
		/// The Z component of the vector.
		/// </summary>
		public float Z;

		/// <summary>
		/// The W (or scalar) component of the vector.
		/// </summary>
		public float W;

		/// <summary>
		/// Creates a new <see cref="Vector4f"/> object from four input floats.
		/// </summary>
		/// <param name="inX">The input X component.</param>
		/// <param name="inY">The input Y component.</param>
		/// <param name="inZ">The input Z component.</param>
		/// <param name="inW">The input W component.</param>
		public Vector4f(float inX, float inY, float inZ, float inW)
		{
			this.X = inX;
			this.Y = inY;
			this.Z = inZ;
			this.W = inW;
		}

		/// <summary>
		/// Creates a new <see cref="Vector4f"/> object from a single input float, filling all components.
		/// </summary>
		/// <param name="all">The input component.</param>
		public Vector4f(float all)
			:this(all, all, all, all)
		{

		}

		public IReadOnlyCollection<float> Flatten()
		{
			return new[] {this.X, this.Y, this.Z, this.W};
		}
	}
}
