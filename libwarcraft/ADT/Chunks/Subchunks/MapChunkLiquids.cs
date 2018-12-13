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
using System;
using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
    public class MapChunkLiquids : IIFFChunk
    {
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

    public class LiquidVertex : IBinarySerializable
    {
        public Tuple<ushort, ushort> TextureCoordinates;
        public float Height;

        public LiquidVertex(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    TextureCoordinates = new Tuple<ushort, ushort>(br.ReadUInt16(), br.ReadUInt16());
                    Height = br.ReadSingle();
                }
            }
        }

        public static int GetSize()
        {
            return 8;
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(TextureCoordinates.Item1);
                    bw.Write(TextureCoordinates.Item2);

                    bw.Write(Height);
                }

                return ms.ToArray();
            }
        }
    }

    [Flags]
    public enum LiquidFlags : byte
    {
        Hidden         = 0x08,
        // Unknown1 = 0x10,
        // Unknown2 = 0x20,
        Fishable     = 0x40,
        Shared         = 0x80
    }
}

