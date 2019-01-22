//
//  ModelMaterial.cs
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
using Warcraft.Core.Shading;
using Warcraft.Core.Shading.Blending;
using Warcraft.Core.Structures;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Represents a material in a model.
    /// </summary>
    public class ModelMaterial : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the material flags.
        /// </summary>
        public MaterialFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the fragment shader type.
        /// </summary>
        public WMOFragmentShaderType Shader { get; set; }

        /// <summary>
        /// Gets or sets the blending mode.
        /// </summary>
        public BlendingMode BlendMode { get; set; }

        /// <summary>
        /// Gets or sets the offset to the diffuse texture.
        /// </summary>
        public uint DiffuseTextureOffset { get; set; }

        /// <summary>
        /// Gets or sets the first colour.
        /// </summary>
        public RGBA EmissiveColour { get; set; }

        /// <summary>
        /// Gets or sets the runtime emissive colour for the current frame.
        /// </summary>
        public RGBA FrameEmissiveColour { get; set; }

        /// <summary>
        /// Gets or sets the offset to the environment map.
        /// </summary>
        public uint EnvironmentMapTextureOffset { get; set; }

        /// <summary>
        /// Gets or sets the diffuse colour.
        /// </summary>
        public RGBA DiffuseColour { get; set; }

        /// <summary>
        /// Gets or sets the ground type of the material.
        /// </summary>
        public ForeignKey<uint> GroundType { get; set; }

        /// <summary>
        /// Gets or sets the offset to the specular texture.
        /// </summary>
        public uint SpecularTextureOffset { get; set; }

        /// <summary>
        /// Gets or sets the base diffuse colour.
        /// </summary>
        public RGBA BaseDiffuseColour { get; set; }

        /// <summary>
        /// Gets or sets some unknown flags.
        /// </summary>
        public uint UnknownFlags { get; set; }

        /// <summary>
        /// Gets or sets runtime data.
        /// </summary>
        public uint RuntimeData1 { get; set; }

        /// <summary>
        /// Gets or sets runtime data.
        /// </summary>
        public uint RuntimeData2 { get; set; }

        /// <summary>
        /// Gets or sets runtime data.
        /// </summary>
        public uint RuntimeData3 { get; set; }

        /// <summary>
        /// Gets or sets runtime data.
        /// </summary>
        public uint RuntimeData4 { get; set; }

        /*
            Nonserialized utility fields
        */

        /// <summary>
        /// Gets or sets the name of the diffuse texture.
        /// </summary>
        public string DiffuseTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the environment map texture.
        /// </summary>
        public string EnvironmentMapTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the specularity texture.
        /// </summary>
        public string SpecularTexture
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMaterial"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelMaterial(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    Flags = (MaterialFlags)br.ReadUInt32();
                    Shader = (WMOFragmentShaderType)br.ReadUInt32();
                    BlendMode = (BlendingMode)br.ReadUInt32();

                    DiffuseTextureOffset = br.ReadUInt32();
                    EmissiveColour = br.ReadRGBA();
                    FrameEmissiveColour = br.ReadRGBA();

                    EnvironmentMapTextureOffset = br.ReadUInt32();
                    DiffuseColour = br.ReadRGBA();

                    GroundType = new ForeignKey<uint>(DatabaseName.TerrainType, nameof(DBCRecord.ID), br.ReadUInt32()); // TODO: Implement TerrainTypeRecord
                    SpecularTextureOffset = br.ReadUInt32();
                    BaseDiffuseColour = br.ReadRGBA();
                    UnknownFlags = br.ReadUInt32();

                    RuntimeData1 = br.ReadUInt32();
                    RuntimeData2 = br.ReadUInt32();
                    RuntimeData3 = br.ReadUInt32();
                    RuntimeData4 = br.ReadUInt32();
                }
            }
        }

        /// <summary>
        /// Gets the serialized size of the instance.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 64;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write((uint)Flags);
                    bw.Write((uint)Shader);
                    bw.Write((uint)BlendMode);

                    bw.Write(DiffuseTextureOffset);
                    bw.WriteRGBA(EmissiveColour);
                    bw.WriteRGBA(FrameEmissiveColour);

                    bw.Write(EnvironmentMapTextureOffset);
                    bw.WriteRGBA(DiffuseColour);

                    bw.Write(GroundType.Key);
                    bw.Write(SpecularTextureOffset);
                    bw.WriteRGBA(BaseDiffuseColour);
                    bw.Write(UnknownFlags);

                    bw.Write(RuntimeData1);
                    bw.Write(RuntimeData2);
                    bw.Write(RuntimeData3);
                    bw.Write(RuntimeData4);
                }

                return ms.ToArray();
            }
        }
    }
}
