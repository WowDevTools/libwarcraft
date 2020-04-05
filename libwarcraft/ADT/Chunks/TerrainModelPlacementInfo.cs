//
//  TerrainModelPlacementInfo.cs
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
    /// MMDF Chunk - Contains M2 model placement information.
    /// </summary>
    public class TerrainModelPlacementInfo : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MMDF";

        /// <summary>
        /// Gets or sets a list of MDDF entries with model placement information.
        /// </summary>
        public List<ModelPlacementEntry> Entries { get; set; } = new List<ModelPlacementEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainModelPlacementInfo"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public TerrainModelPlacementInfo(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            var entryCount = br.BaseStream.Length / ModelPlacementEntry.GetSize();

            for (var i = 0; i < entryCount; ++i)
            {
                Entries.Add(new ModelPlacementEntry(br.ReadBytes(ModelPlacementEntry.GetSize())));
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }
    }
}
