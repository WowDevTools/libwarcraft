//
//  GroupInformation.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.WMO.GroupFile;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Defines information about a model group.
    /// </summary>
    public class GroupInformation : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the group's flags.
        /// </summary>
        public GroupFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the bounding box of the group.
        /// </summary>
        public Box BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the offset to the group's name.
        /// </summary>
        public int GroupNameOffset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupInformation"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
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

        /// <summary>
        /// Determines whether or not the group has a name.
        /// </summary>
        /// <returns>true if the group has a name; otherwise, false.</returns>
        public bool HasGroupName()
        {
            return GroupNameOffset > -1;
        }

        /// <summary>
        /// Gets the serialized size of the instance.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 32;
        }

        /// <inheritdoc/>
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
