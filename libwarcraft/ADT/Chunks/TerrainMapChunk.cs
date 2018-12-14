//
//  TerrainMapChunk.cs
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
using Warcraft.ADT.Chunks.Subchunks;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MCNK Chunk - Main map chunk which contains a number of smaller subchunks. 256 of these are present in an ADT file.
    /// </summary>
    public class TerrainMapChunk : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCNK";
        /// <summary>
        /// Header contains information about the MCNK and its subchunks such as offsets, position and flags.
        /// </summary>
        public MapChunkHeader Header;

        /// <summary>
        /// Heightmap Chunk
        /// </summary>
        public MapChunkHeightmap Heightmap;

        /// <summary>
        /// Normal map chunk
        /// </summary>
        public MapChunkVertexNormals VertexNormals;

        /// <summary>
        /// Alphamap Layer chunk
        /// </summary>
        public MapChunkTextureLayers TextureLayers;

        /// <summary>
        /// Map Object References chunk
        /// </summary>
        public MapChunkModelReferences ModelReferences;

        /// <summary>
        /// Alphamap chunk
        /// </summary>
        public MapChunkAlphaMaps AlphaMaps;

        /// <summary>
        /// The baked shadows.
        /// </summary>
        public MapChunkBakedShadows BakedShadows;

        /// <summary>
        /// Sound Emitter Chunk
        /// </summary>
        public MapChunkSoundEmitters SoundEmitters;

        /// <summary>
        /// Liquid Chunk
        /// </summary>
        public MapChunkLiquids Liquid;

        /// <summary>
        /// The vertex shading chunk.
        /// </summary>
        public MapChunkVertexShading VertexShading;

        /// <summary>
        /// The vertex lighting chunk
        /// </summary>
        public MapChunkVertexLighting VertexLighting;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainMapChunk"/> class.
        /// </summary>
        public TerrainMapChunk()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainMapChunk"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public TerrainMapChunk(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Header = new MapChunkHeader(br.ReadBytes(MapChunkHeader.GetSize()));

                    if (Header.HeightmapOffset > 0)
                    {
                        br.BaseStream.Position = Header.HeightmapOffset;
                        Heightmap = br.ReadIFFChunk<MapChunkHeightmap>();
                    }

                    if (Header.VertexNormalOffset > 0)
                    {
                        br.BaseStream.Position = Header.VertexNormalOffset;
                        VertexNormals = br.ReadIFFChunk<MapChunkVertexNormals>();
                    }

                    if (Header.TextureLayersOffset > 0)
                    {
                        br.BaseStream.Position = Header.TextureLayersOffset;
                        TextureLayers = br.ReadIFFChunk<MapChunkTextureLayers>();
                    }

                    if (Header.ModelReferencesOffset > 0)
                    {
                        br.BaseStream.Position = Header.ModelReferencesOffset;
                        ModelReferences = br.ReadIFFChunk<MapChunkModelReferences>();

                        ModelReferences.PostLoadReferences(Header.ModelReferenceCount, Header.WorldModelObjectReferenceCount);
                    }

                    if (Header.AlphaMapsOffset > 0)
                    {
                        br.BaseStream.Position = Header.AlphaMapsOffset;
                        AlphaMaps = br.ReadIFFChunk<MapChunkAlphaMaps>();
                    }

                    if (Header.BakedShadowsOffset > 0 && Header.Flags.HasFlag(MapChunkFlags.HasBakedShadows))
                    {
                        br.BaseStream.Position = Header.BakedShadowsOffset;
                        BakedShadows = br.ReadIFFChunk<MapChunkBakedShadows>();
                    }

                    if (Header.SoundEmittersOffset > 0 && Header.SoundEmitterCount > 0)
                    {
                        br.BaseStream.Position = Header.SoundEmittersOffset;
                        SoundEmitters = br.ReadIFFChunk<MapChunkSoundEmitters>();
                    }

                    if (Header.LiquidOffset > 0 && Header.LiquidSize > 8)
                    {
                        br.BaseStream.Position = Header.LiquidOffset;
                        Liquid = br.ReadIFFChunk<MapChunkLiquids>();
                    }

                    if (Header.VertexShadingOffset > 0 && Header.Flags.HasFlag(MapChunkFlags.HasVertexShading))
                    {
                        br.BaseStream.Position = Header.SoundEmittersOffset;
                        VertexShading = br.ReadIFFChunk<MapChunkVertexShading>();
                    }

                    if (Header.VertexLightingOffset > 0)
                    {
                        br.BaseStream.Position = Header.VertexLightingOffset;
                        VertexLighting = br.ReadIFFChunk<MapChunkVertexLighting>();
                    }
                }
            }
        }

        public string GetSignature()
        {
            return Signature;
        }
    }
}

