//
//  MapChunkSoundEmitters.cs
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
using System;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// MCSE chunk - holds sound emitters.
    /// </summary>
    public class MapChunkSoundEmitters : IIFFChunk, IBinarySerializable, IPostLoad<uint>
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCSE";

        private bool hasFinishedLoading;
        private byte[] Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkSoundEmitters"/> class.
        /// </summary>
        public MapChunkSoundEmitters()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkSoundEmitters"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public MapChunkSoundEmitters(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            Data = inData;
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool HasFinishedLoading()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void PostLoad(uint loadingParameters)
        {
            throw new NotImplementedException();
        }
    }
}
