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

using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
	// TODO: Rework to support offset-based seeking and adding of strings
	public class ModelTextures : IIFFChunk, IBinarySerializable
	{
		public const string Signature = "MOTX";

		public readonly Dictionary<long, string> Textures = new Dictionary<long, string>();

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
							this.Textures.Add(ms.Position, br.ReadNullTerminatedString());
						}
						else
						{
							ms.Position += (4 - (ms.Position % 4));
						}
					}
				}
			}
		}

		public string GetTexturePathByOffset(uint nameOffset)
		{
			foreach (KeyValuePair<long, string> textureName in this.Textures)
			{
				if (textureName.Key == nameOffset)
				{
					return textureName.Value;
				}
			}

			return string.Empty;
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
					foreach (KeyValuePair<long, string> texture in this.Textures)
					{
						if (ms.Position % 4 == 0)
						{
							bw.WriteNullTerminatedString(texture.Value);
						}
						else
						{
							// Pad with nulls, then write
							long stringPadCount = 4 - (ms.Position % 4);
							for (int i = 0; i < stringPadCount; ++i)
							{
								bw.Write('\0');
							}

							bw.WriteNullTerminatedString(texture.Value);
						}
					}

					// Finally, pad until the next alignment
					long padCount = 4 - (ms.Position % 4);
					for (int i = 0; i < padCount; ++i)
					{
						bw.Write('\0');
					}
				}

				return ms.ToArray();
			}
		}
	}
}

