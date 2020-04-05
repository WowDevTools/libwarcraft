//
//  ModelTextures.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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
    /// Holds the textures used in the model.
    /// </summary>
    public class ModelTextures : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOTX";

        /// <summary>
        /// Gets a dictionary of the texture offets mapped to the actual names.
        /// </summary>
        public Dictionary<long, string> Textures { get; } = new Dictionary<long, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelTextures"/> class.
        /// </summary>
        public ModelTextures()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelTextures"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelTextures(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            while (ms.Position < ms.Length)
            {
                if (ms.Position % 4 == 0)
                {
                    Textures.Add(ms.Position, br.ReadNullTerminatedString());
                }
                else
                {
                    ms.Position += 4 - (ms.Position % 4);
                }
            }
        }

        /// <summary>
        /// Gets a texture path by its offset.
        /// </summary>
        /// <param name="nameOffset">The offset.</param>
        /// <returns>The texture.</returns>
        public string GetTexturePathByOffset(uint nameOffset)
        {
            foreach (var textureName in Textures)
            {
                if (textureName.Key == nameOffset)
                {
                    return textureName.Value;
                }
            }

            return string.Empty;
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                foreach (var texture in Textures)
                {
                    if (ms.Position % 4 == 0)
                    {
                        bw.WriteNullTerminatedString(texture.Value);
                    }
                    else
                    {
                        // Pad with nulls, then write
                        var stringPadCount = 4 - (ms.Position % 4);
                        for (var i = 0; i < stringPadCount; ++i)
                        {
                            bw.Write('\0');
                        }

                        bw.WriteNullTerminatedString(texture.Value);
                    }
                }

                // Finally, pad until the next alignment
                var padCount = 4 - (ms.Position % 4);
                for (var i = 0; i < padCount; ++i)
                {
                    bw.Write('\0');
                }
            }

            return ms.ToArray();
        }
    }
}
