//
//  ExtendedModelIO.cs
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
using System.Linq;
using Warcraft.Core.Interfaces;
using Warcraft.MDX.Animation;
using Warcraft.MDX.Data;
using Warcraft.MDX.Geometry;
using Warcraft.MDX.Geometry.Skin;
using Warcraft.MDX.Visual;

namespace Warcraft.Core.Extensions
{
	/// <summary>
	/// Extension methods used internally in the library for data IO. This class is specialized and only deals with
	/// MDX and WMO classes.
	/// </summary>
	public static class ExtendedModelIO
	{
		/// <summary>
		/// Reads an <see cref="MDXSkin"/> from the data stream.
		/// </summary>
		/// <param name="binaryReader">The reader to use.</param>
		/// <param name="version">The contextually relevant version to target.</param>
		/// <returns>A fully read skin.</returns>
		public static MDXSkin ReadMDXSkin(this BinaryReader binaryReader, WarcraftVersion version)
		{
			MDXSkin skin = new MDXSkin
			{
				VertexIndices = binaryReader.ReadMDXArray<ushort>(),
				Triangles = binaryReader.ReadMDXArray<ushort>(),
				VertexProperties = binaryReader.ReadMDXArray<MDXVertexProperty>(),
				Sections = binaryReader.ReadMDXArray<MDXSkinSection>(version),
				RenderBatches = binaryReader.ReadMDXArray<MDXRenderBatch>(),
				BoneCountMax = binaryReader.ReadUInt32()
			};

			return skin;
		}

		/// <summary>
		/// Reads an <see cref="MDXArray{T}"/> of type <typeparamref name="T"/> from the data stream.
		/// This advances the position of the reader by 8 bytes.
		/// </summary>
		/// <param name="binaryReader">binaryReader.</param>
		/// <typeparam name="T">The type which the array encapsulates.</typeparam>
		/// <returns>A new array, filled with the values it references.</returns>
		public static MDXArray<T> ReadMDXArray<T>(this BinaryReader binaryReader)
		{
			if (typeof(T).GetInterfaces().Contains(typeof(IVersionedClass)))
			{
				throw new InvalidOperationException("Versioned classes must be provided with a target version.");
			}

			return new MDXArray<T>(binaryReader);
		}

		/// <summary>
		/// Reads an <see cref="MDXArray{T}"/> of type <typeparamref name="T"/> from the data stream.
		/// This advances the position of the reader by 8 bytes.
		/// </summary>
		/// <param name="binaryReader">binaryReader.</param>
		/// <param name="version">The contextually relevant version of the stored objects.</param>
		/// <typeparam name="T">The type which the array encapsulates.</typeparam>
		/// <returns>A new array, filled with the values it references.</returns>
		public static MDXArray<T> ReadMDXArray<T>(this BinaryReader binaryReader, WarcraftVersion version)
		{
			if (!typeof(T).GetInterfaces().Contains(typeof(IVersionedClass)))
			{
				return new MDXArray<T>(binaryReader);
			}

			return new MDXArray<T>(binaryReader, version);
		}

		/// <summary>
		/// Reads an <see cref="MDXTrack{T}"/> of type <typeparamref name="T"/> from the data stream.
		/// </summary>
		/// <param name="binaryReader"></param>
		/// <param name="version"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static MDXTrack<T> ReadMDXTrack<T>(this BinaryReader binaryReader, WarcraftVersion version)
		{
			return new MDXTrack<T>(binaryReader, version);
		}

		/// <summary>
		/// Reads an <see cref="MDXVertexProperty"/> from the data stream.
		/// </summary>
		/// <param name="binaryReader"></param>
		/// <returns></returns>
		public static MDXVertexProperty ReadMDXVertexProperty(this BinaryReader binaryReader)
		{
			return new MDXVertexProperty
			(
				binaryReader.ReadByte(),
				binaryReader.ReadByte(),
				binaryReader.ReadByte(),
				binaryReader.ReadByte()
			);
		}

		/// <summary>
		/// Reads an <see cref="MDXSkinSection"/> from the data stream.
		/// </summary>
		/// <param name="binaryReader"></param>
		/// <param name="version"></param>
		/// <returns></returns>
		public static MDXSkinSection ReadMDXSkinSection(this BinaryReader binaryReader, WarcraftVersion version)
		{
			return new MDXSkinSection(binaryReader.ReadBytes(MDXSkinSection.GetSize(version)));
		}

		/// <summary>
		/// Reads an <see cref="MDXRenderBatch"/> from the data stream.
		/// </summary>
		/// <param name="binaryReader"></param>
		/// <returns></returns>
		public static MDXRenderBatch ReadMDXRenderBatch(this BinaryReader binaryReader)
		{
			return new MDXRenderBatch(binaryReader.ReadBytes(MDXRenderBatch.GetSize()));
		}
	}
}