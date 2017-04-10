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
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks
{
	/// <summary>
	/// MODF Chunk - Contains WMO model placement information
	/// </summary>
	public class TerrainWorldModelObjectPlacementInfo : IIFFChunk, IBinarySerializable
	{
		public const string Signature = "MODF";

		/// <summary>
		/// An array of WMO model information entries
		/// </summary>
		public List<WorldModelObjectPlacementEntry> Entries = new List<WorldModelObjectPlacementEntry>();

		public TerrainWorldModelObjectPlacementInfo()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainWorldModelObjectPlacementInfo"/> class.
		/// </summary>
		/// <param name="inData">Data.</param>
		public TerrainWorldModelObjectPlacementInfo(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					long entryCount = br.BaseStream.Length / WorldModelObjectPlacementEntry.GetSize();
					for (int i = 0; i < entryCount; ++i)
					{
						this.Entries.Add(new WorldModelObjectPlacementEntry(br.ReadBytes(WorldModelObjectPlacementEntry.GetSize())));
					}
				}
			}
        }

        public string GetSignature()
        {
        	return Signature;
        }

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					foreach (WorldModelObjectPlacementEntry entry in this.Entries)
					{
						bw.Write(entry.Serialize());
					}
				}

				return ms.ToArray();
			}
		}
	}

	/// <summary>
	/// An entry struct containing information about the WMO
	/// </summary>
	public class WorldModelObjectPlacementEntry : IBinarySerializable
	{
		/// <summary>
		/// Specifies which WHO to use via the MMID chunk
		/// </summary>
		public uint WorldModelObjectEntryIndex;

		/// <summary>
		/// A unique actor ID for the model. Blizzard implements this as game global, but it's only checked in loaded tiles.
		/// When not in use, it's set to -1.
		/// </summary>
		public int UniqueID;

		/// <summary>
		/// Position of the WMO
		/// </summary>
		public Vector3f Position;
		/// <summary>
		/// Rotation of the model
		/// </summary>
		public Rotator Rotation;

		/// <summary>
		/// The bounding box of the model.
		/// </summary>
		public Box BoundingBox;

		/// <summary>
		/// Flags of the model
		/// </summary>
		public WorldModelObjectFlags Flags;

		/// <summary>
		/// Doodadset of the model
		/// </summary>
		public ushort DoodadSet;

		/// <summary>
		/// Nameset of the model
		/// </summary>
		public ushort NameSet;

		/// <summary>
		/// An unused value. Seems to have data in Legion tiles.
		/// </summary>
		public ushort Unused;

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.WorldModelObjectPlacementEntry"/> class.
		/// </summary>
		/// <param name="data">Data.</param>
		public WorldModelObjectPlacementEntry(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.WorldModelObjectEntryIndex = br.ReadUInt32();
					this.UniqueID = br.ReadInt32();

					this.Position = br.ReadVector3f();
					this.Rotation = br.ReadRotator();
					this.BoundingBox = br.ReadBox();

					this.Flags = (WorldModelObjectFlags)br.ReadUInt16();
					this.DoodadSet = br.ReadUInt16();
					this.NameSet = br.ReadUInt16();
					this.Unused = br.ReadUInt16();
				}
			}
		}

		/// <summary>
		/// Gets the size of a placement entry.
		/// </summary>
		/// <returns>The size.</returns>
		public static int GetSize()
		{
			return 64;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					bw.Write(this.WorldModelObjectEntryIndex);
					bw.Write(this.UniqueID);

					bw.WriteVector3f(this.Position);
					bw.WriteRotator(this.Rotation);
					bw.WriteBox(this.BoundingBox);

					bw.Write((ushort)this.Flags);
					bw.Write(this.DoodadSet);
					bw.Write(this.NameSet);
					bw.Write(this.Unused);
				}

				return ms.ToArray();
			}
		}
	}

	/// <summary>
	/// Flags for the WMO
	/// </summary>
	[Flags]
	public enum WorldModelObjectFlags : ushort
	{
		Destructible = 1,
		UseLOD = 2,
		Unknown = 4
	}
}

