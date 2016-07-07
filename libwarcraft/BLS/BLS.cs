//
//  BLS.cs
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

using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.BLS
{
	public class BLS : IBinarySerializable
	{
		public BLSHeader Header;
		public List<ShaderBlock> Shaders = new List<ShaderBlock>();

		public BLS(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
            {
            	using (BinaryReader br = new BinaryReader(ms))
            	{
					this.Header = new BLSHeader(br.ReadBytes(BLSHeader.GetSize()));

		            for (int i = 0; i < this.Header.ShaderBlockCount; ++i)
		            {
			            ShaderBlock shaderBlock = new ShaderBlock(br.ReadBytes(ShaderBlock.GetSize()));
			            shaderBlock.Data = br.ReadChars((int)shaderBlock.DataSize);

			            this.Shaders.Add(shaderBlock);

			            if ((ms.Position % 4) != 0)
			            {
				            long padCount = 4 - (ms.Position % 4);
				            ms.Position += padCount;
			            }
		            }
            	}
            }
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
					bw.Write(this.Header.Serialize());
		            foreach (ShaderBlock shaderBlock in this.Shaders)
		            {
			            bw.Write(shaderBlock.Serialize());
		            }
            	}

            	return ms.ToArray();
            }
		}
	}
}