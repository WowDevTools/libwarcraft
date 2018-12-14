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

using System;
using System.IO;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.WMO.RootFile
{
    public class ModelRootHeader : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOHD";

        public uint TextureCount;
        public uint GroupCount;
        public uint PortalCount;
        public uint LightCount;
        public uint DoodadNameCount;
        public uint DoodadDefinitionCount;
        public uint DoodadSetCount;
        public RGBA BaseAmbientColour;
        public ForeignKey<uint> WMOID;
        public Box BoundingBox;
        public RootFlags Flags;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelRootHeader"/> class.
        /// </summary>
        public ModelRootHeader()
        {
        }

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
                    Flags = (RootFlags) br.ReadUInt32();
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
                    bw.Write((uint) Flags);
                }

                return ms.ToArray();
            }
        }
    }

    [Flags]
    public enum RootFlags : uint
    {
        AttenuateVerticesBasedOnPortalDistance     = 0x01,
        SkipAddingBaseAmbientColour             = 0x02,
        LiquidFilled                             = 0x04,
        HasOutdoorGroups                         = 0x08,
        HasLevelsOfDetail                        = 0x10
        // Followed by 27 unused flags
    }
}

