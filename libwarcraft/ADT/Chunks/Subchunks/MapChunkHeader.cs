//
//  MapChunkHeader.cs
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
using Warcraft.Core;
using System.IO;
using System.Collections.Generic;

namespace Warcraft.ADT.Chunks.Subchunks
{
	public class MapChunkHeader
	{
		/// <summary>
		/// Flags for the MCNK
		/// </summary>
		public MapChunkFlags Flags;

		/// <summary>
		/// Zero-based X position of the MCNK
		/// </summary>
		public uint MapIndexX;

		/// <summary>
		/// Zero-based Y position of the MCNK
		/// </summary>
		public uint MapIndexY;

		/// <summary>
		/// Alpha map layers in the MCNK
		/// </summary>
		public uint TextureLayerCount;

		/// <summary>
		/// Number of doodad references in the MCNK
		/// </summary>
		public uint ModelReferenceCount;

		/// <summary>
		/// The high res holes.
		/// </summary>
		public ulong HighResHoles;


		/// <summary>
		/// MCNK-based Offset of the MCVT Heightmap Chunk
		/// </summary>
		public uint HeightmapOffset;

		/// <summary>
		/// MCNK-based Offset of the MMCNR Normal map Chunk
		/// </summary>
		public uint VertexNormalOffset;

		/// <summary>
		/// MCNK-based Offset of the MCLY Alpha Map Layer Chunk
		/// </summary>
		public uint TextureLayersOffset;

		/// <summary>
		/// MCNK-based Offset of the MCRF Object References Chunk
		/// </summary>
		public uint ModelReferencesOffset;


		/// <summary>
		/// MCNK-based Offset of the MCAL Alpha Map Chunk
		/// </summary>
		public uint AlphaMapsOffset;

		/// <summary>
		/// Size of the Alpha Map chunk
		/// </summary>
		public uint AlphaMapsSize;


		/// <summary>
		/// MCNK-based Offset of the MCSH Static shadow Chunk. This is only set with flags MCNK_MCSH.
		/// </summary>
		public uint BakedShadowsOffset;

		/// <summary>
		/// Size of the shadow map chunk.
		/// </summary>
		public uint BakedShadowsSize;


		/// <summary>
		/// Area ID for the MCNK.
		/// </summary>
		public uint AreaID;

		/// <summary>
		/// Number of object references in this MCNK.
		/// </summary>
		public uint WorldModelObjectReferenceCount;

		/// <summary>
		/// Holes in the MCNK.
		/// </summary>
		public ushort LowResHoles;

		/// <summary>
		/// Unknown value, but it is used.
		/// </summary>
		public ushort Unknown;

		/// <summary>
		/// A low-quality texture map of the MCNK. Used with LOD.
		/// </summary>
		public ushort LowResTextureMap;

		/// <summary>
		/// It is not yet known what predTex does.
		/// </summary>
		public uint predTex;
		/// <summary>
		/// It is not yet known what noEffectDoodad does.
		/// </summary>
		public uint noEffectDoodad;

		/// <summary>
		/// MCNK-based Offset of the MCSE Sound Emitters Chunk
		/// </summary>
		public uint SoundEmittersOffset;
		/// <summary>
		/// Number of sound emitters in the MCNK
		/// </summary>
		public uint SoundEmitterCount;

		/// <summary>
		/// MCNK-based Offset of the MCLQ Liquid Chunk
		/// </summary>
		public uint LiquidOffset;
		/// <summary>
		/// Size of the liquid chunk. This is 8 when not used - if it is 8, use the newer MH2O chunk.
		/// </summary>
		public uint LiquidSize;

		/// <summary>
		/// The map tile position is a global offset that is applied to the entire heightmap to allow for
		/// far greater height differences in the world.
		/// </summary>
		public Vector3f MapTilePosition;

		/// <summary>
		/// MCNK-based Offset of the MCCV Chunk
		/// </summary>
		public uint VertexShadingOffset;

