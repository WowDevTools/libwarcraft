//
//  Vector3f.cs
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
	/// A structure representing a 3D vector of floats.
	/// </summary>
	public struct Vector3f : IFlattenableData<float>
	{
		/// <summary>
		/// X coordinate of this vector
		/// </summary>
		public float X;

		/// <summary>
		/// Y coordinate of this vector
		/// </summary>
		public float Y;

		/// <summary>
		/// Z coordinate of this vector
		/// </summary>
		public float Z;

		/// <summary>
		/// Creates a new 3D vector object from three floats.
		/// </summary>
		/// <param name="inX">X coordinate.</param>
		/// <param name="inY">Y coordinate.</param>
		/// <param name="inZ">Z coordinate.</param>
		public Vector3f(float inX, float inY, float inZ)
		{
			this.X = inX;
			this.Y = inY;
			this.Z = inZ;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector3f"/> struct using
		/// normalized signed bytes instead of straight floating-point values.
		/// </summary>
		/// <param name="inX">X.</param>
		/// <param name="inY">Y.</param>
		/// <param name="inZ">Z.</param>
		public Vector3f(sbyte inX, sbyte inY, sbyte inZ)
		{
			this.X = 127 / inX;
			this.Y = 127 / inY;
			this.Z = 127 / inZ;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Vector3f"/> struct.
		/// </summary>
		/// <param name="all">All.</param>
		public Vector3f(float all)
			:this(all, all, all)
		{

		}

		/// <summary>
		/// Computes the dot product of two vectors.
		/// </summary>
		/// <param name="start">The start vector.</param>
		/// <param name="end">The end vector.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public static float Dot(Vector3f start, Vector3f end)
		{
			return (start.X * end.X) + (start.Y * end.Y) + (start.Z * end.Z);
		}

		/// <summary>
		/// Computes the cross product of two vectors, producing a new vector which
		/// is orthogonal to the two original vectors.
		/// </summary>
		/// <param name="start">The start vector.</param>
		/// <param name="end">The end vector.</param>
		/// <returns>The cross product of the two vectors.</returns>
		public static Vector3f Cross(Vector3f start, Vector3f end)
		{
			float x = start.Y * end.Z - end.Y * start.Z;
			float y = (start.X * end.Z - end.X * start.Z) * -1;
			float z = start.X * end.Y - end.X * start.Y;

			var rtnvector = new Vector3f(x, y, z);
			return rtnvector;
		}

		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		/// <param name="vect1">The initial vector.</param>
		/// <param name="vect2">The argument vector.</param>
		/// <returns>The two vectors added together.</returns>
		public static Vector3f operator+(Vector3f vect1, Vector3f vect2)
		{
			return new Vector3f(vect1.X + vect2.X, vect1.Y + vect2.Y, vect1.Z + vect2.Z);
		}

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="vect1">The initial vector.</param>
		/// <param name="vect2">The argument vector.</param>
		/// <returns>The two vectors subtracted from each other.</returns>
		public static Vector3f operator-(Vector3f vect1, Vector3f vect2)
		{
			return new Vector3f(vect1.X - vect2.X, vect1.Y - vect2.Y, vect1.Z - vect2.Z);
		}

		/// <summary>
		/// Inverts a vector.
		/// </summary>
		/// <param name="vect1">The initial vector.</param>
		/// <returns>The initial vector in inverted form..</returns>
		public static Vector3f operator-(Vector3f vect1)
		{
			return new Vector3f(-vect1.X, -vect1.Y, -vect1.Z);
		}

		/// <summary>
		/// Divides one vector with another on a per-component basis.
		/// </summary>
		/// <param name="vect1">The initial vector.</param>
		/// <param name="vect2">The argument vector.</param>
		/// <returns>The initial vector, divided by the argument vector.</returns>
		public static Vector3f operator/(Vector3f vect1, Vector3f vect2)
		{
			return new Vector3f(vect1.X / vect2.X, vect1.Y / vect2.Y, vect1.Z / vect2.Z);
		}

		/// <summary>
		/// Creates a new vector from an integer, placing it in every component.
		/// </summary>
		/// <param name="i">The component integer.</param>
		/// <returns>A new vector with the integer as all components.</returns>
		public static implicit operator Vector3f(int i)
		{
			return new Vector3f(i);
		}

		/// <summary>
		/// Creates a string representation of the current instance.
		/// </summary>
		/// <returns>A string representation of the current instance.</returns>
		public override string ToString()
		{
			return $"{this.X}, {this.Y}, {this.Z}";
		}

		public IReadOnlyCollection<float> Flatten()
		{
			return new[] {this.X, this.Y, this.Z};
		}
	}
}
