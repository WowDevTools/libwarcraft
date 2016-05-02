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

namespace Warcraft.WDT
{
	public class WDT
	{
		public TerrainVersion Version;
		public WDTHeader Header;
		public WDTMainChunk MainChunk;
		public TerrainWorldModelObjects WorldModelObjects;
		public TerrainWorldModelObjectPlacementInfo WorldModelObjectPlacementInfo;

		public WDT(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Version = (TerrainVersion)br.ReadTerrainChunk();
					this.Header = (WDTHeader)br.ReadTerrainChunk();
					this.MainChunk = (WDTMainChunk)br.ReadTerrainChunk();
					this.WorldModelObjects = (TerrainWorldModelObjects)br.ReadTerrainChunk();

					if (this.WorldModelObjects.Filenames.Count > 0)
					{
						this.WorldModelObjectPlacementInfo = (TerrainWorldModelObjectPlacementInfo)br.ReadTerrainChunk();
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
					bw.Write(this.Version.Serialize());
					bw.Write(this.Header.Serialize());
					bw.Write(this.MainChunk.Serialize());
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

