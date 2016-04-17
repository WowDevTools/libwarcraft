//
//  TerrainMapChunk.cs
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
using Warcraft.ADT.Chunks.Subchunks;
using Warcraft.Core;

namespace Warcraft.ADT.Chunks
{
	/// <summary>
	/// MCNK Chunk - Main map chunk which contains a number of smaller subchunks. 256 of these are present in an ADT file.
	/// </summary>
	public class TerrainMapChunk
	{
		/// <summary>
		/// Header contains information about the MCNK and its subchunks such as offsets, position and flags.
		/// </summary>
		public MCNKHeader Header;
		/// <summary>
		/// Chunks contains references to the different subchunks of the MCNK.
		/// </summary>
		public MCNKChunks Chunks;

		/// <summary>
		/// Flags available for this MCNK
		/// </summary>
		public enum MCNKFlags
		{
			/// <summary>
			/// Flags the MCNK as containing a static shadow map
			/// </summary>
			MCHNK_MCSH = 1,
			/// <summary>
			/// Flags the MCNK as impassible
			/// </summary>
			MCNK_Impassible = 2,
			/// <summary>
			/// Flags the MCNK as a river
			/// </summary>
			MCNK_River = 4,
			/// <summary>
			/// Flags the MCNK as an ocean
			/// </summary>
			MCNK_Ocean = 8,
			/// <summary>
			/// Flags the MCNK as magma
			/// </summary>
			MCNK_Magma = 16,
			/// <summary>
			/// Flags the MCNK as containing an MCCV chunk
			/// </summary>
			MCNK_MCCV = 32,
			/// <summary>
			/// Flags the MCNK for high-resolution holes. Introduced in WoW 5.3
			/// </summary>
			MCNK_HighResHoles = 64,
			//introduced in WoW 5.3. Shouldn't be needed.
		}

		/// <summary>
		/// The header of the MCNK
		/// </summary>
		public struct MCNKHeader
		{
			//header
			/// <summary>
			/// Size of the MCNK
			/// </summary>
			public int size;

			/// <summary>
			/// Flags for the MCNK
			/// </summary>
			public int flags;
			/// <summary>
			/// Zero-based X position of the MCNK
			/// </summary>
			public int PositionX;
			/// <summary>
			/// Zero-based Y position of the MCNK
			/// </summary>
			public int PositionY;
			/// <summary>
			/// Alpha map layers in the MCNK
			/// </summary>
			public int layers;
			/// <summary>
			/// Number of doodad references in the MCNK
			/// </summary>
			public int DoodadReferences;

			/// <summary>
			/// MCNK-based Offset of the MCVT Heightmap Chunk
			/// </summary>
			public int HeightChunkOffset;
			/// <summary>
			/// MCNK-based Offset of the MMCNR Normal map Chunk
			/// </summary>
			public int NormalChunkOffset;
			/// <summary>
			/// MCNK-based Offset of the MCLY Alpha Map Layer Chunk
			/// </summary>
			public int LayerChunkOffset;
			/// <summary>
			/// MCNK-based Offset of the MCRF Object References Chunk
			/// </summary>
			public int MapReferencesChunkOffset;

			/// <summary>
			/// MCNK-based Offset of the MCAL Alpha Map Chunk
			/// </summary>
			public int AlphaMapsChunkOffset;
			/// <summary>
			/// Size of the Alpha Map chunk
			/// </summary>
			public int sizeAlpha;

			/// <summary>
			/// MCNK-based Offset of the MCSH Static shadow Chunk. This is only set with flags MCNK_MCSH.
			/// </summary>
			public int ShadowMapChunkOffset;
			/// <summary>
			/// Size of the shadow map chunk.
			/// </summary>
			public int sizeShadow;

			/// <summary>
			/// Area ID for the MCNK.
			/// </summary>
			public int areaID;
			/// <summary>
			/// Number of object references in this MCNK.
			/// </summary>
			public int mapObjectReferences;
			/// <summary>
			/// Holes in the MCNK.
			/// </summary>
			public int holes;

			/// <summary>
			/// A low-quality texture map of the MCNK. Used with LOD.
			/// </summary>
			public UInt16[] LowQTextureMap;

			/// <summary>
			/// It is not yet known what predTex does.
			/// </summary>
			public int predTex;
			/// <summary>
			/// It is not yet known what noEffectDoodad does.
			/// </summary>
			public int noEffectDoodad;

			/// <summary>
			/// MCNK-based Offset of the MCSE Sound Emitters Chunk
			/// </summary>
			public int SoundEmittersChunkOffset;
			/// <summary>
			/// Number of sound emitters in the MCNK
			/// </summary>
			public int nSoundEmitters;

			/// <summary>
			/// MCNK-based Offset of the MCLQ Liquid Chunk
			/// </summary>
			public int LiquidChunkOffset;
			/// <summary>
			/// Size of the liquid chunk. This is 8 when not used - if it is 8, use the newer MH2O chunk.
			/// </summary>
			public int sizeLiquid;

			/// <summary>
			/// A Vector3f of the position.
			/// </summary>
			public Vector3f Position;

			/// <summary>
			/// MCNK-based Offset of the MCCV Chunk
			/// </summary>
			public int MCCVChunkOffset;
			/// <summary>
			/// MCNK-based Offset of the MCLV Chunk. Introduced in Cataclysm.
			/// </summary>
			public int MCLVChunkOffset;

			/// <summary>
			/// Unknown int 1.
			/// </summary>
			public int unknown1;
			/// <summary>
			/// Unknown int 2.
			/// </summary>
			public int unknown2;
		}

