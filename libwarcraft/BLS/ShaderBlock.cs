//
//  ShaderBlock.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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
using Warcraft.Core.Interfaces;

namespace Warcraft.BLS
{
	public class ShaderBlock : IBinarySerializable
	{
		public ShaderFlags1 Flags1;
		public ShaderFlags2 Flags2;
		public uint Unknown;

		public uint DataSize;
		public char[] Data;

		public ShaderBlock(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Flags1 = (ShaderFlags1) br.ReadUInt32();
					this.Flags2 = (ShaderFlags2) br.ReadUInt32();
					this.Unknown = br.ReadUInt32();

					this.DataSize = br.ReadUInt32();

					// The data is postloaded into the shader block structure outside
					// of the constructor.
				}
			}
		}


		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					bw.Write((uint)this.Flags1);
					bw.Write((uint)this.Flags2);
					bw.Write(this.Unknown);

					bw.Write((uint)this.Data.Length);
					bw.Write(this.Data);
				}

				return ms.ToArray();
			}
		}

		public static int GetSize()
		{
			return 16;
		}
	}

	public enum ShaderFlags1 : uint
	{

	}

	public enum ShaderFlags2 : uint
	{

	}
}