//
//  ModelGroupData.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;
using Warcraft.WMO.GroupFile.Chunks;

namespace Warcraft.WMO.GroupFile
{
    /// <summary>
    /// Holds group data for a model.
    /// </summary>
    public class ModelGroupData : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOGP";

        /// <summary>
        /// Gets or sets the offset of the group name.
        /// </summary>
        public uint GroupNameOffset { get; set; }

        /// <summary>
        /// Gets or sets the offset of the descriptive name.
        /// </summary>
        public uint DescriptiveGroupNameOffset { get; set; }

        /// <summary>
        /// Gets or sets the flags of the group.
        /// </summary>
        public GroupFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the bounding box of the group.
        /// </summary>
        public Box BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the first index of the portal references.
        /// </summary>
        public ushort FirstPortalReferenceIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of portal references.
        /// </summary>
        public ushort PortalReferenceCount { get; set; }

        /// <summary>
        /// Gets or sets the rendering batch count.
        /// </summary>
        public ushort RenderBatchCountA { get; set; }

        /// <summary>
        /// Gets or sets the number of interior render batches.
        /// </summary>
        public ushort RenderBatchCountInterior { get; set; }

        /// <summary>
        /// Gets or sets the number of exterior render batches.
        /// </summary>
        public ushort RenderBatchCountExterior { get; set; }

        /// <summary>
        /// Gets or sets an unknown value.
        /// </summary>
        public ushort Unknown { get; set; }

        /// <summary>
        /// Gets the fog indexes.
        /// </summary>
        public List<byte> FogIndices { get; } = new List<byte>(4);

        /// <summary>
        /// Gets or sets the model's liquid type.
        /// </summary>
        public uint LiquidType { get; set; }

        /// <summary>
        /// Gets or sets the model's group ID.
        /// </summary>
        public ForeignKey<uint> GroupID { get; set; }

        /// <summary>
        /// Gets or sets a set of unknown flags.
        /// </summary>
        public uint UnknownFlags { get; set; }

        /// <summary>
        /// Gets or sets an unused value.
        /// </summary>
        public uint Unused { get; set; }

        // The chunks in a model group file are actually subchunks of this one. They are listed here
        // for the sake of correctness, and are then given abstraction accessors in the ModelGroup class.

        /// <summary>
        /// Gets or sets the polygon materials.
        /// </summary>
        public ModelPolygonMaterials PolygonMaterials { get; set; }

        /// <summary>
        /// Gets or sets the vertex indexes.
        /// </summary>
        public ModelVertexIndices VertexIndices { get; set; }

        /// <summary>
        /// Gets or sets the vertexes.
        /// </summary>
        public ModelVertices Vertices { get; set; }

        /// <summary>
        /// Gets or sets the normals.
        /// </summary>
        public ModelNormals Normals { get; set; }

        /// <summary>
        /// Gets or sets the texture coordinates.
        /// </summary>
        public ModelTextureCoordinates TextureCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the render batches.
        /// </summary>
        public ModelRenderBatches RenderBatches { get; set; }

        // The following chunks are optional, and are read based on flags in the header.

        /// <summary>
        /// Gets or sets the light references.
        /// </summary>
        public ModelLightReferences LightReferences { get; set; }

        /// <summary>
        /// Gets or sets doodad references.
        /// </summary>
        public ModelDoodadReferences DoodadReferences { get; set; }

        /// <summary>
        /// Gets or sets the BSP nodes.
        /// </summary>
        public ModelBSPNodes BSPNodes { get; set; }

        /// <summary>
        /// Gets or sets the BSP face indexes.
        /// </summary>
        public ModelBSPFaceIndices BSPFaceIndices { get; set; }

        /// <summary>
        /// Gets or sets the vertex colours.
        /// </summary>
        public ModelVertexColours VertexColours { get; set; }

        /// <summary>
        /// Gets or sets the liquids.
        /// </summary>
        public ModelLiquids Liquids { get; set; }

