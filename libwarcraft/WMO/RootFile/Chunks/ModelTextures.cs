//
//  ModelTextures.cs
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
	// TODO: Rework to support offset-based seeking and adding of strings
	public class ModelTextures : IChunk
	{
		public const string Signature = "MOTX";

		public readonly List<KeyValuePair<long, string>> MaterialTextures = new List<KeyValuePair<long, string>>();

		public ModelTextures()
		{

		}

		public ModelTextures(byte[] inData)
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
						if (ms.Position % 4 == 0)
						{
							MaterialTextures.Add(new KeyValuePair<long, string>(ms.Position, br.ReadNullTerminatedString()));
						}
						else
						{
							ms.Position += (4 - (ms.Position % 4));
						}
					}
				}
			}

			// Remove null entries from the texture list
			MaterialTextures.RemoveAll(s => s.Value.Equals("\0"));
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
					for (int i = 0; i < MaterialTextures.Count; ++i)
					{
						if (ms.Position % 4 == 0)
						{
							bw.WriteNullTerminatedString(MaterialTextures[i].Value);
						}
						else
						{
							// Pad with nulls
							bw.Write('\0');
						}
					}
				}

				return ms.ToArray();
			}
		}
	}
}

