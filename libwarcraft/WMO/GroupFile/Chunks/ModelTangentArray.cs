//
//  ModelTangentArray.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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
		public const string Signature = "MOTA";

		private bool InternalHasFinishedLoading;
		private byte[] Data;

		public List<short> FirstIndices = new List<short>();
		public List<Vector4> Tangents = new List<Vector4>();

		public ModelTangentArray()
		{
			this.InternalHasFinishedLoading = true;
		}

		public ModelTangentArray(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
		{
			this.Data = inData;
			this.InternalHasFinishedLoading = false;
		}

		public string GetSignature()
		{
			return Signature;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
		            foreach (short firstIndex in this.FirstIndices)
		            {
			            bw.Write(firstIndex);
		            }

		            foreach (Vector4 tangent in this.Tangents)
		            {
			            bw.WriteVector4(tangent);
		            }
            	}

            	return ms.ToArray();
            }
		}

		public bool HasFinishedLoading()
		{
			return this.InternalHasFinishedLoading;
		}

		public void PostLoad(ModelTangentArrayPostLoadParameters loadingParameters)
		{
			using (MemoryStream ms = new MemoryStream(this.Data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (int i = 0; i < loadingParameters.RenderBatchCount; ++i)
					{
						this.FirstIndices.Add(br.ReadInt16());
					}

					for (int i = 0; i < loadingParameters.AccumulatedIndexCount; ++i)
					{
						this.Tangents.Add(br.ReadVector4());
					}
				}
			}

			this.Data = null;
		}
	}

	public class ModelTangentArrayPostLoadParameters
	{
		public uint RenderBatchCount;
		public uint AccumulatedIndexCount;

		public ModelTangentArrayPostLoadParameters()
		{
		}

		public ModelTangentArrayPostLoadParameters(uint inRenderBatchCount, uint inAccumulatedIndexCount)
		{
			this.RenderBatchCount = inRenderBatchCount;
			this.AccumulatedIndexCount = inAccumulatedIndexCount;
		}
	}
}

