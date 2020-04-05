//
//  ModelDoodadSets.cs
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

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Represents a list of doodad sets present in this model. Each set is stored separately in this block, and
    /// defines a list of doodad instances to render when the set is selected.
    /// </summary>
    public class ModelDoodadSets : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// The static block signature of this chunk.
        /// </summary>
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MODS";

        /// <summary>
        /// Gets a list of the doodad sets contained in this chunk.
        /// </summary>
        public List<DoodadSet> DoodadSets { get; } = new List<DoodadSet>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelDoodadSets"/> class.
        /// </summary>
        public ModelDoodadSets()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelDoodadSets"/> class.
        /// </summary>
        /// <param name="inData">The binary data containing the doodad set object.</param>
        public ModelDoodadSets(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <summary>
        /// Deserialzes the provided binary data of the object. This is the full data block which follows the data
        /// signature and data block length.
        /// </summary>
        /// <param name="inData">The binary data containing the object.</param>
        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            var setCount = inData.Length / DoodadSet.GetSize();
            for (uint i = 0; i < setCount; ++i)
            {
                DoodadSets.Add(new DoodadSet(br.ReadBytes(DoodadSet.GetSize())));
            }
        }

        /// <summary>
        /// Gets the static data signature of this data block type.
        /// </summary>
        /// <returns>A string representing the block signature.</returns>
        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <summary>
        /// Serializes the current object into a byte array.
        /// </summary>
        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                foreach (var doodadSet in DoodadSets)
                {
                    bw.Write(doodadSet.Serialize());
                }
            }

            return ms.ToArray();
        }
    }
}
