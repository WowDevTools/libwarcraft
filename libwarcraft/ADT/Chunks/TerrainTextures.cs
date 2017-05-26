//
//  TerrainTextures.cs
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

namespace Warcraft.ADT.Chunks
{
	/// <summary>
	/// MTEX Chunk - Contains a list of all referenced textures in this ADT.
	/// </summary>
	public class TerrainTextures : IIFFChunk
	{
		public const string Signature = "MTEX";

		/// <summary>
		///A list of full paths to the textures referenced in this ADT.
		/// </summary>
		public List<string> Filenames = new List<string>();

		public TerrainTextures()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainTextures"/> class.
		/// </summary>
		/// <param name="inData">ExtendedData.</param>
		public TerrainTextures(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					while (br.BaseStream.Position != br.BaseStream.Length)
					{
						this.Filenames.Add(br.ReadNullTerminatedString());
					}
				}
			}
        }

        public string GetSignature()
        {
        	return Signature;
        }
	}
}

