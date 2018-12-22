//
//  TerrainTile.cs
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

using System.Collections.Generic;
using System.IO;
using Warcraft.ADT.Chunks;
using Warcraft.Core.Extensions;
using Warcraft.WDT.Chunks;

namespace Warcraft.ADT
{
    /// <summary>
    /// A complete ADT object created from a file on disk.
    /// </summary>
    public class TerrainTile
    {
        /// <summary>
        /// Gets or sets the contains the ADT version.
        /// </summary>
        public TerrainVersion Version { get; set; }

        /// <summary>
        /// Gets or sets the contains the ADT Header with offsets. The header has offsets to the other chunks in the
        /// ADT.
        /// </summary>
        public TerrainHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the contains an array of offsets where MCNKs are in the file.
        /// </summary>
        public TerrainMapChunkOffsets MapChunkOffsets { get; set; }

        /// <summary>
        /// Gets or sets the contains a list of all textures referenced by this ADT.
        /// </summary>
        public TerrainTextures Textures { get; set; }

        /// <summary>
        /// Gets or sets the contains a list of all M2 models refereced by this ADT.
        /// </summary>
        public TerrainModels Models { get; set; }

        /// <summary>
        /// Gets or sets the contains M2 model indexes for the list in ADTModels (MMDX chunk).
        /// </summary>
        public TerrainModelIndices ModelIndices { get; set; }

        /// <summary>
        /// Gets or sets the contains a list of all WMOs referenced by this ADT.
        /// </summary>
        public TerrainWorldModelObjects WorldModelObjects { get; set; }

        /// <summary>
        /// Gets or sets the contains WMO indexes for the list in ADTWMOs (MWMO chunk).
        /// </summary>
        public TerrainWorldModelObjectIndices WorldModelObjectIndices { get; set; }

        /// <summary>
        /// Gets or sets the contains position information for all M2 models in this ADT.
        /// </summary>
        public TerrainModelPlacementInfo ModelPlacementInfo { get; set; }

        /// <summary>
        /// Gets or sets the contains position information for all WMO models in this ADT.
        /// </summary>
        public TerrainWorldModelObjectPlacementInfo WorldModelObjectPlacementInfo { get; set; }

        /// <summary>
        /// Gets or sets the contains water data for this ADT. This chunk is present in WOTLK chunks and above.
        /// </summary>
        public TerrainLiquid Liquids { get; set; }

        /// <summary>
        /// Gets or sets the the texture flags. This chunk is present in WOTLK chunks and above.
        /// </summary>
        public TerrainTextureFlags TextureFlags { get; set; }

        /// <summary>
        /// Gets or sets the contains an array of all MCNKs in this ADT.
        /// </summary>
        public List<TerrainMapChunk> MapChunks { get; set; } = new List<TerrainMapChunk>();

        private readonly bool _hasWorldFlags;
        private readonly WorldTableFlags _worldFlags;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainTile"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        /// <param name="inWorldFlags">The world flags.</param>
        public TerrainTile(byte[] data, WorldTableFlags inWorldFlags)
            : this(data)
        {
            _hasWorldFlags = true;
            _worldFlags = inWorldFlags;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainTile"/> class.
        /// </summary>
        /// <returns>A parsed ADT file with objects for all chunks.</returns>
        /// <param name="data">The binary data.</param>
        public TerrainTile(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    // In all ADT files, the version chunk and header chunk are at the beginning of the file,
                    // with the header following the version. From them, the rest of the chunks can be
                    // seeked to and read.

                    // Read Version Chunk
                    Version = br.ReadIFFChunk<TerrainVersion>();

                    // Read the header chunk
                    Header = br.ReadIFFChunk<TerrainHeader>();

                    if (Header.MapChunkOffsetsOffset > 0)
                    {
                        br.BaseStream.Position = Header.MapChunkOffsetsOffset;
                        MapChunkOffsets = br.ReadIFFChunk<TerrainMapChunkOffsets>();
                    }

                    if (Header.TexturesOffset > 0)
                    {
                        br.BaseStream.Position = Header.TexturesOffset;
                        Textures = br.ReadIFFChunk<TerrainTextures>();
                    }

                    if (Header.ModelsOffset > 0)
                    {
                        br.BaseStream.Position = Header.ModelsOffset;
                        Models = br.ReadIFFChunk<TerrainModels>();
                    }

                    if (Header.ModelIndicesOffset > 0)
                    {
                        br.BaseStream.Position = Header.ModelIndicesOffset;
                        ModelIndices = br.ReadIFFChunk<TerrainModelIndices>();
                    }

                    if (Header.WorldModelObjectsOffset > 0)
                    {
                        br.BaseStream.Position = Header.WorldModelObjectsOffset;
                        WorldModelObjects = br.ReadIFFChunk<TerrainWorldModelObjects>();
                    }

                    if (Header.WorldModelObjectIndicesOffset > 0)
                    {
                        br.BaseStream.Position = Header.WorldModelObjectIndicesOffset;
                        WorldModelObjectIndices = br.ReadIFFChunk<TerrainWorldModelObjectIndices>();
                    }

                    if (Header.LiquidOffset > 0)
                    {
                        br.BaseStream.Position = Header.LiquidOffset;
                        Liquids = br.ReadIFFChunk<TerrainLiquid>();
                        // TODO: [#9] Pass in DBC liquid type to load the vertex data
                    }

                    // Read and fill the map chunks
                    foreach (MapChunkOffsetEntry entry in MapChunkOffsets.Entries)
                    {
                        br.BaseStream.Position = entry.MapChunkOffset;
                        MapChunks.Add(br.ReadIFFChunk<TerrainMapChunk>());
                    }
                }
            }
        }
    }
}
