//
//  ModelPortals.cs
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

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Holds the portals.
    /// </summary>
    public class ModelPortals : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOPT";

        /// <summary>
        /// Gets the portals.
        /// </summary>
        public List<Portal> Portals { get; } = new List<Portal>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPortals"/> class.
        /// </summary>
        public ModelPortals()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPortals"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelPortals(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int portalCount = inData.Length / Portal.GetSize();
                    for (uint i = 0; i < portalCount; ++i)
                    {
                        Portals.Add(new Portal(br.ReadBytes(Portal.GetSize())));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    foreach (Portal portal in Portals)
                    {
                        bw.Write(portal.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
