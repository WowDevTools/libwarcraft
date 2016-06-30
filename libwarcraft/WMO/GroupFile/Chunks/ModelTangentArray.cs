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

using System;
using System.Collections.Generic;
using System.IO;
using Warcraft.Core;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.GroupFile.Chunks
{
	public class ModelTangentArray : IRIFFChunk, IBinarySerializable, IPostLoad<ModelTangentArrayPostLoadParameters>
	{
		public const string Signature = "MOTA";

		private bool hasFinishedLoading;
		private byte[] data;

		public List<short> FirstIndices = new List<short>();
		public List<Vector4f> Tangents = new List<Vector4f>();

		public ModelTangentArray()
		{
			hasFinishedLoading = true;
		}

		public ModelTangentArray(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
		{
			this.data = inData;
			hasFinishedLoading = false;
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

		            foreach (Vector4f tangent in this.Tangents)
		            {
			            bw.WriteVector4f(tangent);
		            }
            	}

            	return ms.ToArray();
            }
		}

		public bool HasFinishedLoading()
		{
			return hasFinishedLoading;
		}

		public void PostLoad(ModelTangentArrayPostLoadParameters loadingParameters)
		{
			using (MemoryStream ms = new MemoryStream(this.data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (int i = 0; i < loadingParameters.RenderBatchCount; ++i)
					{
						this.FirstIndices.Add(br.ReadInt16());
					}

					for (int i = 0; i < loadingParameters.AccumulatedIndexCount; ++i)
					{
						this.Tangents.Add(br.ReadVector4f());
					}
				}
			}
		}
	}

	public class ModelTangentArrayPostLoadParameters : IPostLoadParameter
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

