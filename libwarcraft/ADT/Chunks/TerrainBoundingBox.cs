//
//  TerrainBoundingBox.cs
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

using System.IO;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MFBO chunk - holds a bounding box for the terrain chunk.
    /// </summary>
    public class TerrainBoundingBox : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MFBO";

        /// <summary>
        /// Gets or sets the maximum bounding plane.
        /// </summary>
        public ShortPlane Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum bounding plane.
        /// </summary>
        public ShortPlane Minimum { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainBoundingBox"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public TerrainBoundingBox(byte[] inData)
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
                    Maximum = br.ReadShortPlane();
                    Minimum = br.ReadShortPlane();
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
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.WriteShortPlane(Maximum);
                    bw.WriteShortPlane(Minimum);
                }

                return ms.ToArray();
            }
        }
    }
}
