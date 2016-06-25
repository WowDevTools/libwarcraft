//
//  WDT.cs
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
using Warcraft.ADT.Chunks;
using Warcraft.WDT.Chunks;
using System.IO;
using Warcraft.Core;
using System.Collections.Generic;

namespace Warcraft.WDT
{
	public class WorldData
	{
		public TerrainVersion Version;
		public WDTHeader Header;
		public AreaInfoChunk AreaInfo;
		public TerrainWorldModelObjects WorldModelObjects;
		public TerrainWorldModelObjectPlacementInfo WorldModelObjectPlacementInfo;

		public WorldData(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Version = (TerrainVersion)br.ReadTerrainChunk();
					this.Header = (WDTHeader)br.ReadTerrainChunk();
					this.AreaInfo = (AreaInfoChunk)br.ReadTerrainChunk();
					this.WorldModelObjects = (TerrainWorldModelObjects)br.ReadTerrainChunk();

					if (this.WorldModelObjects.Filenames.Count > 0)
					{
						this.WorldModelObjectPlacementInfo = (TerrainWorldModelObjectPlacementInfo)br.ReadTerrainChunk();
					}
				}
			}
		}

		/// <summary>
		/// Gets a list of loaded areas. When working with unloaded files (i.e, not in a game or application),
		/// this method will never return any areas.
		/// </summary>
		/// <returns>The loaded areas.</returns>
		public List<AreaInfoEntry> GetLoadedAreas()
		{
			List<AreaInfoEntry> LoadedAreas = new List<AreaInfoEntry>();
			foreach (AreaInfoEntry Entry in AreaInfo.Entries)
			{
				if (Entry.Flags.HasFlag(AreaInfoFlags.IsLoaded))
				{
					LoadedAreas.Add(Entry);
				}
			}

			return LoadedAreas;
		}

		/// <summary>
		/// Gets a list of area information entries that have terrain tiles.
		/// </summary>
		/// <returns>The areas with terrain.</returns>
		public List<AreaInfoEntry> GetAreasWithTerrain()
		{
			List<AreaInfoEntry> TerrainAreas = new List<AreaInfoEntry>();
			foreach (AreaInfoEntry Entry in AreaInfo.Entries)
			{
				if (Entry.Flags.HasFlag(AreaInfoFlags.HasTerrainData))
				{
					TerrainAreas.Add(Entry);
				}
			}

			return TerrainAreas;
		}

		/// <summary>
		/// Gets the area info for the specified coordinates.
		/// </summary>
		/// <returns>The area info.</returns>
		/// <param name="InTileX">In tile x.</param>
		/// <param name="InTileY">In tile y.</param>
		public AreaInfoEntry GetAreaInfo(uint InTileX, uint InTileY)
		{
			return this.AreaInfo.GetAreaInfo(InTileX, InTileY);
		}

		/// <summary>
		/// Determines whether this instance has any terrain.
		/// </summary>
		/// <returns><c>true</c> if this instance has any terrain; otherwise, <c>false</c>.</returns>
		public bool HasAnyTerrain()
		{
			foreach (AreaInfoEntry Entry in AreaInfo.Entries)
			{
				if (Entry.Flags.HasFlag(AreaInfoFlags.HasTerrainData))
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
		/// <param name="InTileX">0-based x coordinate of the tile.</param>
		/// <param name="InTileY">0-based y coordinate of the tile.</param>
		public bool IsTilePopulated(uint InTileX, uint InTileY)
		{
			AreaInfoEntry InfoEntry = AreaInfo.GetAreaInfo(InTileX, InTileY);
			return InfoEntry.Flags.HasFlag(AreaInfoFlags.HasTerrainData);
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					bw.Write(this.Version.Serialize());
					bw.Write(this.Header.Serialize());
					bw.Write(this.AreaInfo.Serialize());
					bw.Write(this.WorldModelObjects.Serialize());

					if (WorldModelObjects.Filenames.Count > 0 && WorldModelObjectPlacementInfo != null)
					{
						bw.Write(this.WorldModelObjectPlacementInfo.Serialize());
					}
				}

				return ms.ToArray();
			}
		}
	}
}

