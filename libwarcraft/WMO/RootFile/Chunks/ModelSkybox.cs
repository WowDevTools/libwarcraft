//
//  ModelSkybox.cs
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

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Holds the model's skybox.
    /// </summary>
    public class ModelSkybox : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOSB";

        /// <summary>
        /// Gets or sets the skybox name.
        /// </summary>
        public string SkyboxName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelSkybox"/> class.
        /// </summary>
        public ModelSkybox()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelSkybox"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelSkybox(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    SkyboxName = br.ReadNullTerminatedString();
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
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    if (string.IsNullOrEmpty(SkyboxName))
                    {
                        bw.Write(new byte[4]);
                    }
                    else
                    {
                        bw.WriteNullTerminatedString(SkyboxName);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}