		/// <summary>
		/// Subchunks of the MCNK
		/// </summary>
		public struct MCNKChunks
		{
			/// <summary>
			/// Heightmap Chunk
			/// </summary>
			public MapChunkHeightmap HeightChunk;
			/// <summary>
			/// Normal map chunk
			/// </summary>
			//public MCNR NormalChunk;
			/// <summary>
			/// Alphamap Layer chunk
			/// </summary>
			public MapChunkAlphaMapDefinitions LayerChunk;
			/// <summary>
			/// Map Object References chunk
			/// </summary>
			//public MCRF MapReferencesChunk;
			/// <summary>
			/// Alphamap chunk
			/// </summary>
			public MapChunkAlphaMaps AlphaChunk;
			/// <summary>
			/// Sound Emitter Chunk
			/// </summary>
			//public MCSE SoundEmitterChunk;
			/// <summary>
			/// Liquid Chunk
			/// </summary>
			//public MCLQ LiquidChunk;
			/// <summary>
			/// MCCV chunk
			/// </summary>
			//public MCCV MCCVChunk;
			/// <summary>
			/// MCLV chunk
			/// </summary>
			//public MCLV MCLVChunk;
		}


		/// <summary>
		/// Creates a new MCNK object from a file on disk and an offset into the file.
		/// </summary>
		/// <param name="adtFile">Path to the file on disk</param>                
		/// <param name="position">Offset into the file where the MCNK chunk begins</param>
		/// <returns>An MCNK object containing a header and the subchunks</returns>
		public TerrainMapChunk(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;
			int MCNKStart = (int)br.BaseStream.Position;

			//read size of the chunk
			this.Header.size = br.ReadInt32();



			//read the ADT flags
			this.Header.flags = br.ReadInt32();

			//read the X coordinate
			this.Header.PositionX = br.ReadInt32();

			//read the Y coordinate
			this.Header.PositionY = br.ReadInt32();

			//read the number of alpha layers
			this.Header.layers = br.ReadInt32();

			//read the number of doodad references
			this.Header.DoodadReferences = br.ReadInt32();

			//read height data offset - this is a local offset, not a global one.
			this.Header.HeightChunkOffset = br.ReadInt32();

			//read vertex normals offset - this is a local offset, not a global one.
			this.Header.NormalChunkOffset = br.ReadInt32();

			//read texture layer definitions offset - this is a local offset, not a global one.
			this.Header.LayerChunkOffset = br.ReadInt32();

			//read M2 and WMO references. This is used to determine what is drawn in every MCNK
			this.Header.MapReferencesChunkOffset = br.ReadInt32();

			//read alpha maps offset - this is a local offset, not a global one.
			this.Header.AlphaMapsChunkOffset = br.ReadInt32();
			//make sure we're at offset 0x028 from MCNK start here - the above chunks should manage that and work on the local stream
			this.Header.sizeAlpha = br.ReadInt32();

			//read static shadow maps offset - this is a local offset, not a global one.
			this.Header.ShadowMapChunkOffset = br.ReadInt32();
			//make sure we're at offset 0x030 here, same reason as above
			this.Header.sizeShadow = br.ReadInt32();

			//read the Area ID
			this.Header.areaID = br.ReadInt32();

			//read map object reference count
			this.Header.mapObjectReferences = br.ReadInt32();

			//read holes (count?)
			this.Header.holes = br.ReadInt32();

			//read LQ texture map
			UInt16[] LQTexBuilder = new UInt16[16];
			for (int i = 0; i < 16; i++)
			{
				LQTexBuilder[i] = (ushort)br.ReadInt16();
			}
			this.Header.LowQTextureMap = LQTexBuilder;

			//read predTex
			this.Header.predTex = br.ReadInt32();

			//read noEffectDoodad
			this.Header.noEffectDoodad = br.ReadInt32();

			//read sound emitters offset - this is a local offset, not a global one.
			this.Header.SoundEmittersChunkOffset = br.ReadInt32();
			//read sound emitter count - make sure MCSE puts us at offset 0x05C
			this.Header.nSoundEmitters = br.ReadInt32();

			//read liquid data offset - this is a local offset, not a global one.
			this.Header.LiquidChunkOffset = br.ReadInt32();
			//read liquid size - make sure MCLQ puts us at offset 0x064
			//if this is 8, we'll be using the new MH2O chunk instead.
			this.Header.sizeLiquid = br.ReadInt32();

			//read position
			float x = br.ReadSingle();
			float y = br.ReadSingle();
			float z = br.ReadSingle();

			this.Header.Position = new Vector3f(x, y, z);

			//read final offsets - these are local offsets, not global ones.
			this.Header.MCCVChunkOffset = br.ReadInt32();
			this.Header.MCLVChunkOffset = br.ReadInt32();

			this.Header.unknown1 = br.ReadInt32();
			this.Header.unknown2 = br.ReadInt32();


			//Fill the chunks
			this.Chunks.HeightChunk = new MapChunkHeightmap(adtFile, MCNKStart + this.Header.HeightChunkOffset);
			this.Chunks.LayerChunk = new MapChunkAlphaMapDefinitions(adtFile, MCNKStart + this.Header.LayerChunkOffset);
			this.Chunks.AlphaChunk = new MapChunkAlphaMaps(adtFile, MCNKStart + this.Header.AlphaMapsChunkOffset);

		}

		public MapChunkAlphaMapDefinitions.MCLYEntry[] GetTextureLayers()
		{
			int layerCount = this.Chunks.LayerChunk.Layer.Count;
			MapChunkAlphaMapDefinitions.MCLYEntry[] layers = new MapChunkAlphaMapDefinitions.MCLYEntry[layerCount];

			for (int i = 0; i < layerCount; i++)
			{
				layers[i] = this.Chunks.LayerChunk.Layer[i];
			}

			return layers;
		}
	}
}

