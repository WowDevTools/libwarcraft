//
//  MapChunkVertexShading.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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

using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// MCCV chunk - holds painted per-vertex shading.
    /// </summary>
    public class MapChunkVertexShading : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCCV";

        /// <summary>
        /// Gets or sets the high-resolution vertex shading.
        /// </summary>
        public List<RGBA> HighResVertexShading { get; set; } = new List<RGBA>();

        /// <summary>
        /// Gets or sets the high-resolution vertex shading.
        /// </summary>
        public List<RGBA> LowResVertexShading { get; set; } = new List<RGBA>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkVertexShading"/> class.
        /// </summary>
        public MapChunkVertexShading()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkVertexShading"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public MapChunkVertexShading(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            for (var y = 0; y < 16; ++y)
            {
                if (y % 2 == 0)
                {
                    // Read a block of 9 high res vertices
                    for (var x = 0; x < 9; ++x)
                    {
                        HighResVertexShading.Add(br.ReadRGBA());
                    }
                }
                else
                {
                    // Read a block of 8 low res vertices
                    for (var x = 0; x < 8; ++x)
                    {
                        LowResVertexShading.Add(br.ReadRGBA());
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
