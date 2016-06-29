//
//  ModelDoodadInstances.cs
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

namespace Warcraft.WMO.RootFile.Chunks
{
	public class ModelDoodadInstances : IChunk
	{
		public const string Signature = "MODD";

		public  readonly List<DoodadInstance> DoodadInstances = new List<DoodadInstance>();

		public ModelDoodadInstances()
		{

		}

		public ModelDoodadInstances(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int instanceCount = inData.Length / DoodadInstance.GetSize();
					for (int i = 0; i < instanceCount; ++i)
					{
						this.DoodadInstances.Add(new DoodadInstance(br.ReadBytes(DoodadInstance.GetSize())));
					}
				}
			}
        }

        public string GetSignature()
        {
        	return Signature;
        }
	}

	public class DoodadInstance
	{
		public string Name
		{
			get;
			set;
		}

		// The nameoffset and flags are actually stored as an uint24 and an uint8,
		// that is, three bytes for the offset and one byte for the flags. It's weird.
		public uint NameOffset;
		public DoodadInstanceFlags Flags;


		public Vector3f Position;
		public Quaternion Orientation;
		public float Scale;
		public BGRA StaticLightingColour;

		public DoodadInstance(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					byte[] finalNameBytes = new byte[4];
					byte[] nameOffsetBytes = br.ReadBytes(3);
					Buffer.BlockCopy(nameOffsetBytes, 0, finalNameBytes, 0, 3);

					this.NameOffset= BitConverter.ToUInt32(finalNameBytes, 0);

					this.Flags = (DoodadInstanceFlags) br.ReadByte();

					this.Position = br.ReadVector3f();

					// TODO: Investigate whether or not this is a Quat16 in >= BC
					this.Orientation = br.ReadQuaternion32();

					this.Scale = br.ReadSingle();
					this.StaticLightingColour = br.ReadBGRA();
				}
			}
		}

		public static int GetSize()
		{
			return 40;
		}
	}

	[Flags]
	public enum DoodadInstanceFlags : byte
	{
		AcceptProjectedTexture 		= 0x1,
		Unknown1					= 0x2,
		Unknown2					= 0x4,
		Unknown3					= 0x8
	}
}

