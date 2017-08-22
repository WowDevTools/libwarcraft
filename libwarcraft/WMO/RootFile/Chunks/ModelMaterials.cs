//
//  ModelMaterials.cs
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
	public class ModelMaterials : IIFFChunk, IBinarySerializable
	{
		public const string Signature = "MOMT";

		public readonly List<ModelMaterial> Materials = new List<ModelMaterial>();

		public ModelMaterials()
		{

		}

		public ModelMaterials(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
	        using (MemoryStream ms = new MemoryStream(inData))
	        {
		        using (BinaryReader br = new BinaryReader(ms))
		        {
			        int materialCount = inData.Length / ModelMaterial.GetSize();
			        for (int i = 0; i < materialCount; ++i)
			        {
				        this.Materials.Add(new ModelMaterial(br.ReadBytes(ModelMaterial.GetSize())));
			        }
		        }
	        }
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
		            foreach (ModelMaterial modelMaterial in this.Materials)
		            {
			            bw.Write(modelMaterial.Serialize());
		            }
            	}

            	return ms.ToArray();
            }
		}
	}

	public class ModelMaterial : IBinarySerializable
	{
		public MaterialFlags Flags;
		public WMOFragmentShaderType Shader;
		public BlendingMode BlendMode;

		public uint FirstTextureOffset;
		public RGBA FirstColour;
		public MaterialFlags FirstFlags;

		public uint SecondTextureOffset;
		public RGBA SecondColour;

		public ForeignKey<uint> GroundType;
		public uint ThirdTextureOffset;
		public RGBA BaseDiffuseColour;
		public MaterialFlags ThirdFlags;

		public uint RuntimeData1;
		public uint RuntimeData2;
		public uint RuntimeData3;
		public uint RuntimeData4;

		/*
			Nonserialized utility fields
		*/

		public string Texture0
		{
			get;
			set;
		}

		public string Texture1
		{
			get;
			set;
		}

		public string Texture2
		{
			get;
			set;
		}

		public ModelMaterial(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Flags = (MaterialFlags) br.ReadUInt32();
					this.Shader = (WMOFragmentShaderType) br.ReadUInt32();
					this.BlendMode = (BlendingMode) br.ReadUInt32();

					this.FirstTextureOffset = br.ReadUInt32();
					this.FirstColour = br.ReadRGBA();
					this.FirstFlags  = (MaterialFlags)br.ReadUInt32();

					this.SecondTextureOffset = br.ReadUInt32();
					this.SecondColour = br.ReadRGBA();

					this.GroundType = new ForeignKey<uint>(DatabaseName.TerrainType, nameof(DBCRecord.ID), br.ReadUInt32()); // TODO: Implement TerrainTypeRecord
					this.ThirdTextureOffset = br.ReadUInt32();
					this.BaseDiffuseColour = br.ReadRGBA();
					this.ThirdFlags = (MaterialFlags)br.ReadUInt32();

					this.RuntimeData1 = br.ReadUInt32();
					this.RuntimeData2 = br.ReadUInt32();
					this.RuntimeData3 = br.ReadUInt32();
					this.RuntimeData4 = br.ReadUInt32();
				}
			}
		}

		public static int GetSize()
		{
			return 64;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
            		bw.Write((uint)this.Flags);
		            bw.Write((uint)this.Shader);
		            bw.Write((uint)this.BlendMode);

		            bw.Write(this.FirstTextureOffset);
		            bw.WriteRGBA(this.FirstColour);
		            bw.Write((uint)this.FirstFlags);

		            bw.Write(this.SecondTextureOffset);
		            bw.WriteRGBA(this.SecondColour);

		            bw.Write((uint)this.GroundType.Key);
		            bw.Write(this.ThirdTextureOffset);
		            bw.WriteRGBA(this.BaseDiffuseColour);
		            bw.Write((uint)this.ThirdFlags);

		            bw.Write(this.RuntimeData1);
		            bw.Write(this.RuntimeData2);
		            bw.Write(this.RuntimeData3);
		            bw.Write(this.RuntimeData4);
            	}

            	return ms.ToArray();
            }
		}
	}

	[Flags]
	public enum MaterialFlags : uint
	{
		UnknownPossiblyLightmap = 0x1,
		Unknown2				= 0x2,
		TwoSided				= 0x4,
		Darken					= 0x8,
		UnshadedDuringNight		= 0x10,
		Unknown3				= 0x20,
		TextureWrappingClamp	= 0x40,
		TextureWrappingRepeat	= 0x80,
		Unknown4				= 0x100

		// Followed by 23 unused flags
	}
}

