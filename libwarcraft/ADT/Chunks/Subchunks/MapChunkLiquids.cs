//
//  MapChunkLiquids.cs
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

using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// MCLQ chunk - holds liquid vertex data.
    /// </summary>
    public class MapChunkLiquids : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCLQ";

        public float MinimumLiquidLevel;
        public float MaxiumLiquidLevel;

        public List<LiquidVertex> LiquidVertices = new List<LiquidVertex>();
        public List<LiquidFlags> LiquidTileFlags = new List<LiquidFlags>();

        public MapChunkLiquids(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public MapChunkLiquids()
        {
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    MinimumLiquidLevel = br.ReadSingle();
                    MaxiumLiquidLevel = br.ReadSingle();

                    for (int y = 0; y < 9; ++y)
                    {
                        for (int x = 0; x < 9; ++x)
                        {
                            LiquidVertices.Add(new LiquidVertex(br.ReadBytes(LiquidVertex.GetSize())));
                        }
                    }

                    for (int y = 0; y < 8; ++y)
                    {
                        for (int x = 0; x < 8; ++x)
                        {
                            LiquidTileFlags.Add(((LiquidFlags)br.ReadByte()));
                        }
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

