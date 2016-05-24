using System;
using Warcraft.Core.Interpolation;
using System.Collections.Generic;

namespace Warcraft.Core
{
	public struct Range
	{
		public float Minimum
		{
			get;
			private set;
		}

		public float Maximum
		{
			get;
			private set;
		}

		public Range(float InMin, float InMax)
		{
			if (!(InMin <= InMax))
			{
				throw new ArgumentOutOfRangeException("InMin", "InMin must be less than or equal to InMax");
			}

			this.Minimum = InMin;
			this.Maximum = InMax;
		}
	}
	public struct Box
	{
		public Vector3f BottomCorner;
		public Vector3f TopCorner;

		public Box(Vector3f InBottomCorner, Vector3f InTopCorner)
		{
			this.BottomCorner = InBottomCorner;
			this.TopCorner = InTopCorner;
		}
	}

	public struct Sphere
	{
		public Vector3f Position;
		public float Radius;

		public Sphere(Vector3f InPosition, float InRadius)
		{
			this.Position = InPosition;
			this.Radius = InRadius;
		}
	}

	public struct RGBA
	{
		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public RGBA(byte InA, byte InR, byte InG, byte InB)
		{
			this.A = InA;
			this.R = InR;
			this.G = InG;
			this.B = InB;
		}
	}

	public struct RGB
	{
		private Vector3f Values;

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

		public RGB(float R, float G, float B)
		{
			this.Values = new Vector3f(R, G, B);
		}

		public RGB(Vector3f InValues)
		{
			this.Values = InValues;
		}
	}

	/// <summary>
	/// A 3D vector of floats.
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
		/// <param name="X">X coordinate</param>
		/// <param name="Y">Y coordinate</param>
		/// <param name="Z">Z coordinate</param>
		public Vector3f(float X, float Y, float Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.Core.Vector3f"/> struct using
		/// normalized signed bytes instead of straight floating-point values.
		/// </summary>
		/// <param name="X">X.</param>
		/// <param name="Y">Y.</param>
		/// <param name="Z">Z.</param>
		public Vector3f(sbyte X, sbyte Y, sbyte Z)
		{
			this.X = 127 / X;
			this.Y = 127 / Y;
			this.Z = 127 / Z;
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

	public struct Plane
	{
		public List<List<short>> Coordinates;

		public Plane(short InAllCoordinates)
		{
			this.Coordinates = new List<List<short>>();

			for (int y = 0; y < 3; ++y)
			{
				List<short> CoordinateRow = new List<short>();
				for (int x = 0; x < 3; ++x)
				{
					CoordinateRow.Add(InAllCoordinates);
				}
				Coordinates.Add(CoordinateRow);
			}
		}
	}

	/// <summary>
	/// A 3D rotator of floats.
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
		/// <param name="Pitch">Pitch</param>
		/// <param name="Yaw">Yaw</param>
		/// <param name="Roll">Roll</param>
		public Rotator(float Pitch, float Yaw, float Roll)
		{
			this.Pitch = Pitch;
			this.Yaw = Yaw;
			this.Roll = Roll;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.Core.Rotator"/> struct.
		/// </summary>
		/// <param name="InVector">In vector.</param>
		public Rotator(Vector3f InVector)
		{
			this.Pitch = InVector.X;
			this.Yaw = InVector.Y;
			this.Roll = InVector.Z;
		}
	}

	/// <summary>
	/// A 3D rotator of floats.
	/// </summary>
	public struct Quaternion : IInterpolatable<Quaternion>
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
		/// The scalar of the quaternion
		/// </summary>
		public float Scalar;

		/// <summary>
		/// Creates a new rotator object from three floats.
		/// </summary>
		/// <param name="Pitch">Pitch</param>
		/// <param name="Yaw">Yaw</param>
		/// <param name="Roll">Roll</param>
		/// <param name="Scalar">Scalar</param>
		public Quaternion(float Pitch, float Yaw, float Roll, float Scalar)
		{
			this.Pitch = Pitch;
			this.Yaw = Yaw;
			this.Roll = Roll;
			this.Scalar = Scalar;
		}

		public Quaternion Interpolate(Quaternion Target, float Alpha, InterpolationType Type)
		{
			return new Quaternion(0, 0, 0, 0);
		}
	}

	public struct Vector2f : IInterpolatable<Vector2f>
	{
		public float X;
		public float Y;

		public Vector2f(float X, float Y)
		{
			this.X = X;
			this.Y = Y;
		}

		public Vector2f Interpolate(Vector2f Target, float Alpha, InterpolationType Type)
		{
			return new Vector2f(0, 0);
		}
	}

	public struct Vector4f
	{
		public float X;
		public float Y;
		public float Z;
		public float W;

		public Vector4f(float X, float Y, float Z, float W)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
			this.W = W;
		}

		public Vector4f(float all)
		{
			this.X = all;
			this.Y = all;
			this.Z = all;
			this.W = all;
		}
	}

	public struct Resolution
	{
		public uint X;
		public uint Y;

		public Resolution(uint X, uint Y)
		{
			this.X = X;
			this.Y = Y;
		}

		public override string ToString()
		{
			return string.Format("{0}x{1}", X, Y);
		}
	}
}

