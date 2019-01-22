//
//  DoodadSet.cs
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
using System.Text;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Represents a doodad set of a WMO model. This is a first-index counted list of all instances contained in the set.
    /// </summary>
    public class DoodadSet : IBinarySerializable
    {
        private string _internalName;

        /// <summary>
        /// Gets or sets the name of the doodad set. This name is stored in the block as binary data, and may not be
        /// longer than 20 characters.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the assigned name is longer than 20 characters.</exception>
        public string Name
        {
            get => _internalName;

            set
            {
                if (value.Length > 20)
                {
                    throw new ArgumentException("Doodad set names may not be longer than 20 characters.", nameof(Name));
                }

                _internalName = value;
            }
        }

        /// <summary>
        /// Gets or sets the index of the first doodad instance in this set. This index points to a doodad in a <see cref="ModelDoodadInstances"/>
        /// object.
        /// </summary>
        public uint FirstDoodadInstanceIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of instances after the first which are a part of this set.
        /// </summary>
        public uint DoodadInstanceCount { get; set; }

        /// <summary>
        /// Gets or sets an unknown or unused field.
        /// </summary>
        public uint Unused { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DoodadSet"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public DoodadSet(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    // The name of the doodad set can be up to 20 bytes, and is always padded to this length.
                    // Therefore, we read 20 bytes, convert it to a string and trim the end of any null characters.
                    var nameBytes = br.ReadBytes(20);
                    Name = Encoding.UTF8.GetString(nameBytes).TrimEnd('\0');

                    FirstDoodadInstanceIndex = br.ReadUInt32();
                    DoodadInstanceCount = br.ReadUInt32();
                    Unused = br.ReadUInt32();
                }
            }
        }

        /// <summary>
        /// The static binary size of this class when serialized.
        /// </summary>
        /// <returns>The serialized size in bytes.</returns>
        public static int GetSize()
        {
            return 32;
        }

        /// <summary>
        /// Serializes the current object into a byte array.
        /// </summary>
        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Name.PadRight(20, '\0').ToCharArray());
                    bw.Write(FirstDoodadInstanceIndex);
                    bw.Write(DoodadInstanceCount);
                    bw.Write(Unused);
                }

                return ms.ToArray();
            }
        }
    }
}
