//
//  TerrainModelIndices.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MMID Chunk - Contains a list of M2 model indexes.
    /// </summary>
    public class TerrainModelIndices : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MMID";

        /// <summary>
        /// Gets or sets the list of indexes for models in an MMID chunk.
        /// </summary>
        public List<uint> ModelFilenameOffsets { get; set; } = new List<uint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainModelIndices"/> class.
        /// </summary>
        public TerrainModelIndices()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainModelIndices"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public TerrainModelIndices(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    var offsetCount = inData.Length / 4;
                    for (var i = 0; i < offsetCount; ++i)
                    {
                        ModelFilenameOffsets.Add(br.ReadUInt32());
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
