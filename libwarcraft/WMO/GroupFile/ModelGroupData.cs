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

using System;
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
    public class ModelGroupData : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
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
        public ForeignKey<uint> GroupID;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelGroupData"/> class.
        /// </summary>
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
                    GroupNameOffset = br.ReadUInt32();
                    DescriptiveGroupNameOffset = br.ReadUInt32();

                    Flags = (GroupFlags) br.ReadUInt32();

                    BoundingBox = br.ReadBox();

                    PortalReferenceStartingIndex = br.ReadUInt16();
                    PortalReferenceCount = br.ReadUInt16();

                    RenderBatchCountA = br.ReadUInt16();
                    RenderBatchCountInterior = br.ReadUInt16();
                    RenderBatchCountExterior = br.ReadUInt16();
                    Unknown = br.ReadUInt16();

                    for (int i = 0; i < 4; ++i)
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
                    if (br.PeekChunkSignature() == MOBS.Signature)
                    {
                        mobs = br.ReadIFFChunk<MOBS>();
                    }

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

                    if (Flags.HasFlag(GroupFlags.UnknownLODRelated))
                    {
                        mpbv = br.ReadIFFChunk<MPBV>();
                        mpbp = br.ReadIFFChunk<MPBP>();
                        mpbi = br.ReadIFFChunk<MPBI>();
                        mpbg = br.ReadIFFChunk<MPBG>();
                    }

                    if (Flags.HasFlag(GroupFlags.HasVertexColours))
                    {
                        VertexColours = br.ReadIFFChunk<ModelVertexColours>();
                    }

                    if (Flags.HasFlag(GroupFlags.HasLiquids))
                    {
                        Liquids = br.ReadIFFChunk<ModelLiquids>();
                    }

                    if (Flags.HasFlag(GroupFlags.HasTriangleStrips))
                    {
                        TriangleStripIndices = br.ReadIFFChunk<ModelTriangleStripIndices>();
                        TriangleStrips = br.ReadIFFChunk<ModelTriangleStrips>();
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
                    bw.Write(GroupNameOffset);
                    bw.Write(DescriptiveGroupNameOffset);

                    // Set the flags according to present chunks
                    UpdateFlags();
                    bw.Write((uint)Flags);

                    bw.WriteBox(BoundingBox);

                    bw.Write(PortalReferenceStartingIndex);
                    bw.Write(PortalReferenceCount);

                    bw.Write(RenderBatchCountA);
                    bw.Write(RenderBatchCountInterior);
                    bw.Write(RenderBatchCountExterior);
                    bw.Write(Unknown);

                    foreach (byte fogIndex in FogIndices)
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
                    if (mobs != null)
                    {
                        bw.WriteIFFChunk(mobs);
                    }

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

                    if (Flags.HasFlag(GroupFlags.UnknownLODRelated))
                    {
                        bw.WriteIFFChunk(mpbv);
                        bw.WriteIFFChunk(mpbp);
                        bw.WriteIFFChunk(mpbi);
                        bw.WriteIFFChunk(mpbg);
                    }

                    if (Flags.HasFlag(GroupFlags.HasVertexColours))
                    {
                        bw.WriteIFFChunk(VertexColours);
                    }

                    if (Flags.HasFlag(GroupFlags.HasLiquids))
                    {
                        bw.WriteIFFChunk(Liquids);
                    }

                    if (Flags.HasFlag(GroupFlags.HasTriangleStrips))
                    {
                        bw.WriteIFFChunk(TriangleStripIndices);
                        bw.WriteIFFChunk(TriangleStrips);
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

            if (mpbv != null && mpbp != null && mpbi != null && mpbg != null)
            {
                Flags |= GroupFlags.UnknownLODRelated;
            }

            if (VertexColours != null)
            {
                Flags |= GroupFlags.HasVertexColours;
            }

            if (Liquids != null)
            {
                Flags |= GroupFlags.HasLiquids;
            }

            if (TriangleStripIndices != null && TriangleStrips != null)
            {
                Flags |= GroupFlags.HasTriangleStrips;
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

    [Flags]
    public enum GroupFlags : uint
    {
        HasBSP                             = 0x00000001,
        SubtractAmbientColour             = 0x00000002,
        HasVertexColours                 = 0x00000004,
        IsOutdoors                        = 0x00000008,
        // Unused1                        = 0x00000010,
        // Unused2                        = 0x00000020,
        DoNotUseLocalDiffuseLighting    = 0x00000040,
        Unreachable                        = 0x00000080,
        // Unused3                        = 0x00000100
        HasLights                        = 0x00000200,
        UnknownLODRelated                = 0x00000400,
        HasDoodads                        = 0x00000800,
        HasLiquids                        = 0x00001000,
        IsIndoors                        = 0x00002000,
        // Unused4                        = 0x00004000,
        // Unused5                        = 0x00008000,
        AlwaysDrawEvenIfOutdoors        = 0x00010000,
        HasTriangleStrips                = 0x00020000,
        ShowSkybox                        = 0x00040000,
        IsOceanicWater                    = 0x00080000,
        // Unused6                        = 0x00100000,
        // Unused7                        = 0x00200000,
        // Unused8                        = 0x00400000,
        // Unused9                        = 0x00800000,
        HasTwoVertexShadingSets            = 0x01000000,
        HasTwoTextureCoordinateSets        = 0x02000000,
        CreateOccluWithoutClearingBSP    = 0x04000000,
        UnknownOcclusionRelated            = 0x08000000,
        // Unused10                        = 0x10000000,
        // Unused11                        = 0x20000000,
        HasThreeTextureCoordinateSets    = 0x40000000
        // Unused12                        = 0x80000000
    }
}

