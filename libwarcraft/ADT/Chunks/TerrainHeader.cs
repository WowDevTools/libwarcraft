//
//  TerrainHeader.cs
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

namespace Warcraft.ADT.Chunks
{
	/// <summary>
	/// MHDR Chunk - Contains offset for all major chunks in the ADT. All offsets are from the start of the MHDR + 4 bytes to compensate for the size field.
	/// </summary>
	public class TerrainHeader
	{
		public const string Signature = "MHDR";

		/// <summary>
		/// Flags for this ADT
		/// </summary>
		public TerrainHeaderFlags Flags;

		/// <summary>
		/// Offset into the file where the MCIN Chunk can be found. 
		/// </summary>
		public int MapChunkOffsetsOffset;
		/// <summary>
		/// Offset into the file where the MTEX Chunk can be found. 
		/// </summary>
		public int TexturesOffset;

		/// <summary>
		/// Offset into the file where the MMDX Chunk can be found. 
		/// </summary>
		public int ModelsOffset;
		/// <summary>
		/// Offset into the file where the MMID Chunk can be found. 
		/// </summary>
		public int ModelIndicesOffset;

		/// <summary>
		/// Offset into the file where the MWMO Chunk can be found. 
		/// </summary>
		public int WorldModelObjectsOffset;
		/// <summary>
		/// Offset into the file where the MWID Chunk can be found. 
		/// </summary>
		public int WorldModelObjectIndicesOffset;

		/// <summary>
		/// Offset into the file where the MMDF Chunk can be found. 
		/// </summary>
		public int ModelPlacementInformationOffset;
		/// <summary>
		/// Offset into the file where the MODF Chunk can be found. 
		/// </summary>
		public int WorldModelObjectPlacementInformationOffset;

		/// <summary>
		/// Offset into the file where the MFBO Chunk can be found. This is only set if the Flags contains MDHR_MFBO.
		/// </summary>
		public int BoundingBoxOffset;

		/// <summary>
		/// Offset into the file where the MH2O Chunk can be found. 
		/// </summary>
		public int WaterOffset;
		/// <summary>
		/// Offset into the file where the MTXF Chunk can be found. 
		/// </summary>
		public int TextureMetadataOffset;

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainHeader"/> class.
		/// </summary>
		/// <param name="data">Data.</param>
		public TerrainHeader(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					//read values
					this.Flags = (TerrainHeaderFlags)br.ReadInt32();

					this.MapChunkOffsetsOffset = br.ReadInt32();
					this.TexturesOffset = br.ReadInt32();

					this.ModelsOffset = br.ReadInt32();
					this.ModelIndicesOffset = br.ReadInt32();

					this.WorldModelObjectsOffset = br.ReadInt32();
					this.WorldModelObjectIndicesOffset = br.ReadInt32();

					this.ModelPlacementInformationOffset = br.ReadInt32();
					this.WorldModelObjectPlacementInformationOffset = br.ReadInt32();

					this.BoundingBoxOffset = br.ReadInt32();
					this.WaterOffset = br.ReadInt32();
					this.TextureMetadataOffset = br.ReadInt32();
				}
			}
		}
	}

	/// <summary>
	/// Flags for the ADT.
	/// </summary>
	[Flags]
	public enum TerrainHeaderFlags
	{
		/// <summary>
		/// This terrain file contains a bounding box.
		/// </summary>
		HasBoundingBox = 1,

		/// <summary>
		/// Flag if the ADT is from Northrend. This flag is not always set.
		/// </summary>
		MHDR_Northrend = 2,
	}
}

