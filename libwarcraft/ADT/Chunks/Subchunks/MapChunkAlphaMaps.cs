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
        private byte[] _data;

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
            _data = inData;
        }

        /// <inheritdoc />
        public string GetSignature()
        {
            return Signature;
        }

        private IEnumerable<byte> DecompressAlphaMap(uint mapOffset)
        {
            var decompressedAlphaMap = new List<byte>();

            using (var ms = new MemoryStream(_data))
            {
                using (var br = new BinaryReader(ms))
                {
                    br.BaseStream.Position = mapOffset;

                    while (decompressedAlphaMap.Count > 4096)
                    {
                        var headerByte = br.ReadSByte();
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
                            var fillByte = br.ReadByte();

                            for (var i = 0; i < compressionCount; ++i)
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
            var decompressedAlphaMap = new List<byte>();
            for (var y = 0; y < 64; y++)
            {
                for (var x = 0; x < 32; x++)
                {
                    if (mapFlags.HasFlag(MapChunkFlags.DoNotRepairAlphaMaps))
                    {
                        // Fill in normally
                        var alpha1 = (byte)(compressedAlphaMap[x + (y * 32)] & 0xf0);
                        var alpha2 = (byte)((compressedAlphaMap[x + (y * 32)] << 4) & 0xf0);

                        var normalizedAlpha1 = (byte)(alpha1 * 17);
                        var normalizedAlpha2 = (byte)(alpha2 * 17);

                        decompressedAlphaMap.Add(normalizedAlpha1);
                        decompressedAlphaMap.Add(normalizedAlpha2);
                    }
                    else
                    {
                        // Bottom row
                        if (y == 63)
                        {
                            var yminus = y - 1;

                            // Attempt to repair map on vertical axis
                            var alpha1 = (byte)(compressedAlphaMap[x + (yminus * 32)] & 0xf0);
                            var alpha2 = (byte)((compressedAlphaMap[x + 1 + (yminus * 32)] << 4) & 0xf0);

                            var normalizedAlpha1 = (byte)(alpha1 * 17);
                            var normalizedAlpha2 = (byte)(alpha2 * 17);

                            decompressedAlphaMap.Add(normalizedAlpha1);
                            decompressedAlphaMap.Add(normalizedAlpha2);
                        }
                        else if (x == 31)
                        {
                            var xminus = x - 1;

                            // Attempt to repair map on horizontal axis
                            var alpha = (byte)(compressedAlphaMap[xminus + (y * 32)] << 4 & 0xf0);
                            var normalizedAlpha = (byte)(alpha * 17);

                            decompressedAlphaMap.Add(normalizedAlpha);
                        }
                        else
                        {
                            // Fill in normally
                            var alpha1 = (byte)(compressedAlphaMap[x + (y * 32)] & 0xf0);
                            var alpha2 = (byte)((compressedAlphaMap[x + (y * 32)] << 4) & 0xf0);

                            var normalizedAlpha1 = (byte)(alpha1 * 17);
                            var normalizedAlpha2 = (byte)(alpha2 * 17);

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