		/// <summary>
		/// MCNK-based Offset of the MCLV Chunk. Introduced in Cataclysm.
		/// </summary>
		public uint VertexLightingOffset;

		public MapChunkHeader(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Flags = (MapChunkFlags)br.ReadUInt32();
					this.MapIndexX = br.ReadUInt32();
					this.MapIndexY = br.ReadUInt32();
					this.TextureLayerCount = br.ReadUInt32();
					this.ModelReferenceCount = br.ReadUInt32();

					if (this.Flags.HasFlag(MapChunkFlags.UsesHighResHoles))
					{
						this.HighResHoles = br.ReadUInt64();
					}

					this.HeightmapOffset = br.ReadUInt32();
					this.VertexNormalOffset = br.ReadUInt32();
					this.TextureLayersOffset = br.ReadUInt32();
					this.ModelReferencesOffset = br.ReadUInt32();
					this.AlphaMapsOffset = br.ReadUInt32();
					this.AlphaMapsSize = br.ReadUInt32();
					this.BakedShadowsOffset = br.ReadUInt32();
					this.BakedShadowsSize = br.ReadUInt32();

					this.AreaID = br.ReadUInt32();
					this.WorldModelObjectReferenceCount = br.ReadUInt32();

					// TODO: Turn into bitmapped boolean field
					if (!this.Flags.HasFlag(MapChunkFlags.UsesHighResHoles))
					{
						this.LowResHoles = br.ReadUInt16();
					}

					this.Unknown = br.ReadUInt16();

					// TODO: This is a set of 8 by 8 2-bit integers. Shift and read into a byte array.
					this.LowResTextureMap = br.ReadUInt16();

					this.predTex = br.ReadUInt32();
					this.noEffectDoodad = br.ReadUInt32();

					this.SoundEmittersOffset = br.ReadUInt32();
					this.SoundEmitterCount = br.ReadUInt32();
					this.LiquidOffset = br.ReadUInt32();
					this.LiquidSize = br.ReadUInt32();

					this.MapTilePosition = br.ReadVector3f();

					if (this.Flags.HasFlag(MapChunkFlags.HasVertexShading))
					{
						this.VertexShadingOffset = br.ReadUInt32();
					}
				}
			}
		}

		/// <summary>
		/// Gets the size of the map chunk header.
		/// </summary>
		/// <returns>The size.</returns>
		public static int GetSize()
		{
			return 128;
		}
	}

	/// <summary>
	/// Flags available for this MCNK
	/// </summary>
	public enum MapChunkFlags : uint
	{
		/// <summary>
		/// Flags the MCNK as containing a static shadow map
		/// </summary>
		HasBakedShadows = 1,

		/// <summary>
		/// Flags the MCNK as impassible
		/// </summary>
		Impassible = 2,

		/// <summary>
		/// Flags the MCNK as a river
		/// </summary>
		IsRiver = 4,

		/// <summary>
		/// Flags the MCNK as an ocean
		/// </summary>
		IsOcean = 8,

		/// <summary>
		/// Flags the MCNK as magma
		/// </summary>
		IsMagma = 16,

		/// <summary>
		/// Flags the MCNK as slime
		/// </summary>
		IsSlime = 32,

		/// <summary>
		/// Flags the MCNK as containing an MCCV chunk
		/// </summary>
		HasVertexShading = 64,

		/// <summary>
		/// Unknown flag, but occasionally set.
		/// </summary>
		Unknown = 128,

		// 7 unused bits

		/// <summary>
		/// Disables repair of the alpha maps in this chunk.
		/// </summary>
		DoNotRepairAlphaMaps = 32768,

		/// <summary>
		/// Flags the MCNK for high-resolution holes. Introduced in WoW 5.3
		/// </summary>
		UsesHighResHoles = 65536,

		// 15 unused bits
	}
}

