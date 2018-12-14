//
//  ModelConvexPlanes.cs
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
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Contains a set of planes defining the actual volume which should be considered "inside" a model. If a point is
    /// behind all planes (that is, the point-plane distance is less than zero for all planes), the point is considered
    /// inside the model.
    ///
    /// This is used, for example, in transport models where it is important to know if the player should stick to the
    /// model or not.
    /// </summary>
    public class ModelConvexPlanes : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// The static block signature of the convex plane block.
        /// </summary>
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MCVP";

        /// <summary>
        /// The convex planes contained in this object. These planes are used to define regions in the model
        /// object.
        /// </summary>
        public readonly List<Plane> ConvexPlanes = new List<Plane>();

        /// <summary>
        /// Creates a new empty convex plane block object.
        /// </summary>
        public ModelConvexPlanes()
        {
        }

        /// <summary>
        /// Creates a new convex plane block object from binary data.
        /// </summary>
        /// <param name="inData"></param>
        public ModelConvexPlanes(byte[] inData)
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
                    int planeCount = inData.Length / 16;
                    for (int i = 0; i < planeCount; ++i)
                    {
                        ConvexPlanes.Add(br.ReadPlane());
                    }
                }
            }
        }

        /// <summary>
        /// Gets the static data signature of this data block type.
        /// </summary>
        /// <returns>A string representing the block signature.</returns>
        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
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
                    foreach (Plane convexPlane in ConvexPlanes)
                    {
                        bw.WritePlane(convexPlane);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}

