//
//  ModelBSPNodes.cs
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

namespace Warcraft.WMO.GroupFile.Chunks
{
    public class ModelBSPNodes : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOBN";

        public readonly List<BSPNode> BSPNodes = new List<BSPNode>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBSPNodes"/> class.
        /// </summary>
        public ModelBSPNodes()
        {
        }

        public ModelBSPNodes(byte[] inData)
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
                        BSPNodes.Add(new BSPNode(br.ReadBytes(BSPNode.GetSize())));
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
                    foreach (BSPNode bspNode in BSPNodes)
                    {
                        bw.Write(bspNode.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }

    public class BSPNode : IBinarySerializable
    {
        public PlaneType Type;
        public short FirstChildIndex;
        public short SecondChildIndex;
        public ushort FaceCount;
        public uint FirstFaceIndex;
        public float DistanceFromCenter;

        public BSPNode(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Type = (PlaneType) br.ReadUInt16();
                    FirstChildIndex = br.ReadInt16();
                    SecondChildIndex = br.ReadInt16();
                    FaceCount = br.ReadUInt16();
                    FirstFaceIndex = br.ReadUInt32();
                    DistanceFromCenter = br.ReadSingle();
                }
            }
        }

        public static int GetSize()
        {
            return 16;
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((ushort)Type);
                    bw.Write(FirstChildIndex);
                    bw.Write(SecondChildIndex);
                    bw.Write(FaceCount);
                    bw.Write(FirstFaceIndex);
                    bw.Write(DistanceFromCenter);
                }

                return ms.ToArray();
            }
        }
    }

    public enum PlaneType : ushort
    {
        YZ         = 0,
        XZ         = 1,
        XY         = 2,
        Leaf     = 4
    }
}

