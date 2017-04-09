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
using Warcraft.Core.Interfaces;

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

		/// <summary>
		/// Creates a string representation of the current instance.
		/// </summary>
		/// <returns>A string representation of the current instance.</returns>
		public override string ToString()
		{
			return $"Range: {this.Minimum} to {this.Maximum}";
		}
	}

	/// <summary>
	/// A structure representing an axis-aligned bounding box, comprised of two <see cref="Vector3f"/> objects
	/// defining the bottom and top corners of the box.
	/// </summary>
	public struct Box : IFlattenableData<float>
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

		/// <summary>
		/// Gets the coordinates of the center of the box.
		/// </summary>
		/// <returns>A vector with the coordinates of the center of the box.</returns>
		public Vector3f GetCenterCoordinates()
		{
			return (this.BottomCorner + this.TopCorner) / 2;
		}
	}

	/// <summary>
	/// A structure representing an axis-aligned bounding box, comprised of two <see cref="Vector3s"/> objects
	/// defining the bottom and top corners of the box.
	/// </summary>
	public struct ShortBox : IFlattenableData<short>
	{
		/// <summary>
		/// The bottom corner of the bounding box.
		/// </summary>
		public Vector3s BottomCorner;

		/// <summary>
		/// The top corner of the bounding box.
		/// </summary>
		public Vector3s TopCorner;

		/// <summary>
		/// Creates a new <see cref="Box"/> object from a top and bottom corner.
		/// </summary>
		/// <param name="inBottomCorner">The bottom corner of the box.</param>
		/// <param name="inTopCorner">The top corner of the box.</param>
		/// <returns>A new <see cref="Box"/> object.</returns>
		public ShortBox(Vector3s inBottomCorner, Vector3s inTopCorner)
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
		/// Creates a new <see cref="RGBA"/> object from a set of byte component values.
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

		/// <summary>
		/// Creates a new <see cref="RGBA"/> object from a byte that fills all components.
		/// </summary>
		/// <param name="all">The input byte component.</param>
		public RGBA(byte all)
		:this(all, all, all, all)
		{

		}

		/// <summary>
		/// Creates a string representation of the current instance.
		/// </summary>
		/// <returns>A string representation of the current instance.</returns>
		public override string ToString()
		{
			return $"rgba({this.R}, {this.G}, {this.B}, {this.A})";
		}
	}

	/// <summary>
	/// A structure representing an BGRA colour value.
	/// </summary>
	public struct BGRA
	{
		/// <summary>
		/// The red component.
		/// </summary>
		public byte B;

		/// <summary>
		/// The green component.
		/// </summary>
		public byte G;

		/// <summary>
		/// The blue component.
		/// </summary>
		public byte R;

		/// <summary>
		/// The alpha component.
		/// </summary>
		public byte A;

		/// <summary>
		/// Creates a new <see cref="BGRA"/> object from a set of byte component values.
		/// </summary>
		/// <param name="inG">The input blue component.</param>
		/// <param name="inB">The input green component.</param>
		/// <param name="inR">The input red component.</param>
		/// <param name="inA">The input alpha component.</param>
		public BGRA(byte inB, byte inG, byte inR, byte inA)
		{
			this.B = inB;
			this.G = inG;
			this.R = inR;
			this.A = inA;
		}

		/// <summary>
		/// Creates a new <see cref="BGRA"/> object from a byte that fills all components.
		/// </summary>
		/// <param name="all">The input byte component.</param>
		public BGRA(byte all)
		:this(all, all, all, all)
		{

		}

		/// <summary>
		/// Creates a string representation of the current instance.
		/// </summary>
		/// <returns>A string representation of the current instance.</returns>
		public override string ToString()
		{
			return $"bgra({this.B}, {this.G}, {this.R}, {this.A})";
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
				return this.Values.X;
			}
			set
			{
				this.Values.X = value;
			}
		}

		/// <summary>
		/// The green component.
		/// </summary>
		public float G
		{
			get
			{
				return this.Values.Y;
			}
			set
			{
				this.Values.Y = value;
			}
		}

		/// <summary>
		/// The blue component.
		/// </summary>
		public float B
		{
			get
			{
				return this.Values.Z;
			}
			set
			{
				this.Values.Z = value;
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

		/// <summary>
		/// Creates a string representation of the current instance.
		/// </summary>
		/// <returns>A string representation of the current instance.</returns>
		public override string ToString()
		{
			return $"rgb({this.R}, {this.G}, {this.B})";
		}
	}

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

	/// <summary>
	/// A structure representing a 3D vector of shorts.
	/// </summary>
	public struct Vector3s : IFlattenableData<short>
	{
		/// <summary>
		/// X coordinate of this vector
		/// </summary>
		public short X;

		/// <summary>
		/// Y coordinate of this vector
		/// </summary>
		public short Y;

		/// <summary>
		/// Z coordinate of this vector
		/// </summary>
		public short Z;

		/// <summary>
		/// Creates a new 3D vector object from three short.
		/// </summary>
		/// <param name="inX">X coordinate.</param>
		/// <param name="inY">Y coordinate.</param>
		/// <param name="inZ">Z coordinate.</param>
		public Vector3s(short inX, short inY, short inZ)
		{
			this.X = inX;
			this.Y = inY;
			this.Z = inZ;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.Core.Vector3s"/> struct using
		/// normalized signed bytes instead of straight short values.
		/// </summary>
		/// <param name="inX">X.</param>
		/// <param name="inY">Y.</param>
		/// <param name="inZ">Z.</param>
		public Vector3s(sbyte inX, sbyte inY, sbyte inZ)
		{
			this.X = (short)(127 / inX);
			this.Y = (short)(127 / inY);
			this.Z = (short)(127 / inZ);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.Core.Vector3s"/> struct.
		/// </summary>
		/// <param name="all">All.</param>
		public Vector3s(short all)
			:this(all, all, all)
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
			short x = (short)(start.Y * end.Z - end.Y * start.Z);
			short y = (short)((start.X * end.Z - end.X * start.Z) * -1);
			short z = (short)(start.X * end.Y - end.X * start.Y);

			var rtnvector = new Vector3s(x, y, z);
			return rtnvector;
		}

		/// <summary>
		/// Adds two vectors together.
		/// </summary>
		/// <param name="vect1">The initial vector.</param>
		/// <param name="vect2">The argument vector.</param>
		/// <returns>The two vectors added together.</returns>
		public static Vector3s operator+(Vector3s vect1, Vector3s vect2)
		{
			return new Vector3s((short)(vect1.X + vect2.X), (short)(vect1.Y + vect2.Y), (short)(vect1.Z + vect2.Z));
		}

		/// <summary>
		/// Subtracts two vectors.
		/// </summary>
		/// <param name="vect1">The initial vector.</param>
		/// <param name="vect2">The argument vector.</param>
		/// <returns>The two vectors subtracted from each other.</returns>
		public static Vector3s operator-(Vector3s vect1, Vector3s vect2)
		{
			return new Vector3s((short)(vect1.X - vect2.X), (short)(vect1.Y - vect2.Y), (short)(vect1.Z - vect2.Z));
		}

		/// <summary>
		/// Inverts a vector.
		/// </summary>
		/// <param name="vect1">The initial vector.</param>
		/// <returns>The initial vector in inverted form..</returns>
		public static Vector3s operator-(Vector3s vect1)
		{
			return new Vector3s((short)-vect1.X, (short)-vect1.Y, (short)-vect1.Z);
		}

		/// <summary>
		/// Divides one vector with another on a per-component basis.
		/// </summary>
		/// <param name="vect1">The initial vector.</param>
		/// <param name="vect2">The argument vector.</param>
		/// <returns>The initial vector, divided by the argument vector.</returns>
		public static Vector3s operator/(Vector3s vect1, Vector3s vect2)
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

		/// <summary>
		/// Creates a string representation of the current instance.
		/// </summary>
		/// <returns>A string representation of the current instance.</returns>
		public override string ToString()
		{
			return $"{this.X}, {this.Y}, {this.Z}";
		}

		public IReadOnlyCollection<short> Flatten()
		{
			return new[] {this.X, this.Y, this.Z};
		}
	}

	/// <summary>
	/// An axis configuration, that is, how vector data should be interpreted.
	/// </summary>
	public enum AxisConfiguration
	{
		/// <summary>
		/// No assumptions should be made about the vector storage format, and should be read as XYZ.
		/// </summary>
		Native,

		/// <summary>
		/// Assume that the data is stored as Y-up.
		/// </summary>
		YUp,

		/// <summary>
		/// Assume that the data is stored as Z-up.
		/// </summary>
		ZUp
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
				List<short> coordinateRow = new List<short>();
				for (int x = 0; x < 3; ++x)
				{
					coordinateRow.Add(inAllCoordinates);
				}
				this.Coordinates.Add(coordinateRow);
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
	public struct Rotator : IFlattenableData<float>
	{
		private Vector3f Values;

		/// <summary>
		/// Pitch of the rotator
		/// </summary>
		public float Pitch
		{
			get { return this.Values.X; }
			set { this.Values.X = value; }
		}

		/// <summary>
		/// Yaw of the rotator
		/// </summary>
		public float Yaw
		{
			get { return this.Values.Y; }
			set { this.Values.Y = value; }
		}

		/// <summary>
		/// Roll of the rotator
		/// </summary>
		public float Roll
		{
			get { return this.Values.Z; }
			set { this.Values.Z = value; }
		}

		/// <summary>
		/// Creates a new rotator object from three floats.
		/// </summary>
		/// <param name="inPitch">Pitch</param>
		/// <param name="inYaw">Yaw</param>
		/// <param name="inRoll">Roll</param>
		public Rotator(float inPitch, float inYaw, float inRoll)
		{
			this.Values = new Vector3f(inPitch, inYaw, inRoll);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.Core.Rotator"/> struct.
		/// </summary>
		/// <param name="inVector">In vector.</param>
		public Rotator(Vector3f inVector)
			:this(inVector.X, inVector.Y, inVector.Z)
		{

		}

		/// <summary>
		/// Creates a string representation of the current instance.
		/// </summary>
		/// <returns>A string representation of the current instance.</returns>
		public override string ToString()
		{
			return $"Pitch: {this.Pitch}, Yaw: {this.Yaw}, Roll: {this.Roll}";
		}

		public IReadOnlyCollection<float> Flatten()
		{
			return this.Values.Flatten();
		}
	}

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

		/// <summary>
		/// Creates a string representation of the current object.
		/// </summary>
		/// <returns>A string representation of the current object.</returns>
		public override string ToString()
		{
			return $"{this.X}x{this.Y}";
		}
	}
}

