//
//  TerrainMapChunkOffsets.cs
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
	/// MCIN Chunk - Contains a list of all MCNKs with associated information in the ADT file.
	/// </summary>
	public class TerrainMapChunkOffsets
	{
		/// <summary>
		/// Size of the MCIN chunk
		/// </summary>
		public int size;

		/// <summary>
		/// A struct containing information about the referenced MCNK
		/// </summary>
		public struct MCINEntry
		{
			/// <summary>
			/// Absolute offset of the MCNK
			/// </summary>
			public int MCNKOffset;
			/// <summary>
			/// Size of the MCNK
			/// </summary>
			public int size;
			/// <summary>
			/// Flags of the MCNK. This is only set on the client, and is as such always 0.
			/// </summary>
			public int flags;
			/// <summary>
			/// Async loading ID of the MCNK. This is only set on the client, and is as such always 0.
			/// </summary>
			public int asyncID;
		}

		/// <summary>
		/// An array of 256 MCIN entries, containing MCNK offsets and sizes.
		/// </summary>
		public List<MCINEntry> entries;

		/// <summary>
		/// Creates a new MCIN object from a file path and offset into the file
		/// </summary>
		/// <param name="adtFile">Path to the file on disk</param>                
		/// <param name="position">Offset into the file where the MCIN chunk begins</param>
		/// <returns>An MCIN object containing an array with information about all MCNK chunks</returns>
		public TerrainMapChunkOffsets(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;

			//read size, n of entries is size / 16
			this.size = br.ReadInt32();                    
			int nEntries = size / 16;
			entries = new List<MCINEntry>();

			for (int i = 0; i < nEntries; i++)
			{
				MCINEntry entry = new MCINEntry();

				entry.MCNKOffset = br.ReadInt32();
				entry.size = br.ReadInt32();
				entry.flags = br.ReadInt32();
				entry.asyncID = br.ReadInt32();

				entries.Add(entry);
			}
			br.Close();
			adtStream.Close();
		}
	}
}

