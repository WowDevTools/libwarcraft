//
//  MapChunkVertexLighting.cs
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
using System.Collections.Generic;
using Warcraft.Core;

namespace Warcraft.ADT.Chunks.Subchunks
{
	public class MapChunkVertexLighting : IChunk
	{
		public const string Signature = "MCLV";

		public List<RGBA> HighResVertexLights = new List<RGBA>();
		public List<RGBA> LowResVertexLights = new List<RGBA>();

		public MapChunkVertexLighting(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (int y = 0; y < 16; ++y)
					{
						if (y % 2 == 0)
						{
							// Read a block of 9 high res vertices
							for (int x = 0; x < 9; ++x)
							{
								this.HighResVertexLights.Add(br.ReadRGBA());
							}
						}
						else
						{
							// Read a block of 8 low res vertices
							for (int x = 0; x < 8; ++x)
							{
								this.LowResVertexLights.Add(br.ReadRGBA());
							}
						}
					}
				}
			}
		}
	}
}

