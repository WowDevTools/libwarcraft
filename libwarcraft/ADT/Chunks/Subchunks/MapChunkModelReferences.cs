//
//  MapChunkModelReferences.cs
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

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// MCRF chunk - holds model references.
    /// </summary>
    public class MapChunkModelReferences : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCRF";

        private byte[]? _data;

        /// <summary>
        /// Gets or sets the game model references.
        /// </summary>
        public List<uint> GameModelObjectReferences { get; set; } = new List<uint>();

        /// <summary>
        /// Gets or sets the world model references.
        /// </summary>
        public List<uint> WorldModelObjectReferences { get; set; } = new List<uint>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkModelReferences"/> class.
        /// </summary>
        public MapChunkModelReferences()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkModelReferences"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public MapChunkModelReferences(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            _data = inData;
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <summary>
        /// Performs post-creation loading of data reliant on external sources.
        /// </summary>
        /// <param name="gameModelObjectCount">The number of game model objects in the chunk.</param>
        /// <param name="worldModelObjectCount">The number of world model objects in the chunk.</param>
        public void PostLoadReferences(uint gameModelObjectCount, uint worldModelObjectCount)
        {
            if (_data is null)
            {
                return;
            }

            using var ms = new MemoryStream(_data);
            using var br = new BinaryReader(ms);
            for (var i = 0; i < gameModelObjectCount; ++i)
            {
                GameModelObjectReferences.Add(br.ReadUInt32());
            }

            for (var i = 0; i < worldModelObjectCount; ++i)
            {
                WorldModelObjectReferences.Add(br.ReadUInt32());
            }
        }
    }
}
