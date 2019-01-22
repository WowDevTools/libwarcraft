//
//  WorldTable.cs
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

using System.Collections.Generic;
using System.IO;

using Warcraft.ADT.Chunks;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.WDT.Chunks;

namespace Warcraft.WDT
{
    /// <summary>
    /// Represents a world table.
    /// </summary>
    public class WorldTable : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the table terrain version.
        /// </summary>
        public TerrainVersion Version { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public WorldTableHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the area info.
        /// </summary>
        public WorldTableAreaInfo AreaInfo { get; set; }

        /// <summary>
        /// Gets or sets the global model objects.
        /// </summary>
        public TerrainWorldModelObjects WorldModelObjects { get; set; }

        /// <summary>
        /// Gets or sets the global model placement info.
        /// </summary>
        public TerrainWorldModelObjectPlacementInfo WorldModelObjectPlacementInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldTable"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        public WorldTable(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    Version = br.ReadIFFChunk<TerrainVersion>();
                    Header = br.ReadIFFChunk<WorldTableHeader>();
                    AreaInfo = br.ReadIFFChunk<WorldTableAreaInfo>();
                    WorldModelObjects = br.ReadIFFChunk<TerrainWorldModelObjects>();

                    if (WorldModelObjects.Filenames.Count > 0)
                    {
                        WorldModelObjectPlacementInfo = br.ReadIFFChunk<TerrainWorldModelObjectPlacementInfo>();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a list of loaded areas. When working with unloaded files (i.e, not in a game or application),
        /// this method will never return any areas.
        /// </summary>
        /// <returns>The loaded areas.</returns>
        public IEnumerable<AreaInfoEntry> GetLoadedAreas()
        {
            foreach (var entry in AreaInfo.Entries)
            {
                if (entry.Flags.HasFlag(AreaInfoFlags.IsLoaded))
                {
                    yield return entry;
                }
            }
        }

        /// <summary>
        /// Gets a list of area information entries that have terrain tiles.
        /// </summary>
        /// <returns>The areas with terrain.</returns>
        public IEnumerable<AreaInfoEntry> GetAreasWithTerrain()
        {
            foreach (var entry in AreaInfo.Entries)
            {
                if (entry.Flags.HasFlag(AreaInfoFlags.HasTerrainData))
                {
                    yield return entry;
                }
            }
        }

        /// <summary>
        /// Gets the area info for the specified coordinates.
        /// </summary>
        /// <returns>The area info.</returns>
        /// <param name="inTileX">In tile x.</param>
        /// <param name="inTileY">In tile y.</param>
        public AreaInfoEntry GetAreaInfo(uint inTileX, uint inTileY)
        {
            return AreaInfo.GetAreaInfo(inTileX, inTileY);
        }

        /// <summary>
        /// Determines whether this instance has any terrain.
        /// </summary>
        /// <returns><c>true</c> if this instance has any terrain; otherwise, <c>false</c>.</returns>
        public bool HasAnyTerrain()
        {
            foreach (var entry in AreaInfo.Entries)
            {
                if (entry.Flags.HasFlag(AreaInfoFlags.HasTerrainData))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the tile at the specified coordinates is populated with terrain data or not.
        /// </summary>
        /// <returns><c>true</c> if the tile is populated; otherwise, <c>false</c>.</returns>
        /// <param name="inTileX">0-based x coordinate of the tile.</param>
        /// <param name="inTileY">0-based y coordinate of the tile.</param>
        public bool IsTilePopulated(uint inTileX, uint inTileY)
        {
            var infoEntry = AreaInfo.GetAreaInfo(inTileX, inTileY);
            return infoEntry.Flags.HasFlag(AreaInfoFlags.HasTerrainData);
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.WriteIFFChunk(Version);
                    bw.WriteIFFChunk(Header);
                    bw.WriteIFFChunk(AreaInfo);
                    bw.WriteIFFChunk(WorldModelObjects);

                    if (WorldModelObjects.Filenames.Count > 0 && WorldModelObjectPlacementInfo != null)
                    {
                        bw.WriteIFFChunk(WorldModelObjectPlacementInfo);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
