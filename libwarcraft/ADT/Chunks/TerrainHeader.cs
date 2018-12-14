//
//  TerrainHeader.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MHDR Chunk - Contains offset for all major chunks in the ADT. All offsets are from the start of the MHDR + 4 bytes to compensate for the size field.
    /// </summary>
    public class TerrainHeader : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MHDR";

        /// <summary>
        /// Gets or sets flags for this ADT.
        /// </summary>
        public TerrainHeaderFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MCIN Chunk can be found.
        /// </summary>
        public int MapChunkOffsetsOffset { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MTEX Chunk can be found.
        /// </summary>
        public int TexturesOffset { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MMDX Chunk can be found.
        /// </summary>
        public int ModelsOffset { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MMID Chunk can be found.
        /// </summary>
        public int ModelIndicesOffset { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MWMO Chunk can be found.
        /// </summary>
        public int WorldModelObjectsOffset { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MWID Chunk can be found.
        /// </summary>
        public int WorldModelObjectIndicesOffset { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MMDF Chunk can be found.
        /// </summary>
        public int ModelPlacementInformationOffset { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MODF Chunk can be found.
        /// </summary>
        public int WorldModelObjectPlacementInformationOffset { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MFBO Chunk can be found. This is only set if the Flags contains MDHR_MFBO.
        /// </summary>
        public int BoundingBoxOffset { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MH2O Chunk can be found.
        /// </summary>
        public int LiquidOffset { get; set; }

        /// <summary>
        /// Gets or sets offset into the file where the MTXF Chunk can be found.
        /// </summary>
        public int TextureFlagsOffset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainHeader"/> class.
        /// </summary>
        public TerrainHeader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainHeader"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public TerrainHeader(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    //read values
                    Flags = (TerrainHeaderFlags)br.ReadInt32();

                    MapChunkOffsetsOffset = br.ReadInt32();
                    TexturesOffset = br.ReadInt32();

                    ModelsOffset = br.ReadInt32();
                    ModelIndicesOffset = br.ReadInt32();

                    WorldModelObjectsOffset = br.ReadInt32();
                    WorldModelObjectIndicesOffset = br.ReadInt32();

                    ModelPlacementInformationOffset = br.ReadInt32();
                    WorldModelObjectPlacementInformationOffset = br.ReadInt32();

                    BoundingBoxOffset = br.ReadInt32();
                    LiquidOffset = br.ReadInt32();
                    TextureFlagsOffset = br.ReadInt32();
                }
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }
    }
}
