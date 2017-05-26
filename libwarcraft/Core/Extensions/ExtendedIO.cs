//
//  ExtendedIO.cs
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
using System.IO;
using System.Numerics;
using System.Text;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.MDX.Animation;
using Warcraft.MDX.Data;
using Warcraft.MDX.Gameplay;
using Warcraft.MDX.Geometry;
using Warcraft.MDX.Geometry.Skin;
using Warcraft.MDX.Visual;
using Warcraft.MDX.Visual.FX;

// ReSharper disable MemberCanBePrivate.Global

namespace Warcraft.Core.Extensions
{
	/// <summary>
	/// Extension methods used internally in the library for data IO.
	/// </summary>
	public static class ExtendedIO
	{
		/*
			Generic BinaryReader read function
		*/

		/// <summary>
		/// Type mapping function dictionary. All supported types of the generic reading function are listed here.
		/// Note that strings are read as C-style null-terminated strings, and not C#-style length-prefixed strings.
		/// </summary>
		private static readonly Dictionary<Type, Func<BinaryReader, dynamic>> TypeReaderMap = new Dictionary<Type, Func<BinaryReader, dynamic>>
		{
			// Builtin types
			{ typeof(byte), r => r.ReadByte() },
			{ typeof(char), r => r.ReadChar() },
			{ typeof(bool), r => r.ReadBoolean() },
			{ typeof(sbyte), r => r.ReadSByte() },
			{ typeof(short), r => r.ReadInt16() },
			{ typeof(ushort), r => r.ReadUInt16() },
			{ typeof(int), r => r.ReadInt32() },
			{ typeof(uint), r => r.ReadUInt32() },
			{ typeof(long), r => r.ReadInt64() },
			{ typeof(ulong), r => r.ReadUInt64() },
			{ typeof(float), r => r.ReadSingle() },
			{ typeof(double), r => r.ReadDouble() },
			{ typeof(decimal), r => r.ReadDecimal() },
			{ typeof(string), r => r.ReadNullTerminatedString() },

			// Standard Warcraft library types
			{ typeof(Range), r => r.ReadRange() },
			{ typeof(IntegerRange), r => r.ReadIntegerRange() },
			{ typeof(RGB), r => r.ReadRGB() },
			{ typeof(RGBA), r => r.ReadRGBA() },
			{ typeof(BGRA), r => r.ReadBGRA() },
			{ typeof(Plane), r => r.ReadPlane() },
			{ typeof(ShortPlane), r => r.ReadShortPlane() },
			{ typeof(Rotator), r => r.ReadRotator() },
			{ typeof(Vector3s), r => r.ReadVector3s() },
			{ typeof(Box), r => r.ReadBox() },
			{ typeof(ShortBox), r => r.ReadShortBox() },

			// System.Numerics.Vectors types
			{ typeof(Vector2), r => r.ReadVector2() },
			{ typeof(Vector3), r => r.ReadVector3() },
			{ typeof(Vector4), r => r.ReadVector4() },

			// MDX types
			{ typeof(MDXVertexProperty), r => r.ReadMDXVertexProperty() },
			{ typeof(MDXRenderBatch), r => r.ReadMDXRenderBatch() },
			{ typeof(MDXVertex), r => r.ReadMDXVertex() },
			{ typeof(MDXTexture), r => r.ReadMDXTexture() },
			{ typeof(MDXAttachmentType), r => r.ReadMDXAttachmentType() },
			{ typeof(MDXCameraType), r => r.ReadMDXCameraType() },
			{ typeof(MDXPlayableAnimationLookupTableEntry), r => r.ReadMDXPlayableAnimationLookupTableEntry()},

			// A few very specific MDXArray types, which are used with M2Tracks. This is a dirty, dirty hack to enable
			// jagged MDXArrays, since we can't cram a generic type in here

			{ typeof(MDXArray<Vector3>), r => r.ReadMDXArray<Vector3>()},
			{ typeof(MDXArray<byte>), r => r.ReadMDXArray<byte>()},
			{ typeof(MDXArray<short>), r => r.ReadMDXArray<short>()},
			{ typeof(MDXArray<ushort>), r => r.ReadMDXArray<ushort>()},
			{ typeof(MDXArray<int>), r => r.ReadMDXArray<int>()},
			{ typeof(MDXArray<uint>), r => r.ReadMDXArray<uint>()},
			{ typeof(MDXArray<float>), r => r.ReadMDXArray<float>()},
			{ typeof(MDXArray<RGB>), r => r.ReadMDXArray<RGB>()},
			{ typeof(MDXArray<RGBA>), r => r.ReadMDXArray<RGBA>()},
			{ typeof(MDXArray<BGRA>), r => r.ReadMDXArray<BGRA>()},
			{ typeof(MDXArray<SplineKey<float>>), r => r.ReadMDXArray<SplineKey<float>>()},
			{ typeof(MDXArray<SplineKey<Vector3>>), r => r.ReadMDXArray<SplineKey<Vector3>>()},

			// Some spline key types
			{ typeof(SplineKey<float>), r => r.ReadSplineKey<float>()},
			{ typeof(SplineKey<Vector3>), r => r.ReadSplineKey<Vector3>()}
		};

