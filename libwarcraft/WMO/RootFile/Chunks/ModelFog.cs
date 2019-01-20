//
//  ModelFog.cs
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
    /// Represents the fog instances in a model.
    /// </summary>
    public class ModelFog : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MFOG";

        /// <summary>
        /// Gets the fog instances.
        /// </summary>
        public List<FogInstance> FogInstances { get; } = new List<FogInstance>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFog"/> class.
        /// </summary>
        public ModelFog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFog"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelFog(byte[] inData)
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
                    int fogInstanceCount = inData.Length / FogInstance.GetSize();
                    for (int i = 0; i < fogInstanceCount; ++i)
                    {
                        FogInstances.Add(new FogInstance(br.ReadBytes(FogInstance.GetSize())));
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
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    foreach (FogInstance fogInstance in FogInstances)
                    {
                        bw.Write(fogInstance.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
