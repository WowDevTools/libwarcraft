//
//  WorldLODMapArea.cs
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

namespace Warcraft.WDL.Chunks
{
	public class WorldLODMapArea : IRIFFChunk, IBinarySerializable
	{
		public const string Signature = "MARE";

		public readonly List<short> HighResVertices = new List<short>();
		public readonly List<short> LowResVertices = new List<short>();

		public WorldLODMapArea()
		{

		}

		public WorldLODMapArea(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					// TODO: Verify if this isn't mapped the same way as ADT.Chunks.MapChunkHeightmap
					for (int y = 0; y < 17; ++y)
					{
						for (int x = 0; x < 17; ++x)
						{
							this.HighResVertices.Add(br.ReadInt16());
						}
					}

					for (int y = 0; y < 16; ++y)
					{
						for (int x = 0; x < 16; ++x)
						{
							this.LowResVertices.Add(br.ReadInt16());
						}
					}
				}
			}
        }

		public static int GetSize()
		{
			return (17 * 17) * sizeof(short) + (16 * 16) * sizeof(short);
		}

        public string GetSignature()
        {
        	return Signature;
        }

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
		            foreach (short lodVertex in HighResVertices)
		            {
						bw.Write(lodVertex);
		            }

		            foreach (short lodVertex in LowResVertices)
					{
						bw.Write(lodVertex);
					}
            	}

            	return ms.ToArray();
            }
		}
	}
}

