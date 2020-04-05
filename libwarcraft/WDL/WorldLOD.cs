//
//  WorldLOD.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.WDL.Chunks;

namespace Warcraft.WDL
{
    /// <summary>
    /// Represets a LOD level for a world.
    /// </summary>
    public class WorldLOD : IBinarySerializable
    {
        /// <summary>
        /// Gets the terrain version.
        /// </summary>
        public TerrainVersion Version { get; }

        /*
            WMO fields are only present in WDL files with a version >= Wrath.
        */

        /// <summary>
        /// Gets the WMOs included in the LOD level.
        /// </summary>
        public TerrainWorldModelObjects? WorldModelObjects { get; }

        /// <summary>
        /// Gets the WMO indexes in the LOD level.
        /// </summary>
        public TerrainWorldModelObjectIndices? WorldModelObjectIndices { get; }

        /// <summary>
        /// Gets the placement info of the WMOs.
        /// </summary>
        public TerrainWorldModelObjectPlacementInfo? WorldModelObjectPlacementInfo { get; }

        /// <summary>
        /// Gets the map area offsets.
        /// </summary>
        public WorldLODMapAreaOffsets MapAreaOffsets { get; }

        /// <summary>
        /// Gets or sets the map areas.
        /// </summary>
        public List<WorldLODMapArea?> MapAreas { get; set; } = new List<WorldLODMapArea?>(4096);

        /// <summary>
        /// Gets or sets the map area holes.
        /// </summary>
        public List<WorldLODMapAreaHoles?> MapAreaHoles { get; set; } = new List<WorldLODMapAreaHoles?>(4096);

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldLOD"/> class.
        /// </summary>
        /// <param name="inData">The input data.</param>
        public WorldLOD(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);

            // Set up the two area lists with default values
            for (var i = 0; i < 4096; ++i)
            {
                MapAreas.Add(null);
                MapAreaHoles.Add(null);
            }

            Version = br.ReadIFFChunk<TerrainVersion>();

            if (br.PeekChunkSignature() == TerrainWorldModelObjects.Signature)
            {
                WorldModelObjects = br.ReadIFFChunk<TerrainWorldModelObjects>();
            }

            if (br.PeekChunkSignature() == TerrainWorldModelObjectIndices.Signature)
            {
                WorldModelObjectIndices = br.ReadIFFChunk<TerrainWorldModelObjectIndices>();
            }

            if (br.PeekChunkSignature() == TerrainWorldModelObjectPlacementInfo.Signature)
            {
                WorldModelObjectPlacementInfo = br.ReadIFFChunk<TerrainWorldModelObjectPlacementInfo>();
            }

            MapAreaOffsets = br.ReadIFFChunk<WorldLODMapAreaOffsets>();

            // Read the map areas and their holes
            for (var y = 0; y < 64; ++y)
            {
                for (var x = 0; x < 64; ++x)
                {
                    var mapAreaOffsetIndex = (y * 64) + x;
                    var mapAreaOffset = MapAreaOffsets.MapAreaOffsets[mapAreaOffsetIndex];

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

        /// <summary>
        /// Determines if the LOD world has an entry at the given coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>true if the LOD world has an entry at the given coordinate; otherwise, false.</returns>
        public bool HasEntry(int x, int y)
        {
            if (x < 0 || y < 0 || x > 63 || y > 63)
            {
                return false;
            }

            var index = x + (y * 64);
            return MapAreas[index] != null;
        }

        /// <summary>
        /// Gets the entry at the given coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The entry.</returns>
        public WorldLODMapArea GetEntry(int x, int y)
        {
            if (x < 0 || y < 0 || x > 63 || y > 63)
            {
                throw new ArgumentException();
            }

            var index = x + (y * 64);
            var entry = MapAreas[index];
            if (entry is null)
            {
                throw new InvalidOperationException();
            }

            return entry;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
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
                for (var y = 0; y < 64; ++y)
                {
                    for (var x = 0; x < 64; ++x)
                    {
                        var mapAreaOffsetIndex = (y * 64) + x;
                        const uint offsetChunkHeaderSize = 8;

                        if (MapAreas[mapAreaOffsetIndex] != null)
                        {
                            // This tile is populated, so we update the offset table
                            var newOffset = (uint)(ms.Position + offsetChunkHeaderSize + WorldLODMapAreaOffsets.GetSize() + writtenMapAreaSize);
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
                for (var y = 0; y < 64; ++y)
                {
                    for (var x = 0; x < 64; ++x)
                    {
                        var mapAreaOffsetIndex = (y * 64) + x;

                        if (MapAreas[mapAreaOffsetIndex] != null)
                        {
                            bw.WriteIFFChunk(MapAreas[mapAreaOffsetIndex] !);
                        }

                        if (MapAreaHoles[mapAreaOffsetIndex] != null)
                        {
                            bw.WriteIFFChunk(MapAreaHoles[mapAreaOffsetIndex] !);
                        }
                    }
                }
            }

            return ms.ToArray();
        }
    }
}
