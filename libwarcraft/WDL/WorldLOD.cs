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
using Warcraft.Core.Interfaces;

namespace Warcraft.WDL
{
	public class WorldLOD : IBinarySerializable
	{
		public readonly TerrainVersion Version;

		/*
			WMO fields are only present in WDL files with a version >= Wrath.
		*/
		public readonly TerrainWorldModelObjects WorldModelObjects;
		public readonly TerrainWorldObjectModelIndices WorldModelObjectIndices;
		public readonly TerrainWorldModelObjectPlacementInfo WorldModelObjectPlacementInfo;
		// End specific fields

		public readonly WorldLODMapAreaOffsets MapAreaOffsets;
		public List<WorldLODMapArea> MapAreas = new List<WorldLODMapArea>(4096);
		public List<WorldLODMapAreaHoles> MapAreaHoles = new List<WorldLODMapAreaHoles>(4096);

		public WorldLOD(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					// Set up the two area lists with default values
					for (int i = 0; i < 4096; ++i)
					{
						this.MapAreas.Add(null);
						this.MapAreaHoles.Add(null);
					}

					this.Version = br.ReadIFFChunk<TerrainVersion>();

					if (br.PeekChunkSignature() == TerrainWorldModelObjects.Signature)
					{
						this.WorldModelObjects = br.ReadIFFChunk<TerrainWorldModelObjects>();
					}

					if (br.PeekChunkSignature() == TerrainWorldObjectModelIndices.Signature)
					{
						this.WorldModelObjectIndices = br.ReadIFFChunk<TerrainWorldObjectModelIndices>();
					}

					if (br.PeekChunkSignature() == TerrainWorldModelObjectPlacementInfo.Signature)
					{
						this.WorldModelObjectPlacementInfo = br.ReadIFFChunk<TerrainWorldModelObjectPlacementInfo>();
					}

					this.MapAreaOffsets = br.ReadIFFChunk<WorldLODMapAreaOffsets>();

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
								this.MapAreas[mapAreaOffsetIndex] = br.ReadIFFChunk<WorldLODMapArea>();

								if (br.PeekChunkSignature() == WorldLODMapAreaHoles.Signature)
								{
									this.MapAreaHoles[mapAreaOffsetIndex] = br.ReadIFFChunk<WorldLODMapAreaHoles>();
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

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
            		bw.WriteIFFChunk(this.Version);

		            // >= Wrath stores WMO data here as well
		            if (this.WorldModelObjects != null)
		            {
			            bw.WriteIFFChunk(this.WorldModelObjects);
		            }

		            if (this.WorldModelObjectIndices != null)
					{
						bw.WriteIFFChunk(this.WorldModelObjectIndices);
					}

		            if (this.WorldModelObjectPlacementInfo != null)
					{
						bw.WriteIFFChunk(this.WorldModelObjectPlacementInfo);
					}

		            // Populate the offset table
		            long writtenMapAreaSize = 0;
		            for (int y = 0; y < 64; ++y)
		            {
			            for (int x = 0; x < 64; ++x)
			            {
				            int mapAreaOffsetIndex = (y * 64) + x;
				            const uint offsetChunkHeaderSize = 8;

				            if (this.MapAreas[mapAreaOffsetIndex] != null)
				            {
					            // This tile is populated, so we update the offset table
					            uint newOffset = (uint) (ms.Position + offsetChunkHeaderSize + WorldLODMapAreaOffsets.GetSize() + writtenMapAreaSize);
					            this.MapAreaOffsets.MapAreaOffsets[mapAreaOffsetIndex] = newOffset;

					            writtenMapAreaSize += WorldLODMapArea.GetSize() + offsetChunkHeaderSize;
				            }

				            if (this.MapAreaHoles[mapAreaOffsetIndex] != null)
							{
								writtenMapAreaSize += WorldLODMapAreaHoles.GetSize() + offsetChunkHeaderSize;
							}
			            }
		            }

		            // Write the offset table
		            bw.WriteIFFChunk(this.MapAreaOffsets);

		            // Write the valid entries
		            for (int y = 0; y < 64; ++y)
					{
						for (int x = 0; x < 64; ++x)
						{
							int mapAreaOffsetIndex = (y * 64) + x;

							if (this.MapAreas[mapAreaOffsetIndex] != null)
							{
								bw.WriteIFFChunk(this.MapAreas[mapAreaOffsetIndex]);
							}

							if (this.MapAreaHoles[mapAreaOffsetIndex] != null)
							{
								bw.WriteIFFChunk(this.MapAreaHoles[mapAreaOffsetIndex]);
							}
						}
					}
            	}

            	return ms.ToArray();
            }
		}
	}
}

