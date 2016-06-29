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
using Warcraft.ADT.Chunks;
using Warcraft.Core;
using Warcraft.Core.Shading;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.WMO.RootFile.Chunks
{
	public class ModelMaterials : IChunk
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
	}

	public class ModelMaterial
	{
		public ShaderTypes Shader;
		public BlendingMode BlendMode;

		public uint Texture0Offset;
		public RGBA Colour0;
		public MaterialFlags Flags0;

		public uint Texture1Offset;
		public RGBA Colour1;

		public UInt32ForeignKey GroundType;
		public uint Texture2Offset;
		public RGBA BaseDiffuseColour;
		public MaterialFlags Flags2;

		public ModelMaterial(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Shader = (ShaderTypes) br.ReadUInt32();
					this.BlendMode = (BlendingMode) br.ReadUInt32();

					this.Texture0Offset = br.ReadUInt32();
					this.Colour0 = br.ReadRGBA();
					this.Flags0  = (MaterialFlags)br.ReadUInt32();

					this.Texture1Offset = br.ReadUInt32();
					this.Colour1 = br.ReadRGBA();

					this.GroundType = new UInt32ForeignKey("TerrainType", "ID", br.ReadUInt32());
					this.Texture2Offset = br.ReadUInt32();
					this.BaseDiffuseColour = br.ReadRGBA();
					this.Flags2 = (MaterialFlags)br.ReadUInt32();
				}
			}
		}

		public static int GetSize()
		{
			return 64;
		}
	}

	[Flags]
	public enum MaterialFlags : uint
	{
		UnknownPossiblyLightmap = 0x001,
		Unknown2				= 0x002,
		TwoSided				= 0x004,
		Darken					= 0x008,
		UnshadedDuringNight		= 0x010,
		Unknown3				= 0x020,
		TextureWrappingClamp	= 0x040,
		TextureWrappingRepeat	= 0x080,
		Unknown4				= 0x100

		// Followed by 23 unused flags
	}

	public enum BlendingMode : uint
	{
		Opaque = 0,
		Transparent = 1
	}
}

