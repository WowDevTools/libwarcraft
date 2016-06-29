//
//  MainChunk.cs
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

namespace Warcraft.WDT.Chunks
{
	public class AreaInfoChunk : IRIFFChunk
	{
		public const string Signature = "MAIN";

		public List<AreaInfoEntry> Entries = new List<AreaInfoEntry>();

		public AreaInfoChunk()
		{

		}

		public AreaInfoChunk(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (uint y = 0; y < 64; ++y)
					{
						for (uint x = 0; x < 64; ++x)
						{
							Entries.Add(new AreaInfoEntry(br.ReadBytes((int)AreaInfoEntry.GetSize()), x, y));
						}
					}
				}
			}
        }

        public string GetSignature()
        {
        	return Signature;
        }

		public AreaInfoEntry GetAreaInfo(uint inTileX, uint inTileY)
		{
			if (inTileX > 63)
			{
				throw new ArgumentOutOfRangeException(nameof(inTileX), "The tile coordinate may not be more than 63 in either dimension.");
			}

			if (inTileY > 63)
			{
				throw new ArgumentOutOfRangeException(nameof(inTileY), "The tile coordinate may not be more than 63 in either dimension.");
			}

			int tileIndex = (int)((inTileY * 64) + inTileX);
			return this.Entries[tileIndex];
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					bw.WriteChunkSignature(AreaInfoChunk.Signature);
					uint chunkSize = (uint)(Entries.Count * AreaInfoEntry.GetSize());
					bw.Write(chunkSize);

					foreach (AreaInfoEntry Entry in Entries)
					{
						bw.Write(Entry.Serialize());
					}

					bw.Flush();
				}

				return ms.ToArray();
			}
		}
	}

	public class AreaInfoEntry
	{
		public AreaInfoFlags Flags;
		public uint AreaID;

		/*
			The following fields are not serialized, and are provided as
			helper fields for programmers.
		*/

		public readonly uint TileX;
		public readonly uint TileY;

		public AreaInfoEntry(byte[] data, uint inTileX, uint inTileY)
		{
			this.TileX = inTileX;
			this.TileY = inTileY;
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Flags = (AreaInfoFlags)br.ReadUInt32();
					this.AreaID = br.ReadUInt32();
				}
			}
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream(8))
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					bw.Write((uint)this.Flags);
					bw.Write(this.AreaID);

					bw.Flush();
				}

				return ms.ToArray();
			}
		}

		public static uint GetSize()
		{
			return 8;
		}
	}

	public enum AreaInfoFlags : uint
	{
		HasTerrainData = 1,
		IsLoaded = 2,
	}
}

