//
//  ModelFog.cs
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

namespace Warcraft.WMO.RootFile.Chunks
{
	public class ModelFog : IRIFFChunk, IBinarySerializable
	{
		public const string Signature = "MFOG";

		public readonly List<FogInstance> FogInstances = new List<FogInstance>();

		public ModelFog()
		{

		}

		public ModelFog(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int fogInstanceCount = inData.Length / FogInstance.GetSize();
					for (int i = 0; i < fogInstanceCount; ++i)
					{
						this.FogInstances.Add(new FogInstance(br.ReadBytes(FogInstance.GetSize())));
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
		            foreach (FogInstance fogInstance in this.FogInstances)
		            {
			            bw.Write(fogInstance.Serialize());
		            }
            	}

            	return ms.ToArray();
            }
		}
	}

	public class FogInstance : IBinarySerializable
	{
		public FogFlags Flags;
		public Vector3f Position;

		public float GlobalStartRadius;
		public float GlobalEndRadius;

		public FogDefinition LandFog;
		public FogDefinition UnderwaterFog;

		public FogInstance(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Flags = (FogFlags) br.ReadUInt32();
					this.Position = br.ReadVector3f();

					this.GlobalStartRadius = br.ReadSingle();
					this.GlobalEndRadius = br.ReadSingle();

					this.LandFog = new FogDefinition(br.ReadBytes(FogDefinition.GetSize()));
					this.UnderwaterFog = new FogDefinition(br.ReadBytes(FogDefinition.GetSize()));
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
            		bw.Write((uint)this.Flags);
		            bw.WriteVector3f(this.Position);

		            bw.Write(this.GlobalStartRadius);
		            bw.Write(this.GlobalEndRadius);

		            bw.Write(this.LandFog.Serialize());
		            bw.Write(this.UnderwaterFog.Serialize());
            	}

            	return ms.ToArray();
            }
		}
	}

	public class FogDefinition : IBinarySerializable
	{
		public float EndRadius;
		public float StartMultiplier;
		public BGRA Colour;

		public FogDefinition(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.EndRadius = br.ReadSingle();
					this.StartMultiplier = br.ReadSingle();
					this.Colour = br.ReadBGRA();
				}
			}
		}

		public static int GetSize()
		{
			return 12;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
            		bw.Write(this.EndRadius);
		            bw.Write(this.StartMultiplier);
		            bw.WriteBGRA(this.Colour);
            	}

            	return ms.ToArray();
            }
		}
	}

	[Flags]
	public enum FogFlags : uint
	{
		InfiniteRadius	= 0x01,
		// Unused1		= 0x02,
		// Unused2		= 0x04,
		Unknown1		= 0x10,
		// Followed by 27 unused values
	}
}

