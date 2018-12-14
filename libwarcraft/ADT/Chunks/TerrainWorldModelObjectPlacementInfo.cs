//
//  TerrainWorldModelObjectPlacementInfo.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MODF Chunk - Contains WMO model placement information
    /// </summary>
    public class TerrainWorldModelObjectPlacementInfo : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MODF";

        /// <summary>
        /// An array of WMO model information entries
        /// </summary>
        public List<WorldModelObjectPlacementEntry> Entries = new List<WorldModelObjectPlacementEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainWorldModelObjectPlacementInfo"/> class.
        /// </summary>
        public TerrainWorldModelObjectPlacementInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainWorldModelObjectPlacementInfo"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
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
                        Entries.Add(new WorldModelObjectPlacementEntry(br.ReadBytes(WorldModelObjectPlacementEntry.GetSize())));
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
                    foreach (WorldModelObjectPlacementEntry entry in Entries)
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
        public Vector3 Position;
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
        /// <param name="data">ExtendedData.</param>
        public WorldModelObjectPlacementEntry(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    WorldModelObjectEntryIndex = br.ReadUInt32();
                    UniqueID = br.ReadInt32();

                    Position = br.ReadVector3();
                    Rotation = br.ReadRotator();
                    BoundingBox = br.ReadBox();

                    Flags = (WorldModelObjectFlags)br.ReadUInt16();
                    DoodadSet = br.ReadUInt16();
                    NameSet = br.ReadUInt16();
                    Unused = br.ReadUInt16();
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
                    bw.Write(WorldModelObjectEntryIndex);
                    bw.Write(UniqueID);

                    bw.WriteVector3(Position);
                    bw.WriteRotator(Rotation);
                    bw.WriteBox(BoundingBox);

                    bw.Write((ushort)Flags);
                    bw.Write(DoodadSet);
                    bw.Write(NameSet);
                    bw.Write(Unused);
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

