//
//  WorldTableHeader.cs
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
using Warcraft.Core.Interfaces;

namespace Warcraft.WDT.Chunks
{
	public class WorldTableHeader : IIFFChunk, IBinarySerializable
	{
		public const string Signature = "MPHD";

		public WorldTableFlags Flags;
		public uint Unknown;

		// Six unused fields

		public WorldTableHeader()
		{

		}

		public WorldTableHeader(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Flags = (WorldTableFlags)br.ReadUInt32();
					this.Unknown = br.ReadUInt32();
				}
			}
        }

        public string GetSignature()
        {
        	return Signature;
        }

		/// <summary>
		/// Gets the size of the data contained in this chunk.
		/// </summary>
		/// <returns>The size.</returns>
		public static uint GetSize()
		{
			return 32;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					bw.Write((uint)this.Flags);
					bw.Write(this.Unknown);

					// Write the six unused fields
					for (int i = 0; i < 6; ++i)
					{
						bw.Write((uint)0);
					}
				}

				return ms.ToArray();
			}
		}
	}

	[Flags]
	public enum WorldTableFlags : uint
	{
		UsesGlobalModels 				= 0x01,
		UsesVertexShading 				= 0x02,
		UsesEnvironmentMapping 			= 0x04,
		DisableUnknownRenderingFlag 	= 0x08,
		UsesVertexLighting 				= 0x10,
		FlipGroundNormals 				= 0x20,
		Unknown 						= 0x40,
		UsesHardAlphaFalloff 			= 0x80,
		UnknownHardAlphaRelated 		= 0x100,
		UnknownContinentRelated 		= 0x8000
	}
}

