//
//  TerrainWorldModelObjectPlacementInfo.cs
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
    /// MODF Chunk - Contains WMO model placement information.
    /// </summary>
    public class TerrainWorldModelObjectPlacementInfo : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MODF";

        /// <summary>
        /// Gets or sets an array of WMO model information entries.
        /// </summary>
        public List<WorldModelObjectPlacementEntry> Entries { get; set; } = new List<WorldModelObjectPlacementEntry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainWorldModelObjectPlacementInfo"/> class.
        /// </summary>
        public TerrainWorldModelObjectPlacementInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainWorldModelObjectPlacementInfo"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public TerrainWorldModelObjectPlacementInfo(byte[] inData)
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
                    var entryCount = br.BaseStream.Length / WorldModelObjectPlacementEntry.GetSize();
                    for (var i = 0; i < entryCount; ++i)
                    {
                        Entries.Add(new WorldModelObjectPlacementEntry(br.ReadBytes(WorldModelObjectPlacementEntry.GetSize())));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    foreach (var entry in Entries)
                    {
                        bw.Write(entry.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
