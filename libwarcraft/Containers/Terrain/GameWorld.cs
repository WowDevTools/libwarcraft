//
//  GameWorld.cs
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

		public readonly WorldData WorldDataTable;

		public readonly List<TerrainTile> LoadedTerrainTiles = new List<TerrainTile>();

		public GameWorld(string InWorldPath, string InWorldName, WarcraftVersion InGameVersion, IPackage InPackage, byte[] InWorldData)
		{
			this.WorldPath = InWorldPath;
			this.WorldName = InWorldName;
			this.GameVersion = InGameVersion;
			this.Package = InPackage;

			// Load the world metadata
			this.WorldDataTable = new WorldData(InWorldData);

			// Load all ADTs here? Bad for memory, perhaps better to leave it to the user to lazy load as needed
		}

		public TerrainTile LoadTile(uint TileXPosition, uint TileYPosition)
		{
			string TilePath = CreateTilePath(TileXPosition, TileYPosition);

			if (Package.ContainsFile(TilePath))
			{
				byte[] terrainTileData = Package.ExtractFile(TilePath);
				if (terrainTileData != null)
				{
					// TODO: [#9] Pass in DBC entry to allow loading of liquid vertex data
					//TerrainTile terrainTile = new TerrainTile(terrainTileData);

					return null;
				}

			}

			return null;
		}

		private string CreateTilePath(uint TileXPosition, uint TileYPosition)
		{
			return $"{WorldPath}\\{WorldName}_{TileXPosition}_{TileYPosition}.adt";
		}
	}
}

