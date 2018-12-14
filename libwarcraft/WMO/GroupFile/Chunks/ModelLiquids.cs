//
//  ModelLiquids.cs
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
using System.Numerics;
using Warcraft.ADT.Chunks.Subchunks;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.GroupFile.Chunks
{
    public class ModelLiquids : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MLIQ";

        public uint WidthVertices;
        public uint HeightVertices;
        public uint WidthTileFlags;
        public uint HeightTileFlags;

        public Vector3 Location;
        public ushort MaterialIndex;

        public List<LiquidVertex> LiquidVertices = new List<LiquidVertex>();
        public List<LiquidFlags> LiquidTileFlags = new List<LiquidFlags>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelLiquids"/> class.
        /// </summary>
        public ModelLiquids()
        {
        }

        public ModelLiquids(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    WidthVertices = br.ReadUInt32();
                    HeightVertices = br.ReadUInt32();

                    WidthTileFlags = br.ReadUInt32();
                    HeightTileFlags = br.ReadUInt32();

                    Location = br.ReadVector3();
                    MaterialIndex = br.ReadUInt16();

                    uint vertexCount = WidthVertices * HeightVertices;
                    for (int i = 0; i < vertexCount; ++i)
                    {
                        LiquidVertices.Add(new LiquidVertex(br.ReadBytes(LiquidVertex.GetSize())));
                    }

                    uint tileFlagCount = WidthTileFlags * HeightTileFlags;
                    for (int i = 0; i < tileFlagCount; ++i)
                    {
                        LiquidTileFlags.Add((LiquidFlags)br.ReadByte());
                    }
                }
            }
        }

        /// <inheritdoc/>
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
                    bw.Write(WidthVertices);
                    bw.Write(HeightVertices);

                    bw.Write(WidthTileFlags);
                    bw.Write(HeightTileFlags);

                    bw.WriteVector3(Location);
                    bw.Write(MaterialIndex);

                    foreach (LiquidVertex liquidVertex in LiquidVertices)
                    {
                        bw.Write(liquidVertex.Serialize());
                    }

                    foreach (LiquidFlags liquidFlag in LiquidTileFlags)
                    {
                        bw.Write((byte)liquidFlag);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}

