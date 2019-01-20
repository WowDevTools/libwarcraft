//
//  ModelRenderBatches.cs
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

namespace Warcraft.WMO.GroupFile.Chunks
{
    /// <summary>
    /// Holds render batches.
    /// </summary>
    public class ModelRenderBatches : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOBA";

        /// <summary>
        /// Gets the render batches.
        /// </summary>
        public List<RenderBatch> RenderBatches { get; } = new List<RenderBatch>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelRenderBatches"/> class.
        /// </summary>
        public ModelRenderBatches()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelRenderBatches"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelRenderBatches(byte[] inData)
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
                    while (ms.Position < ms.Length)
                    {
                        RenderBatches.Add(new RenderBatch(br.ReadBytes(RenderBatch.GetSize())));
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
                    foreach (RenderBatch renderBatch in RenderBatches)
                    {
                        bw.Write(renderBatch.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
