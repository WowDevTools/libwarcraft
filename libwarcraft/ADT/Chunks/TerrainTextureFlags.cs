//
//  TerrainTextureFlags.cs
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
using System.Collections.Generic;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks
{
    public class TerrainTextureFlags : IIFFChunk
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MTXF";

        private List<TerrainTextureFlag> TextureFlags = new List<TerrainTextureFlag>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainTextureFlags"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public TerrainTextureFlags(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    long entryCount = br.BaseStream.Length / 4;

                    for (int i = 0; i < entryCount; ++i)
                    {
                        TextureFlags.Add((TerrainTextureFlag)br.ReadUInt32());
                    }
                }
            }
        }

        public string GetSignature()
        {
            return Signature;
        }
    }

    public enum TerrainTextureFlag : uint
    {
        FlatShading = 1,
        Unknown = 3,
        ScaledTexture = 4,
        Unknown2 = 24
    }
}

