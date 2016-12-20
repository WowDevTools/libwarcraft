//
//  MapChunkHeightmap.cs
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
	/// <summary>
	/// MCVT Chunk - Contains heightmap information
	///
	/// The vertices are arranged as two distinct grids, one
	/// inside the other.
	/// </summary>
	public class MapChunkHeightmap : IRIFFChunk
	{
		public const string Signature = "MCVT";
		/// <summary>
		/// The high res vertices, used when viewing a map tile up close. When these
		/// are visible, the low res vertices are also used.
		/// </summary>
		public List<float> HighResVertices = new List<float>();

		/// <summary>
		/// The low res vertices, used when viewing a map tile from far away.
		/// </summary>
		public List<float> LowResVertices = new List<float>();

		public MapChunkHeightmap()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.Subchunks.MapChunkHeightmap"/> class.
		/// </summary>
		/// <param name="inData">Data.</param>
		public MapChunkHeightmap(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
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
								this.HighResVertices.Add(br.ReadSingle());
							}
						}
						else
						{
							// Read a block of 8 low res vertices
							for (int x = 0; x < 8; ++x)
							{
								this.LowResVertices.Add(br.ReadSingle());
							}
						}
					}
				}
			}
        }

        public string GetSignature()
        {
        	return Signature;
        }
	}
}

