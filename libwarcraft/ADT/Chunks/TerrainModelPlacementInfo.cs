//
//  TerrainModelPlacementInfo.cs
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
using Warcraft.Core;
using System.Runtime.InteropServices;

namespace Warcraft.ADT.Chunks
{
	/// <summary>
	/// MMDF Chunk - Contains M2 model placement information
	/// </summary>
	public class TerrainModelPlacementInfo
	{
		/// <summary>
		/// Flags for the model
		/// </summary>
		[Flags]
		public enum MDDFFlags
		{
			/// <summary>
			/// Biodome flag
			/// </summary>
			MDDF_BioDome = 1,
			/// <summary>
			/// BRING ME A SHRUBBERY
			/// </summary>
			MMDF_Shrubbery = 2,
		}

		/// <summary>
		/// An entry struct containing information about the model
		/// </summary>
		public struct MDDFEntry
		{
			/// <summary>
			/// Specifies which model to use via the MMID chunk
			/// </summary>
			public int MMIDEntry;
			/// <summary>
			/// A unique actor ID for the model. Blizzard implements this as game global, but it's only checked in loaded tiles
			/// </summary>
			public int uniqueID;

			/// <summary>
			/// Position of the model
			/// </summary>
			public Vector3f position;
			/// <summary>
			/// Rotation of the model
			/// </summary>
			public Rotator rotation;

			/// <summary>
			/// Scale of the model. 1024 is the default value, equating to 1.0f. There is no uneven scaling of objects
			/// </summary>
			public int scale;
			/// <summary>
			/// MMDF flags for the object
			/// </summary>
			public MDDFFlags flags;
		}

		/// <summary>
		/// Size of the MDDF chunk
		/// </summary>
		public int size;

		/// <summary>
		/// Contains a list of MDDF entries with model placement information
		/// </summary>
		public List<MDDFEntry> entries;


		public TerrainModelPlacementInfo(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;

			//read size
			this.size = br.ReadInt32();

			//create a new empty list
			this.entries = new List<MDDFEntry>();

			int i = 0;
			while (i < this.size)
			{
				MDDFEntry entry = new MDDFEntry();

				entry.MMIDEntry = br.ReadInt32();
				entry.uniqueID = br.ReadInt32();

				entry.position.X = br.ReadSingle();
				entry.position.Y = br.ReadSingle();
				entry.position.Z = br.ReadSingle();

				entry.rotation.Pitch = br.ReadSingle();
				entry.rotation.Yaw = br.ReadSingle();
				entry.rotation.Roll = br.ReadSingle();

				entry.scale = br.ReadInt16();
				entry.flags = (MDDFFlags)br.ReadInt16();

				this.entries.Add(entry);

				i += Marshal.SizeOf(entry);
			}

			br.Close();
			adtStream.Close();
		}
	}

}

