using System;
using System.IO;

namespace Warcraft.MPQ
{
	public class MPQShunt
	{
		private string Filetype;
		private uint ShuntedArchiveAllocatedSize;
		private uint ShuntedArchiveOffset;

		public MPQShunt(byte[] data)
		{
			MemoryStream dataStream = new MemoryStream(data);
			BinaryReader br = new BinaryReader(dataStream);

			this.Filetype = br.ReadChars(4).ToString();
			this.ShuntedArchiveAllocatedSize = br.ReadUInt32();
			this.ShuntedArchiveOffset = br.ReadUInt32();

			br.Close();
		}
	}
}

