//
//  TerrainWater.cs
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
using System.IO;
using System.Collections.Generic;

namespace Warcraft.ADT.Chunks
{
	/// <summary>
	/// MH2O Chunk - Contains liquid information about the ADT file, superseding the older MCLQ chunk.
	/// </summary>
	public class TerrainLiquid : IChunk
	{
		public const string Signature = "MH2O";

		/// <summary>
		/// The liquid chunks in this map tile.
		/// </summary>
		public List<TerrainLiquidChunk> LiquidChunks = new List<TerrainLiquidChunk>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainLiquid"/> class.
		/// </summary>
		/// <param name="data">Data.</param>
		public TerrainLiquid(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (int i = 0; i < 256; ++i)
					{
						LiquidChunks.Add(new TerrainLiquidChunk(br.ReadBytes(TerrainLiquidChunk.GetSize())));
					}

					foreach (TerrainLiquidChunk LiquidChunk in LiquidChunks)
					{
						br.BaseStream.Position = LiquidChunk.WaterInstanceOffset;
						for (int i = 0; i < LiquidChunk.LayerCount; ++i)
						{
							byte[] instanceData = br.ReadBytes(TerrainLiquidInstance.GetSize());
							LiquidChunk.LiquidInstances.Add(new TerrainLiquidInstance(instanceData));
						}

						br.BaseStream.Position = LiquidChunk.AttributesOffset;
						if (LiquidChunk.LayerCount > 0)
						{
							byte[] attributeData = br.ReadBytes(TerrainLiquidAttributes.GetSize());
							LiquidChunk.LiquidAttributes = new TerrainLiquidAttributes(attributeData);
						}
						else
						{
							LiquidChunk.LiquidAttributes = new TerrainLiquidAttributes();
						}
					}
				}
			}
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
					this.WaterInstanceOffset = br.ReadUInt32();
					this.LayerCount = br.ReadUInt32();
					this.AttributesOffset = br.ReadUInt32();
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
					this.Fishable = br.ReadUInt64();
					this.Deep = br.ReadUInt64();
				}
			}
		}

		public TerrainLiquidAttributes()
		{
			this.Fishable = 0;
			this.Deep = 0;
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
		public ushort LiquidType;

		/// <summary>
		/// The liquid object. Foreign key reference to LiquidObjectRec::ID.
		/// </summary>
		public ushort LiquidObject;

		public float MinHeightLevel;
		public float MaxHeightLevel;

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
		/// a DBC entry, it's up to external programs to read the correct values.
		/// </summary>
		public uint VertexDataOffset;
		public LiquidVertexData VertexData;

		public TerrainLiquidInstance(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.LiquidType = br.ReadUInt16();
					this.LiquidObject = br.ReadUInt16();

					this.MinHeightLevel = br.ReadSingle();
					this.MaxHeightLevel = br.ReadSingle();

					this.XLiquidOffset = br.ReadByte();
					this.YLiquidOffset = br.ReadByte();
					this.Width = br.ReadByte();
					this.Height = br.ReadByte();

					this.LiquidBooleanMapOffset = br.ReadUInt32();				
					this.VertexDataOffset = br.ReadUInt32();

					// TODO: Read boolean map
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

		public HeightDepthVertexData(byte[] data, byte Width, byte Height)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int ArrayEntryCount = (Width + 1) * (Height + 1);
					for (int i = 0; i < ArrayEntryCount; ++i)
					{
						Heightmap.Add(br.ReadSingle());
					}

					for (int i = 0; i < ArrayEntryCount; ++i)
					{
						Depthmap.Add(br.ReadByte());
					}
				}
			}
		}
	}

	public sealed class HeightUVVertexData : LiquidVertexData
	{
		public List<float> Heightmap = new List<float>();
		public List<Tuple<ushort, ushort>> UVMap = new List<Tuple<ushort, ushort>>();

		public HeightUVVertexData(byte[] data, byte Width, byte Height)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int ArrayEntryCount = (Width + 1) * (Height + 1);
					for (int i = 0; i < ArrayEntryCount; ++i)
					{
						Heightmap.Add(br.ReadSingle());
					}

					for (int i = 0; i < ArrayEntryCount; ++i)
					{
						Tuple<ushort, ushort> UVEntry = new Tuple<ushort, ushort>(br.ReadUInt16(), br.ReadUInt16());
						UVMap.Add(UVEntry);
					}
				}
			}
		}
	}

	public sealed class DepthOnlyVertexData : LiquidVertexData
	{
		public List<byte> Depthmap = new List<byte>();

		public DepthOnlyVertexData(byte[] data, byte Width, byte Height)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int ArrayEntryCount = (Width + 1) * (Height + 1);

					for (int i = 0; i < ArrayEntryCount; ++i)
					{
						Depthmap.Add(br.ReadByte());
					}
				}
			}
		}
	}

	public sealed class HeightDepthUVVertexData : LiquidVertexData
	{
		public List<float> Heightmap = new List<float>();
		public List<byte> Depthmap = new List<byte>();
		public List<Tuple<ushort, ushort>> UVMap = new List<Tuple<ushort, ushort>>();

		public HeightDepthUVVertexData(byte[] data, byte Width, byte Height)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int ArrayEntryCount = (Width + 1) * (Height + 1);
					for (int i = 0; i < ArrayEntryCount; ++i)
					{
						Heightmap.Add(br.ReadSingle());
					}

					for (int i = 0; i < ArrayEntryCount; ++i)
					{
						Tuple<ushort, ushort> UVEntry = new Tuple<ushort, ushort>(br.ReadUInt16(), br.ReadUInt16());
						UVMap.Add(UVEntry);
					}

					for (int i = 0; i < ArrayEntryCount; ++i)
					{
						Depthmap.Add(br.ReadByte());
					}
				}
			}
		}
	}
}

