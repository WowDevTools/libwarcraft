//
//  TerrainModelIndices.cs
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

namespace Warcraft.ADT.Chunks
{
	/// <summary>
	/// MMID Chunk - Contains a list of M2 model indexes
	/// </summary>
	public class TerrainModelIndices : IChunk
	{
		public const string Signature = "MMID";

		/// <summary>
		/// List indexes for models in an MMID chunk
		/// </summary>
		public List<uint> ModelFilenameOffsets = new List<uint>();

		public TerrainModelIndices()
		{

		}

		public TerrainModelIndices(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int offsetCount = inData.Length / 4;
					for (int i = 0; i < offsetCount; ++i)
					{
						ModelFilenameOffsets.Add(br.ReadUInt32());
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

