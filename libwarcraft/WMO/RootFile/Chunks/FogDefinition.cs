//
//  FogDefinition.cs
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
using Warcraft.Core.Structures;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Represents a fog definition.
    /// </summary>
    public class FogDefinition : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the end radius of the fog.
        /// </summary>
        public float EndRadius { get; set; }

        /// <summary>
        /// Gets or sets the start multiplier of the fog.
        /// </summary>
        public float StartMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the colour of the fog.
        /// </summary>
        public BGRA Colour { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FogDefinition"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public FogDefinition(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    EndRadius = br.ReadSingle();
                    StartMultiplier = br.ReadSingle();
                    Colour = br.ReadBGRA();
                }
            }
        }

        /// <summary>
        /// Gets the serialized size of the instance.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 12;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(EndRadius);
                    bw.Write(StartMultiplier);
                    bw.WriteBGRA(Colour);
                }

                return ms.ToArray();
            }
        }
    }
}
