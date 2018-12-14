//
//  ModelRenderBatches.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;

namespace Warcraft.WMO.GroupFile.Chunks
{
    public class ModelRenderBatches : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOBA";

        public readonly List<RenderBatch> RenderBatches = new List<RenderBatch>();

        public ModelRenderBatches()
        {
        }

        public ModelRenderBatches(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    while (ms.Position < ms.Length)
                    {
                        RenderBatches.Add(new RenderBatch(br.ReadBytes(RenderBatch.GetSize())));
                    }
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
                    foreach (RenderBatch renderBatch in RenderBatches)
                    {
                        bw.Write(renderBatch.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }

    public class RenderBatch : IBinarySerializable
    {
        public ShortBox BoundingBox;

        public uint FirstPolygonIndex;
        public ushort PolygonIndexCount;

        public ushort FirstVertexIndex;
        public ushort LastVertexIndex;

        public byte UnknownFlags;
        public byte MaterialIndex;

        public RenderBatch(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    BoundingBox = br.ReadShortBox();
                    FirstPolygonIndex = br.ReadUInt32();
                    PolygonIndexCount = br.ReadUInt16();
                    FirstVertexIndex = br.ReadUInt16();
                    LastVertexIndex = br.ReadUInt16();

                    UnknownFlags = br.ReadByte();
                    MaterialIndex = br.ReadByte();
                }
            }
        }

        public static int GetSize()
        {
            return 24;
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.WriteShortBox(BoundingBox);

                    bw.Write(FirstPolygonIndex);
                    bw.Write(PolygonIndexCount);

                    bw.Write(FirstVertexIndex);
                    bw.Write(LastVertexIndex);

                    bw.Write(UnknownFlags);
                    bw.Write(MaterialIndex);
                }

                return ms.ToArray();
            }
        }
    }
}

