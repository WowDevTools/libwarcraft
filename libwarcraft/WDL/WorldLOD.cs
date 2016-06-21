//
//  WDL.cs
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
using Warcraft.ADT.Chunks;
using Warcraft.WDL.Chunks;
using System.Collections.Generic;
using Warcraft.Core;

namespace Warcraft.WDL
{
	public class WorldLOD
	{
		public readonly TerrainVersion Version;

		/*
			WMO fields are only present in WDL files with a version > Wrath.
		*/

		public readonly TerrainWorldModelObjects WorldModelObjects;
		public readonly TerrainWorldObjectModelIndices WorldModelObjectIndices;
		public readonly TerrainWorldModelObjectPlacementInfo WorldModelObjectPlacementInfo;

		// End specific fields

		public readonly WorldLODMapAreaOffsets MapAreaOffsets;
		public List<WorldLODMapArea> MapAreas = new List<WorldLODMapArea>(4096);
		public List<WorldLODMapAreaHoles> MapAreaHoles = new List<WorldLODMapAreaHoles>(4096);

		public WorldLOD(byte[] data, WarcraftVersion GameVersion)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Version = (TerrainVersion)br.ReadTerrainChunk();

					if (GameVersion > WarcraftVersion.BurningCrusade)
					{
						this.WorldModelObjects = (TerrainWorldModelObjects)br.ReadTerrainChunk();
						this.WorldModelObjectIndices = (TerrainWorldObjectModelIndices)br.ReadTerrainChunk();
						this.WorldModelObjectPlacementInfo = (TerrainWorldModelObjectPlacementInfo)br.ReadTerrainChunk();
					}

					this.MapAreaOffsets = (WorldLODMapAreaOffsets)br.ReadTerrainChunk();

					// Read the map areas and their holes
					for (int y = 0; y < 64; ++y)
					{
						for (int x = 0; x < 64; ++x)
						{
							int mapAreaOffsetIndex = (y * 64) + x;
							uint mapAreaOffset = this.MapAreaOffsets.MapAreaOffsets[mapAreaOffsetIndex];

							if (mapAreaOffset > 0)
							{
								br.BaseStream.Position = mapAreaOffset;
								this.MapAreas[mapAreaOffsetIndex] = (WorldLODMapArea)br.ReadTerrainChunk();

								if (PeekChunkSignature(br) == WorldLODMapAreaHoles.Signature)
								{
									this.MapAreaHoles[mapAreaOffsetIndex] = (WorldLODMapAreaHoles)br.ReadTerrainChunk();
								}
								else
								{
									this.MapAreaHoles[mapAreaOffsetIndex] = WorldLODMapAreaHoles.CreateEmpty();
								}
							}
							else
							{
								this.MapAreas[mapAreaOffsetIndex] = null;
								this.MapAreaHoles[mapAreaOffsetIndex] = null;
							}
						}
					}
				}
			}
		}

		private static string PeekChunkSignature(BinaryReader Reader)
		{
			long originalPosition = Reader.BaseStream.Position;

			string Signature = Reader.ReadChunkSignature();
			Reader.BaseStream.Position = originalPosition;

			return Signature;
		}
	}
}

