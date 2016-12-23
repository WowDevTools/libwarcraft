//
//  ExtensionMethods.cs
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
using System.Drawing;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Warcraft.Core.Interfaces;

namespace Warcraft.Core
{
	public static class ExtensionMethods
	{
		public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0)
			{
				return min;
			}
			else if (val.CompareTo(max) > 0)
			{
				return max;
			}
			else
			{
				return val;
			}
		}

		// Taken from the Arduino reference (https://www.arduino.cc/en/Reference/Map)
		public static int Map(int val, int in_min, int in_max, int out_min, int out_max)
		{
			return (val - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
		}

		private static float ShortQuatValueToFloat(short inShort)
		{
			return (float) (inShort / (float) short.MaxValue);
		}

		private static short FloatQuatValueToShort(float inFloat)
		{
			return (short)((inFloat + 1.0f) * (float)short.MaxValue);
		}

		public static Bitmap Invert(this Bitmap map, bool KeepAlpha = true)
		{
			Bitmap outMap = new Bitmap(map);

			for (int y = 0; y < map.Height; ++y)
			{
				for (int x = 0; x < map.Width; ++x)
				{
					Color pixel = map.GetPixel(x, y);
					byte pixelAlpha = pixel.A;
					if (!KeepAlpha)
					{
						pixelAlpha = (byte)(255 - pixel.A);
					}

					Color negativePixel = Color.FromArgb(pixelAlpha, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
					//Color negativePixel = Color.FromArgb(pixelAlpha, 255 - pixel.G, 255 - pixel.B, 255 - pixel.A);

					outMap.SetPixel(x, y, negativePixel);
				}
			}

			return outMap;
		}

		public static bool HasAlpha(this Bitmap map)
		{
			for (int y = 0; y < map.Height; ++y)
			{
				for (int x = 0; x < map.Width; ++x)
				{
					Color pixel = map.GetPixel(x, y);
					if (pixel.A != 255)
					{
						return true;
					}
				}
			}
			return false;
		}

		/*
			Binary Reader Extensions for standard typess
		*/

		/// <summary>
		/// Reads an 8-byte Range value from the data stream.
		/// </summary>
		/// <returns>The range.</returns>
		/// <param name="Reader">Reader.</param>
		public static Range ReadRange(this BinaryReader Reader)
		{
			return new Range(Reader.ReadSingle(), Reader.ReadSingle());
		}

		/// <summary>
		/// Reads a 4-byte RGBA value from the data stream.
		/// </summary>
		/// <returns>The argument.</returns>
		/// <param name="Reader">Reader.</param>
		public static RGBA ReadRGBA(this BinaryReader Reader)
		{
			byte R = Reader.ReadByte();
			byte G = Reader.ReadByte();
			byte B = Reader.ReadByte();
			byte A = Reader.ReadByte();

			RGBA rgba = new RGBA(R, G, B, A);

			return rgba;
		}

		/// <summary>
		/// Reads a 4-byte RGBA value from the data stream.
		/// </summary>
		/// <returns>The argument.</returns>
		/// <param name="Reader">Reader.</param>
		public static BGRA ReadBGRA(this BinaryReader Reader)
		{
			byte B = Reader.ReadByte();
			byte G = Reader.ReadByte();
			byte R = Reader.ReadByte();
			byte A = Reader.ReadByte();

			BGRA bgra = new BGRA(B, G, R, A);

			return bgra;
		}

		/// <summary>
		/// Reads a standard null-terminated string from the data stream.
		/// </summary>
		/// <returns>The null terminated string.</returns>
		/// <param name="Reader">Reader.</param>
		public static string ReadNullTerminatedString(this BinaryReader Reader)
		{
			StringBuilder sb = new StringBuilder();

			char c;
			while ((c = Reader.ReadChar()) != 0)
			{
				sb.Append(c);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Reads a 4-byte RIFF chunk signature from the data stream.
		/// </summary>
		/// <returns>The signature as a string.</returns>
		public static string ReadChunkSignature(this BinaryReader Reader)
		{
			// The signatures are stored in reverse in the file, so we'll need to read them backwards into
			// the buffer.
			char[] signatureBuffer = new char[4];
			for (int i = 0; i < 4; ++i)
			{
				signatureBuffer[3 - i] = Reader.ReadChar();
			}

			string signature = new string(signatureBuffer);
			return signature;
		}

		/// <summary>
		/// Peeks a 4-byte RIFF chunk signature from the data stream. This does not
		/// advance the position of the stream.
		/// </summary>
		public static string PeekChunkSignature(this BinaryReader Reader)
		{
			string chunkSignature = Reader.ReadChunkSignature();
			Reader.BaseStream.Position -= chunkSignature.Length;

			return chunkSignature;
		}

		/// <summary>
		/// Reads an RIFF-style chunk from the stream. The chunk must have the <see cref="IRIFFChunk"/>
		/// interface, and implement a parameterless constructor.
		/// </summary>
		public static T ReadIFFChunk<T>(this BinaryReader Reader) where T : IRIFFChunk, new()
		{
			string chunkSignature = Reader.ReadChunkSignature();
			uint chunkSize = Reader.ReadUInt32();
			byte[] chunkData = Reader.ReadBytes((int)chunkSize);

			T chunk = new T();
			if (chunk.GetSignature() != chunkSignature)
			{
				throw new InvalidChunkSignatureException($"An unknown chunk with the signature \"{chunkSignature}\" was read.");
			}

			chunk.LoadBinaryData(chunkData);

			return chunk;
		}

		/// <summary>
		/// Reads an 16-byte <see cref="Plane"/> from the data stream.
		/// </summary>
		/// <returns>The plane.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static Plane ReadPlane(this BinaryReader Reader)
		{
			return new Plane(Reader.ReadVector3f(), Reader.ReadSingle());
		}

		/// <summary>
		/// Reads an 18-byte <see cref="ShortPlane"/> from the data stream.
		/// </summary>
		/// <returns>The plane.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static ShortPlane ReadShortPlane(this BinaryReader Reader)
		{
			ShortPlane shortPlane = new ShortPlane();
			for (int y = 0; y < 3; ++y)
			{
				List<short> CoordinateRow = new List<short>();
				for (int x = 0; x < 3; ++x)
				{
					CoordinateRow.Add(Reader.ReadInt16());
				}
				shortPlane.Coordinates.Add(CoordinateRow);
			}

			return shortPlane;
		}

		/// <summary>
		/// Reads a 16-byte 32-bit <see cref="Quaternion"/> structure from the data stream, and advances the position of the stream by
		/// 16 bytes.
		/// </summary>
		/// <returns>The quaternion.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static Quaternion ReadQuaternion32(this BinaryReader Reader)
		{
			return new Quaternion(Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
		}

		/// <summary>
		/// Reads a 8-byte 16-bit <see cref="Quaternion"/> structure from the data stream, and advances the position of the stream by
		/// 8 bytes.
		/// </summary>
		/// <returns>The quaternion.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static Quaternion ReadQuaternion16(this BinaryReader Reader)
		{
			short x = Reader.ReadInt16();
			short y = Reader.ReadInt16();
			short z = Reader.ReadInt16();
			short w = Reader.ReadInt16();
			return new Quaternion(ShortQuatValueToFloat(x), ShortQuatValueToFloat(y), ShortQuatValueToFloat(z), ShortQuatValueToFloat(w));
		}

		/// <summary>
		/// Reads a 12-byte <see cref="Rotator"/> from the data stream and advances the position of the stream by
		/// 12 bytes.
		/// </summary>
		/// <returns>The rotator.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static Rotator ReadRotator(this BinaryReader Reader)
		{
			return new Rotator(Reader.ReadVector3f());
		}

		/// <summary>
		/// Reads a 12-byte <see cref="Vector3f"/> structure from the data stream and advances the position of the stream by
		/// 12 bytes.
		/// </summary>
		/// <returns>The vector3f.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static Vector3f ReadVector3f(this BinaryReader Reader, AxisConfiguration convertTo = AxisConfiguration.YUp)
		{
			switch (convertTo)
			{
				case AxisConfiguration.Native:
				{
					return new Vector3f(Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
				}
				case AxisConfiguration.YUp:
				{
					float X = Reader.ReadSingle();
					float Z = Reader.ReadSingle();
					float Y = Reader.ReadSingle() * -1.0f;

					return new Vector3f(X, Y, Z);
				}
				case AxisConfiguration.ZUp:
				{
					float X = Reader.ReadSingle();
					float Z = Reader.ReadSingle() * -1.0f;
					float Y = Reader.ReadSingle();

					return new Vector3f(X, Y, Z);
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(convertTo), convertTo, null);
				}
			}
		}

		/// <summary>
		/// Reads a 16-byte <see cref="Vector4f"/> structure from the data stream and advances the position of the stream by
		/// 16 bytes.
		/// </summary>
		/// <returns>The vector3f.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static Vector4f ReadVector4f(this BinaryReader Reader, AxisConfiguration convertTo = AxisConfiguration.YUp)
		{
			switch (convertTo)
			{
				case AxisConfiguration.Native:
				{
					return new Vector4f(Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
				}
				case AxisConfiguration.YUp:
				{
					float X = Reader.ReadSingle();
					float Z = Reader.ReadSingle();
					float Y = Reader.ReadSingle() * -1.0f;

					float W = Reader.ReadSingle();

					return new Vector4f(X, Y, Z, W);
				}
				case AxisConfiguration.ZUp:
				{
					float X = Reader.ReadSingle();
					float Z = Reader.ReadSingle() * -1.0f;
					float Y = Reader.ReadSingle();

					float W = Reader.ReadSingle();

					return new Vector4f(X, Y, Z, W);
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(convertTo), convertTo, null);
				}
			}
		}

		/// <summary>
		/// Reads a 6-byte <see cref="Vector3s"/> structure from the data stream and advances the position of the stream by
		/// 6 bytes.
		/// </summary>
		/// <returns>The vector3s.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static Vector3s ReadVector3s(this BinaryReader Reader, AxisConfiguration convertTo = AxisConfiguration.YUp)
		{
			switch (convertTo)
			{
				case AxisConfiguration.Native:
				{
					return new Vector3s(Reader.ReadInt16(), Reader.ReadInt16(), Reader.ReadInt16());
				}
				case AxisConfiguration.YUp:
				{
					short X = Reader.ReadInt16();
					short Z = Reader.ReadInt16();
					short Y = (short)(Reader.ReadInt16() * -1);

					return new Vector3s(X, Y, Z);
				}
				case AxisConfiguration.ZUp:
				{
					short X = Reader.ReadInt16();
					short Z = (short)(Reader.ReadInt16() * -1);
					short Y = Reader.ReadInt16();

					return new Vector3s(X, Y, Z);
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(convertTo), convertTo, null);
				}
			}
		}

		/// <summary>
		/// Reads an 8-byte <see cref="Vector2f"/> structure from the data stream and advances the position of the stream by
		/// 8 bytes.
		/// </summary>
		/// <returns>The vector2f.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static Vector2f ReadVector2f(this BinaryReader Reader)
		{
			return new Vector2f(Reader.ReadSingle(), Reader.ReadSingle());
		}

		/// <summary>
		/// Reads a 24-byte <see cref="Box"/> structure from the data stream and advances the position of the stream by
		/// 24 bytes.
		/// </summary>
		/// <returns>The box.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static Box ReadBox(this BinaryReader Reader)
		{
			return new Box(Reader.ReadVector3f(), Reader.ReadVector3f());
		}

		/// <summary>
		/// Reads a 12-byte <see cref="Box"/> structure from the data stream and advances the position of the stream by
		/// 12 bytes.
		/// </summary>
		/// <returns>The box.</returns>
		/// <param name="Reader">The current <see cref="BinaryReader"/></param>
		public static ShortBox ReadShortBox(this BinaryReader Reader)
		{
			return new ShortBox(Reader.ReadVector3s(), Reader.ReadVector3s());
		}

		/*
			Binary Writer extensions for standard types
		*/

		/// <summary>
		/// Writes a 8-byte <see cref="Range"/> to the data stream.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="range">In range.</param>
		public static void WriteRange(this BinaryWriter Writer, Range range)
		{
			Writer.Write(range.Minimum);
			Writer.Write(range.Maximum);
		}

		/// <summary>
		/// Writes a 4-byte <see cref="RGBA"/> value to the data stream.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="rgba">The RGBA value to write.</param>
		public static void WriteRGBA(this BinaryWriter Writer, RGBA rgba)
		{
			Writer.Write(rgba.R);
			Writer.Write(rgba.G);
			Writer.Write(rgba.B);
			Writer.Write(rgba.A);
		}

		/// <summary>
		/// Writes a 4-byte <see cref="RGBA"/> value to the data stream.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="bgra">The RGBA value to write.</param>
		public static void WriteBGRA(this BinaryWriter Writer, BGRA bgra)
		{
			Writer.Write(bgra.B);
			Writer.Write(bgra.G);
			Writer.Write(bgra.R);
			Writer.Write(bgra.A);
		}

		/// <summary>
		/// Writes the provided string to the data stream as a C-style null-terminated string.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="InputString">Input string.</param>
		public static void WriteNullTerminatedString(this BinaryWriter Writer, string InputString)
		{
			foreach (char c in InputString)
			{
				Writer.Write(c);
			}

			Writer.Write((char)0);
		}

		/// <summary>
		/// Writes an RIFF-style chunk signature to the data stream.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="Signature">Signature.</param>
		public static void WriteChunkSignature(this BinaryWriter Writer, string Signature)
		{
			if (Signature.Length != 4)
			{
				throw new InvalidDataException("The signature must be an ASCII string of exactly four characters.");
			}

			for (int i = 3; i >= 0; --i)
			{
				Writer.Write(Signature[i]);
			}
		}


		/// <summary>
		/// Writes an RIFF-style chunk to the data stream.
		/// </summary>
		public static void WriteIFFChunk<T>(this BinaryWriter Writer, T chunk) where T : IRIFFChunk, IBinarySerializable
		{
			byte[] serializedChunk = chunk.Serialize();

			Writer.WriteChunkSignature(chunk.GetSignature());
			Writer.Write((uint)serializedChunk.Length);
			Writer.Write(serializedChunk);
		}

		/// <summary>
		/// Writes a 16-byte <see cref="Plane"/> to the data stream.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
        /// <param name="plane">The plane to write.</param>
		public static void WritePlane(this BinaryWriter Writer, Plane plane)
		{
			Writer.WriteVector3f(plane.Normal);
			Writer.Write(plane.DistanceFromCenter);
		}

		/// <summary>
		/// Writes an 18-byte <see cref="ShortPlane"/> to the data stream.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="shortPlane">The plane to write.</param>
		public static void WriteShortPlane(this BinaryWriter Writer, ShortPlane shortPlane)
		{
			for (int y = 0; y < 3; ++y)
			{
				for (int x = 0; x < 3; ++x)
				{
					Writer.Write(shortPlane.Coordinates[y][x]);
				}
			}
		}

		/// <summary>
		/// Writes a 16-byte <see cref="Quaternion"/> to the data stream.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="quaternion">The quaternion to write.</param>
		public static void WriteQuaternion32(this BinaryWriter Writer, Quaternion quaternion)
		{
			Writer.Write(quaternion.X);
			Writer.Write(quaternion.Y);
			Writer.Write(quaternion.Z);
			Writer.Write(quaternion.Scalar);
		}

		/// <summary>
		/// Writes a 8-byte <see cref="Quaternion"/> to the data stream. This is a packed format of a normal 32-bit quaternion with
		/// some data loss.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="quaternion">The quaternion to write.</param>
		public static void WriteQuaternion16(this BinaryWriter Writer, Quaternion quaternion)
		{
			Writer.Write(FloatQuatValueToShort(quaternion.X));
			Writer.Write(FloatQuatValueToShort(quaternion.Y));
			Writer.Write(FloatQuatValueToShort(quaternion.Z));
			Writer.Write(FloatQuatValueToShort(quaternion.Scalar));
		}

		/// <summary>
		/// Writes a 12-byte <see cref="Rotator"/> value to the data stream in Pitch/Yaw/Roll order.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="InRotator">Rotator.</param>
		public static void WriteRotator(this BinaryWriter Writer, Rotator InRotator)
		{
			Writer.Write(InRotator.Pitch);
			Writer.Write(InRotator.Yaw);
			Writer.Write(InRotator.Roll);
		}

		/// <summary>
		/// Writes a 12-byte <see cref="Vector3f"/> value to the data stream in XYZ order. This function
		/// expects a Y-up vector.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="vector">The Vector to write.</param>
		public static void WriteVector3f(this BinaryWriter Writer, Vector3f vector, AxisConfiguration storeAs = AxisConfiguration.ZUp)
		{
			switch (storeAs)
			{
				case AxisConfiguration.Native:
				{
					Writer.Write(vector.X);
					Writer.Write(vector.Y);
					Writer.Write(vector.Z);
					break;
				}
				case AxisConfiguration.YUp:
				{
					Writer.Write(vector.X);
					Writer.Write(vector.Y);
					Writer.Write(vector.Z);
					break;
				}
				case AxisConfiguration.ZUp:
				{
					Writer.Write(vector.X);
					Writer.Write(vector.Z);
					Writer.Write(vector.Y * -1.0f);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(storeAs), storeAs, null);
			}
		}

		/// <summary>
		/// Writes a 16-byte <see cref="Vector4f"/> value to the data stream in XYZW order. This function
		/// expects a Y-up vector.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="vector">The Vector to write.</param>
		public static void WriteVector4f(this BinaryWriter Writer, Vector4f vector, AxisConfiguration storeAs = AxisConfiguration.ZUp)
		{
			switch (storeAs)
			{
				case AxisConfiguration.Native:
				{
					Writer.Write(vector.X);
					Writer.Write(vector.Y);
					Writer.Write(vector.Z);
					Writer.Write(vector.W);
					break;
				}
				case AxisConfiguration.YUp:
				{
					Writer.Write(vector.X);
					Writer.Write(vector.Y);
					Writer.Write(vector.Z);
					Writer.Write(vector.W);
					break;
				}
				case AxisConfiguration.ZUp:
				{
					Writer.Write(vector.X);
					Writer.Write(vector.Z);
					Writer.Write(vector.Y * -1.0f);
					Writer.Write(vector.W);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(storeAs), storeAs, null);
			}
		}

		/// <summary>
		/// Writes a 6-byte <see cref="Vector3s"/> value to the data stream in XYZ order. This function
		/// expects a Y-up vector.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="vector">The Vector to write.</param>
		public static void WriteVector3s(this BinaryWriter Writer, Vector3s vector, AxisConfiguration storeAs = AxisConfiguration.ZUp)
		{
			switch (storeAs)
			{
				case AxisConfiguration.Native:
				{
					Writer.Write(vector.X);
					Writer.Write(vector.Y);
					Writer.Write(vector.Z);
					break;
				}
				case AxisConfiguration.YUp:
				{
					Writer.Write(vector.X);
					Writer.Write(vector.Y);
					Writer.Write(vector.Z);
					break;
				}
				case AxisConfiguration.ZUp:
				{
					Writer.Write(vector.X);
					Writer.Write(vector.Z);
					Writer.Write((short)(vector.Y * -1));
					break;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(storeAs), storeAs, null);
			}
		}

		/// <summary>
        /// Writes an 8-byte <see cref="Vector2f"/> value to the data stream in XY order.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="vector">The Vector to write.</param>
		public static void WriteVector2f(this BinaryWriter Writer, Vector2f vector)
		{
			Writer.Write(vector.X);
			Writer.Write(vector.Y);
		}


		/// <summary>
		/// Writes a 24-byte <see cref="Box"/> to the data stream.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="box">In box.</param>
		public static void WriteBox(this BinaryWriter Writer, Box box)
		{
			Writer.WriteVector3f(box.BottomCorner);
			Writer.WriteVector3f(box.TopCorner);
		}

		/// <summary>
		/// Writes a 12-byte <see cref="ShortBox"/> to the data stream.
		/// </summary>
		/// <param name="Writer">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="box">In box.</param>
		public static void WriteShortBox(this BinaryWriter Writer, ShortBox box)
		{
			Writer.WriteVector3s(box.BottomCorner);
			Writer.WriteVector3s(box.TopCorner);
		}
	}
}