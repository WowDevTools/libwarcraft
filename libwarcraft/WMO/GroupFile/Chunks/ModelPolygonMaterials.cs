//
//  ModelPolygonMaterials.cs
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
    public class ModelPolygonMaterials : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOPY";

        public List<PolygonMaterial> PolygonMaterials = new List<PolygonMaterial>();

        public ModelPolygonMaterials()
        {
        }

        public ModelPolygonMaterials(byte[] inData)
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
                        PolygonMaterials.Add(new PolygonMaterial(br.ReadBytes(PolygonMaterial.GetSize())));
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
                    foreach (PolygonMaterial polygonMaterial in PolygonMaterials)
                    {
                        bw.Write(polygonMaterial.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }

    public class PolygonMaterial : IBinarySerializable
    {
        public PolygonMaterialFlags Flags;
        public byte MaterialIndex;

        public PolygonMaterial(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Flags = (PolygonMaterialFlags)br.ReadByte();
                    MaterialIndex = br.ReadByte();
                }
            }
        }

        public static int GetSize()
        {
            return 2;
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)Flags);
                    bw.Write(MaterialIndex);
                }

                return ms.ToArray();
            }
        }
    }

    public enum PolygonMaterialFlags : byte
    {
        Unknown1             = 0x01,
        NoCameraCollide     = 0x02,
        Detail                = 0x04,
        HasCollision        = 0x08,
        Hint                = 0x10,
        Render                = 0x20,
        Unknown2            = 0x40,
        CollideHit            = 0x80
    }
}

