//
//  WDL.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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
using Warcraft.Core.Extensions;
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
                        MapAreas.Add(null);
                        MapAreaHoles.Add(null);
                    }

                    Version = br.ReadIFFChunk<TerrainVersion>();

                    if (br.PeekChunkSignature() == TerrainWorldModelObjects.Signature)
                    {
                        WorldModelObjects = br.ReadIFFChunk<TerrainWorldModelObjects>();
                    }

                    if (br.PeekChunkSignature() == TerrainWorldObjectModelIndices.Signature)
                    {
                        WorldModelObjectIndices = br.ReadIFFChunk<TerrainWorldObjectModelIndices>();
                    }

                    if (br.PeekChunkSignature() == TerrainWorldModelObjectPlacementInfo.Signature)
                    {
                        WorldModelObjectPlacementInfo = br.ReadIFFChunk<TerrainWorldModelObjectPlacementInfo>();
                    }

                    MapAreaOffsets = br.ReadIFFChunk<WorldLODMapAreaOffsets>();

                    // Read the map areas and their holes
                    for (int y = 0; y < 64; ++y)
                    {
                        for (int x = 0; x < 64; ++x)
                        {
                            int mapAreaOffsetIndex = (y * 64) + x;
                            uint mapAreaOffset = MapAreaOffsets.MapAreaOffsets[mapAreaOffsetIndex];

                            if (mapAreaOffset > 0)
                            {
                                br.BaseStream.Position = mapAreaOffset;
                                MapAreas[mapAreaOffsetIndex] = br.ReadIFFChunk<WorldLODMapArea>();

                                if (br.PeekChunkSignature() == WorldLODMapAreaHoles.Signature)
                                {
                                    MapAreaHoles[mapAreaOffsetIndex] = br.ReadIFFChunk<WorldLODMapAreaHoles>();
                                }
                                else
                                {
                                    MapAreaHoles[mapAreaOffsetIndex] = WorldLODMapAreaHoles.CreateEmpty();
                                }
                            }
                            else
                            {
                                MapAreas[mapAreaOffsetIndex] = null;
                                MapAreaHoles[mapAreaOffsetIndex] = null;
                            }
                        }
                    }
                }
            }
        }

        public bool HasEntry(int x, int y)
        {
            if (x < 0 || y < 0 || x > 63 || y > 63)
            {
                return false;
            }

            var index = x + y * 64;
            return MapAreas[index] != null;
        }

        public WorldLODMapArea GetEntry(int x, int y)
        {
            if (x < 0 || y < 0 || x > 63 || y > 63)
            {
                throw new ArgumentException();
            }

            var index = x + y * 64;
            return MapAreas[index];
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.WriteIFFChunk(Version);

                    // >= Wrath stores WMO data here as well
                    if (WorldModelObjects != null)
                    {
                        bw.WriteIFFChunk(WorldModelObjects);
                    }

                    if (WorldModelObjectIndices != null)
                    {
                        bw.WriteIFFChunk(WorldModelObjectIndices);
                    }

                    if (WorldModelObjectPlacementInfo != null)
                    {
                        bw.WriteIFFChunk(WorldModelObjectPlacementInfo);
                    }

                    // Populate the offset table
                    long writtenMapAreaSize = 0;
                    for (int y = 0; y < 64; ++y)
                    {
                        for (int x = 0; x < 64; ++x)
                        {
                            int mapAreaOffsetIndex = (y * 64) + x;
                            const uint offsetChunkHeaderSize = 8;

                            if (MapAreas[mapAreaOffsetIndex] != null)
                            {
                                // This tile is populated, so we update the offset table
                                uint newOffset = (uint) (ms.Position + offsetChunkHeaderSize + WorldLODMapAreaOffsets.GetSize() + writtenMapAreaSize);
                                MapAreaOffsets.MapAreaOffsets[mapAreaOffsetIndex] = newOffset;

                                writtenMapAreaSize += WorldLODMapArea.GetSize() + offsetChunkHeaderSize;
                            }

                            if (MapAreaHoles[mapAreaOffsetIndex] != null)
                            {
                                writtenMapAreaSize += WorldLODMapAreaHoles.GetSize() + offsetChunkHeaderSize;
                            }
                        }
                    }

                    // Write the offset table
                    bw.WriteIFFChunk(MapAreaOffsets);

                    // Write the valid entries
                    for (int y = 0; y < 64; ++y)
                    {
                        for (int x = 0; x < 64; ++x)
                        {
                            int mapAreaOffsetIndex = (y * 64) + x;

                            if (MapAreas[mapAreaOffsetIndex] != null)
                            {
                                bw.WriteIFFChunk(MapAreas[mapAreaOffsetIndex]);
                            }

                            if (MapAreaHoles[mapAreaOffsetIndex] != null)
                            {
                                bw.WriteIFFChunk(MapAreaHoles[mapAreaOffsetIndex]);
                            }
                        }
                    }
                }

                return ms.ToArray();
            }
        }
    }
}

