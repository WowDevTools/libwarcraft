//
//  ModelGroupData.cs
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
using Warcraft.DBC.SpecialFields;
using Warcraft.WMO.GroupFile.Chunks;

namespace Warcraft.WMO.GroupFile
{
	public class ModelGroupData : IIFFChunk, IBinarySerializable
	{
		public const string Signature = "MOGP";

		public uint GroupNameOffset;
		public uint DescriptiveGroupNameOffset;

		public GroupFlags Flags;

		public Box BoundingBox;

		public ushort PortalReferenceStartingIndex;
		public ushort PortalReferenceCount;

		public ushort RenderBatchCountA;
		public ushort RenderBatchCountInterior;
		public ushort RenderBatchCountExterior;
		public ushort Unknown;

		public readonly List<byte> FogIndices = new List<byte>(4);
		public uint LiquidType;
		public UInt32ForeignKey GroupID;

		public uint UnknownFlags;
		public uint Unused;

		// The chunks in a model group file are actually subchunks of this one. They are listed here
		// for the sake of correctness, and are then given abstraction accessors in the ModelGroup class.
		public ModelPolygonMaterials PolygonMaterials;
		public ModelVertexIndices VertexIndices;
		public ModelVertices Vertices;
		public ModelNormals Normals;
		public ModelTextureCoordinates TextureCoordinates;
		public ModelRenderBatches RenderBatches;

		// The following chunks are optional, and are read based on flags in the header.
		public MOBS mobs;
		public ModelLightReferences LightReferences;
		public ModelDoodadReferences DoodadReferences;
		public ModelBSPNodes BSPNodes;
		public ModelBSPFaceIndices BSPFaceIndices;

		public MPBV mpbv;
		public MPBP mpbp;
		public MPBI mpbi;
		public MPBG mpbg;

		public ModelVertexColours VertexColours;
		public ModelLiquids Liquids;

		public ModelTriangleStripIndices TriangleStripIndices;
		public ModelTriangleStrips TriangleStrips;

		public ModelTextureCoordinates AdditionalTextureCoordinates;
		public ModelVertexColours AdditionalVertexColours;
		public ModelTextureCoordinates SecondAddtionalTextureCoordinates;

		public ModelGroupData()
		{

		}

