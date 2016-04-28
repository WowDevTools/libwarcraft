//
//  MapChunkBakedShadows.cs
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
using System.IO;
using System.Collections;

namespace Warcraft.ADT.Chunks.Subchunks
{
	public class MapChunkBakedShadows : IChunk
	{
		public const string Signature = "MCSH";

		public List<List<bool>> ShadowMap = new List<List<bool>>();

		public MapChunkBakedShadows(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (int y = 0; y < 64; ++y)
					{						
						List<bool> mapRow = new List<bool>();
						for (int x = 0; x < 2; ++x)
						{
							BitArray valueBits = new BitArray(br.ReadInt32());

							for (int i = 0; i < 32; ++i)
							{
								mapRow.Add(valueBits.Get(i));
							}
						}

						ShadowMap.Add(mapRow);
					}
				}
			}
		}
	}
}

