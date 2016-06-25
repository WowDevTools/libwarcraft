//
//  GenericStructs.cs
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
using Warcraft.Core.Interpolation;
using System.Collections.Generic;

namespace Warcraft.Core
{
	/// <summary>
	/// A structure representing a float range with a maximum and minimum value.
	/// </summary>
	public struct Range
	{
		/// <summary>
		/// The minimum value included in the range.
		/// </summary>
		public float Minimum
		{
			get;
			private set;
		}

		/// <summary>
		/// The maximum value included in the range.
		/// </summary>
		public float Maximum
		{
			get;
			private set;
		}

		/// <summary>
		/// Whether or not the range is inclusive - that is, if the <see cref="Minimum"/> and <see cref="Maximum"/>
		/// values are considered a part of the range.
		/// </summary>
		public bool IsInclusive
		{
			get;
			private set;
		}

		/// <summary>
		/// Creates a new <see cref="Range"/> object from a maximum and minimum value.
		/// </summary>
		/// <param name="inMin">The minimum value in the range.</param>
		/// <param name="inMax">The maximum value in the range.</param>
		/// <param name="inIsInclusive">Whether or not the range is inclusive.</param>
		/// <returns>A new <see cref="Range"/> object.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// An <see cref="ArgumentOutOfRangeException"/> can be thrown if the minimum value is greater than the maximum
		/// value.
		/// </exception>
		public Range(float inMin, float inMax, bool inIsInclusive = true)
		{
			if (!(inMin <= inMax))
			{
				throw new ArgumentOutOfRangeException(nameof(inMin), "inMin must be less than or equal to inMax");
			}

			this.Minimum = inMin;
			this.Maximum = inMax;
			this.IsInclusive = inIsInclusive;
		}
	}

	/// <summary>
	/// A structure representing an axis-aligned bounding box, comprised of two <see cref="Vector3f"/> objects
	/// defining the bottom and top corners of the box.
	/// </summary>
	public struct Box
	{
		/// <summary>
		/// The bottom corner of the bounding box.
		/// </summary>
		public Vector3f BottomCorner;

		/// <summary>
		/// The top corner of the bounding box.
		/// </summary>
		public Vector3f TopCorner;

		/// <summary>
		/// Creates a new <see cref="Box"/> object from a top and bottom corner.
		/// </summary>
		/// <param name="inBottomCorner">The bottom corner of the box.</param>
		/// <param name="inTopCorner">The top corner of the box.</param>
		/// <returns>A new <see cref="Box"/> object.</returns>
		public Box(Vector3f inBottomCorner, Vector3f inTopCorner)
		{
			this.BottomCorner = inBottomCorner;
			this.TopCorner = inTopCorner;
		}
	}

	/// <summary>
	/// A structure representing an axis-aligned sphere, comprised of a <see cref="Vector3f"/> position and a
	/// <see cref="float"/> radius.
	/// </summary>
	public struct Sphere
	{
		/// <summary>
		/// The position of the sphere in model space.
		/// </summary>
		public Vector3f Position;

		/// <summary>
		/// The radius of the sphere.
		/// </summary>
		public float Radius;

		/// <summary>
		/// Creates a new <see cref="Sphere"/> object from a position and a radius.
		/// </summary>
		/// <param name="inPosition">The sphere's position in model space.</param>
		/// <param name="inRadius">The sphere's radius.</param>
		public Sphere(Vector3f inPosition, float inRadius)
		{
			this.Position = inPosition;
			this.Radius = inRadius;
		}
	}

	/// <summary>
	/// A structure representing an RGBA colour value.
	/// </summary>
	public struct RGBA
	{
		/// <summary>
		/// The red component.
		/// </summary>
		public byte R;

		/// <summary>
		/// The green component.
		/// </summary>
		public byte G;

		/// <summary>
		/// The blue component.
		/// </summary>
		public byte B;

		/// <summary>
		/// The alpha component.
		/// </summary>
		public byte A;

