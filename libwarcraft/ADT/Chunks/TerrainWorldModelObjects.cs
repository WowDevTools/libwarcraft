//
//  TerrainWorldModelObjects.cs
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

namespace Warcraft.ADT.Chunks
{
	/// <summary>
	/// MWMO Chunk - Contains a list of all referenced WMO models in this ADT.
	/// </summary>
	public class TerrainWorldModelObjects : IChunk
	{
		public const string Signature = "MWMO";

		/// <summary>
		///A list of full paths to the M2 models referenced in this ADT.
		/// </summary>
		public List<string> Filenames = new List<string>();

		public TerrainWorldModelObjects()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainWorldModelObjects"/> class.
		/// </summary>
		/// <param name="inData">Data.</param>
		public TerrainWorldModelObjects(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					Filenames.Add(br.ReadNullTerminatedString());
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
					bw.WriteChunkSignature(TerrainWorldModelObjects.Signature);

					int chunkSize = 0;
					foreach (string filename in Filenames)
					{
						chunkSize += filename.Length + 1;
					}
					bw.Write(chunkSize);

					foreach (string filename in Filenames)
					{
						bw.WriteNullTerminatedString(filename);
					}
				}

				return ms.ToArray();
			}
		}
	}
}

