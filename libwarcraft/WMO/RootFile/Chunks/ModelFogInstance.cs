using System.IO;
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
    public class FogInstance : IBinarySerializable
    {
        public FogFlags Flags;
        public Vector3 Position;

        public float GlobalStartRadius;
        public float GlobalEndRadius;

        public FogDefinition LandFog;
        public FogDefinition UnderwaterFog;

        public FogInstance(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Flags = (FogFlags) br.ReadUInt32();
                    Position = br.ReadVector3();

                    GlobalStartRadius = br.ReadSingle();
                    GlobalEndRadius = br.ReadSingle();

                    LandFog = new FogDefinition(br.ReadBytes(FogDefinition.GetSize()));
                    UnderwaterFog = new FogDefinition(br.ReadBytes(FogDefinition.GetSize()));
                }
            }
        }

        public static int GetSize()
        {
            return 48;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
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
