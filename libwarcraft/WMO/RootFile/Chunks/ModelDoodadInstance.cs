using System;
using System.IO;
using Warcraft.Core;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{

	/// <summary>
	/// A set of flags which can affect the way a doodad instance is rendered.
	/// </summary>
	[Flags]
	public enum DoodadInstanceFlags : byte
	{
		AcceptProjectedTexture 		= 0x1,
		Unknown1					= 0x2,
		Unknown2					= 0x4,
		Unknown3					= 0x8
	}

	public class DoodadInstance : IBinarySerializable
	{
		public string Name
		{
			get;
			set;
		}

		// The nameoffset and flags are actually stored as an uint24 and an uint8,
		// that is, three bytes for the offset and one byte for the flags. It's weird.
		public uint NameOffset;
		public DoodadInstanceFlags Flags;


		public Vector3f Position;
		public Quaternion Orientation;
		public float Scale;
		public BGRA StaticLightingColour;

		public DoodadInstance(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					byte[] finalNameBytes = new byte[4];
					byte[] nameOffsetBytes = br.ReadBytes(3);
					Buffer.BlockCopy(nameOffsetBytes, 0, finalNameBytes, 0, 3);

					this.NameOffset= BitConverter.ToUInt32(finalNameBytes, 0);

					this.Flags = (DoodadInstanceFlags) br.ReadByte();

					this.Position = br.ReadVector3f();

					// TODO: Investigate whether or not this is a Quat16 in >= BC
					this.Orientation = br.ReadQuaternion32();

					this.Scale = br.ReadSingle();
					this.StaticLightingColour = br.ReadBGRA();
				}
			}
		}

		public static int GetSize()
		{
			return 40;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					byte[] nameOffsetBytes = BitConverter.GetBytes(this.NameOffset);
					byte[] finalNameOffsetBytes = new byte[3];
					Buffer.BlockCopy(nameOffsetBytes, 0, finalNameOffsetBytes, 0, 3);

					bw.Write(finalNameOffsetBytes);
					bw.Write((byte)this.Flags);

					bw.WriteVector3f(this.Position);

					// TODO: Investigate whether or not this is a Quat16 in >= BC
					bw.WriteQuaternion32(this.Orientation);
					bw.Write(this.Scale);
					bw.WriteBGRA(this.StaticLightingColour);
				}

				return ms.ToArray();
			}
		}
	}
}