//
//  ModelStaticLighting.cs
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
using Warcraft.Core.Structures;

namespace Warcraft.WMO.RootFile.Chunks
{
    public class ModelStaticLighting : IIFFChunk, IBinarySerializable
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MOLT";

        public readonly List<StaticLight> StaticLights = new List<StaticLight>();

        public ModelStaticLighting()
        {

        }

        public ModelStaticLighting(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int lightCount = inData.Length / StaticLight.GetSize();
                    for (uint i = 0; i < lightCount; ++i)
                    {
                        StaticLights.Add(new StaticLight(br.ReadBytes(StaticLight.GetSize())));
                    }
                }
            }
        }

        public string GetSignature()
        {
            return Signature;
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    foreach (StaticLight staticLight in StaticLights)
                    {
                        bw.Write(staticLight.Serialize());
                    }
                }

                return ms.ToArray();
            }
        }
    }

    public class StaticLight : IBinarySerializable
    {
        public LightType Type;

        public bool UseAttenuation;
        public bool UseUnknown1;
        public bool UseUnknown2;

        public BGRA Colour;
        public Vector3 Position;
        public float Intensity;

        public float AttenuationStartRadius;
        public float AttenuationEndRadius;

        public float Unknown1StartRadius;
        public float Unknown1EndRadius;

        public float Unknown2StartRadius;
        public float Unknown2EndRadius;

        public StaticLight(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Type = (LightType) br.ReadByte();
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
            }
        }

        public static int GetSize()
        {
            return 48;
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
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

    public enum LightType : byte
    {
        Omnidirectional = 0,
        Spot             = 1,
        Directional     = 2,
        Ambient         = 3
    }
}

