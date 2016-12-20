//
//  ModelPortals.cs
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
	public class ModelPortals : IRIFFChunk, IBinarySerializable
	{
		public const string Signature = "MOPT";

		public readonly List<Portal> Portals = new List<Portal>();

		public ModelPortals()
		{

		}

		public ModelPortals(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int portalCount = inData.Length / Portal.GetSize();
					for (uint i = 0; i < portalCount; ++i)
					{
						Portals.Add(new Portal(br.ReadBytes(Portal.GetSize())));
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
		            foreach (Portal portal in this.Portals)
		            {
			            bw.Write(portal.Serialize());
		            }
            	}

            	return ms.ToArray();
            }
		}
	}

	public class Portal : IBinarySerializable
	{
		public ushort BaseVertexIndex;
		public ushort VertexCount;
		public Plane PortalPlane;

		public Portal(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.BaseVertexIndex = br.ReadUInt16();
					this.VertexCount = br.ReadUInt16();
					this.PortalPlane = br.ReadPlane();
				}
			}
		}

		public static int GetSize()
		{
			return 20;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
            		bw.Write(this.BaseVertexIndex);
		            bw.Write(this.VertexCount);
		            bw.WritePlane(this.PortalPlane);
            	}

            	return ms.ToArray();
            }
		}
	}
}