		public ModelGroupData(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		/// <summary>
		/// Deserialzes the provided binary data of the object. This is the full data block which follows the data
		/// signature and data block length.
		/// </summary>
		/// <param name="inData">The binary data containing the object.</param>
		public void LoadBinaryData(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
            {
            	using (BinaryReader br = new BinaryReader(ms))
	            {
		            this.GroupNameOffset = br.ReadUInt32();
		            this.DescriptiveGroupNameOffset = br.ReadUInt32();

		            this.Flags = (GroupFlags) br.ReadUInt32();

		            this.BoundingBox = br.ReadBox();

		            this.PortalReferenceStartingIndex = br.ReadUInt16();
		            this.PortalReferenceCount = br.ReadUInt16();

		            this.RenderBatchCountA = br.ReadUInt16();
		            this.RenderBatchCountInterior = br.ReadUInt16();
		            this.RenderBatchCountExterior = br.ReadUInt16();
		            this.Unknown = br.ReadUInt16();

		            for (int i = 0; i < 4; ++i)
		            {
			            this.FogIndices.Add(br.ReadByte());
		            }

		            this.LiquidType = br.ReadUInt32();
		            this.GroupID = new UInt32ForeignKey("WMOAreaTable", "WMOGroupID", br.ReadUInt32());

		            this.UnknownFlags = br.ReadUInt32();
		            this.Unused = br.ReadUInt32();

		            // Required subchunks
		            this.PolygonMaterials = br.ReadIFFChunk<ModelPolygonMaterials>();
		            this.VertexIndices = br.ReadIFFChunk<ModelVertexIndices>();
		            this.Vertices = br.ReadIFFChunk<ModelVertices>();
		            this.Normals = br.ReadIFFChunk<ModelNormals>();
		            this.TextureCoordinates = br.ReadIFFChunk<ModelTextureCoordinates>();
		            this.RenderBatches = br.ReadIFFChunk<ModelRenderBatches>();

		            // Optional chunks
		            if (br.PeekChunkSignature() == MOBS.Signature)
		            {
			            this.mobs = br.ReadIFFChunk<MOBS>();
		            }

		            if (this.Flags.HasFlag(GroupFlags.HasLights))
		            {
			            this.LightReferences = br.ReadIFFChunk<ModelLightReferences>();
		            }

		            if (this.Flags.HasFlag(GroupFlags.HasDoodads))
		            {
			            this.DoodadReferences = br.ReadIFFChunk<ModelDoodadReferences>();
		            }

		            if (this.Flags.HasFlag(GroupFlags.HasBSP))
		            {
			            this.BSPNodes = br.ReadIFFChunk<ModelBSPNodes>();
			            this.BSPFaceIndices = br.ReadIFFChunk<ModelBSPFaceIndices>();
		            }

		            if (this.Flags.HasFlag(GroupFlags.UnknownLODRelated))
		            {
			            this.mpbv = br.ReadIFFChunk<MPBV>();
			            this.mpbp = br.ReadIFFChunk<MPBP>();
			            this.mpbi = br.ReadIFFChunk<MPBI>();
			            this.mpbg = br.ReadIFFChunk<MPBG>();
		            }

		            if (this.Flags.HasFlag(GroupFlags.HasVertexColours))
		            {
			            this.VertexColours = br.ReadIFFChunk<ModelVertexColours>();
		            }

		            if (this.Flags.HasFlag(GroupFlags.HasLiquids))
		            {
			            this.Liquids = br.ReadIFFChunk<ModelLiquids>();
		            }

		            if (this.Flags.HasFlag(GroupFlags.HasTriangleStrips))
		            {
			            this.TriangleStripIndices = br.ReadIFFChunk<ModelTriangleStripIndices>();
			            this.TriangleStrips = br.ReadIFFChunk<ModelTriangleStrips>();
		            }

		            if (this.Flags.HasFlag(GroupFlags.HasTwoTextureCoordinateSets))
		            {
			            this.AdditionalTextureCoordinates = br.ReadIFFChunk<ModelTextureCoordinates>();
		            }

		            if (this.Flags.HasFlag(GroupFlags.HasTwoVertexShadingSets))
		            {
			            this.AdditionalVertexColours = br.ReadIFFChunk<ModelVertexColours>();
		            }

		            if (this.Flags.HasFlag(GroupFlags.HasThreeTextureCoordinateSets))
		            {
			            this.SecondAddtionalTextureCoordinates = br.ReadIFFChunk<ModelTextureCoordinates>();
		            }
	            }
            }
		}

		/// <summary>
		/// Gets the static data signature of this data block type.
		/// </summary>
		/// <returns>A string representing the block signature.</returns>
		public string GetSignature()
		{
			return Signature;
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
            		bw.Write(this.GroupNameOffset);
		            bw.Write(this.DescriptiveGroupNameOffset);

		            // Set the flags according to present chunks
					UpdateFlags();
		            bw.Write((uint)this.Flags);

		            bw.WriteBox(this.BoundingBox);

		            bw.Write(this.PortalReferenceStartingIndex);
		            bw.Write(this.PortalReferenceCount);

		            bw.Write(this.RenderBatchCountA);
		            bw.Write(this.RenderBatchCountInterior);
		            bw.Write(this.RenderBatchCountExterior);
		            bw.Write(this.Unknown);

		            foreach (byte fogIndex in this.FogIndices)
		            {
			            bw.Write(fogIndex);
		            }

		            bw.Write(this.LiquidType);
		            bw.Write(this.GroupID.Value);

		            bw.Write(this.UnknownFlags);
		            bw.Write(this.Unused);

		            // Write the mandatory chunks
		            bw.WriteIFFChunk(this.PolygonMaterials);
		            bw.WriteIFFChunk(this.VertexIndices);
		            bw.WriteIFFChunk(this.Vertices);
		            bw.WriteIFFChunk(this.Normals);
		            bw.WriteIFFChunk(this.TextureCoordinates);
		            bw.WriteIFFChunk(this.RenderBatches);

		            // Write the optional chunks based on flags
		            if (this.mobs != null)
		            {
			            bw.WriteIFFChunk(this.mobs);
		            }

		            if (this.Flags.HasFlag(GroupFlags.HasLights))
					{
						bw.WriteIFFChunk(this.LightReferences);
					}

					if (this.Flags.HasFlag(GroupFlags.HasDoodads))
					{
						bw.WriteIFFChunk(this.DoodadReferences);
					}

					if (this.Flags.HasFlag(GroupFlags.HasBSP))
					{
						bw.WriteIFFChunk(this.BSPNodes);
						bw.WriteIFFChunk(this.BSPFaceIndices);
					}

					if (this.Flags.HasFlag(GroupFlags.UnknownLODRelated))
					{
						bw.WriteIFFChunk(this.mpbv);
						bw.WriteIFFChunk(this.mpbp);
						bw.WriteIFFChunk(this.mpbi);
						bw.WriteIFFChunk(this.mpbg);
					}

					if (this.Flags.HasFlag(GroupFlags.HasVertexColours))
					{
						bw.WriteIFFChunk(this.VertexColours);
					}

					if (this.Flags.HasFlag(GroupFlags.HasLiquids))
					{
						bw.WriteIFFChunk(this.Liquids);
					}

					if (this.Flags.HasFlag(GroupFlags.HasTriangleStrips))
					{
						bw.WriteIFFChunk(this.TriangleStripIndices);
						bw.WriteIFFChunk(this.TriangleStrips);
					}

					if (this.Flags.HasFlag(GroupFlags.HasTwoTextureCoordinateSets))
					{
						bw.WriteIFFChunk(this.AdditionalTextureCoordinates);
					}

					if (this.Flags.HasFlag(GroupFlags.HasTwoVertexShadingSets))
					{
						bw.WriteIFFChunk(this.AdditionalVertexColours);
					}

					if (this.Flags.HasFlag(GroupFlags.HasThreeTextureCoordinateSets))
					{
						bw.WriteIFFChunk(this.SecondAddtionalTextureCoordinates);
					}
            	}

            	return ms.ToArray();
            }
		}

