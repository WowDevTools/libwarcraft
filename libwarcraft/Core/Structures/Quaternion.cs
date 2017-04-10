//
//  Quaternion.cs
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
	/// A structure representing a quaternion rotation (a three-dimensonal rotation with a scalar component)
	/// </summary>
	public struct Quaternion : IInterpolatable<Quaternion>, IFlattenableData<float>
	{
		private Vector4f Values;

		/// <summary>
		/// Pitch of the rotator
		/// </summary>
		public float X
		{
			get { return this.Values.X; }
			set { this.Values.X = value; }
		}

		/// <summary>
		/// Yaw of the rotator
		/// </summary>
		public float Y
		{
			get { return this.Values.Y; }
			set { this.Values.Y = value; }
		}

		/// <summary>
		/// Roll of the rotator
		/// </summary>
		public float Z
		{
			get { return this.Values.Z; }
			set { this.Values.Z = value; }
		}

		/// <summary>
		/// The scalar of the quaternion
		/// </summary>
		public float Scalar
		{
			get { return this.Values.W; }
			set { this.Values.W = value; }
		}

		/// <summary>
		/// Creates a new rotator object from three floats.
		/// </summary>
		/// <param name="inX">Pitch</param>
		/// <param name="inY">Yaw</param>
		/// <param name="inZ">Roll</param>
		/// <param name="inScalar">Scalar</param>
		public Quaternion(float inX, float inY, float inZ, float inScalar)
		{
			this.Values = new Vector4f(inX, inY, inZ, inScalar);
		}

		// TODO: Implement quaternion interpolation.
		/// <summary>
		/// Interpolates the instance between itself and the <paramref name="target"/> object by an alpha factor,
		/// using the interpolation algorithm specified in <paramref name="interpolationType"/>.
		/// </summary>
		/// <param name="target">The target point.</param>
		/// <param name="alpha">The alpha factor.</param>
		/// <param name="interpolationType">The interpolation algorithm to use.</param>
		/// <returns>An interpolated object.</returns>
		public Quaternion Interpolate(Quaternion target, float alpha, InterpolationType interpolationType)
		{
			throw new NotImplementedException();
		}

		public IReadOnlyCollection<float> Flatten()
		{
			return this.Values.Flatten();
		}
	}
}
