//
//  TerrainWorldModelObjects.cs
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
	/// MWMO Chunk - Contains a list of all referenced WMO models in this ADT.
	/// </summary>
	public class TerrainWorldModelObjects
	{
		/// <summary>
		/// Size of the MWMO chunk.
		/// </summary>
		public int size;

		/// <summary>
		///A list of full paths to the M2 models referenced in this ADT.
		/// </summary>
		public List<string> fileNames;

		public TerrainWorldModelObjects(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;

			//read the MWMO size
			this.size = br.ReadInt32();

			//create an empty list
			this.fileNames = new List<string>();

			string str = "";
			int i = 0;

			while (i < +this.size)
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
				i += 1;
			}

			br.Close();
			adtStream.Close();
		}
	}
}

