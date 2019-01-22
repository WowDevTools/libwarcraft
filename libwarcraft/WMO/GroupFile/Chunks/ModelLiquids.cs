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
    /// <summary>
    /// Holds liquids in a model.
    /// </summary>
    public class ModelLiquids : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MLIQ";

        /// <summary>
        /// Gets or sets the number of vertexes in the X direction.
        /// </summary>
        public uint XVertexCount { get; set; }

        /// <summary>
        /// Gets or sets the number of vertexes in the Y direction.
        /// </summary>
        public uint YVertexCount { get; set; }

        /// <summary>
        /// Gets or sets the number of X tiles.
        /// </summary>
        public uint WidthTileFlags { get; set; }

        /// <summary>
        /// Gets or sets the number of Y tiles.
        /// </summary>
        public uint HeightTileFlags { get; set; }

        /// <summary>
        /// Gets or sets the base location of the liquid.
        /// </summary>
        public Vector3 Location { get; set; }

        /// <summary>
        /// Gets or sets the liquid's material index.
        /// </summary>
        public ushort MaterialIndex { get; set; }

        /// <summary>
        /// Gets the liquid vertexes.
        /// </summary>
        public List<LiquidVertex> LiquidVertices { get; } = new List<LiquidVertex>();

        /// <summary>
        /// Gets the liquid tiles.
        /// </summary>
        public List<LiquidFlags> LiquidTileFlags { get; } = new List<LiquidFlags>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelLiquids"/> class.
        /// </summary>
        public ModelLiquids()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelLiquids"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelLiquids(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    XVertexCount = br.ReadUInt32();
                    YVertexCount = br.ReadUInt32();

                    WidthTileFlags = br.ReadUInt32();
                    HeightTileFlags = br.ReadUInt32();

                    Location = br.ReadVector3();
                    MaterialIndex = br.ReadUInt16();

                    var vertexCount = XVertexCount * YVertexCount;
                    for (var i = 0; i < vertexCount; ++i)
                    {
                        LiquidVertices.Add(new LiquidVertex(br.ReadBytes(LiquidVertex.GetSize())));
                    }

                    var tileFlagCount = WidthTileFlags * HeightTileFlags;
                    for (var i = 0; i < tileFlagCount; ++i)
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

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(XVertexCount);
                    bw.Write(YVertexCount);

                    bw.Write(WidthTileFlags);
                    bw.Write(HeightTileFlags);

                    bw.WriteVector3(Location);
                    bw.Write(MaterialIndex);

                    foreach (var liquidVertex in LiquidVertices)
                    {
                        bw.Write(liquidVertex.Serialize());
                    }

                    foreach (var liquidFlag in LiquidTileFlags)
                    {
                        bw.Write((byte)liquidFlag);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
