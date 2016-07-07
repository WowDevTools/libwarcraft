//
//  BLSHeader.cs
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
using Warcraft.Core;
using Warcraft.Core.Interfaces;

namespace Warcraft.BLS
{
	public class BLSHeader : IBinarySerializable
	{
		public const string VertexShaderSignature = "GXVS";
		public const string FragmentShaderSignature = "GXPS";

		public ShaderContainerFormat ContainerFormat;

		public uint Version;
		public uint ShaderBlockCount;

		public BLSHeader(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
            {
            	using (BinaryReader br = new BinaryReader(ms))
	            {
		            string Signature = br.ReadChunkSignature();
		            if (Signature != VertexShaderSignature && Signature != FragmentShaderSignature)
		            {
			            throw new FileLoadException("BLS data must begin with a valid shader signature.");
		            }

		            if (Signature == VertexShaderSignature)
		            {
			            this.ContainerFormat = ShaderContainerFormat.Vertex;
		            }
		            else
		            {
			            this.ContainerFormat = ShaderContainerFormat.Fragment;
		            }

		            this.Version = br.ReadUInt32();
		            this.ShaderBlockCount = br.ReadUInt32();
	            }
            }
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
		            if (this.ContainerFormat == ShaderContainerFormat.Vertex)
		            {
			            bw.WriteChunkSignature(VertexShaderSignature);
		            }
		            else
		            {
			            bw.WriteChunkSignature(FragmentShaderSignature);
		            }

		            bw.Write(this.Version);
		            bw.Write(this.ShaderBlockCount);
            	}

            	return ms.ToArray();
            }
		}

		public static int GetSize()
		{
			return 12;
		}
	}

	public enum ShaderContainerFormat : byte
	{
		Vertex,
		Fragment
	}
}