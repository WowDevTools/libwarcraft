//
//  BSPNode.cs
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

using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.GroupFile.Chunks
{
    /// <summary>
    /// Represents a a BSP node.
    /// </summary>
    public class BSPNode : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the plane type of the node.
        /// </summary>
        public BSPPlaneType Type { get; set; }

        /// <summary>
        /// Gets or sets the index of the node's first child.
        /// </summary>
        public short FirstChildIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the node's second child.
        /// </summary>
        public short SecondChildIndex { get; set; }

        /// <summary>
        /// Gets or sets the face count of the node.
        /// </summary>
        public ushort FaceCount { get; set; }

        /// <summary>
        /// Gets or sets the index of the first face of the node.
        /// </summary>
        public uint FirstFaceIndex { get; set; }

        /// <summary>
        /// Gets or sets the node's distance from the center of the model.
        /// </summary>
        public float DistanceFromCenter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BSPNode"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public BSPNode(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    Type = (BSPPlaneType)br.ReadUInt16();
                    FirstChildIndex = br.ReadInt16();
                    SecondChildIndex = br.ReadInt16();
                    FaceCount = br.ReadUInt16();
                    FirstFaceIndex = br.ReadUInt32();
                    DistanceFromCenter = br.ReadSingle();
                }
            }
        }

        /// <summary>
        /// Gets the serialized size of the instance.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 16;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
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
}
