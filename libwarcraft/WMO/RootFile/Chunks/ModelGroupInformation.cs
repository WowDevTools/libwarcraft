//
//  ModelGroupInformation.cs
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
using Warcraft.WMO.GroupFile;

namespace Warcraft.WMO.RootFile.Chunks
{
    public class ModelGroupInformation : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOGI";

        public readonly List<GroupInformation> GroupInformations = new List<GroupInformation>();

        public ModelGroupInformation()
        {

        }

        public ModelGroupInformation(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    long groupInfoCount = ms.Length / GroupInformation.GetSize();
                    for (int i = 0; i < groupInfoCount; ++i)
                    {
                        GroupInformations.Add(new GroupInformation(br.ReadBytes(GroupInformation.GetSize())));
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
                    foreach (GroupInformation groupInformation in GroupInformations)
                    {
                        bw.Write(groupInformation.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }

    public class GroupInformation : IBinarySerializable
    {
        public GroupFlags Flags;
        public Box BoundingBox;
        public int GroupNameOffset;

        public GroupInformation(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Flags = (GroupFlags)br.ReadUInt32();
                    BoundingBox = br.ReadBox();
                    GroupNameOffset = br.ReadInt32();
                }
            }
        }

        public bool HasGroupName()
        {
            return GroupNameOffset > -1;
        }

        public static int GetSize()
        {
            return 32;
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write((uint)Flags);
                    bw.WriteBox(BoundingBox);
                    bw.Write(GroupNameOffset);
                }

                return ms.ToArray();
            }
        }
    }
}

