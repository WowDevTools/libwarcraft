//
//  MapChunkBakedShadows.cs
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

using System.Collections;
using System.Collections.Generic;
using System.IO;

using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// MCSH chunk - holds baked terrain shadows.
    /// </summary>
    public class MapChunkBakedShadows : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCSH";

        /// <summary>
        /// Gets the shadow map contained in the chunk. Each chunk contains 64x64 values, indicating whether the section
        /// is shadowed or not.
        /// </summary>
        public List<List<bool>> ShadowMap { get; } = new List<List<bool>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkBakedShadows"/> class.
        /// </summary>
        public MapChunkBakedShadows()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkBakedShadows"/> class.
        /// </summary>
        /// <param name="inData">The input binary data.</param>
        public MapChunkBakedShadows(byte[] inData)
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
                    for (int y = 0; y < 64; ++y)
                    {
                        List<bool> mapRow = new List<bool>();
                        for (int x = 0; x < 2; ++x)
                        {
                            BitArray valueBits = new BitArray(br.ReadInt32());

                            for (int i = 0; i < 32; ++i)
                            {
                                mapRow.Add(valueBits.Get(i));
                            }
                        }

                        ShadowMap.Add(mapRow);
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
