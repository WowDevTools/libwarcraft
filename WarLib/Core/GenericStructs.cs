using System;

namespace WarLib.Core
{
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

	public struct Vector2f
	{
		public float X;
		public float Y;

		public Vector2f(float X, float Y)
		{
			this.X = X;
			this.Y = Y;
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

