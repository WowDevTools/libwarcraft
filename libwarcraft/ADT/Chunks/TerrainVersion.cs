//
//  TerrainVersion.cs
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
using System.IO;
using Warcraft.Core;

namespace Warcraft.ADT.Chunks
{
	/// <summary>
	/// MVER Chunk - Contains the ADT version
	/// </summary>
	public class TerrainVersion : IChunk
	{
		public const string Signature = "MVER";

		/// <summary>
		/// ADT version from MVER
		/// </summary>
		public uint Version;

		public TerrainVersion()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainVersion"/> class.
		/// </summary>
		/// <param name="inData">Data.</param>
		public TerrainVersion(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Version = br.ReadUInt32();
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
			return 4;
		}


		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					bw.WriteChunkSignature(TerrainVersion.Signature);

					// The size of this chunk is alwas 4; A single field.
					bw.Write((uint)4);

					// Write the version
					bw.Write(this.Version);
				}

				return ms.ToArray();
			}
		}
	}
}

