//
//  DoodadInstance.cs
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
using System.IO;
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Represents a doodad instance.
    /// </summary>
    public class DoodadInstance : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the name of the doodad instance.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        // The nameoffset and flags are actually stored as an uint24 and an uint8,
        // that is, three bytes for the offset and one byte for the flags. It's weird.

        /// <summary>
        /// Gets or sets the name offset.
        /// </summary>
        public uint NameOffset { get; set; }

        /// <summary>
        /// Gets or sets the instance flags.
        /// </summary>
        public DoodadInstanceFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the position of the doodad.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the orientation of the doodad.
        /// </summary>
        public Quaternion Orientation { get; set; }

        /// <summary>
        /// Gets or sets the scale of the doodad.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Gets or sets the static lighting colour of the doodad.
        /// </summary>
        public BGRA StaticLightingColour { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoodadInstance"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public DoodadInstance(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    var finalNameBytes = new byte[4];
                    var nameOffsetBytes = br.ReadBytes(3);
                    Buffer.BlockCopy(nameOffsetBytes, 0, finalNameBytes, 0, 3);

                    NameOffset = BitConverter.ToUInt32(finalNameBytes, 0);

                    Flags = (DoodadInstanceFlags)br.ReadByte();

                    Position = br.ReadVector3();

                    // TODO: Investigate whether or not this is a Quat16 in >= BC
                    Orientation = br.ReadQuaternion32();

                    Scale = br.ReadSingle();
                    StaticLightingColour = br.ReadBGRA();
                }
            }
        }

        /// <summary>
        /// Gets the serialized size of the instance.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 40;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    var nameOffsetBytes = BitConverter.GetBytes(NameOffset);
                    var finalNameOffsetBytes = new byte[3];
                    Buffer.BlockCopy(nameOffsetBytes, 0, finalNameOffsetBytes, 0, 3);

                    bw.Write(finalNameOffsetBytes);
                    bw.Write((byte)Flags);

                    bw.WriteVector3(Position);

                    // TODO: Investigate whether or not this is a Quat16 in >= BC
                    bw.WriteQuaternion32(Orientation);
                    bw.Write(Scale);
                    bw.WriteBGRA(StaticLightingColour);
                }

                return ms.ToArray();
            }
        }
    }
}
