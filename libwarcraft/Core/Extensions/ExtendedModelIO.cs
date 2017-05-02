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
		/// <returns>A fully read skin.</returns>
		public static MDXSkin ReadMDXSkin(this BinaryReader binaryReader)
		{
			MDXSkin skin = new MDXSkin
			{
				VertexIndices = binaryReader.ReadMDXArray<ushort>(),
				Triangles = binaryReader.ReadMDXArray<ushort>(),
				VertexProperties = binaryReader.ReadMDXArray<MDXVertexProperty>(),
				Sections = binaryReader.ReadMDXArray<MDXSkinSection>(),
				RenderBatches = binaryReader.ReadMDXArray<MDXRenderBatch>(),
				BoneCountMax = binaryReader.ReadUInt32()
			};

			return skin;
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