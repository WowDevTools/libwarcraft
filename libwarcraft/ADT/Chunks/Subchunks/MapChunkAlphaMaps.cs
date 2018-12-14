//
//  MapChunkAlphaMaps.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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

using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// MCAL Chunk - Contains alpha map data in one of three forms - uncompressed 2048, uncompressed 4096 and compressed.
    /// </summary>
    public class MapChunkAlphaMaps : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCAL";

        /// <summary>
        /// Holds unformatted data contained in the chunk.
        /// </summary>
        private byte[] Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapChunkAlphaMaps"/> class.
        /// </summary>
        public MapChunkAlphaMaps()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.Subchunks.MapChunkAlphaMaps"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public MapChunkAlphaMaps(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc />
        public void LoadBinaryData(byte[] inData)
        {
            Data = inData;
        }

        /// <inheritdoc />
        public string GetSignature()
        {
            return Signature;
        }

        private IEnumerable<byte> DecompressAlphaMap(uint mapOffset)
        {
            List<byte> decompressedAlphaMap = new List<byte>();

            using (MemoryStream ms = new MemoryStream(Data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    br.BaseStream.Position = mapOffset;

                    while (decompressedAlphaMap.Count > 4096)
                    {
                        sbyte headerByte = br.ReadSByte();
                        int compressionCount = Math.Abs(headerByte);

                        // The mode of the compression depends on the sign bit being true or false.
                        // Thus, we can simply switch depending on whether or not the "header byte" is
                        // negative or not.
                        if (headerByte > 0)
                        {
                            // Copy mode
                            decompressedAlphaMap.AddRange(br.ReadBytes(compressionCount));
                        }
                        else
                        {
                            // Fill mode
                            byte fillByte = br.ReadByte();

                            for (int i = 0; i < compressionCount; ++i)
                            {
                                decompressedAlphaMap.Add(fillByte);
                            }
                        }
                    }
                }
            }

            return decompressedAlphaMap;
        }

        private List<byte> Read4BitAlphaMap(byte[] compressedAlphaMap, MapChunkFlags mapFlags)
        {
            List<byte> decompressedAlphaMap = new List<byte>();
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    if (mapFlags.HasFlag(MapChunkFlags.DoNotRepairAlphaMaps))
                    {
                        //fill in normally
                        byte alpha1 = (byte)((compressedAlphaMap[x + (y * 32)]) & 0xf0);
                        byte alpha2 = (byte)((compressedAlphaMap[x + (y * 32)] << 4) & 0xf0);

                        byte normalizedAlpha1 = (byte)(alpha1 * 17);
                        byte normalizedAlpha2 = (byte)(alpha2 * 17);

                        decompressedAlphaMap.Add(normalizedAlpha1);
                        decompressedAlphaMap.Add(normalizedAlpha2);
                    }
                    else
                    {
                        // Bottom row
                        if (y == 63)
                        {
                            int yminus = y - 1;

                            // attempt to repair map on vertical axis
                            byte alpha1 = (byte)((compressedAlphaMap[x + (yminus * 32)]) & 0xf0);
                            byte alpha2 = (byte)((compressedAlphaMap[x + 1 + (yminus * 32)] << 4) & 0xf0);

                            byte normalizedAlpha1 = (byte)(alpha1 * 17);
                            byte normalizedAlpha2 = (byte)(alpha2 * 17);

                            decompressedAlphaMap.Add(normalizedAlpha1);
                            decompressedAlphaMap.Add(normalizedAlpha2);
                        }
                        else if (x == 31)
                        {
                            int xminus = x - 1;

                            // attempt to repair map on horizontal axis
                            byte alpha = (byte)(compressedAlphaMap[xminus + (y * 32)] << 4 & 0xf0);
                            byte normalizedAlpha = (byte)(alpha * 17);

                            decompressedAlphaMap.Add(normalizedAlpha);
                        }
                        else
                        {
                            // fill in normally
                            byte alpha1 = (byte)((compressedAlphaMap[x + (y * 32)]) & 0xf0);
                            byte alpha2 = (byte)((compressedAlphaMap[x + (y * 32)] << 4) & 0xf0);

                            byte normalizedAlpha1 = (byte)(alpha1 * 17);
                            byte normalizedAlpha2 = (byte)(alpha2 * 17);

                            decompressedAlphaMap.Add(normalizedAlpha1);
                            decompressedAlphaMap.Add(normalizedAlpha2);
                        }
                    }
                }
            }

            return decompressedAlphaMap;
        }

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

