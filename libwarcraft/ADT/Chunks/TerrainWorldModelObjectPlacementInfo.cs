//
//  TerrainWorldModelObjectPlacementInfo.cs
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
	/// MODF Chunk - Contains WMO model placement information
	/// </summary>
	public class TerrainWorldModelObjectPlacementInfo
	{

		/// <summary>
		/// Flags for the WMO
		/// </summary>
		[Flags]
		public enum MODFFlags
		{
			/// <summary>
			/// Flag if the WMO is a destructible WMO
			/// </summary>
			MODF_Destructible = 1,
		}

		/// <summary>
		/// An entry struct containing information about the WMO
		/// </summary>
		public struct MODFEntry
		{
			/// <summary>
			/// Specifies which WHO to use via the MMID chunk
			/// </summary>
			public uint MWIDEntry;
			/// <summary>
			/// A unique actor ID for the model. Blizzard implements this as game global, but it's only checked in loaded tiles
			/// </summary>
			public uint uniqueID;

			/// <summary>
			/// Position of the WMO
			/// </summary>
			public Vector3f position;
			/// <summary>
			/// Rotation of the model
			/// </summary>
			public Rotator rotation;

			/// <summary>
			/// Lower bounds of the model
			/// </summary>
			public Vector3f lowerBounds;
			/// <summary>
			/// Upper of the model
			/// </summary>
			public Vector3f upperBounds;

			/// <summary>
			/// Flags of the model
			/// </summary>
			public MODFFlags flags;
			/// <summary>
			/// Doodadset of the model
			/// </summary>
			public int doodadSet;
			/// <summary>
			/// Nameset of the model
			/// </summary>
			public int nameSet;
			/// <summary>
			/// A bit of padding in the chunk
			/// </summary>
			public int padding;
		}

		/// <summary>
		/// Size of the MODF chunk.
		/// </summary>
		public int size;

		/// <summary>
		/// An array of WMO model information entries
		/// </summary>
		public List<MODFEntry> entries;

		public TerrainWorldModelObjectPlacementInfo(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;

			//read size
			this.size = br.ReadInt32();

			//create a new empty list
			this.entries = new List<MODFEntry>();

			int i = 0;
			while (i < this.size)
			{
				MODFEntry entry = new MODFEntry();

				entry.MWIDEntry = br.ReadUInt32();
				entry.uniqueID = br.ReadUInt32();

				entry.position.X = br.ReadSingle();
				entry.position.Y = br.ReadSingle();
				entry.position.Z = br.ReadSingle();

				entry.rotation.Pitch = br.ReadSingle();
				entry.rotation.Yaw = br.ReadSingle();
				entry.rotation.Roll = br.ReadSingle();

				entry.lowerBounds.X = br.ReadSingle();
				entry.lowerBounds.Y = br.ReadSingle();
				entry.lowerBounds.Z = br.ReadSingle();

				entry.upperBounds.X = br.ReadSingle();
				entry.upperBounds.Y = br.ReadSingle();
				entry.upperBounds.Z = br.ReadSingle();

				entry.flags = (MODFFlags)br.ReadUInt16();
				entry.doodadSet = br.ReadUInt16();
				entry.nameSet = br.ReadUInt16();
				entry.padding = br.ReadUInt16();

				this.entries.Add(entry);

				i += Marshal.SizeOf(entry);
			}

			br.Close();
			adtStream.Close();
		}
	}
}

