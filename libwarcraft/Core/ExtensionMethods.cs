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
using Warcraft.ADT.Chunks;
using System.Text;
using System.Collections.Generic;
using Warcraft.ADT.Chunks.Subchunks;
using Warcraft.WDT.Chunks;

namespace Warcraft.Core
{
	static class ExtensionMethods
	{
		public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
		{
			if (val.CompareTo(min) < 0)
				return min;
			else if (val.CompareTo(max) > 0)
				return max;
			else
				return val;
		}

		// Taken from the Arduino reference (https://www.arduino.cc/en/Reference/Map)
		public static int Map(int val, int in_min, int in_max, int out_min, int out_max)
		{
			return (val - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
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

			RGBA rgba = new RGBA(A, R, G, B);

			return rgba;
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
		/// Reads a standard RIFF-style chunk from the data stream, and advances the position of the stream
		/// by the size of the chunk.
		/// </summary>
		/// <returns>The terrain chunk.</returns>
		/// <param name="Reader">Reader.</param>
		public static IChunk ReadTerrainChunk(this BinaryReader Reader)
		{
			// The signatures are stored in reverse in the file, so we'll need to read them backwards into
			// the buffer.
			char[] signatureBuffer = new char[4];
			for (int i = 0; i < 4; ++i)
			{
				signatureBuffer[3 - i] = Reader.ReadChar();
			}

			string Signature = new string(signatureBuffer);
			uint ChunkSize = Reader.ReadUInt32();

			byte[] chunkData = Reader.ReadBytes((int)ChunkSize);

			switch (Signature)
			{
				case TerrainVersion.Signature:
					{
						return new TerrainVersion(chunkData);
					}
				case TerrainHeader.Signature:
					{
						return new TerrainHeader(chunkData);
					}
				case TerrainMapChunkOffsets.Signature:
					{
						return new TerrainMapChunkOffsets(chunkData);
					}
				case TerrainTextures.Signature:
					{
						return new TerrainTextures(chunkData);
					}
				case TerrainModels.Signature:
					{
						return new TerrainModels(chunkData);
					}
				case TerrainModelIndices.Signature:
					{
						return new TerrainModelIndices(chunkData);
					}
				case TerrainWorldModelObjects.Signature:
					{
						return new TerrainWorldModelObjects(chunkData);
					}
				case TerrainWorldObjectModelIndices.Signature:
					{
						return new TerrainWorldObjectModelIndices(chunkData);
					}
				case TerrainModelPlacementInfo.Signature:
					{
						return new TerrainModelPlacementInfo(chunkData);
					}
				case TerrainWorldModelObjectPlacementInfo.Signature:
					{
						return new TerrainWorldModelObjectPlacementInfo(chunkData);
					}
				case TerrainBoundingBox.Signature:
					{
						return new TerrainBoundingBox(chunkData);
					}
				case TerrainLiquid.Signature:
					{
						return new TerrainLiquid(chunkData);
					}
				case TerrainTextureFlags.Signature:
					{
						return new TerrainTextureFlags(chunkData);
					}
				case TerrainMapChunk.Signature:
					{
						return new TerrainMapChunk(chunkData);
					}
				case MapChunkHeightmap.Signature:
					{
						return new MapChunkHeightmap(chunkData);
					}
				case MapChunkVertexNormals.Signature:
					{
						return new MapChunkVertexNormals(chunkData);
					}
				case MapChunkTextureLayers.Signature:
					{
						return new MapChunkTextureLayers(chunkData);
					}
				case MapChunkModelReferences.Signature:
					{
						return new MapChunkModelReferences(chunkData);
					}
				case MapChunkAlphaMaps.Signature:
					{
						return new MapChunkAlphaMaps(chunkData);
					}
				case MapChunkBakedShadows.Signature:
					{
						return new MapChunkBakedShadows(chunkData);
					}
				case MapChunkVertexLighting.Signature:
					{
						return new MapChunkVertexLighting(chunkData);
					}
				case MapChunkVertexShading.Signature:
					{
						return new MapChunkVertexShading(chunkData);
					}
				case MapChunkLiquids.Signature:
					{
						return new MapChunkLiquids(chunkData);
					}
				case MapChunkSoundEmitters.Signature:
					{
						return new MapChunkSoundEmitters(chunkData);
					}
				case WDTHeader.Signature:
					{
						return new WDTHeader(chunkData);
					}
				case WDTMainChunk.Signature:
					{
						return new WDTMainChunk(chunkData);
					}
				default:
					{
						throw new FileLoadException("An unknown chunk with the signature \"" + Signature + "\" was encountered in the terrain file.");
					}
			}
		}

		/// <summary>
		/// Reads a 18-byte, 3 by 3 coordinate matrix from the data stream.
		/// </summary>
		/// <returns>The plane.</returns>
		/// <param name="Reader">Reader.</param>
		public static Plane ReadPlane(this BinaryReader Reader)
		{
			Plane plane = new Plane();
			for (int y = 0; y < 3; ++y)
			{
				List<short> CoordinateRow = new List<short>();
				for (int x = 0; x < 3; ++x)
				{
					CoordinateRow.Add(Reader.ReadInt16());
				}
				plane.Coordinates.Add(CoordinateRow);
			}

			return plane;
		}

		/// <summary>
		/// Reads a 16-byte 32-bit quaternion structure from the data stream, and advances the position of the stream by
		/// 16 bytes.
		/// </summary>
		/// <returns>The quaternion.</returns>
		/// <param name="Reader">Binary reader.</param>
		public static Quaternion ReadQuaternion32(this BinaryReader Reader)
		{
			return new Quaternion(Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
		}

		/// <summary>
		/// Reads a 8-byte 16-bit quaternion structure from the data stream, and advances the position of the stream by
		/// 8 bytes.
		/// </summary>
		/// <returns>The quaternion.</returns>
		/// <param name="Reader">Binary reader.</param>
		public static Quaternion ReadQuaternion16(this BinaryReader Reader)
		{
			short x = Reader.ReadInt16();
			short y = Reader.ReadInt16();
			short z = Reader.ReadInt16();
			short w = Reader.ReadInt16();
			return new Quaternion(ShortQuatValueToFloat(x), ShortQuatValueToFloat(y), ShortQuatValueToFloat(z), ShortQuatValueToFloat(w));
		}

		private static float ShortQuatValueToFloat(short InShort)
		{
			return (float)(InShort < 0 ? InShort + 32768 : InShort - 32768) / 32767.0f;
		}

		/// <summary>
		/// Reads a 12-byte rotator from the data stream and advances the position of the stream by 
		/// 12 bytes.
		/// </summary>
		/// <returns>The rotator.</returns>
		/// <param name="Reader">Reader.</param>
		public static Rotator ReadRotator(this BinaryReader Reader)
		{
			return new Rotator(Reader.ReadVector3f());
		}

		/// <summary>
		/// Reads a 12-byte Vector3f structure from the data stream	and advances the position of the stream by
		/// 12 bytes.
		/// </summary>
		/// <returns>The vector3f.</returns>
		/// <param name="Reader">Binary reader.</param>
		public static Vector3f ReadVector3f(this BinaryReader Reader)
		{
			return new Vector3f(Reader.ReadSingle(), Reader.ReadSingle(), Reader.ReadSingle());
		}

		/// <summary>
		/// Reads an 8-byte Vector2f structure from the data stream and advances the position of the stream by
		/// 8 bytes.
		/// </summary>
		/// <returns>The vector2f.</returns>
		/// <param name="Reader">Binary reader.</param>
		public static Vector2f ReadVector2f(this BinaryReader Reader)
		{
			return new Vector2f(Reader.ReadSingle(), Reader.ReadSingle());
		}

		/// <summary>
		/// Reads a 24-byte Box structure from the data stream and advances the position of the stream by
		// 24 bytes.
		/// </summary>
		/// <returns>The box.</returns>
		/// <param name="Reader">Binary reader.</param>
		public static Box ReadBox(this BinaryReader Reader)
		{
			return new Box(Reader.ReadVector3f(), Reader.ReadVector3f());
		}

		/*
			Binary Writer extensions for standard types
		*/

		/// <summary>
		/// Writes a 24-byte bounding box to the data stream.
		/// </summary>
		/// <param name="Writer">Writer.</param>
		/// <param name="InBox">In box.</param>
		public static void WriteBox(this BinaryWriter Writer, Box InBox)
		{
			Writer.WriteVector3f(InBox.BottomCorner);
			Writer.WriteVector3f(InBox.TopCorner);
		}

		/// <summary>
		/// Writes a 12-byte Vector3f value to the data stream in XYZ order.
		/// </summary>
		/// <param name="Writer">Writer.</param>
		/// <param name="Vector">Vector.</param>
		public static void WriteVector3f(this BinaryWriter Writer, Vector3f Vector)
		{
			Writer.Write(Vector.X);
			Writer.Write(Vector.Y);
			Writer.Write(Vector.Z);
		}

		/// <summary>
		/// Writes a 12-byte Rotator value to the data stream in Pitch/Yaw/Roll order.
		/// </summary>
		/// <param name="Writer">Writer.</param>
		/// <param name="InRotator">Rotator.</param>
		public static void WriteRotator(this BinaryWriter Writer, Rotator InRotator)
		{
			Writer.Write(InRotator.Pitch);
			Writer.Write(InRotator.Yaw);
			Writer.Write(InRotator.Roll);
		}

		/// <summary>
		/// Writes an RIFF-style chunk signature to the data stream.
		/// </summary>
		/// <param name="Writer">Writer.</param>
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
		/// Writes the provided string to the data stream as a C-style null-terminated string.
		/// </summary>
		/// <param name="Writer">Writer.</param>
		/// <param name="InputString">Input string.</param>
		public static void WriteNullTerminatedString(this BinaryWriter Writer, string InputString)
		{
			foreach (char c in InputString)
			{
				Writer.Write(c);
			}

			Writer.Write((char)0);
		}
	}
}