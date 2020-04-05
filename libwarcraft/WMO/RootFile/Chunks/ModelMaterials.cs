//
//  ModelMaterials.cs
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
    /// Holds model materials.
    /// </summary>
    public class ModelMaterials : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOMT";

        /// <summary>
        /// Gets the model materials.
        /// </summary>
        public List<ModelMaterial> Materials { get; } = new List<ModelMaterial>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMaterials"/> class.
        /// </summary>
        public ModelMaterials()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMaterials"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelMaterials(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            var materialCount = inData.Length / ModelMaterial.GetSize();
            for (var i = 0; i < materialCount; ++i)
            {
                Materials.Add(new ModelMaterial(br.ReadBytes(ModelMaterial.GetSize())));
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
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                foreach (var modelMaterial in Materials)
                {
                    bw.Write(modelMaterial.Serialize());
                }
            }

            return ms.ToArray();
        }
    }
}