        /// <summary>
        /// Gets or sets the additional texture coordinates.
        /// </summary>
        public ModelTextureCoordinates AdditionalTextureCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the additional vertex colours.
        /// </summary>
        public ModelVertexColours AdditionalVertexColours { get; set; }

        /// <summary>
        /// Gets or sets the second set of additional texture coordinates.
        /// </summary>
        public ModelTextureCoordinates SecondAddtionalTextureCoordinates { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelGroupData"/> class.
        /// </summary>
        public ModelGroupData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelGroupData"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelGroupData(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <summary>
        /// Deserialzes the provided binary data of the object. This is the full data block which follows the data
        /// signature and data block length.
        /// </summary>
        /// <param name="inData">The binary data containing the object.</param>
        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    GroupNameOffset = br.ReadUInt32();
                    DescriptiveGroupNameOffset = br.ReadUInt32();

                    Flags = (GroupFlags)br.ReadUInt32();

                    BoundingBox = br.ReadBox();

                    FirstPortalReferenceIndex = br.ReadUInt16();
                    PortalReferenceCount = br.ReadUInt16();

                    RenderBatchCountA = br.ReadUInt16();
                    RenderBatchCountInterior = br.ReadUInt16();
                    RenderBatchCountExterior = br.ReadUInt16();
                    Unknown = br.ReadUInt16();

                    for (var i = 0; i < 4; ++i)
                    {
                        FogIndices.Add(br.ReadByte());
                    }

                    LiquidType = br.ReadUInt32();
                    GroupID = new ForeignKey<uint>(DatabaseName.WMOAreaTable, nameof(WMOAreaTableRecord.WMOGroupID), br.ReadUInt32());

                    UnknownFlags = br.ReadUInt32();
                    Unused = br.ReadUInt32();

                    // Required subchunks
                    PolygonMaterials = br.ReadIFFChunk<ModelPolygonMaterials>();
                    VertexIndices = br.ReadIFFChunk<ModelVertexIndices>();
                    Vertices = br.ReadIFFChunk<ModelVertices>();
                    Normals = br.ReadIFFChunk<ModelNormals>();
                    TextureCoordinates = br.ReadIFFChunk<ModelTextureCoordinates>();
                    RenderBatches = br.ReadIFFChunk<ModelRenderBatches>();

                    // Optional chunks
                    if (Flags.HasFlag(GroupFlags.HasLights))
                    {
                        LightReferences = br.ReadIFFChunk<ModelLightReferences>();
                    }

                    if (Flags.HasFlag(GroupFlags.HasDoodads))
                    {
                        DoodadReferences = br.ReadIFFChunk<ModelDoodadReferences>();
                    }

                    if (Flags.HasFlag(GroupFlags.HasBSP))
                    {
                        BSPNodes = br.ReadIFFChunk<ModelBSPNodes>();
                        BSPFaceIndices = br.ReadIFFChunk<ModelBSPFaceIndices>();
                    }

                    if (Flags.HasFlag(GroupFlags.HasVertexColours))
                    {
                        VertexColours = br.ReadIFFChunk<ModelVertexColours>();
                    }

                    if (Flags.HasFlag(GroupFlags.HasLiquids))
                    {
                        Liquids = br.ReadIFFChunk<ModelLiquids>();
                    }

                    if (Flags.HasFlag(GroupFlags.HasTwoTextureCoordinateSets))
                    {
                        AdditionalTextureCoordinates = br.ReadIFFChunk<ModelTextureCoordinates>();
                    }

                    if (Flags.HasFlag(GroupFlags.HasTwoVertexShadingSets))
                    {
                        AdditionalVertexColours = br.ReadIFFChunk<ModelVertexColours>();
                    }

                    if (Flags.HasFlag(GroupFlags.HasThreeTextureCoordinateSets))
                    {
                        SecondAddtionalTextureCoordinates = br.ReadIFFChunk<ModelTextureCoordinates>();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the static data signature of this data block type.
        /// </summary>
        /// <returns>A string representing the block signature.</returns>
        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <summary>
        /// Serializes the current object into a byte array.
        /// </summary>
        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(GroupNameOffset);
                    bw.Write(DescriptiveGroupNameOffset);

                    // Set the flags according to present chunks
                    UpdateFlags();
                    bw.Write((uint)Flags);

                    bw.WriteBox(BoundingBox);

                    bw.Write(FirstPortalReferenceIndex);
                    bw.Write(PortalReferenceCount);

                    bw.Write(RenderBatchCountA);
                    bw.Write(RenderBatchCountInterior);
                    bw.Write(RenderBatchCountExterior);
                    bw.Write(Unknown);

                    foreach (var fogIndex in FogIndices)
                    {
                        bw.Write(fogIndex);
                    }

                    bw.Write(LiquidType);
                    bw.Write(GroupID.Key);

                    bw.Write(UnknownFlags);
                    bw.Write(Unused);

                    // Write the mandatory chunks
                    bw.WriteIFFChunk(PolygonMaterials);
                    bw.WriteIFFChunk(VertexIndices);
                    bw.WriteIFFChunk(Vertices);
                    bw.WriteIFFChunk(Normals);
                    bw.WriteIFFChunk(TextureCoordinates);
                    bw.WriteIFFChunk(RenderBatches);

                    // Write the optional chunks based on flags
                    if (Flags.HasFlag(GroupFlags.HasLights))
                    {
                        bw.WriteIFFChunk(LightReferences);
                    }

                    if (Flags.HasFlag(GroupFlags.HasDoodads))
                    {
                        bw.WriteIFFChunk(DoodadReferences);
                    }

                    if (Flags.HasFlag(GroupFlags.HasBSP))
                    {
                        bw.WriteIFFChunk(BSPNodes);
                        bw.WriteIFFChunk(BSPFaceIndices);
                    }

                    if (Flags.HasFlag(GroupFlags.HasVertexColours))
                    {
                        bw.WriteIFFChunk(VertexColours);
                    }

                    if (Flags.HasFlag(GroupFlags.HasLiquids))
                    {
                        bw.WriteIFFChunk(Liquids);
                    }

                    if (Flags.HasFlag(GroupFlags.HasTwoTextureCoordinateSets))
                    {
                        bw.WriteIFFChunk(AdditionalTextureCoordinates);
                    }

                    if (Flags.HasFlag(GroupFlags.HasTwoVertexShadingSets))
                    {
                        bw.WriteIFFChunk(AdditionalVertexColours);
                    }

                    if (Flags.HasFlag(GroupFlags.HasThreeTextureCoordinateSets))
                    {
                        bw.WriteIFFChunk(SecondAddtionalTextureCoordinates);
                    }
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Updates the flags of the model.
        /// </summary>
        public void UpdateFlags()
        {
            if (LightReferences != null)
            {
                Flags |= GroupFlags.HasLights;
            }

            if (DoodadReferences != null)
            {
                Flags |= GroupFlags.HasDoodads;
            }

            if (BSPNodes != null && BSPFaceIndices != null)
            {
                Flags |= GroupFlags.HasBSP;
            }

            if (VertexColours != null)
            {
                Flags |= GroupFlags.HasVertexColours;
            }

            if (Liquids != null)
            {
                Flags |= GroupFlags.HasLiquids;
            }

            if (AdditionalTextureCoordinates != null)
            {
                Flags |= GroupFlags.HasTwoTextureCoordinateSets;
            }

            if (AdditionalVertexColours != null)
            {
                Flags |= GroupFlags.HasTwoVertexShadingSets;
            }

            if (SecondAddtionalTextureCoordinates != null)
            {
                Flags |= GroupFlags.HasThreeTextureCoordinateSets;
            }
        }
    }
}
