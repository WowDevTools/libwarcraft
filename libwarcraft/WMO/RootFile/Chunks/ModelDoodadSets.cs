//
//  ModelDoodadSets.cs
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
using System.Text;
using Warcraft.ADT.Chunks;

namespace Warcraft.WMO.RootFile.Chunks
{
	public class ModelDoodadSets : IChunk
	{
		public const string Signature = "MODS";

		public readonly List<DoodadSet> DoodadSets = new List<DoodadSet>();

		public ModelDoodadSets()
		{

		}

		public ModelDoodadSets(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int setCount = inData.Length / DoodadSet.GetSize();
					for (uint i = 0; i < setCount; ++i)
					{
						this.DoodadSets.Add(new DoodadSet(br.ReadBytes(DoodadSet.GetSize())));
					}
				}
			}
        }

        public string GetSignature()
        {
        	return Signature;
        }
	}

	public class DoodadSet
	{
		public string Name;
		public uint FirstDoodadInstanceIndex;
		public uint DoodadInstanceCount;
		public uint Unknown;

		public DoodadSet(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					// The name of the doodad set can be up to 20 bytes, and is always padded to this length.
					// Therefore, we read 20 bytes, convert it to a string and trim the end of any null characters.
					byte[] nameBytes = br.ReadBytes(20);
					this.Name = Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');

					this.FirstDoodadInstanceIndex = br.ReadUInt32();
					this.DoodadInstanceCount = br.ReadUInt32();
					this.Unknown = br.ReadUInt32();
				}
			}
		}

		public static int GetSize()
		{
			return 32;
		}
	}
}

