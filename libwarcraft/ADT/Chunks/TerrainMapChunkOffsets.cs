//
//  TerrainMapChunkOffsets.cs
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
    /// MCIN Chunk - Contains a list of all MCNKs with associated information in the ADT file.
    /// </summary>
    public class TerrainMapChunkOffsets : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCIN";

        /// <summary>
        /// Gets or sets an array of 256 MCIN entries, containing map chunk offsets and sizes.
        /// </summary>
        public List<MapChunkOffsetEntry> Entries { get; set; } = new List<MapChunkOffsetEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainMapChunkOffsets"/> class.
        /// </summary>
        public TerrainMapChunkOffsets()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainMapChunkOffsets"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public TerrainMapChunkOffsets(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);

            // read size, n of entries is size / 16
            var entryCount = br.BaseStream.Length / 16;

            for (var i = 0; i < entryCount; ++i)
            {
                var entry = new MapChunkOffsetEntry
                {
                    MapChunkOffset = br.ReadInt32(),
                    MapChunkSize = br.ReadInt32(),
                    Flags = br.ReadInt32(),
                    AsynchronousLoadingID = br.ReadInt32()
                };

                Entries.Add(entry);
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }
    }
}
