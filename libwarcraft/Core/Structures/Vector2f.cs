//
//  Vector2f.cs
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
using Warcraft.Core.Interfaces;
using Warcraft.Core.Interpolation;

namespace Warcraft.Core.Structures
{
	/// <summary>
	/// A structure representing a two-dimensional floating point vector.
	/// </summary>
	public struct Vector2f : IInterpolatable<Vector2f>, IFlattenableData<float>
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
		/// Creates a new <see cref="Vector2f"/> object from two input floats.
		/// </summary>
		/// <param name="inX">The input X component.</param>
		/// <param name="inY">The input Y component.</param>
		public Vector2f(float inX, float inY)
		{
			this.X = inX;
			this.Y = inY;
		}

		/// <summary>
		/// Creates a new <see cref="Vector4f"/> object from a single input float, filling all components.
		/// </summary>
		/// <param name="all">The input component.</param>
		public Vector2f(float all)
			:this(all, all)
		{

		}

		// TODO: Implement vector2f interpolation.
		/// <summary>
		/// Interpolates the instance between itself and the <paramref name="target"/> object by an alpha factor,
		/// using the interpolation algorithm specified in <paramref name="interpolationType"/>.
		/// </summary>
		/// <param name="target">The target point.</param>
		/// <param name="alpha">The alpha factor.</param>
		/// <param name="interpolationType">The interpolation algorithm to use.</param>
		/// <returns>An interpolated object.</returns>
		public Vector2f Interpolate(Vector2f target, float alpha, InterpolationType interpolationType)
		{
			//return new Vector2f(0, 0);
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a string representation of the current object.
		/// </summary>
		/// <returns>A string representation of the current object.</returns>
		public override string ToString()
		{
			return $"{this.X}, {this.Y}";
		}

		public IReadOnlyCollection<float> Flatten()
		{
			return new[] {this.X, this.Y};
		}
	}
}
