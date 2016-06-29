//
//  MapChunkModelReferences.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
	public class MapChunkModelReferences : IRIFFChunk
	{
		public const string Signature = "MCRF";

		private byte[] data;

		public List<uint> GameModelObjectReferences = new List<uint>();
		public List<uint> WorldModelObjectReferences = new List<uint>();

		public MapChunkModelReferences()
		{

		}

		public MapChunkModelReferences(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
		{
			this.data = inData;
		}

        public string GetSignature()
        {
        	return Signature;
        }

		public void PostLoadReferences(uint GameModelObjectCount, uint WorldModelObjectCount)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (int i = 0; i < GameModelObjectCount; ++i)
					{
						GameModelObjectReferences.Add(br.ReadUInt32());
					}

					for (int i = 0; i < WorldModelObjectCount; ++i)
					{
						WorldModelObjectReferences.Add(br.ReadUInt32());
					}
				}
			}
		}
	}
}

