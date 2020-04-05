//
//  ModelLightReferences.cs
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

namespace Warcraft.WMO.GroupFile.Chunks
{
    /// <summary>
    /// Holds light references in a model.
    /// </summary>
    public class ModelLightReferences : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOLR";

        /// <summary>
        /// Gets the light references.
        /// </summary>
        public List<ushort> LightReferences { get; } = new List<ushort>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelLightReferences"/> class.
        /// </summary>
        public ModelLightReferences()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelLightReferences"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelLightReferences(byte[] inData)
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
                        LightReferences.Add(br.ReadUInt16());
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
                    foreach (var lightReference in LightReferences)
                    {
                        bw.Write(lightReference);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
