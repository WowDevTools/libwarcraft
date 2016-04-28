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

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainVersion"/> class.
		/// </summary>
		/// <param name="data">Data.</param>
		public TerrainVersion(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Version = br.ReadUInt32();
				}
			}
		}
	}
}