		/// <summary>
		/// Versioned type mapping function dictionary. All supported types of the versioned generic reading function
		/// are listed here.
		/// </summary>
		private static readonly Dictionary<Type, Func<BinaryReader, WarcraftVersion, dynamic>> VersionedTypeReaderMap = new Dictionary<Type, Func<BinaryReader, WarcraftVersion, dynamic>>
		{
			// MDX types
			{ typeof(MDXSkin), (r, v) => r.ReadMDXSkin(v) },
			{ typeof(MDXSkinSection), (r, v) => r.ReadMDXSkinSection(v)},
			{ typeof(MDXAnimationSequence), (r, v) => r.ReadMDXAnimationSequence(v)},

			{ typeof(MDXBone), (r, v) => r.ReadMDXBone(v)},
			{ typeof(MDXTextureWeight), (r, v) => r.ReadMDXTextureWeight(v)},
			{ typeof(MDXTextureTransform), (r, v) => r.ReadMDXTextureTransform(v)},
			{ typeof(MDXAttachment), (r, v) => r.ReadMDXAttachment(v)},
			{ typeof(MDXAnimationEvent), (r, v) => r.ReadMDXAnimationEvent(v)},
			{ typeof(MDXLight), (r, v) => r.ReadMDXLight(v)},
			{ typeof(MDXCamera), (r, v) => r.ReadMDXCamera(v)},
			{ typeof(MDXRibbonEmitter), (r, v) => r.ReadMDXRibbonEmitter(v)},
			{ typeof(MDXMaterial), (r, v)=> r.ReadMDXMaterial(v) },

			// System.Numerics.Vectors types
			{ typeof(Quaternion), (r, v) => v >= WarcraftVersion.BurningCrusade ? r.ReadQuaternion16() : r.ReadQuaternion32()},

			// Specific versioned MDXArray types
			{ typeof(MDXArray<Quaternion>), (r, v) => r.ReadMDXArray<Quaternion>(v)},
		};

		/// <summary>
		/// Reads a value of type <typeparamref name="T"/> from the data stream. The generic type must be
		/// explicitly implemented in <see cref="TypeReaderMap"/>. Note that strings are read as C-style null-terminated
		/// strings, and not C#-style length-prefixed strings.
		/// </summary>
		/// <param name="br"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Read<T>(this BinaryReader br)
		{
			if (!TypeReaderMap.ContainsKey(typeof(T)))
			{
				throw new ArgumentException("The given generic type has no supported reading function associated " +
				                            "with it.", typeof(T).Name);
			}

			return TypeReaderMap[typeof(T)](br);
		}

		/// <summary>
		/// Reads a versioned value of type <typeparamref name="T"/> from the data stream. The generic type must be
		/// explicitly implemented in <see cref="VersionedTypeReaderMap"/>.
		/// </summary>
		/// <param name="br"></param>
		/// <param name="version">The contextually relevant version for the object to be read.</param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T Read<T>(this BinaryReader br, WarcraftVersion version)
		{
			if (!VersionedTypeReaderMap.ContainsKey(typeof(T)))
			{
				throw new ArgumentException("The given generic type has no supported reading function associated " +
				                            "with it.", nameof(T));
			}

			return VersionedTypeReaderMap[typeof(T)](br, version);
		}

		/*
			BinaryReader Extensions for standard typess
		*/

		/// <summary>
		/// Reads a spline key with a value and in/out tangents from the data stream.
		/// </summary>
		/// <param name="binaryReader"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static SplineKey<T> ReadSplineKey<T>(this BinaryReader binaryReader)
		{
			return new SplineKey<T>(binaryReader);
		}

		/// <summary>
		/// Reads a 12-byte <see cref="RGB"/> value from the data stream.
		/// </summary>
		/// <param name="binaryReader"></param>
		/// <returns></returns>
		public static RGB ReadRGB(this BinaryReader binaryReader)
		{
			return new RGB(binaryReader.ReadVector3());
		}

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
		/// Reads an 8-byte <see cref="IntegerRange"/> value from the data stream.
		/// </summary>
		/// <param name="binaryReader"></param>
		/// <returns></returns>
		public static IntegerRange ReadIntegerRange(this BinaryReader binaryReader)
		{
			return new IntegerRange(binaryReader.ReadUInt32(), binaryReader.ReadUInt32());
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
		public static string ReadBinarySignature(this BinaryReader binaryReader)
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
			string chunkSignature = binaryReader.ReadBinarySignature();
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
			string chunkSignature = reader.ReadBinarySignature();
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
		/// Reads an 16-byte <see cref="Plane"/> from the data stream.
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
		/// Reads a 16-byte 32-bit <see cref="Quaternion"/> structure from the data stream, and advances the position of the stream by
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
			return new Quaternion(ExtendedData.ShortQuatValueToFloat(x), ExtendedData.ShortQuatValueToFloat(y), ExtendedData.ShortQuatValueToFloat(z), ExtendedData.ShortQuatValueToFloat(w));
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
		/// Reads a 12-byte <see cref="Vector3"/> structure from the data stream and advances the position of the stream by
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
			binaryWriter.Write(ExtendedData.FloatQuatValueToShort(quaternion.X));
			binaryWriter.Write(ExtendedData.FloatQuatValueToShort(quaternion.Y));
			binaryWriter.Write(ExtendedData.FloatQuatValueToShort(quaternion.Z));
			binaryWriter.Write(ExtendedData.FloatQuatValueToShort(quaternion.W));
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
		/// Writes a 12-byte <see cref="Vector3"/> value to the data stream. This function
		/// expects a Y-up vector. By default, this function will store the vector in a Z-up axis configuration, which
		/// is what World of Warcraft expects. This can be overridden. Passing <see cref="AxisConfiguration.Native"/> is
		/// considered Y-up, as it is the way vectors are handled internally in the library.
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
		/// expects a Y-up vector. By default, this function will store the vector in a Z-up axis configuration, which
		/// is what World of Warcraft expects. This can be overridden. Passing <see cref="AxisConfiguration.Native"/> is
		/// considered Y-up, as it is the way vectors are handled internally in the library.
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