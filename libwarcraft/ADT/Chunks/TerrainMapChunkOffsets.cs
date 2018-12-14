//
//  TerrainMapChunkOffsets.cs
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
    /// MCIN Chunk - Contains a list of all MCNKs with associated information in the ADT file.
    /// </summary>
    public class TerrainMapChunkOffsets : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCIN";

        /// <summary>
        /// An array of 256 MCIN entries, containing map chunk offsets and sizes.
        /// </summary>
        public List<MapChunkOffsetEntry> Entries = new List<MapChunkOffsetEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainMapChunkOffsets"/> class.
        /// </summary>
        public TerrainMapChunkOffsets()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainMapChunkOffsets"/> class.
        /// </summary>
        public TerrainMapChunkOffsets(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    //read size, n of entries is size / 16
                    long nEntries = br.BaseStream.Length / 16;

                    for (int i = 0; i < nEntries; ++i)
                    {
                        MapChunkOffsetEntry entry = new MapChunkOffsetEntry
                        {
                            MapChunkOffset = br.ReadInt32(),
                            MapChunkSize = br.ReadInt32(),
                            Flags = br.ReadInt32(),
                            AsynchronousLoadingID = br.ReadInt32()
                        };

                        Entries.Add(entry);
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

    /// <summary>
    /// A struct containing information about the referenced MCNK
    /// </summary>
    public class MapChunkOffsetEntry
    {
        /// <summary>
        /// Absolute offset of the MCNK
        /// </summary>
        public int MapChunkOffset;

        /// <summary>
        /// Size of the MCNK
        /// </summary>
        public int MapChunkSize;

        /// <summary>
        /// Flags of the MCNK. This is only set on the client, and is as such always 0.
        /// </summary>
        public int Flags;

        /// <summary>
        /// Async loading ID of the MCNK. This is only set on the client, and is as such always 0.
        /// </summary>
        public int AsynchronousLoadingID;
    }
}

