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
using System;
using System.Collections.Generic;
using System.IO;

namespace Warcraft.ADT.Chunks.Subchunks
{
	/// <summary>
	/// MCVT Chunk - Contains heightmap information
	/// </summary>
	public class MapChunkHeightmap
	{
		/// <summary>
		/// An array of vertices
		/// </summary>
		public float[] vertices;

		/// <summary>
		/// Creates a new MCVT object from a file on disk and an offset into the file.
		/// </summary>
		/// <param name="adtFile">Path to the file on disk</param>                
		/// <param name="position">Offset into the file where the MCVT chunk begins</param>
		/// <returns>An MCVT object containing an array of vertices</returns>
		public MapChunkHeightmap(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;

			vertices = new float[144];

			for (int i = 0; i < 144; i++)
			{
				vertices[i] = br.ReadSingle();
			}
		}
	}
}

