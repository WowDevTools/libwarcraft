//
//  ModelDoodadNames.cs
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
	public class ModelDoodadNames : IRIFFChunk, IBinarySerializable
	{
		public const string Signature = "MODN";

		public readonly List<KeyValuePair<long, string>> DoodadNames = new List<KeyValuePair<long, string>>();

		public ModelDoodadNames()
		{

		}

		public ModelDoodadNames(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					while (ms.Position < ms.Length)
					{
						this.DoodadNames.Add(new KeyValuePair<long, string>(ms.Position, br.ReadNullTerminatedString()));
					}
				}
			}

			// Remove null entries from the doodad list
			this.DoodadNames.RemoveAll(s => s.Value.Equals("\0"));
        }

        public string GetSignature()
        {
        	return Signature;
        }

		public string GetNameByOffset(uint nameOffset)
		{
			foreach (KeyValuePair<long, string> doodadName in this.DoodadNames)
			{
				if (doodadName.Key == nameOffset)
				{
					return doodadName.Value;
				}
			}

			return string.Empty;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
		            foreach (KeyValuePair<long, string> doodadName in this.DoodadNames)
		            {
			            bw.WriteNullTerminatedString(doodadName.Value);
		            }
            	}

            	return ms.ToArray();
            }
		}
	}
}

