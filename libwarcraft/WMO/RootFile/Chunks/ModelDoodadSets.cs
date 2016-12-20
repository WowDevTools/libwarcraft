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

using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
	public class ModelDoodadSets : IRIFFChunk, IBinarySerializable
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

		/// <summary>
		/// Deserialzes the provided binary data of the object. This is the full data block which follows the data
		/// signature and data block length.
		/// </summary>
		/// <param name="inData">The binary data containing the object.</param>
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

		/// <summary>
		/// Gets the static data signature of this data block type.
		/// </summary>
		/// <returns>A string representing the block signature.</returns>
		public string GetSignature()
        {
        	return Signature;
        }

		/// <summary>
		/// Serializes the current object into a byte array.
		/// </summary>
		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
		            foreach (DoodadSet doodadSet in this.DoodadSets)
		            {
			            bw.Write(doodadSet.Serialize());
		            }
            	}

            	return ms.ToArray();
            }
		}
	}
}