		/// <summary>
		/// Creates a new <seealso cref="RGBA"/> object from a set of byte component values.
		/// </summary>
		/// <param name="inR">The input red component.</param>
		/// <param name="inG">The input blue component.</param>
		/// <param name="inB">The input green component.</param>
		/// <param name="inA">The input alpha component.</param>
		public RGBA(byte inR, byte inG, byte inB, byte inA)
		{
			this.R = inR;
			this.G = inG;
			this.B = inB;
			this.A = inA;
		}
	}

	/// <summary>
	/// A structure representing an RGB colour value.
	/// </summary>
	public struct RGB
	{
		/// <summary>
		/// The values in the structure.
		/// </summary>
		private Vector3f Values;

		/// <summary>
		/// The red component.
		/// </summary>
		public float R
		{
			get
			{
				return Values.X;
			}
			set
			{
				Values.X = value;
			}
		}

		/// <summary>
		/// The green component.
		/// </summary>
		public float G
		{
			get
			{
				return Values.Y;
			}
			set
			{
				Values.Y = value;
			}
		}

		/// <summary>
		/// The blue component.
		/// </summary>
		public float B
		{
			get
			{
				return Values.Z;
			}
			set
			{
				Values.Z = value;
			}
		}

		/// <summary>
		/// Creates a new <see cref="RGB"/> object from a set of floating point colour component
		/// values.
		/// </summary>
		/// <param name="inR">The input red component.</param>
		/// <param name="inG">The input blue component.</param>
		/// <param name="inB">The input green component.</param>
		public RGB(float inR, float inG, float inB)
		{
			this.Values = new Vector3f(inR, inG, inB);
		}

		/// <summary>
		/// Creates a new <see cref="RGB"/> object from a <see cref="Vector3f"/> colour vector.
		/// </summary>
		/// <param name="inVector">The input colour vector.</param>
		public RGB(Vector3f inVector)
		{
			this.Values = inVector;
		}
	}

	/// <summary>
	/// A structure representing a 3D vector of floats.
	/// </summary>
	public struct Vector3f
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
		/// Initializes a new instance of the <see cref="Warcraft.Core.Vector3f"/> struct using
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
		/// Initializes a new instance of the <see cref="Warcraft.Core.Vector3f"/> struct.
		/// </summary>
		/// <param name="all">All.</param>
		public Vector3f(float all)
		{
			this.X = all;
			this.Y = all;
			this.Z = all;
		}

		public static float Dot(Vector3f start, Vector3f end)
		{
			return (start.X * end.X) + (start.Y * end.Y) + (start.Z * end.Z);
		}

		public static Vector3f Cross(Vector3f start, Vector3f end)
		{
			float x = start.Y * end.Z - end.Y * start.Z;
			float y = (start.X * end.Z - end.X * start.Z) * -1;
			float z = start.X * end.Y - end.X * start.Y;

			var rtnvector = new Vector3f(x, y, z);
			return rtnvector;
		}

		public static Vector3f operator+(Vector3f vect1, Vector3f vect2)
		{
			return new Vector3f(vect1.X + vect2.X, vect1.Y + vect2.Y, vect1.Z + vect2.Z);
		}

		public static Vector3f operator-(Vector3f vect1, Vector3f vect2)
		{
			return new Vector3f(vect1.X - vect2.X, vect1.Y - vect2.Y, vect1.Z - vect2.Z);
		}

		public static Vector3f operator-(Vector3f vect1)
		{
			return new Vector3f(-vect1.X, -vect1.Y, -vect1.Z);
		}

		public static Vector3f operator/(Vector3f vect1, Vector3f vect2)
		{
			return new Vector3f(vect1.X / vect2.X, vect1.Y / vect2.Y, vect1.Z / vect2.Z);
		}

