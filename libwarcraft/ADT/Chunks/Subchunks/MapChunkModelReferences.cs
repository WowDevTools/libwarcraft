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

using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
	public class MapChunkModelReferences : IIFFChunk
	{
		public const string Signature = "MCRF";

		private byte[] Data;

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
			this.Data = inData;
		}

        public string GetSignature()
        {
        	return Signature;
        }

		public void PostLoadReferences(uint gameModelObjectCount, uint worldModelObjectCount)
		{
			using (MemoryStream ms = new MemoryStream(this.Data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (int i = 0; i < gameModelObjectCount; ++i)
					{
						this.GameModelObjectReferences.Add(br.ReadUInt32());
					}

					for (int i = 0; i < worldModelObjectCount; ++i)
					{
						this.WorldModelObjectReferences.Add(br.ReadUInt32());
					}
				}
			}
		}
	}
}

