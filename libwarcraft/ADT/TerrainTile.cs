//
//  ADT.cs
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
using Warcraft.ADT.Chunks;
using Warcraft.Core;
using Warcraft.WDT.Chunks;

namespace Warcraft.ADT
{
	/// <summary>
	/// A complete ADT object created from a file on disk.
	/// </summary>
	public class TerrainTile
	{
		/// <summary>
		/// Contains the ADT version.
		/// </summary>
		public TerrainVersion Version;

		/// <summary>
		/// Contains the ADT Header with offsets. The header has offsets to the other chunks in the ADT.
		/// </summary>
		public TerrainHeader Header;

		/// <summary>
		/// Contains an array of offsets where MCNKs are in the file.
		/// </summary>
		public TerrainMapChunkOffsets MapChunkOffsets;

		/// <summary>
		/// Contains a list of all textures referenced by this ADT.
		/// </summary>
		public TerrainTextures Textures;

		/// <summary>
		/// Contains a list of all M2 models refereced by this ADT.
		/// </summary>
		public TerrainModels Models;

		/// <summary>
		/// Contains M2 model indexes for the list in ADTModels (MMDX chunk).
		/// </summary>
		public TerrainModelIndices ModelIndices;

		/// <summary>
		/// Contains a list of all WMOs referenced by this ADT.
		/// </summary>
		public TerrainWorldModelObjects WorldModelObjects;

		/// <summary>
		/// Contains WMO indexes for the list in ADTWMOs (MWMO chunk).
		/// </summary>
		public TerrainWorldObjectModelIndices WorldModelObjectIndices;

		/// <summary>
		/// Contains position information for all M2 models in this ADT.
		/// </summary>
		public TerrainModelPlacementInfo ModelPlacementInfo;

		/// <summary>
		/// Contains position information for all WMO models in this ADT.
		/// </summary>
		public TerrainWorldModelObjectPlacementInfo WorldModelObjectPlacementInfo;

		/// <summary>
		/// Contains water data for this ADT. This chunk is present in WOTLK chunks and above.
		/// </summary>
		public TerrainLiquid Liquids;

		/// <summary>
		/// The texture flags. This chunk is present in WOTLK chunks and above.
		/// </summary>
		public TerrainTextureFlags TextureFlags;

		/// <summary>
		/// Contains an array of all MCNKs in this ADT.
		/// </summary>
		public List<TerrainMapChunk> MapChunks = new List<TerrainMapChunk>();

		private readonly bool HasWorldFlags;
		private readonly WDTFlags WorldFlags;

		public TerrainTile(byte[] data, WDTFlags InWorldFlags)
			: this(data)
		{
			this.HasWorldFlags = true;
			this.WorldFlags = InWorldFlags;
		}

		// TODO: Change to stream-based loading
		/// <summary>
		/// Creates a new ADT object from a file on disk
		/// </summary>
		/// <param name="Data">Byte array containing ADT data.</param>
		/// <returns>A parsed ADT file with objects for all chunks</returns>
		public TerrainTile(byte[] Data)
		{
			using (MemoryStream ms = new MemoryStream(Data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					// In all ADT files, the version chunk and header chunk are at the beginning of the file, 
					// with the header following the version. From them, the rest of the chunks can be
					// seeked to and read.

					// Read Version Chunk
					this.Version = (TerrainVersion)br.ReadTerrainChunk();

					// Read the header chunk
					this.Header = (TerrainHeader)br.ReadTerrainChunk();

					if (this.Header.MapChunkOffsetsOffset > 0)
					{
						br.BaseStream.Position = this.Header.MapChunkOffsetsOffset;
						this.MapChunkOffsets = (TerrainMapChunkOffsets)br.ReadTerrainChunk();
					}

					if (this.Header.TexturesOffset > 0)
					{
						br.BaseStream.Position = this.Header.TexturesOffset;
						this.Textures = (TerrainTextures)br.ReadTerrainChunk();
					}

					if (this.Header.ModelsOffset > 0)
					{
						br.BaseStream.Position = this.Header.ModelsOffset;
						this.Models = (TerrainModels)br.ReadTerrainChunk();
					}

					if (this.Header.ModelIndicesOffset > 0)
					{
						br.BaseStream.Position = this.Header.ModelIndicesOffset;
						this.ModelIndices = (TerrainModelIndices)br.ReadTerrainChunk();
					}

					if (this.Header.WorldModelObjectsOffset > 0)
					{
						br.BaseStream.Position = this.Header.WorldModelObjectsOffset;
						this.WorldModelObjects = (TerrainWorldModelObjects)br.ReadTerrainChunk();
					}

					if (this.Header.WorldModelObjectIndicesOffset > 0)
					{
						br.BaseStream.Position = this.Header.WorldModelObjectIndicesOffset;
						this.WorldModelObjectIndices = (TerrainWorldObjectModelIndices)br.ReadTerrainChunk();
					}

					if (this.Header.LiquidOffset > 0)
					{
						br.BaseStream.Position = this.Header.LiquidOffset;
						this.Liquids = (TerrainLiquid)br.ReadTerrainChunk();

						// TODO: [#9] Pass in DBC liquid type to load the vertex data
					}

					// Read and fill the map chunks
					foreach (MapChunkOffsetEntry Entry in this.MapChunkOffsets.Entries)
					{
						br.BaseStream.Position = Entry.MapChunkOffset;
						this.MapChunks.Add((TerrainMapChunk)br.ReadTerrainChunk());
					}
				}
			}
		}
	}
}
