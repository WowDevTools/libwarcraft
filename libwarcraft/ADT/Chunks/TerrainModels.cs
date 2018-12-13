//
//  TerrainModels.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// MMDX Chunk - Contains a list of all referenced M2 models in this ADT.
    /// </summary>
    public class TerrainModels : IIFFChunk
    {
        public const string Signature = "MMDX";

        /// <summary>
        ///A list of full paths to the M2 models referenced in this ADT.
        /// </summary>
        public List<string> Filenames = new List<string>();

        public TerrainModels()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.TerrainModels"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public TerrainModels(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    this.Filenames.Add(br.ReadNullTerminatedString());
                }
            }
        }

        public string GetSignature()
        {
            return Signature;
        }
    }
}

