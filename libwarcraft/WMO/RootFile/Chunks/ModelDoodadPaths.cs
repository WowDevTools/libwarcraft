//
//  ModelDoodadNames.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Represents a block of paths of doodads used by a WMO object. Each path is referenced by other chunks
    /// as an offset into this block - most likely a leftover from C/C++ style code. Paths can be retrieved from
    /// this class either by indexing the path in <see cref="DoodadNames"/>, or by using the string offset and calling
    /// <see cref="GetNameByOffset"/>.
    /// </summary>
    public class ModelDoodadPaths : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// The RIFF signature for this block.
        /// </summary>
        public const string Signature = "MODN";

        /// <summary>
        /// The paths of all the doodads referenced by this WMO. Each is stored as an offset into the string block,
        /// and the actual string stored there.
        /// </summary>
        public readonly List<KeyValuePair<long, string>> DoodadNames = new List<KeyValuePair<long, string>>();

        /// <summary>
        /// Creates a new empty doodad path block object.
        /// </summary>
        public ModelDoodadPaths()
        {

        }

        /// <summary>
        /// Creates a new doodad path block object by deserializing it from binary data.
        /// </summary>
        /// <param name="inData">The binary data containing the doodad paths.</param>
        public ModelDoodadPaths(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <summary>
        /// Deserialzes the provided binary data of the object. This is the full data block which follows the data
        /// signature and data block length.
        /// </summary>
        /// <param name="inData">The binary data containing the object.</param>
        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    while (ms.Position < ms.Length)
                    {
                        this.DoodadNames.Add(new KeyValuePair<long, string>(ms.Position, br.ReadNullTerminatedString()));
                    }
                }
            }

            // Remove null entries from the doodad list
            this.DoodadNames.RemoveAll(s => s.Value.Equals("\0"));
        }

        /// <summary>
        /// Gets the static data signature of this data block type.
        /// </summary>
        /// <returns>A string representing the block signature.</returns>
        public string GetSignature()
        {
            return Signature;
        }

        /// <summary>
        /// Gets a doodad path by its original offset into the block of paths.
        /// </summary>
        /// <param name="nameOffset">The original byte offset of the name.</param>
        /// <returns>The doodad path.</returns>
        public string GetNameByOffset(uint nameOffset)
        {
            foreach (KeyValuePair<long, string> doodadName in this.DoodadNames)
            {
                if (doodadName.Key == nameOffset)
                {
                    return doodadName.Value;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Serializes the current object into a byte array.
        /// </summary>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    foreach (KeyValuePair<long, string> doodadName in this.DoodadNames)
                    {
                        bw.WriteNullTerminatedString(doodadName.Value);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}

