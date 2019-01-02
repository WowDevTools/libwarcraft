//
//  Vector3s.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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
    /// A structure representing a 3D vector of shorts.
    /// </summary>
    public struct Vector3s : IFlattenableData<short>
    {
        /// <summary>
        /// X coordinate of this vector.
        /// </summary>
        public short X;

        /// <summary>
        /// Y coordinate of this vector.
        /// </summary>
        public short Y;

        /// <summary>
        /// Z coordinate of this vector.
        /// </summary>
        public short Z;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3s"/> struct.
        /// </summary>
        /// <param name="inX">X coordinate.</param>
        /// <param name="inY">Y coordinate.</param>
        /// <param name="inZ">Z coordinate.</param>
        public Vector3s(short inX, short inY, short inZ)
        {
            X = inX;
            Y = inY;
            Z = inZ;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3s"/> struct using
        /// normalized signed bytes instead of straight short values.
        /// </summary>
        /// <param name="inX">X.</param>
        /// <param name="inY">Y.</param>
        /// <param name="inZ">Z.</param>
        public Vector3s(sbyte inX, sbyte inY, sbyte inZ)
        {
            X = (short)(127 / inX);
            Y = (short)(127 / inY);
            Z = (short)(127 / inZ);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3s"/> struct.
        /// </summary>
        /// <param name="all">All.</param>
        public Vector3s(short all)
            : this(all, all, all)
        {
        }

        /// <summary>
        /// Computes the dot product of two vectors.
        /// </summary>
        /// <param name="start">The start vector.</param>
        /// <param name="end">The end vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static short Dot(Vector3s start, Vector3s end)
        {
            return (short)((start.X * end.X) + (start.Y * end.Y) + (start.Z * end.Z));
        }

        /// <summary>
        /// Computes the cross product of two vectors, producing a new vector which
        /// is orthogonal to the two original vectors.
        /// </summary>
        /// <param name="start">The start vector.</param>
        /// <param name="end">The end vector.</param>
        /// <returns>The cross product of the two vectors.</returns>
        public static Vector3s Cross(Vector3s start, Vector3s end)
        {
            short x = (short)((start.Y * end.Z) - (end.Y * start.Z));
            short y = (short)(((start.X * end.Z) - (end.X * start.Z)) * -1);
            short z = (short)((start.X * end.Y) - (end.X * start.Y));

            var rtnvector = new Vector3s(x, y, z);
            return rtnvector;
        }

        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="vect1">The initial vector.</param>
        /// <param name="vect2">The argument vector.</param>
        /// <returns>The two vectors added together.</returns>
        public static Vector3s operator +(Vector3s vect1, Vector3s vect2)
        {
            return new Vector3s((short)(vect1.X + vect2.X), (short)(vect1.Y + vect2.Y), (short)(vect1.Z + vect2.Z));
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="vect1">The initial vector.</param>
        /// <param name="vect2">The argument vector.</param>
        /// <returns>The two vectors subtracted from each other.</returns>
        public static Vector3s operator -(Vector3s vect1, Vector3s vect2)
        {
            return new Vector3s((short)(vect1.X - vect2.X), (short)(vect1.Y - vect2.Y), (short)(vect1.Z - vect2.Z));
        }

        /// <summary>
        /// Inverts a vector.
        /// </summary>
        /// <param name="vect1">The initial vector.</param>
        /// <returns>The initial vector in inverted form..</returns>
        public static Vector3s operator -(Vector3s vect1)
        {
            return new Vector3s((short)-vect1.X, (short)-vect1.Y, (short)-vect1.Z);
        }

        /// <summary>
        /// Divides one vector with another on a per-component basis.
        /// </summary>
        /// <param name="vect1">The initial vector.</param>
        /// <param name="vect2">The argument vector.</param>
        /// <returns>The initial vector, divided by the argument vector.</returns>
        public static Vector3s operator /(Vector3s vect1, Vector3s vect2)
        {
            return new Vector3s((short)(vect1.X / vect2.X), (short)(vect1.Y / vect2.Y), (short)(vect1.Z / vect2.Z));
        }

        /// <summary>
        /// Creates a new vector from a short, placing it in every component.
        /// </summary>
        /// <param name="i">The component short.</param>
        /// <returns>A new vector with the short as all components.</returns>
        public static implicit operator Vector3s(short i)
        {
            return new Vector3s(i);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{X}, {Y}, {Z}";
        }

        /// <inheritdoc />
        public IReadOnlyCollection<short> Flatten()
        {
            return new[] { X, Y, Z };
        }
    }
}
