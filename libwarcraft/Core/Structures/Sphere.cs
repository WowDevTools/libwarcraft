//
//  Sphere.cs
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

using System.Numerics;

namespace Warcraft.Core.Structures
{
	/// <summary>
	/// A structure representing an axis-aligned sphere, comprised of a <see cref="Vector3"/> position and a
	/// <see cref="float"/> radius.
	/// </summary>
	public struct Sphere
	{
		/// <summary>
		/// The position of the sphere in model space.
		/// </summary>
		public Vector3 Position;

		/// <summary>
		/// The radius of the sphere.
		/// </summary>
		public float Radius;

		/// <summary>
		/// Creates a new <see cref="Sphere"/> object from a position and a radius.
		/// </summary>
		/// <param name="inPosition">The sphere's position in model space.</param>
		/// <param name="inRadius">The sphere's radius.</param>
		public Sphere(Vector3 inPosition, float inRadius)
		{
			this.Position = inPosition;
			this.Radius = inRadius;
		}
	}
}
