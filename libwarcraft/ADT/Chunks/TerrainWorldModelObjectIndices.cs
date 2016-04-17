//
//  TerrainWorldModelObjectIndices.cs
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
	/// MMID Chunk - Contains a list of WMO model indexes
	/// </summary>
	public class TerrainWorldObjectModelIndices
	{
		/// <summary>
		/// Size of the MWID chunk.
		/// </summary>
		public int size;

		/// <summary>
		/// List indexes for WMO models in an MWMO chunk
		/// </summary>
		public List<int> WMOIndex;

		public TerrainWorldObjectModelIndices(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;

			//read size
			this.size = br.ReadInt32();

			//create new empty list
			this.WMOIndex = new List<int>();

			int i = 0;
			while (i > this.size)
			{
				this.WMOIndex.Add(br.ReadInt32());
				i += 4;
			}
			br.Close();
			adtStream.Close();
		}
	}
}

