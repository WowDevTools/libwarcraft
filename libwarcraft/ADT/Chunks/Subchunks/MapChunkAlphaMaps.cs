//
//  MapChunkAlphaMaps.cs
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
using System.IO;

namespace Warcraft.ADT.Chunks.Subchunks
{
	/// <summary>
	/// MCAL Chunk - Contains alpha map data in one of three forms - uncompressed 2048, uncompressed 4096 and compressed.
	/// </summary>
	public class MapChunkAlphaMaps
	{
		//chunk (and data) size
		public int size;

		//unformatted data contained in MCAL
		public byte[] data;

		public MapChunkAlphaMaps(string adtFile, int position)
		{
			Stream adtStream = File.OpenRead(adtFile);
			BinaryReader br = new BinaryReader(adtStream);
			br.BaseStream.Position = position;

			this.size = br.ReadInt32();

			this.data = br.ReadBytes(this.size);
		}
		//4 layers of alpha maps. This is the magic right here.
		//each layer is a 32x64 array of alpha values
		//can be formatted according to one of three types:

		/* 
	     * Uncompressed with a size of 4096 (post WOTLK)
	     * Uncompressed with a size of 2048 (pre WOTLK)
	     * Compressed - this is only for WOTLK chunks. Size is not very important when dealing with compressed alpha maps,
	     * considering you are constantly checking if you've extracted 4096 bytes of data. Here's how you do it, according to the wiki:
	     * 
	     * Read a byte.
	     * Check for a sign bit.
	     * If it's set, we're in fill mode. If not, we're in copy mode.
	     * 
	     * 1000000 = set sign bit, fill mode
	     * 0000000 = unset sign bit, copy mode
	     * 
	     * 0            1 0 1 0 1 0 1
	     * sign bit,    7 lesser bits
	     * 
	     * Take the 7 lesser bits of the first byte as a count indicator,
	     * If we're in fill mode, read the next byte and fill it by count in your resulting alpha map
	     * If we're in copy mode, read the next count bytes and copy them to your resulting alpha map
	     * If the alpha map is complete (4096 bytes), we're finished. If not, go back and start at 1 again.
	     */
	}
}

