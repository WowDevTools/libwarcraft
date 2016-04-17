//
//  MapChunkAlphaMaps.cs
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
using System.Linq;

namespace Warcraft.ADT.Chunks.Subchunks
{
	/// <summary>
	/// MCLY Chunk - Contains definitions for the alpha map layers.
	/// </summary>
	public class MapChunkAlphaMapDefinitions
	{
		public int size;

		/// <summary>
		/// Chunk flags
		/// </summary>
		[Flags]
		public enum MCLYFlags
		{
			Anim45Rot = 0x001,

			Anim90Rot = 0x002,

			Anim180Rot = 0x004,

			AnimSpeed1 = 0x008,

			AnimSpeed2 = 0x010,

			AnimSpeed3 = 0x020,

			AnimDeferred = 0x040,

			EmissiveLayer = 0x080,

			UseAlpha = 0x100,

			CompressedAlpha = 0x200,

			SkyboxReflection = 0x400,
		}

		/// <summary>
		/// An array of alpha map layers in this MCNK.
		/// </summary>
		public List<MCLYEntry> Layer;

		/// <summary>
		/// A struct defining a layer entry
		/// </summary>
		public struct MCLYEntry
		{
			/// <summary>
			/// Index of the texture in the MTEX chunk
			/// </summary>
			public int textureID;
			/// <summary>
			/// Flags for the texture. Used for animation data.
			/// </summary>
			public MCLYFlags flags;
			/// <summary>
			/// Offset into MCAL where the alpha map begins.
			/// </summary>
			public int offsetMCAL;
			/// <summary>
			/// Ground effect ID. This is actually a padded Int16.
			/// </summary>
			public int effectID;
		}

		/// <summary>
		/// Creates a new MCLY object from a file on disk and an offset into the file.
		/// </summary>
		/// <param name="adtFile">Path to the file on disk</param>                
		/// <param name="position">Offset into the file where the MCLY chunk begins</param>
		/// <returns>An MCLY object containing an array of layer entries</returns>
		public MapChunkAlphaMapDefinitions(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;

			this.size = br.ReadInt32();
			int nLayers = this.size / 16;
			Layer = new List<MCLYEntry>();

			for (int i = 0; i < nLayers; i++)
			{
				MCLYEntry newEntry = new MCLYEntry();
				newEntry.textureID = br.ReadInt32();
				newEntry.flags = (MCLYFlags)br.ReadInt32();
				newEntry.offsetMCAL = br.ReadInt32();
				newEntry.effectID = br.ReadInt32();

				Layer.Add(newEntry);
			}
		}

		public MCLYEntry GetEntryForTextureID(int ID)
		{
			MCLYEntry matchingEntry = new MCLYEntry();
			bool foundEntry = false;
			//set the offset for the data that corresponds to texture i

			for (int l = 0; l < Layer.Count; l++)
			{
				if (ID == Layer.ElementAt(l).textureID)
				{
					matchingEntry = Layer.ElementAt(l);
					foundEntry = true;
				}
			}

			if (foundEntry == false)
			{
				//set all values to -1 to denote a missing or disabled chunk
				matchingEntry.effectID = -1;
				matchingEntry.flags = (MCLYFlags)0;
				matchingEntry.offsetMCAL = -1;
				matchingEntry.textureID = -1;

				return matchingEntry;
			}
			else
			{
				return matchingEntry;
			}                    
		}
	}
}

