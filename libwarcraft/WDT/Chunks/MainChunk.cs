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

namespace Warcraft.WDT.Chunks
{
	public class MainChunk
	{
		public const string Signature = "MAIN";

		public List<AreaInfoEntry> AreaInfoEntries = new List<AreaInfoEntry>();

		public MainChunk(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					for (int y = 0; y < 64; ++y)
					{
						for (int x = 0; x < 64; ++x)
						{
							AreaInfoEntries.Add(new AreaInfoEntry(br.ReadBytes(AreaInfoEntry.GetSize())));
						}
					}
				}
			}
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream(8))
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					bw.WriteChunkSignature(MainChunk.Signature);
					uint chunkSize = (uint)(AreaInfoEntries.Count * AreaInfoEntry.GetSize());
					bw.Write(chunkSize);

					foreach (AreaInfoEntry Entry in AreaInfoEntries)
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

		public AreaInfoEntry(byte[] data)
		{
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

		public static int GetSize()
		{
			return 8;
		}
	}

	public enum AreaInfoFlags : uint
	{
		Exists = 1,
		Loaded = 2,
	}
}

