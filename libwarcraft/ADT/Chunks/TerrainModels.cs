//
//  TerrainModels.cs
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
	/// MMDX Chunk - Contains a list of all referenced M2 models in this ADT.
	/// </summary>
	public class TerrainModels
	{
		/// <summary>
		/// Size of the MMDX chunk.
		/// </summary>
		public int size;

		/// <summary>
		///A list of full paths to the M2 models referenced in this ADT.
		/// </summary>
		public List<string> fileNames;

		/// <summary>
		/// Creates a new MMDX object from a file path and offset into the file
		/// </summary>
		/// <param name="adtFile">Path to the file on disk</param>                
		/// <param name="position">Offset into the file where the MMDX chunk begins</param>
		/// <returns>An MMDX object containing a list of full M2 model paths</returns>
		public TerrainModels(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;

			//read the MMDX size
			this.size = br.ReadInt32();

			//create an empty list
			this.fileNames = new List<string>();

			string str = "";
			while (br.BaseStream.Position < position + this.size)
			{
				char letterChar = br.ReadChar();
				if (letterChar != Char.MinValue)
				{
					str = str + letterChar.ToString();
				}
				else
				{
					this.fileNames.Add(str);
					//clear string for a new filename
					str = "";
				}
			}

			br.Close();
			adtStream.Close();
		}
	}
}

