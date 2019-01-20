//
//  ModelRootHeader.cs
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

using System.IO;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.WMO.RootFile
{
    /// <summary>
    /// Represents the root header.
    /// </summary>
    public class ModelRootHeader : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOHD";

        /// <summary>
        /// Gets or sets the number of textures in the model.
        /// </summary>
        public uint TextureCount { get; set; }

        /// <summary>
        /// Gets or sets the number of groups in the model.
        /// </summary>
        public uint GroupCount { get; set; }

        /// <summary>
        /// Gets or sets the number of portals in the model.
        /// </summary>
        public uint PortalCount { get; set; }

        /// <summary>
        /// Gets or sets the number of lights in the model.
        /// </summary>
        public uint LightCount { get; set; }

        /// <summary>
        /// Gets or sets the number of doodad names in the group.
        /// </summary>
        public uint DoodadNameCount { get; set; }

        /// <summary>
        /// Gets or sets the number of doodad definitions in the group.
        /// </summary>
        public uint DoodadDefinitionCount { get; set; }

        /// <summary>
        /// Gets or sets the number of doodad sets in the group.
        /// </summary>
        public uint DoodadSetCount { get; set; }

        /// <summary>
        /// Gets or sets the base ambient lighting colour.
        /// </summary>
        public RGBA BaseAmbientColour { get; set; }

        /// <summary>
        /// Gets or sets the ID of the WMO information.
        /// </summary>
        public ForeignKey<uint> WMOID { get; set; }

        /// <summary>
        /// Gets or sets the model's complete bounding box.
        /// </summary>
        public Box BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the flags of the root group.
        /// </summary>
        public RootFlags Flags { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelRootHeader"/> class.
        /// </summary>
        public ModelRootHeader()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelRootHeader"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelRootHeader(byte[] inData)
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
                    TextureCount = br.ReadUInt32();
                    GroupCount = br.ReadUInt32();
                    PortalCount = br.ReadUInt32();
                    LightCount = br.ReadUInt32();
                    DoodadNameCount = br.ReadUInt32();
                    DoodadDefinitionCount = br.ReadUInt32();
                    DoodadSetCount = br.ReadUInt32();

                    BaseAmbientColour = br.ReadRGBA();
                    WMOID = new ForeignKey<uint>(DatabaseName.WMOAreaTable, nameof(WMOAreaTableRecord.WMOID), br.ReadUInt32());
                    BoundingBox = br.ReadBox();
                    Flags = (RootFlags)br.ReadUInt32();
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
                    bw.Write(TextureCount);
                    bw.Write(GroupCount);
                    bw.Write(PortalCount);
                    bw.Write(LightCount);
                    bw.Write(DoodadNameCount);
                    bw.Write(DoodadDefinitionCount);
                    bw.Write(DoodadSetCount);

                    bw.WriteRGBA(BaseAmbientColour);
                    bw.Write(WMOID.Key);
                    bw.WriteBox(BoundingBox);
                    bw.Write((uint)Flags);
                }

                return ms.ToArray();
            }
        }
    }
}
