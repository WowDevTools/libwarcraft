//
//  PortalReference.cs
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

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Represents a reference to a culling portal.
    /// </summary>
    public class PortalReference : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the portal index.
        /// </summary>
        public ushort PortalIndex { get; set; }

        /// <summary>
        /// Gets or sets the group index.
        /// </summary>
        public ushort GroupIndex { get; set; }

        /// <summary>
        /// Gets or sets the portal's side.
        /// </summary>
        public short Side { get; set; }

        /// <summary>
        /// Gets or sets an unknown value.
        /// </summary>
        public ushort Unknown { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortalReference"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public PortalReference(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    PortalIndex = br.ReadUInt16();
                    GroupIndex = br.ReadUInt16();
                    Side = br.ReadInt16();
                    Unknown = br.ReadUInt16();
                }
            }
        }

        /// <summary>
        /// Gets the serialized size of the instance.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 8;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(PortalIndex);
                    bw.Write(GroupIndex);
                    bw.Write(Side);
                    bw.Write(Unknown);
                }

                return ms.ToArray();
            }
        }
    }
}
