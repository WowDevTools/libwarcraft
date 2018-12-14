//
//  MapChunkAlphaMaps.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// MCLY Chunk - Contains definitions for the alpha map layers.
    /// </summary>
    public class MapChunkTextureLayers : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCLY";

        /// <summary>
        /// Gets or sets an array of alpha map layers in this MCNK.
        /// </summary>
        public List<TextureLayerEntry> Layers { get; set; } = new List<TextureLayerEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkTextureLayers"/> class.
        /// </summary>
        public MapChunkTextureLayers()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkTextureLayers"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public MapChunkTextureLayers(byte[] inData)
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
                    long nLayers = br.BaseStream.Length / TextureLayerEntry.GetSize();
                    for (int i = 0; i < nLayers; i++)
                    {
                        Layers.Add(new TextureLayerEntry(br.ReadBytes(TextureLayerEntry.GetSize())));
                    }
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

