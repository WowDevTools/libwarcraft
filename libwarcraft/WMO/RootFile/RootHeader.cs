//
//  RootHeader.cs
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
using System.IO;
using Warcraft.ADT.Chunks;
using Warcraft.Core;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.WMO.RootFile
{
	public class RootHeader : IChunk
	{
		public const string Signature = "MOHD";

		public uint TextureCount;
		public uint GroupCount;
		public uint PortalCount;
		public uint LightCount;
		public uint DoodadNameCount;
		public uint DoodadDefinitionCount;
		public uint DoodadSetCount;
		public RGBA BaseAmbientColour;
		public UInt32ForeignKey AreaTableID;
		public Box BoundingBox;
		public RootFlags Flags;

		public RootHeader(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.TextureCount = br.ReadUInt32();
					this.GroupCount = br.ReadUInt32();
					this.PortalCount = br.ReadUInt32();
					this.LightCount = br.ReadUInt32();
					this.DoodadNameCount = br.ReadUInt32();
					this.DoodadDefinitionCount = br.ReadUInt32();
					this.DoodadSetCount = br.ReadUInt32();

					this.BaseAmbientColour = br.ReadRGBA();
					this.AreaTableID = new UInt32ForeignKey("WMOAreaTable", "WMOID", br.ReadUInt32());
					this.BoundingBox = br.ReadBox();
					this.Flags = (RootFlags) br.ReadUInt32();
				}
			}
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					bw.Write(this.TextureCount);
					bw.Write(this.GroupCount);
					bw.Write(this.PortalCount);
					bw.Write(this.LightCount);
					bw.Write(this.DoodadNameCount);
					bw.Write(this.DoodadDefinitionCount);
					bw.Write(this.DoodadSetCount);

					bw.WriteRGBA(this.BaseAmbientColour);
					bw.Write(this.AreaTableID.Value);
					bw.WriteBox(this.BoundingBox);
					bw.Write((uint) this.Flags);
				}

				return ms.ToArray();
			}
		}
	}

	[Flags]
	public enum RootFlags : uint
	{
		AttenuateVerticesBasedOnPortalDistance 	= 0x01,
		SkipAddingBaseAmbientColour 			= 0x02,
		LiquidFilled 							= 0x04,
		HasOutdoorGroups 						= 0x08,
		HasLevelsOfDetail						= 0x10
		// Followed by 27 unused flags
	}
}

