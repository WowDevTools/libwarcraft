//
//  StaticLight.cs
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
using Warcraft.Core.Structures;

namespace Warcraft.WMO.RootFile.Chunks
{
    /// <summary>
    /// Represents a static light.
    /// </summary>
    public class StaticLight : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the type of the light.
        /// </summary>
        public LightType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attenuation should be used.
        /// </summary>
        public bool UseAttenuation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether some unknown action should be taken.
        /// </summary>
        public bool UseUnknown1 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether some unknown action should be taken.
        /// </summary>
        public bool UseUnknown2 { get; set; }

        /// <summary>
        /// Gets or sets the colour of the light.
        /// </summary>
        public BGRA Colour { get; set; }

        /// <summary>
        /// Gets or sets the position of the light.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the intensity of the light.
        /// </summary>
        public float Intensity { get; set; }

        /// <summary>
        /// Gets or sets the attenuation start radius.
        /// </summary>
        public float AttenuationStartRadius { get; set; }

        /// <summary>
        /// Gets or sets the attenuation end radius.
        /// </summary>
        public float AttenuationEndRadius { get; set; }

        /// <summary>
        /// Gets or sets an unknown start radius.
        /// </summary>
        public float Unknown1StartRadius { get; set; }

        /// <summary>
        /// Gets or sets an unknown end radius.
        /// </summary>
        public float Unknown1EndRadius { get; set; }

        /// <summary>
        /// Gets or sets an unknown start radius.
        /// </summary>
        public float Unknown2StartRadius { get; set; }

        /// <summary>
        /// Gets or sets an unknown end radius.
        /// </summary>
        public float Unknown2EndRadius { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticLight"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public StaticLight(byte[] inData)
        {
            using var ms = new MemoryStream(inData);
            using var br = new BinaryReader(ms);
            Type = (LightType)br.ReadByte();
            UseAttenuation = br.ReadBoolean();
            UseUnknown1 = br.ReadBoolean();
            UseUnknown2 = br.ReadBoolean();

            Colour = br.ReadBGRA();
            Position = br.ReadVector3();
            Intensity = br.ReadSingle();

            AttenuationStartRadius = br.ReadSingle();
            AttenuationEndRadius = br.ReadSingle();

            Unknown1StartRadius = br.ReadSingle();
            Unknown1EndRadius = br.ReadSingle();

            Unknown2StartRadius = br.ReadSingle();
            Unknown2EndRadius = br.ReadSingle();
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
            using var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write((byte)Type);

                bw.Write(UseAttenuation);
                bw.Write(UseUnknown1);
                bw.Write(UseUnknown2);

                bw.WriteBGRA(Colour);
                bw.WriteVector3(Position);
                bw.Write(Intensity);

                bw.Write(AttenuationStartRadius);
                bw.Write(AttenuationEndRadius);

                bw.Write(Unknown1StartRadius);
                bw.Write(Unknown1EndRadius);

                bw.Write(Unknown2StartRadius);
                bw.Write(Unknown2EndRadius);
            }

            return ms.ToArray();
        }
    }
}
