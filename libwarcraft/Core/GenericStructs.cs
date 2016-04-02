using System;
using Warcraft.Core.Interpolation;

namespace Warcraft.Core
{
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

	public struct ARGB
	{
		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public ARGB(byte InA, byte InR, byte InG, byte InB)
		{
			this.A = InA;
			this.R = InR;
			this.G = InG;
			this.B = InB;
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

