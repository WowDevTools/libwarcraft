//
//  MapChunkHeader.cs
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

using System.IO;
using System.Numerics;
using Warcraft.Core.Extensions;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// Header data for a map chunk.
    /// </summary>
    public class MapChunkHeader
    {
        /// <summary>
        /// Gets or sets flags for the MCNK.
        /// </summary>
        public MapChunkFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the zero-based X position of the MCNK.
        /// </summary>
        public uint MapIndexX { get; set; }

        /// <summary>
        /// Gets or sets the zero-based Y position of the MCNK.
        /// </summary>
        public uint MapIndexY { get; set; }

        /// <summary>
        /// Gets or sets the number of alpha map layers in the MCNK.
        /// </summary>
        public uint TextureLayerCount { get; set; }

        /// <summary>
        /// Gets or sets the number of doodad references in the MCNK.
        /// </summary>
        public uint ModelReferenceCount { get; set; }

        /// <summary>
        /// Gets or sets the high res holes. This is a bitmap of boolean values.
        /// </summary>
        public ulong HighResHoles { get; set; }

        /// <summary>
        /// Gets or sets the relative offset of the MCVT Heightmap Chunk.
        /// </summary>
        public uint HeightmapOffset { get; set; }

        /// <summary>
        /// Gets or sets the relative offset of the MMCNR Normal map Chunk.
        /// </summary>
        public uint VertexNormalOffset { get; set; }

        /// <summary>
        /// Gets or sets the relative offset of the MCLY Alpha Map Layer Chunk.
        /// </summary>
        public uint TextureLayersOffset { get; set; }

        /// <summary>
        /// Gets or sets the relative offset of the MCRF Object References Chunk.
        /// </summary>
        public uint ModelReferencesOffset { get; set; }

        /// <summary>
        /// Gets or sets the relative offset of the MCAL Alpha Map Chunk.
        /// </summary>
        public uint AlphaMapsOffset { get; set; }

        /// <summary>
        /// Gets or sets the size of the Alpha Map chunk.
        /// </summary>
        public uint AlphaMapsSize { get; set; }

        /// <summary>
        /// Gets or sets the relative offset of the MCSH Static shadow Chunk. This is only set with flags MCNK_MCSH.
        /// </summary>
        public uint BakedShadowsOffset { get; set; }

        /// <summary>
        /// Gets or sets the size of the shadow map chunk.
        /// </summary>
        public uint BakedShadowsSize { get; set; }

        /// <summary>
        /// Gets or sets the area ID for the MCNK.
        /// </summary>
        public uint AreaID { get; set; }

        /// <summary>
        /// Gets or sets the number of object references in this MCNK.
        /// </summary>
        public uint WorldModelObjectReferenceCount { get; set; }

        /// <summary>
        /// Gets or sets low-resolution holes in the MCNK. This is a bitmap of boolean values.
        /// </summary>
        public ushort LowResHoles { get; set; }

        /// <summary>
        /// Gets or sets an unknown value. It is used, but it's unclear for what.
        /// </summary>
        public ushort Unknown { get; set; }

        /// <summary>
        /// Gets or sets a low-quality texture map of the MCNK. Used with LOD.
        /// </summary>
        public ushort LowResTextureMap { get; set; }

        /// <summary>
        /// Gets or sets it PredTex. It is not yet known what PredTex does.
        /// </summary>
        public uint PredTex { get; set; }

        /// <summary>
        /// Gets or sets NoEffectDoodad. It is not yet known what NoEffectDoodad does.
        /// </summary>
        public uint NoEffectDoodad { get; set; }

        /// <summary>
        /// Gets or sets the relative offset of the MCSE Sound Emitters Chunk.
        /// </summary>
        public uint SoundEmittersOffset { get; set; }

        /// <summary>
        /// Gets or sets the number of sound emitters in the MCNK.
        /// </summary>
        public uint SoundEmitterCount { get; set; }

        /// <summary>
        /// Gets or sets the relative offset of the MCLQ Liquid Chunk.
        /// </summary>
        public uint LiquidOffset { get; set; }

        /// <summary>
        /// Gets or sets the size of the liquid chunk. This is 8 when not used - if it is 8, use the newer MH2O chunk.
        /// </summary>
        public uint LiquidSize { get; set; }

        /// <summary>
        /// Gets or sets the map tile position. This position is a global offset that is applied to the entire heightmap
        /// to allow for far greater height differences in the world.
        /// </summary>
        public Vector3 MapTilePosition { get; set; }

        /// <summary>
        /// Gets or sets the relative offset of the MCCV Chunk.
        /// </summary>
        public uint VertexShadingOffset { get; set; }

        /// <summary>
        /// Gets or sets the relative offset of the MCLV Chunk. Introduced in Cataclysm.
        /// </summary>
        public uint VertexLightingOffset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkHeader"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
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

                    this.PredTex = br.ReadUInt32();
                    this.NoEffectDoodad = br.ReadUInt32();

                    this.SoundEmittersOffset = br.ReadUInt32();
                    this.SoundEmitterCount = br.ReadUInt32();
                    this.LiquidOffset = br.ReadUInt32();
                    this.LiquidSize = br.ReadUInt32();

                    this.MapTilePosition = br.ReadVector3();

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
}
