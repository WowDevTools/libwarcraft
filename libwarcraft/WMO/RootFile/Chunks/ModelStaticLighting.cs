//
//  ModelStaticLighting.cs
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

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Holds the static lights in the model.
    /// </summary>
    public class ModelStaticLighting : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOLT";

        /// <summary>
        /// Gets the static lights.
        /// </summary>
        public List<StaticLight> StaticLights { get; } = new List<StaticLight>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelStaticLighting"/> class.
        /// </summary>
        public ModelStaticLighting()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelStaticLighting"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelStaticLighting(byte[] inData)
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
                    var lightCount = inData.Length / StaticLight.GetSize();
                    for (uint i = 0; i < lightCount; ++i)
                    {
                        StaticLights.Add(new StaticLight(br.ReadBytes(StaticLight.GetSize())));
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
                    foreach (var staticLight in StaticLights)
                    {
                        bw.Write(staticLight.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
