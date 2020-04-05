//
//  FogInstance.cs
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

using System.IO;
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Defines an instance of fog.
    /// </summary>
    public class FogInstance : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the flags of the instance.
        /// </summary>
        public FogFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the center position of the instance.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the global start radius of the fog.
        /// </summary>
        public float GlobalStartRadius { get; set; }

        /// <summary>
        /// Gets or sets the global end radius of the fog.
        /// </summary>
        public float GlobalEndRadius { get; set; }

        /// <summary>
        /// Gets or sets the fog that's used on land.
        /// </summary>
        public FogDefinition LandFog { get; set; }

        /// <summary>
        /// Gets or sets the fog that's used underwater.
        /// </summary>
        public FogDefinition UnderwaterFog { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FogInstance"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public FogInstance(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
                {
                    Flags = (FogFlags)br.ReadUInt32();
                    Position = br.ReadVector3();

                    GlobalStartRadius = br.ReadSingle();
                    GlobalEndRadius = br.ReadSingle();

                    LandFog = new FogDefinition(br.ReadBytes(FogDefinition.GetSize()));
                    UnderwaterFog = new FogDefinition(br.ReadBytes(FogDefinition.GetSize()));
                }
            }
        }

        /// <summary>
        /// Gets the serialized size of the instance.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 48;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write((uint)Flags);
                    bw.WriteVector3(Position);

                    bw.Write(GlobalStartRadius);
                    bw.Write(GlobalEndRadius);

                    bw.Write(LandFog.Serialize());
                    bw.Write(UnderwaterFog.Serialize());
                }

                return ms.ToArray();
            }
        }
    }
}
