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
using System.Numerics;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.MDX.Geometry;

namespace Warcraft.Core
{
	/// <summary>
	/// Extension methods used internally in the library to read, write and otherwise manipulate data.
	/// </summary>
	public static class ExtensionMethods
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
				bw.WriteVector2(vertex.UVCoordinatesChannel1);
				bw.WriteVector2(vertex.UVCoordinatesChannel2);

				return ms.ToArray();
			}
		}

		/// <summary>
		/// Clamps a value between a minimum and maximum point.
		/// </summary>
		/// <param name="val">The value to compare.</param>
		/// <param name="min">The minimum allowed value.</param>
		/// <param name="max">The maximum allowed value.</param>
		/// <typeparam name="T">The type of value to compare.</typeparam>
		/// <returns>The value, if it falls into the range, otherwise the upper or lower bound, whichever was closest. </returns>
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

		/// <summary>
		/// Maps a value in a specified range to a new value in a new range.
		/// Taken from the Arduino reference (https://www.arduino.cc/en/Reference/Map)
		/// </summary>
		/// <param name="val">The input value.</param>
		/// <param name="inMin">The original mininum value.</param>
		/// <param name="inMax">The original maximum value.</param>
		/// <param name="outMin">The new minimum value.</param>
		/// <param name="outMax">The new maxiumum value.</param>
		/// <returns>The value, mapped to the new range.</returns>
		public static int Map(int val, int inMin, int inMax, int outMin, int outMax)
		{
			return (val - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
		}

		private static float ShortQuatValueToFloat(short inShort)
		{
			return inShort / (float) short.MaxValue;
		}

		private static short FloatQuatValueToShort(float inFloat)
		{
			return (short)((inFloat + 1.0f) * short.MaxValue);
		}

		/// <summary>
		/// Inverts colours of the specified bitmap.
		/// </summary>
		/// <param name="map">The bitmap to invert.</param>
		/// <param name="keepAlpha">Whether or not the keep the alpha values, or invert them as well.</param>
		/// <returns>The inverted bitmap.</returns>
		public static Bitmap Invert(this Bitmap map, bool keepAlpha = true)
		{
			Bitmap outMap = new Bitmap(map);

			for (int y = 0; y < map.Height; ++y)
			{
				for (int x = 0; x < map.Width; ++x)
				{
					Color pixel = map.GetPixel(x, y);
					byte pixelAlpha = pixel.A;
					if (!keepAlpha)
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

		/// <summary>
		/// Determines whether or not a given bitmap has any alpha values, and thus if it requires an alpha channel in
		/// other formats.
		/// </summary>
		/// <param name="map">The map to inspect.</param>
		/// <returns><value>true</value> if the bitmap has any alpha values; otherwise, <value>false</value>.</returns>
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
			BinaryReader Extensions for standard typess
		*/

		/// <summary>
		/// Reads an 8-byte Range value from the data stream.
		/// </summary>
		/// <returns>The range.</returns>
		/// <param name="binaryReader">binaryReader.</param>
		public static Range ReadRange(this BinaryReader binaryReader)
		{
			return new Range(binaryReader.ReadSingle(), binaryReader.ReadSingle());
		}

		/// <summary>
		/// Reads a 4-byte RGBA value from the data stream.
		/// </summary>
		/// <returns>The argument.</returns>
		/// <param name="binaryReader">binaryReader.</param>
		public static RGBA ReadRGBA(this BinaryReader binaryReader)
		{
			byte r = binaryReader.ReadByte();
			byte g = binaryReader.ReadByte();
			byte b = binaryReader.ReadByte();
			byte a = binaryReader.ReadByte();

			RGBA rgba = new RGBA(r, g, b, a);

			return rgba;
		}

		/// <summary>
		/// Reads a 4-byte RGBA value from the data stream.
		/// </summary>
		/// <returns>The argument.</returns>
		/// <param name="binaryReader">binaryReader.</param>
		public static BGRA ReadBGRA(this BinaryReader binaryReader)
		{
			byte b = binaryReader.ReadByte();
			byte g = binaryReader.ReadByte();
			byte r = binaryReader.ReadByte();
			byte a = binaryReader.ReadByte();

			BGRA bgra = new BGRA(b, g, r, a);

			return bgra;
		}

		/// <summary>
		/// Reads a standard null-terminated string from the data stream.
		/// </summary>
		/// <returns>The null terminated string.</returns>
		/// <param name="binaryReader">binaryReader.</param>
		public static string ReadNullTerminatedString(this BinaryReader binaryReader)
		{
			StringBuilder sb = new StringBuilder();

			char c;
			while ((c = binaryReader.ReadChar()) != 0)
			{
				sb.Append(c);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Reads a 4-byte RIFF chunk signature from the data stream.
		/// </summary>
		/// <returns>The signature as a string.</returns>
		public static string ReadChunkSignature(this BinaryReader binaryReader)
		{
			// The signatures are stored in reverse in the file, so we'll need to read them backwards into
			// the buffer.
			char[] signatureBuffer = new char[4];
			for (int i = 0; i < 4; ++i)
			{
				signatureBuffer[3 - i] = binaryReader.ReadChar();
			}

			string signature = new string(signatureBuffer);
			return signature;
		}

		/// <summary>
		/// Peeks a 4-byte RIFF chunk signature from the data stream. This does not
		/// advance the position of the stream.
		/// </summary>
		public static string PeekChunkSignature(this BinaryReader binaryReader)
		{
			string chunkSignature = binaryReader.ReadChunkSignature();
			binaryReader.BaseStream.Position -= chunkSignature.Length;

			return chunkSignature;
		}

		/// <summary>
		/// Reads an IFF-style chunk from the stream. The chunk must have the <see cref="IIFFChunk"/>
		/// interface, and implement a parameterless constructor.
		/// </summary>
		/// <param name="reader">The current <see cref="BinaryReader"/></param>
		public static T ReadIFFChunk<T>(this BinaryReader reader) where T : IIFFChunk, new()
		{
			string chunkSignature = reader.ReadChunkSignature();
			uint chunkSize = reader.ReadUInt32();
			byte[] chunkData = reader.ReadBytes((int)chunkSize);

			T chunk = new T();
			if (chunk.GetSignature() != chunkSignature)
			{
				throw new InvalidChunkSignatureException($"An unknown chunk with the signature \"{chunkSignature}\" was read.");
			}

			chunk.LoadBinaryData(chunkData);

			return chunk;
		}

		/// <summary>
		/// Reads an 16-byte <see cref="Structures.Plane"/> from the data stream.
		/// </summary>
		/// <returns>The plane.</returns>
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		public static Plane ReadPlane(this BinaryReader binaryReader)
		{
			return new Plane(binaryReader.ReadVector3(), binaryReader.ReadSingle());
		}

		/// <summary>
		/// Reads an 18-byte <see cref="ShortPlane"/> from the data stream.
		/// </summary>
		/// <returns>The plane.</returns>
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		public static ShortPlane ReadShortPlane(this BinaryReader binaryReader)
		{
			ShortPlane shortPlane = new ShortPlane();
			for (int y = 0; y < 3; ++y)
			{
				List<short> coordinateRow = new List<short>();
				for (int x = 0; x < 3; ++x)
				{
					coordinateRow.Add(binaryReader.ReadInt16());
				}
				shortPlane.Coordinates.Add(coordinateRow);
			}

			return shortPlane;
		}

		/// <summary>
		/// Reads a 16-byte 32-bit <see cref="Structures.Quaternion"/> structure from the data stream, and advances the position of the stream by
		/// 16 bytes.
		/// </summary>
		/// <returns>The quaternion.</returns>
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		public static Quaternion ReadQuaternion32(this BinaryReader binaryReader)
		{
			return new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
		}

		/// <summary>
		/// Reads a 8-byte 16-bit <see cref="Quaternion"/> structure from the data stream, and advances the position of the stream by
		/// 8 bytes.
		/// </summary>
		/// <returns>The quaternion.</returns>
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		public static Quaternion ReadQuaternion16(this BinaryReader binaryReader)
		{
			short x = binaryReader.ReadInt16();
			short y = binaryReader.ReadInt16();
			short z = binaryReader.ReadInt16();
			short w = binaryReader.ReadInt16();
			return new Quaternion(ShortQuatValueToFloat(x), ShortQuatValueToFloat(y), ShortQuatValueToFloat(z), ShortQuatValueToFloat(w));
		}

		/// <summary>
		/// Reads a 12-byte <see cref="Rotator"/> from the data stream and advances the position of the stream by
		/// 12 bytes.
		/// </summary>
		/// <returns>The rotator.</returns>
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		public static Rotator ReadRotator(this BinaryReader binaryReader)
		{
			return new Rotator(binaryReader.ReadVector3());
		}

		/// <summary>
		/// Reads a 12-byte <see cref="Vector3f"/> structure from the data stream and advances the position of the stream by
		/// 12 bytes.
		/// </summary>
		/// <returns>The vector3f.</returns>
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		/// <param name="convertTo">Which axis configuration the read vector should be converted to.</param>
		public static Vector3 ReadVector3(this BinaryReader binaryReader, AxisConfiguration convertTo = AxisConfiguration.YUp)
		{
			switch (convertTo)
			{
				case AxisConfiguration.Native:
				{
					return new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
				}
				case AxisConfiguration.YUp:
				{
					float x = binaryReader.ReadSingle();
					float z = binaryReader.ReadSingle();
					float y = binaryReader.ReadSingle() * -1.0f;

					return new Vector3(x, y, z);
				}
				case AxisConfiguration.ZUp:
				{
					float x =  binaryReader.ReadSingle();
					float z = binaryReader.ReadSingle() * -1.0f;
					float y = binaryReader.ReadSingle();

					return new Vector3(x, y, z);
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(convertTo), convertTo, null);
				}
			}
		}

		/// <summary>
		/// Reads a 16-byte <see cref="Vector4"/> structure from the data stream and advances the position of the stream by
		/// 16 bytes.
		/// </summary>
		/// <returns>The vector3f.</returns>
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		/// <param name="convertTo">Which axis configuration the read vector should be converted to.</param>
		public static Vector4 ReadVector4(this BinaryReader binaryReader, AxisConfiguration convertTo = AxisConfiguration.YUp)
		{
			switch (convertTo)
			{
				case AxisConfiguration.Native:
				{
					return new Vector4(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
				}
				case AxisConfiguration.YUp:
				{
					float x = binaryReader.ReadSingle();
					float z = binaryReader.ReadSingle();
					float y = binaryReader.ReadSingle() * -1.0f;

					float w = binaryReader.ReadSingle();

					return new Vector4(x, y, z, w);
				}
				case AxisConfiguration.ZUp:
				{
					float x = binaryReader.ReadSingle();
					float z = binaryReader.ReadSingle() * -1.0f;
					float y = binaryReader.ReadSingle();

					float w = binaryReader.ReadSingle();

					return new Vector4(x, y, z, w);
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
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		/// <param name="convertTo">Which axis configuration the read vector should be converted to.</param>
		public static Vector3s ReadVector3s(this BinaryReader binaryReader, AxisConfiguration convertTo = AxisConfiguration.YUp)
		{
			switch (convertTo)
			{
				case AxisConfiguration.Native:
				{
					return new Vector3s(binaryReader.ReadInt16(), binaryReader.ReadInt16(), binaryReader.ReadInt16());
				}
				case AxisConfiguration.YUp:
				{
					short x = binaryReader.ReadInt16();
					short z = binaryReader.ReadInt16();
					short y = (short)(binaryReader.ReadInt16() * -1);

					return new Vector3s(x, y, z);
				}
				case AxisConfiguration.ZUp:
				{
					short x = binaryReader.ReadInt16();
					short z = (short)(binaryReader.ReadInt16() * -1);
					short y = binaryReader.ReadInt16();

					return new Vector3s(x, y, z);
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(convertTo), convertTo, null);
				}
			}
		}

		/// <summary>
		/// Reads an 8-byte <see cref="Vector2"/> structure from the data stream and advances the position of the stream by
		/// 8 bytes.
		/// </summary>
		/// <returns>The vector2f.</returns>
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		public static Vector2 ReadVector2(this BinaryReader binaryReader)
		{
			return new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
		}

		/// <summary>
		/// Reads a 24-byte <see cref="Box"/> structure from the data stream and advances the position of the stream by
		/// 24 bytes.
		/// </summary>
		/// <returns>The box.</returns>
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		public static Box ReadBox(this BinaryReader binaryReader)
		{
			return new Box(binaryReader.ReadVector3(), binaryReader.ReadVector3());
		}

		/// <summary>
		/// Reads a 12-byte <see cref="Box"/> structure from the data stream and advances the position of the stream by
		/// 12 bytes.
		/// </summary>
		/// <returns>The box.</returns>
		/// <param name="binaryReader">The current <see cref="BinaryReader"/></param>
		public static ShortBox ReadShortBox(this BinaryReader binaryReader)
		{
			return new ShortBox(binaryReader.ReadVector3s(), binaryReader.ReadVector3s());
		}

		/*
			BinaryWriter extensions for standard types
		*/

		/// <summary>
		/// Writes a 8-byte <see cref="Range"/> to the data stream.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="range">In range.</param>
		public static void WriteRange(this BinaryWriter binaryWriter, Range range)
		{
			binaryWriter.Write(range.Minimum);
			binaryWriter.Write(range.Maximum);
		}

		/// <summary>
		/// Writes a 4-byte <see cref="RGBA"/> value to the data stream.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="rgba">The RGBA value to write.</param>
		public static void WriteRGBA(this BinaryWriter binaryWriter, RGBA rgba)
		{
			binaryWriter.Write(rgba.R);
			binaryWriter.Write(rgba.G);
			binaryWriter.Write(rgba.B);
			binaryWriter.Write(rgba.A);
		}

		/// <summary>
		/// Writes a 4-byte <see cref="RGBA"/> value to the data stream.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="bgra">The RGBA value to write.</param>
		public static void WriteBGRA(this BinaryWriter binaryWriter, BGRA bgra)
		{
			binaryWriter.Write(bgra.B);
			binaryWriter.Write(bgra.G);
			binaryWriter.Write(bgra.R);
			binaryWriter.Write(bgra.A);
		}

		/// <summary>
		/// Writes the provided string to the data stream as a C-style null-terminated string.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="inputString">Input string.</param>
		public static void WriteNullTerminatedString(this BinaryWriter binaryWriter, string inputString)
		{
			foreach (char c in inputString)
			{
				binaryWriter.Write(c);
			}

			binaryWriter.Write((char)0);
		}

		/// <summary>
		/// Writes an RIFF-style chunk signature to the data stream.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="signature">Signature.</param>
		public static void WriteChunkSignature(this BinaryWriter binaryWriter, string signature)
		{
			if (signature.Length != 4)
			{
				throw new InvalidDataException("The signature must be an ASCII string of exactly four characters.");
			}

			for (int i = 3; i >= 0; --i)
			{
				binaryWriter.Write(signature[i]);
			}
		}


		/// <summary>
		/// Writes an RIFF-style chunk to the data stream.
		/// </summary>
		public static void WriteIFFChunk<T>(this BinaryWriter binaryWriter, T chunk) where T : IIFFChunk, IBinarySerializable
		{
			byte[] serializedChunk = chunk.Serialize();

			binaryWriter.WriteChunkSignature(chunk.GetSignature());
			binaryWriter.Write((uint)serializedChunk.Length);
			binaryWriter.Write(serializedChunk);
		}

		/// <summary>
		/// Writes a 16-byte <see cref="Plane"/> to the data stream.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
        /// <param name="plane">The plane to write.</param>
		public static void WritePlane(this BinaryWriter binaryWriter, Plane plane)
		{
			binaryWriter.WriteVector3(plane.Normal);
			binaryWriter.Write(plane.D);
		}

		/// <summary>
		/// Writes an 18-byte <see cref="ShortPlane"/> to the data stream.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="shortPlane">The plane to write.</param>
		public static void WriteShortPlane(this BinaryWriter binaryWriter, ShortPlane shortPlane)
		{
			for (int y = 0; y < 3; ++y)
			{
				for (int x = 0; x < 3; ++x)
				{
					binaryWriter.Write(shortPlane.Coordinates[y][x]);
				}
			}
		}

		/// <summary>
		/// Writes a 16-byte <see cref="Quaternion"/> to the data stream.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="quaternion">The quaternion to write.</param>
		public static void WriteQuaternion32(this BinaryWriter binaryWriter, Quaternion quaternion)
		{
			binaryWriter.Write(quaternion.X);
			binaryWriter.Write(quaternion.Y);
			binaryWriter.Write(quaternion.Z);
			binaryWriter.Write(quaternion.W);
		}

		/// <summary>
		/// Writes a 8-byte <see cref="Quaternion"/> to the data stream. This is a packed format of a normal 32-bit quaternion with
		/// some data loss.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="quaternion">The quaternion to write.</param>
		public static void WriteQuaternion16(this BinaryWriter binaryWriter, Quaternion quaternion)
		{
			binaryWriter.Write(FloatQuatValueToShort(quaternion.X));
			binaryWriter.Write(FloatQuatValueToShort(quaternion.Y));
			binaryWriter.Write(FloatQuatValueToShort(quaternion.Z));
			binaryWriter.Write(FloatQuatValueToShort(quaternion.W));
		}

		/// <summary>
		/// Writes a 12-byte <see cref="Rotator"/> value to the data stream in Pitch/Yaw/Roll order.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="inRotator">Rotator.</param>
		public static void WriteRotator(this BinaryWriter binaryWriter, Rotator inRotator)
		{
			binaryWriter.Write(inRotator.Pitch);
			binaryWriter.Write(inRotator.Yaw);
			binaryWriter.Write(inRotator.Roll);
		}

		/// <summary>
		/// Writes a 12-byte <see cref="Vector3f"/> value to the data stream in XYZ order. This function
		/// expects a Y-up vector.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="vector">The Vector to write.</param>
		/// <param name="storeAs">Which axis configuration the read vector should be stored as.</param>
		public static void WriteVector3(this BinaryWriter binaryWriter, Vector3 vector, AxisConfiguration storeAs = AxisConfiguration.ZUp)
		{
			switch (storeAs)
			{
				case AxisConfiguration.Native:
				case AxisConfiguration.YUp:
				{
					binaryWriter.Write(vector.X);
					binaryWriter.Write(vector.Y);
					binaryWriter.Write(vector.Z);
					break;
				}
				case AxisConfiguration.ZUp:
				{
					binaryWriter.Write(vector.X);
					binaryWriter.Write(vector.Z);
					binaryWriter.Write(vector.Y * -1.0f);
					break;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(storeAs), storeAs, null);
			}
		}

		/// <summary>
		/// Writes a 16-byte <see cref="Vector4"/> value to the data stream in XYZW order. This function
		/// expects a Y-up vector.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="vector">The Vector to write.</param>
		/// <param name="storeAs">Which axis configuration the read vector should be stored as.</param>
		public static void WriteVector4(this BinaryWriter binaryWriter, Vector4 vector, AxisConfiguration storeAs = AxisConfiguration.ZUp)
		{
			switch (storeAs)
			{
				case AxisConfiguration.Native:
				{
					binaryWriter.Write(vector.X);
					binaryWriter.Write(vector.Y);
					binaryWriter.Write(vector.Z);
					binaryWriter.Write(vector.W);
					break;
				}
				case AxisConfiguration.YUp:
				{
					binaryWriter.Write(vector.X);
					binaryWriter.Write(vector.Y);
					binaryWriter.Write(vector.Z);
					binaryWriter.Write(vector.W);
					break;
				}
				case AxisConfiguration.ZUp:
				{
					binaryWriter.Write(vector.X);
					binaryWriter.Write(vector.Z);
					binaryWriter.Write(vector.Y * -1.0f);
					binaryWriter.Write(vector.W);
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
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="vector">The Vector to write.</param>
		/// <param name="storeAs">Which axis configuration the read vector should be stored as.</param>
		public static void WriteVector3s(this BinaryWriter binaryWriter, Vector3s vector, AxisConfiguration storeAs = AxisConfiguration.ZUp)
		{
			switch (storeAs)
			{
				case AxisConfiguration.Native:
				{
					binaryWriter.Write(vector.X);
					binaryWriter.Write(vector.Y);
					binaryWriter.Write(vector.Z);
					break;
				}
				case AxisConfiguration.YUp:
				{
					binaryWriter.Write(vector.X);
					binaryWriter.Write(vector.Y);
					binaryWriter.Write(vector.Z);
					break;
				}
				case AxisConfiguration.ZUp:
				{
					binaryWriter.Write(vector.X);
					binaryWriter.Write(vector.Z);
					binaryWriter.Write((short)(vector.Y * -1));
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(storeAs), storeAs, null);
				}
			}
		}

		/// <summary>
        /// Writes an 8-byte <see cref="Vector2"/> value to the data stream in XY order.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="vector">The Vector to write.</param>
		public static void WriteVector2(this BinaryWriter binaryWriter, Vector2 vector)
		{
			binaryWriter.Write(vector.X);
			binaryWriter.Write(vector.Y);
		}


		/// <summary>
		/// Writes a 24-byte <see cref="Box"/> to the data stream.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="box">In box.</param>
		public static void WriteBox(this BinaryWriter binaryWriter, Box box)
		{
			binaryWriter.WriteVector3(box.BottomCorner);
			binaryWriter.WriteVector3(box.TopCorner);
		}

		/// <summary>
		/// Writes a 12-byte <see cref="ShortBox"/> to the data stream.
		/// </summary>
		/// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
		/// <param name="box">In box.</param>
		public static void WriteShortBox(this BinaryWriter binaryWriter, ShortBox box)
		{
			binaryWriter.WriteVector3s(box.BottomCorner);
			binaryWriter.WriteVector3s(box.TopCorner);
		}
	}
}