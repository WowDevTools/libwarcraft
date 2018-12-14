//
//  ModelTangentArray.cs
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
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.GroupFile.Chunks
{
    public class ModelTangentArray : IIFFChunk, IBinarySerializable, IPostLoad<ModelTangentArrayPostLoadParameters>
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOTA";

        private bool InternalHasFinishedLoading;
        private byte[] Data;

        public List<short> FirstIndices = new List<short>();
        public List<Vector4> Tangents = new List<Vector4>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelTangentArray"/> class.
        /// </summary>
        public ModelTangentArray()
        {
            InternalHasFinishedLoading = true;
        }

        public ModelTangentArray(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            Data = inData;
            InternalHasFinishedLoading = false;
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
                    foreach (short firstIndex in FirstIndices)
                    {
                        bw.Write(firstIndex);
                    }

                    foreach (Vector4 tangent in Tangents)
                    {
                        bw.WriteVector4(tangent);
                    }
                }

                return ms.ToArray();
            }
        }

        public bool HasFinishedLoading()
        {
            return InternalHasFinishedLoading;
        }

        public void PostLoad(ModelTangentArrayPostLoadParameters loadingParameters)
        {
            using (MemoryStream ms = new MemoryStream(Data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    for (int i = 0; i < loadingParameters.RenderBatchCount; ++i)
                    {
                        FirstIndices.Add(br.ReadInt16());
                    }

                    for (int i = 0; i < loadingParameters.AccumulatedIndexCount; ++i)
                    {
                        Tangents.Add(br.ReadVector4());
                    }
                }
            }

            Data = null;
        }
    }

    public class ModelTangentArrayPostLoadParameters
    {
        public uint RenderBatchCount;
        public uint AccumulatedIndexCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelTangentArrayPostLoadParameters"/> class.
        /// </summary>
        public ModelTangentArrayPostLoadParameters()
        {
        }

        public ModelTangentArrayPostLoadParameters(uint inRenderBatchCount, uint inAccumulatedIndexCount)
        {
            RenderBatchCount = inRenderBatchCount;
            AccumulatedIndexCount = inAccumulatedIndexCount;
        }
    }
}

