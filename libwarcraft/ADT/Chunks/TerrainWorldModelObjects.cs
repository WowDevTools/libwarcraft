//
//  TerrainWorldModelObjects.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MWMO Chunk - Contains a list of all referenced WMO models in this ADT.
    /// </summary>
    public class TerrainWorldModelObjects : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MWMO";

        /// <summary>
        /// Gets or sets a list of full paths to the M2 models referenced in this ADT.
        /// </summary>
        public List<string> Filenames { get; set; } = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainWorldModelObjects"/> class.
        /// </summary>
        public TerrainWorldModelObjects()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainWorldModelObjects"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public TerrainWorldModelObjects(byte[] inData)
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
                    while (ms.Position < ms.Length)
                    {
                        Filenames.Add(br.ReadNullTerminatedString());
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
                    foreach (var filename in Filenames)
                    {
                        bw.WriteNullTerminatedString(filename);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
