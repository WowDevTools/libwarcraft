//
//  ModelStaticLighting.cs
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
using Warcraft.Core;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
	public class ModelStaticLighting : IRIFFChunk, IBinarySerializable
	{
		public const string Signature = "MOLT";

		public readonly List<StaticLight> StaticLights = new List<StaticLight>();

		public ModelStaticLighting()
		{

		}

		public ModelStaticLighting(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int lightCount = inData.Length / StaticLight.GetSize();
					for (uint i = 0; i < lightCount; ++i)
					{
						this.StaticLights.Add(new StaticLight(br.ReadBytes(StaticLight.GetSize())));
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
		            foreach (StaticLight staticLight in this.StaticLights)
		            {
			            bw.Write(staticLight.Serialize());
		            }
            	}

            	return ms.ToArray();
            }
		}
	}

	public class StaticLight : IBinarySerializable
	{
		public LightType Type;

		public bool bUseAttenuation;
		public bool bUseUnknown1;
		public bool bUseUnknown2;

		public BGRA Colour;
		public Vector3f Position;
		public float Intensity;

		public float AttenuationStartRadius;
		public float AttenuationEndRadius;

		public float Unknown1StartRadius;
		public float Unknown1EndRadius;

		public float Unknown2StartRadius;
		public float Unknown2EndRadius;

		public StaticLight(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Type = (LightType) br.ReadByte();
					this.bUseAttenuation = br.ReadBoolean();
					this.bUseUnknown1 = br.ReadBoolean();
					this.bUseUnknown2 = br.ReadBoolean();

					this.Colour = br.ReadBGRA();
					this.Position = br.ReadVector3f();
					this.Intensity = br.ReadSingle();

					this.AttenuationStartRadius = br.ReadSingle();
					this.AttenuationEndRadius = br.ReadSingle();

					this.Unknown1StartRadius = br.ReadSingle();
					this.Unknown1EndRadius = br.ReadSingle();

					this.Unknown2StartRadius = br.ReadSingle();
					this.Unknown2EndRadius = br.ReadSingle();
				}
			}
		}

		public static int GetSize()
		{
			return 48;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
            		bw.Write((byte)this.Type);

		            bw.Write(this.bUseAttenuation);
		            bw.Write(this.bUseUnknown1);
		            bw.Write(this.bUseUnknown2);

		            bw.WriteBGRA(this.Colour);
		            bw.WriteVector3f(this.Position);
		            bw.Write(this.Intensity);

		            bw.Write(this.AttenuationStartRadius);
		            bw.Write(this.AttenuationEndRadius);

		            bw.Write(this.Unknown1StartRadius);
		            bw.Write(this.Unknown1EndRadius);

		            bw.Write(this.Unknown2StartRadius);
		            bw.Write(this.Unknown2EndRadius);
	            }

            	return ms.ToArray();
            }
		}
	}

	public enum LightType : byte
	{
		Omnidirectional = 0,
		Spot 			= 1,
		Directional 	= 2,
		Ambient 		= 3
	}
}

