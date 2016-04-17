//
//  TerrainTextures.cs
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
	/// MTEX Chunk - Contains a list of all referenced textures in this ADT.
	/// </summary>
	public class TerrainTextures
	{
		/// <summary>
		/// Size of the MTEX chunk.
		/// </summary>
		public int size;

		/// <summary>
		///A list of full paths to the textures referenced in this ADT.
		/// </summary>
		public List<string> fileNames;

		/// <summary>
		/// Creates a new MTEX object from a file path and offset into the file
		/// </summary>
		/// <param name="adtFile">Path to the file on disk</param>                
		/// <param name="position">Offset into the file where the MTEX chunk begins</param>
		/// <returns>An MTEX object containing a list of full texture paths</returns>
		public TerrainTextures(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;

			//read the MTEX size
			this.size = br.ReadInt32();

			//create an empty list
			this.fileNames = new List<string>();

			string str = "";
			//we add four, because otherwise we miss out on the last string because of the fact that the chunk identifier (MTEX) 
			//needs to be included in the calculations
			while (br.BaseStream.Position <= position + this.size + 4)
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

		public string GetFileName(int nameIndex, bool keepExtension)
		{
			string fileName = "";
			if (nameIndex <= this.fileNames.Count)
			{
				if (keepExtension == false)
				{
					fileName = this.fileNames[nameIndex].Substring(this.fileNames[nameIndex].LastIndexOf(@"\") + 1).Replace(".blp", "");
				}
				else
				{
					fileName = this.fileNames[nameIndex].Substring(this.fileNames[nameIndex].LastIndexOf(@"\") + 1);
				}                        
			}
			else
			{

			}   
			return fileName;
		}
	}
}