		public void UpdateFlags()
		{
			 if (this.LightReferences != null)
			{
				this.Flags |= GroupFlags.HasLights;
			}

			if (this.DoodadReferences != null)
			{
				this.Flags |= GroupFlags.HasDoodads;
			}

			if (this.BSPNodes != null && this.BSPFaceIndices != null)
			{
				this.Flags |= GroupFlags.HasBSP;
			}

			if (this.mpbv != null && this.mpbp != null && this.mpbi != null && this.mpbg != null)
			{
				this.Flags |= GroupFlags.UnknownLODRelated;
			}

			if (this.VertexColours != null)
			{
				this.Flags |= GroupFlags.HasVertexColours;
			}

			if (this.Liquids != null)
			{
				this.Flags |= GroupFlags.HasLiquids;
			}

			if (this.TriangleStripIndices != null && this.TriangleStrips != null)
			{
				this.Flags |= GroupFlags.HasTriangleStrips;
			}

			if (this.AdditionalTextureCoordinates != null)
			{
				this.Flags |= GroupFlags.HasTwoTextureCoordinateSets;
			}

			if (this.AdditionalVertexColours != null)
			{
				this.Flags |= GroupFlags.HasTwoVertexShadingSets;
			}

			if (this.SecondAddtionalTextureCoordinates != null)
			{
				this.Flags |= GroupFlags.HasThreeTextureCoordinateSets;
			}
		}
	}

	[Flags]
	public enum GroupFlags : uint
	{
		HasBSP 							= 0x00000001,
		SubtractAmbientColour 			= 0x00000002,
		HasVertexColours 				= 0x00000004,
		IsOutdoors						= 0x00000008,
		// Unused1						= 0x00000010,
		// Unused2						= 0x00000020,
		DoNotUseLocalDiffuseLighting	= 0x00000040,
		Unreachable						= 0x00000080,
		// Unused3						= 0x00000100
		HasLights						= 0x00000200,
		UnknownLODRelated				= 0x00000400,
		HasDoodads						= 0x00000800,
		HasLiquids						= 0x00001000,
		IsIndoors						= 0x00002000,
		// Unused4						= 0x00004000,
		// Unused5						= 0x00008000,
		AlwaysDrawEvenIfOutdoors		= 0x00010000,
		HasTriangleStrips				= 0x00020000,
		ShowSkybox						= 0x00040000,
		IsOceanicWater					= 0x00080000,
		// Unused6						= 0x00100000,
		// Unused7						= 0x00200000,
		// Unused8						= 0x00400000,
		// Unused9						= 0x00800000,
		HasTwoVertexShadingSets			= 0x01000000,
		HasTwoTextureCoordinateSets		= 0x02000000,
		CreateOccluWithoutClearingBSP	= 0x04000000,
		UnknownOcclusionRelated			= 0x08000000,
		// Unused10						= 0x10000000,
		// Unused11						= 0x20000000,
		HasThreeTextureCoordinateSets	= 0x40000000
		// Unused12						= 0x80000000
	}
}

