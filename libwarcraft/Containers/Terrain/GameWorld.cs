//
//  GameWorld.cs
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
using System.Collections.Generic;
using Warcraft.WDT;
using Warcraft.ADT;
using Warcraft.MPQ;
using Warcraft.Core;

namespace Warcraft.Containers.Terrain
{
    public class GameWorld
    {
        public string WorldPath
        {
            get;
            private set;
        }

        public string WorldName
        {
            get;
            private set;
        }

        public WarcraftVersion GameVersion
        {
            get;
            private set;
        }

        private readonly IPackage Package;

        public readonly WorldTable WorldDataTable;

        public readonly List<TerrainTile> LoadedTerrainTiles = new List<TerrainTile>();

        public GameWorld(string inWorldPath, string inWorldName, WarcraftVersion inGameVersion, IPackage inPackage, byte[] inWorldData)
        {
            this.WorldPath = inWorldPath;
            this.WorldName = inWorldName;
            this.GameVersion = inGameVersion;
            this.Package = inPackage;

            // Load the world metadata
            this.WorldDataTable = new WorldTable(inWorldData);

            // Load all ADTs here? Bad for memory, perhaps better to leave it to the user to lazy load as needed
        }

        public TerrainTile LoadTile(uint tileXPosition, uint tileYPosition)
        {
            string tilePath = CreateTilePath(tileXPosition, tileYPosition);

            if (!this.Package.ContainsFile(tilePath))
            {
                throw new ArgumentException("No tile found for the given coordinates.");
            }

            byte[] terrainTileData = this.Package.ExtractFile(tilePath);
            if (terrainTileData != null)
            {
                // TODO: [#9] Pass in DBC entry to allow loading of liquid vertex data
                //TerrainTile terrainTile = new TerrainTile(terrainTileData);

                return null;
            }

            return null;
        }

        private string CreateTilePath(uint tileXPosition, uint tileYPosition)
        {
            return $"{this.WorldPath}\\{this.WorldName}_{tileXPosition}_{tileYPosition}.adt";
        }
    }
}

