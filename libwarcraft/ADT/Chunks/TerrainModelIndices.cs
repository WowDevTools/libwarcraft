//
//  TerrainModelIndices.cs
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

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MMID Chunk - Contains a list of M2 model indexes
    /// </summary>
    public class TerrainModelIndices : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MMID";

        /// <summary>
        /// List indexes for models in an MMID chunk
        /// </summary>
        public List<uint> ModelFilenameOffsets = new List<uint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainModelIndices"/> class.
        /// </summary>
        public TerrainModelIndices()
        {
        }

        public TerrainModelIndices(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int offsetCount = inData.Length / 4;
                    for (int i = 0; i < offsetCount; ++i)
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