		public static implicit operator Vector3f(int i)
		{
			return new Vector3f(i);
		}
	}

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
				List<short> CoordinateRow = new List<short>();
				for (int x = 0; x < 3; ++x)
				{
					CoordinateRow.Add(inAllCoordinates);
				}
				Coordinates.Add(CoordinateRow);
			}
		}
	}

	/// <summary>
	/// A structure representing a plane in model space.
	/// </summary>
	public struct Plane
	{
		/// <summary>
		/// A normal vector perpendicular to the plane.
		/// </summary>
		public Vector3f Normal;

		/// <summary>
		/// The distance from the center of the model where the plane is.
		/// </summary>
		public float DistanceFromCenter;

		/// <summary>
		/// Creates a new <see cref="Plane"/> from a normal and a distance from the center of the model.
		/// </summary>
		public Plane(Vector3f inNormal, float inDistanceFromCenter)
		{
			this.Normal = inNormal;
			this.DistanceFromCenter = inDistanceFromCenter;
		}
	}

	/// <summary>
	/// A structure representing a three-dimensional collection of euler angles.
	/// </summary>
	public struct Rotator
	{
		/// <summary>
		/// Pitch of the rotator
		/// </summary>
		public float Pitch;

		/// <summary>
		/// Yaw of the rotator
		/// </summary>
		public float Yaw;

		/// <summary>
		/// Roll of the rotator
		/// </summary>
		public float Roll;

		/// <summary>
		/// Creates a new rotator object from three floats.
		/// </summary>
		/// <param name="inPitch">Pitch</param>
		/// <param name="inYaw">Yaw</param>
		/// <param name="inRoll">Roll</param>
		public Rotator(float inPitch, float inYaw, float inRoll)
		{
			this.Pitch = inPitch;
			this.Yaw = inYaw;
			this.Roll = inRoll;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.Core.Rotator"/> struct.
		/// </summary>
		/// <param name="inVector">In vector.</param>
		public Rotator(Vector3f inVector)
		{
			this.Pitch = inVector.X;
			this.Yaw = inVector.Y;
			this.Roll = inVector.Z;
		}
	}

	/// <summary>
	/// A structure representing a quaternion rotation (a three-dimensonal rotation with a scalar component)
	/// </summary>
	public struct Quaternion : IInterpolatable<Quaternion>
	{
		/// <summary>
		/// Pitch of the rotator
		/// </summary>
		public float X;

		/// <summary>
		/// Yaw of the rotator
		/// </summary>
		public float Y;

		/// <summary>
		/// Roll of the rotator
		/// </summary>
		public float Z;

		/// <summary>
		/// The scalar of the quaternion
		/// </summary>
		public float Scalar;

		/// <summary>
		/// Creates a new rotator object from three floats.
		/// </summary>
		/// <param name="inX">Pitch</param>
		/// <param name="inY">Yaw</param>
		/// <param name="inZ">Roll</param>
		/// <param name="inScalar">Scalar</param>
		public Quaternion(float inX, float inY, float inZ, float inScalar)
		{
			this.X = inX;
			this.Y = inY;
			this.Z = inZ;
			this.Scalar = inScalar;
		}

		// TODO: Implement quaternion interpolation.
		public Quaternion Interpolate(Quaternion Target, float Alpha, InterpolationType Type)
		{
			//return new Quaternion(0, 0, 0, 0);
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// A structure representing a two-dimensional floating point vector.
	/// </summary>
	public struct Vector2f : IInterpolatable<Vector2f>
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
		public Vector2f Interpolate(Vector2f Target, float Alpha, InterpolationType Type)
		{
			//return new Vector2f(0, 0);
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// A structure representing a four-dimensional floating point vector.
	/// </summary>
	public struct Vector4f
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
	}

	/// <summary>
	/// A structure representing a graphical resolution, consisting of two uint values.
	/// </summary>
	public struct Resolution
	{
		/// <summary>
		/// The horizontal resolution (or X resolution)
		/// </summary>
		public uint X;

		/// <summary>
		/// The vertical resolution (or Y resolution)
		/// </summary>
		public uint Y;

		/// <summary>
		/// Creates a new <see cref="Resolution"/> object from a height and a width.
		/// </summary>
		/// <param name="inX">The input width component.</param>
		/// <param name="inY">The input height component.</param>
		public Resolution(uint inX, uint inY)
		{
			this.X = inX;
			this.Y = inY;
		}

		/// <summary>
		/// Creates a new <see cref="Resolution"/> object from a single input uint, filling all components.
		/// </summary>
		/// <param name="all">The input component.</param>
		public Resolution(uint all)
			:this(all, all)
		{

		}

		public override string ToString()
		{
			return $"{X}x{Y}";
		}
	}
}

