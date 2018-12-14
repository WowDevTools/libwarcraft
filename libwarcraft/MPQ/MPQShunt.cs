//
//  MPQShunt.cs
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

namespace Warcraft.MPQ
{
    /// <summary>
    /// This class represents a binary structure which can be inserted into any file, and acts as a pointer
    /// to an embedded <see cref="MPQ"/> archive.
    /// </summary>
    public class MPQShunt : IBinarySerializable
    {
        /// <summary>
        /// The data signature of the shunt.
        /// </summary>
        public const string Signature = "MPQ\x1B";

        /// <summary>
        /// The allocated size of the embedded archive.
        /// </summary>
        private uint ShuntedArchiveAllocatedSize;

        /// <summary>
        /// The offset where the embedded archive begins.
        /// </summary>
        private uint ShuntedArchiveOffset;

        /// <summary>
        /// Deserializes an <see cref="MPQShunt"/> object from provided binary data.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <exception cref="InvalidDataException">
        /// Thrown if the input data is null, or if it does not contain a valid signature.
        /// </exception>
        public MPQShunt(byte[] data)
        {
            if (data == null)
            {
                throw new InvalidDataException("The data cannot be null.");
            }

            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    string dataSignature = br.ReadChars(4).ToString();
                    if (dataSignature != Signature)
                    {
                        throw new InvalidDataException("The data did not contain a valid shunt signature.");
                    }

                    ShuntedArchiveAllocatedSize = br.ReadUInt32();
                    ShuntedArchiveOffset = br.ReadUInt32();
                }
            }
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
                    bw.WriteChunkSignature(Signature);
                    bw.Write(ShuntedArchiveAllocatedSize);
                    bw.Write(ShuntedArchiveOffset);
                }

                return ms.ToArray();
            }
        }
    }
}

