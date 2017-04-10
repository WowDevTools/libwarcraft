using System.IO;
using Warcraft.Core;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;

namespace Warcraft.WMO.RootFile.Chunks
{
	public class FogInstance : IBinarySerializable
	{
		public FogFlags Flags;
		public Vector3f Position;

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
					this.Flags = (FogFlags) br.ReadUInt32();
					this.Position = br.ReadVector3f();

					this.GlobalStartRadius = br.ReadSingle();
					this.GlobalEndRadius = br.ReadSingle();

					this.LandFog = new FogDefinition(br.ReadBytes(FogDefinition.GetSize()));
					this.UnderwaterFog = new FogDefinition(br.ReadBytes(FogDefinition.GetSize()));
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
					bw.Write((uint)this.Flags);
					bw.WriteVector3f(this.Position);

					bw.Write(this.GlobalStartRadius);
					bw.Write(this.GlobalEndRadius);

					bw.Write(this.LandFog.Serialize());
					bw.Write(this.UnderwaterFog.Serialize());
				}

				return ms.ToArray();
			}
		}
	}
}