//
//  ExtendedData.cs
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
using System.IO;
using System.Numerics;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.MDX.Geometry;

namespace Warcraft.Core.Extensions
{
	/// <summary>
	/// Extension methods used internally in the library for transforming data.
	/// </summary>
	public static class ExtendedData
	{
		/// <summary>
		/// Flattens a <see cref="Vector4"/> into an array of floats, similar to how the <see cref="IFlattenableData{T}"/>
		/// interface works.
		/// </summary>
		/// <param name="vector4">The vector to flatten.</param>
		/// <returns>An array of floats.</returns>
		public static float[] Flatten(this Vector4 vector4)
		{
			float[] outArr = new float[4];
			vector4.CopyTo(outArr);

			return outArr;
		}

		/// <summary>
		/// Flattens a <see cref="Vector3"/> into an array of floats, similar to how the <see cref="IFlattenableData{T}"/>
		/// interface works.
		/// </summary>
		/// <param name="vector3">The vector to flatten.</param>
		/// <returns>An array of floats.</returns>
		public static float[] Flatten(this Vector3 vector3)
		{
			float[] outArr = new float[3];
			vector3.CopyTo(outArr);

			return outArr;
		}

		/// <summary>
		/// Flattens a <see cref="Vector2"/> into an array of floats, similar to how the <see cref="IFlattenableData{T}"/>
		/// interface works.
		/// </summary>
		/// <param name="vector2">The vector to flatten.</param>
		/// <returns>An array of floats.</returns>
		public static float[] Flatten(this Vector2 vector2)
		{
			float[] outArr = new float[2];
			vector2.CopyTo(outArr);

			return outArr;
		}

		/// <summary>
		/// Packs an <see cref="MDXVertex"/> for use with an OpenGL buffer.
		/// In effect, it transforms it from Z-up to Y-up.
		/// </summary>
		/// <param name="vertex">The vertex to repack.</param>
		/// <returns>A repacked vertex.</returns>
		public static byte[] PackForOpenGL(this MDXVertex vertex)
		{
			using (MemoryStream ms = new MemoryStream())
			using (BinaryWriter bw = new BinaryWriter(ms))
			{
				bw.WriteVector3(vertex.Position, AxisConfiguration.YUp);
				bw.Write(vertex.BoneWeights.ToArray());
				bw.Write(vertex.BoneIndices.ToArray());
				bw.WriteVector3(vertex.Normal, AxisConfiguration.YUp);
				bw.WriteVector2(vertex.UV1);
				bw.WriteVector2(vertex.UV2);

				return ms.ToArray();
			}
		}

		public static float ShortQuatValueToFloat(short inShort)
		{
			return inShort / (float) Int16.MaxValue;
		}

		public static short FloatQuatValueToShort(float inFloat)
		{
			return (short)((inFloat + 1.0f) * Int16.MaxValue);
		}
	}
}