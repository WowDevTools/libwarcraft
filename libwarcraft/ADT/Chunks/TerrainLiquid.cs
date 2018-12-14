//
//  TerrainWater.cs
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
using System.IO;
using System.Collections.Generic;
using Warcraft.DBC.SpecialFields;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MH2O Chunk - Contains liquid information about the ADT file, superseding the older MCLQ chunk.
    /// </summary>
    public class TerrainLiquid : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MH2O";

        /// <summary>
        /// The liquid chunks in this map tile.
        /// </summary>
        public List<TerrainLiquidChunk> LiquidChunks = new List<TerrainLiquidChunk>();

        public TerrainLiquid()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainLiquid"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public TerrainLiquid(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    for (int i = 0; i < 256; ++i)
                    {
                        LiquidChunks.Add(new TerrainLiquidChunk(br.ReadBytes(TerrainLiquidChunk.GetSize())));
                    }

                    foreach (TerrainLiquidChunk liquidChunk in LiquidChunks)
                    {
                        br.BaseStream.Position = liquidChunk.WaterInstanceOffset;
                        for (int i = 0; i < liquidChunk.LayerCount; ++i)
                        {
                            byte[] instanceData = br.ReadBytes(TerrainLiquidInstance.GetSize());
                            liquidChunk.LiquidInstances.Add(new TerrainLiquidInstance(instanceData));
                        }

                        br.BaseStream.Position = liquidChunk.AttributesOffset;
                        if (liquidChunk.LayerCount > 0)
                        {
                            byte[] attributeData = br.ReadBytes(TerrainLiquidAttributes.GetSize());
                            liquidChunk.LiquidAttributes = new TerrainLiquidAttributes(attributeData);
                        }
                        else
                        {
                            liquidChunk.LiquidAttributes = new TerrainLiquidAttributes();
                        }
                    }
                }
            }
        }

        public string GetSignature()
        {
            return Signature;
        }
    }

    /// <summary>
    /// Terrain liquid chunk. Contains information about water and other liquids in a map tile.
    /// </summary>
    public class TerrainLiquidChunk
    {
        public uint WaterInstanceOffset;
        public uint LayerCount;
        public uint AttributesOffset;

        public List<TerrainLiquidInstance> LiquidInstances = new List<TerrainLiquidInstance>();
        public TerrainLiquidAttributes LiquidAttributes;

        public TerrainLiquidChunk(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    WaterInstanceOffset = br.ReadUInt32();
                    LayerCount = br.ReadUInt32();
                    AttributesOffset = br.ReadUInt32();
                }
            }
        }

        /// <summary>
        /// Gets the size of a chunk.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 12;
        }
    }

    public class TerrainLiquidAttributes
    {
        // These are most likely not just numbers, but matrices instead that map to vertex points in the chunk
        public ulong Fishable;
        public ulong Deep;

        public TerrainLiquidAttributes(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Fishable = br.ReadUInt64();
                    Deep = br.ReadUInt64();
                }
            }
        }

        public TerrainLiquidAttributes()
        {
            Fishable = 0;
            Deep = 0;
        }

        public static int GetSize()
        {
            return 16;
        }
    }

    public class TerrainLiquidInstance
    {
        /// <summary>
        /// The type of the liquid. Foreign key reference to LiquidTypeRec::ID.
        /// </summary>
        public ForeignKey<ushort> LiquidType;

        /// <summary>
        /// The liquid object. Foreign key reference to LiquidObjectRec::ID.
        /// </summary>
        public ForeignKey<ushort> LiquidObject;

        public Range HeightLevelRange;

        public byte XLiquidOffset;
        public byte YLiquidOffset;
        public byte Width;
        public byte Height;

        /// <summary>
        /// Offset to an 8 by 8 map of bit boolean values that determine whether or not
        /// an instance is filled or not. If the offset is 0, all tiles are filled.
        /// </summary>
        public uint LiquidBooleanMapOffset;
        public List<List<bool>> BooleanMap = new List<List<bool>>();

        /// <summary>
        /// Offset to the vertex data of this liquid instance. The format of the data is determined
        /// in LiquidMaterialRec::LiquidVertexFormat via LiquidTypeRec::MaterialID. As a side note,
        /// oceans (<see cref="LiquidType"/> = 2) have a hardcoded check that the LiquidVertexFormat also equals 2.
        ///
        /// Since the vertex data format is undetermined (and as such the length is also undetermined) without
        /// a DBC entry, it's up to external programs to read the correct values. You can also use the GameWorld wrapper
        /// class.
        /// </summary>
        public uint VertexDataOffset;
        public LiquidVertexData VertexData;

        public TerrainLiquidInstance(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    LiquidType = new ForeignKey<ushort>(DatabaseName.LiquidType, nameof(DBCRecord.ID), br.ReadUInt16());
                    LiquidObject = new ForeignKey<ushort>(DatabaseName.LiquidObject, nameof(DBCRecord.ID), br.ReadUInt16());

                    HeightLevelRange = new Range(br.ReadSingle(), br.ReadSingle());

                    XLiquidOffset = br.ReadByte();
                    YLiquidOffset = br.ReadByte();
                    Width = br.ReadByte();
                    Height = br.ReadByte();

                    LiquidBooleanMapOffset = br.ReadUInt32();
                    VertexDataOffset = br.ReadUInt32();

                    // TODO: Read boolean map

                    // TODO: [#9] Accept DBC liquid type and read vertex data
                }
            }
        }

        /// <summary>
        /// Gets the size of a liquid instance block.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 24;
        }
    }

    public abstract class LiquidVertexData
    {
    }

    public sealed class HeightDepthVertexData : LiquidVertexData
    {
        public List<float> Heightmap = new List<float>();
        public List<byte> Depthmap = new List<byte>();

        public HeightDepthVertexData(byte[] data, byte width, byte height)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int arrayEntryCount = (width + 1) * (height + 1);
                    for (int i = 0; i < arrayEntryCount; ++i)
                    {
                        Heightmap.Add(br.ReadSingle());
                    }

                    for (int i = 0; i < arrayEntryCount; ++i)
                    {
                        Depthmap.Add(br.ReadByte());
                    }
                }
            }
        }

        public static int GetBlockSize(byte width, byte height)
        {
            int arrayEntryCount = (width + 1) * (height + 1);

            return (sizeof(float) + sizeof(byte)) * arrayEntryCount;
        }
    }

    public sealed class HeightUVVertexData : LiquidVertexData
    {
        public List<float> Heightmap = new List<float>();
        public List<Tuple<ushort, ushort>> UVMap = new List<Tuple<ushort, ushort>>();

        public HeightUVVertexData(byte[] data, byte width, byte height)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int arrayEntryCount = (width + 1) * (height + 1);
                    for (int i = 0; i < arrayEntryCount; ++i)
                    {
                        Heightmap.Add(br.ReadSingle());
                    }

                    for (int i = 0; i < arrayEntryCount; ++i)
                    {
                        Tuple<ushort, ushort> uvEntry = new Tuple<ushort, ushort>(br.ReadUInt16(), br.ReadUInt16());
                        UVMap.Add(uvEntry);
                    }
                }
            }
        }

        public static int GetBlockSize(byte width, byte height)
        {
            int arrayEntryCount = (width + 1) * (height + 1);

            return (sizeof(float) + (sizeof(ushort) * 2)) * arrayEntryCount;
        }
    }

    public sealed class DepthOnlyVertexData : LiquidVertexData
    {
        public List<byte> Depthmap = new List<byte>();

        public DepthOnlyVertexData(byte[] data, byte width, byte height)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int arrayEntryCount = (width + 1) * (height + 1);

                    for (int i = 0; i < arrayEntryCount; ++i)
                    {
                        Depthmap.Add(br.ReadByte());
                    }
                }
            }
        }

        public static int GetBlockSize(byte width, byte height)
        {
            int arrayEntryCount = (width + 1) * (height + 1);

            return sizeof(byte) * arrayEntryCount;
        }
    }

    public sealed class HeightDepthUVVertexData : LiquidVertexData
    {
        public List<float> Heightmap = new List<float>();
        public List<byte> Depthmap = new List<byte>();
        public List<Tuple<ushort, ushort>> UVMap = new List<Tuple<ushort, ushort>>();

        public HeightDepthUVVertexData(byte[] data, byte width, byte height)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int arrayEntryCount = (width + 1) * (height + 1);
                    for (int i = 0; i < arrayEntryCount; ++i)
                    {
                        Heightmap.Add(br.ReadSingle());
                    }

                    for (int i = 0; i < arrayEntryCount; ++i)
                    {
                        Tuple<ushort, ushort> uvEntry = new Tuple<ushort, ushort>(br.ReadUInt16(), br.ReadUInt16());
                        UVMap.Add(uvEntry);
                    }

                    for (int i = 0; i < arrayEntryCount; ++i)
                    {
                        Depthmap.Add(br.ReadByte());
                    }
                }
            }
        }

        public static int GetBlockSize(byte width, byte height)
        {
            int arrayEntryCount = (width + 1) * (height + 1);

            return (sizeof(float) + sizeof(byte) + (sizeof(ushort) * 2)) * arrayEntryCount;
        }
    }
}

