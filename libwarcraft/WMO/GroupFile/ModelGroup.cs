//
//  ModelGroup.cs
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
using Warcraft.ADT.Chunks;
using Warcraft.Core;
using Warcraft.Core.Interfaces;
using Warcraft.WMO.GroupFile.Chunks;

namespace Warcraft.WMO.GroupFile
{
	/// <summary>
	/// This class describes the structure of a model group, that is, the actual bulk data of a world model.
	/// </summary>
	public class ModelGroup : IBinarySerializable
	{
		/// <summary>
		/// The version of the model group.
		/// </summary>
		public TerrainVersion Version;

		/// <summary>
		/// The group data of the model. This is where all of the actual data is stored.
		/// </summary>
		public ModelGroupData GroupData;

		/// <summary>
		/// The name of the model group.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// The descriptive name of the model group.
		/// </summary>
		public string DescriptiveName
		{
			get;
			set;
		}

		/// <summary>
		/// Deserializes a <see cref="ModelGroup"/> object from provided binary data.
		/// </summary>
		/// <param name="inData">The binary data containing the object.</param>
		public ModelGroup(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
            {
            	using (BinaryReader br = new BinaryReader(ms))
	            {
		            this.Version = br.ReadIFFChunk<TerrainVersion>();
		            this.GroupData = br.ReadIFFChunk<ModelGroupData>();
	            }
            }
		}

		/// <summary>
		/// Gets the position of the model group, relative to the model root.
		/// </summary>
		/// <returns></returns>
		public Vector3f GetPosition()
		{
			return GetBoundingBox().GetCenterCoordinates();
		}

		/// <summary>
		/// Gets the bounding box of the model group.
		/// </summary>
		/// <returns>The bounding box of the group.</returns>
		public Box GetBoundingBox()
		{
			return this.GroupData.BoundingBox;
		}

		/// <summary>
		/// Gets the offset to the name of this group inside the name block.
		/// </summary>
		/// <returns>An offset into the name block.</returns>
		public uint GetInternalNameOffset()
		{
			return this.GroupData.GroupNameOffset;
		}

		/// <summary>
		/// Gets the offset to the descriptive name of this group inside the name block.
		/// </summary>
		/// <returns>An offset into the name block.</returns>
		public uint GetInternalDescriptiveNameOffset()
		{
			return this.GroupData.DescriptiveGroupNameOffset;
		}

		/// <summary>
		/// Gets the vertex positions contained in this model group.
		/// </summary>
		/// <returns>A list of vertex positions.</returns>
		public List<Vector3f> GetVertices()
		{
			return this.GroupData.Vertices.Vertices;
		}

		/// <summary>
		/// Gets the vertex normals contained in this model group.
		/// </summary>
		/// <returns>A list of vertex normals.</returns>
		public List<Vector3f> GetNormals()
		{
			return this.GroupData.Normals.Normals;
		}

		/// <summary>
		/// Gets the texture coordinates for the vertices contained in this model group.
		/// </summary>
		/// <returns>A list of texture coordinates.</returns>
		public List<Vector2f> GetTextureCoordinates()
		{
			return this.GroupData.TextureCoordinates.TextureCoordinates;
		}

		/// <summary>
		/// Gets the vertex indices contained in this model group.
		/// </summary>
		/// <returns>A list of the vertex indices.</returns>
		public List<ushort> GetVertexIndices()
		{
			return this.GroupData.VertexIndices.VertexIndices;
		}

		/// <summary>
		/// Gets the render batches contained in this model group.
		/// </summary>
		/// <returns>A list of the render batches.</returns>
		public List<RenderBatch> GetRenderBatches()
		{
			return this.GroupData.RenderBatches.RenderBatches;
		}

		/// <summary>
		/// Serializes the current object into a byte array.
		/// </summary>
		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
            		bw.WriteIFFChunk(this.Version);
		            bw.WriteIFFChunk(this.GroupData);
            	}

            	return ms.ToArray();
            }
		}
	}
}