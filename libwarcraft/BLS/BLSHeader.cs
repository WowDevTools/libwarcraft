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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.BLS
{
	/// <summary>
	/// Header class for a BLS file. This header provides information about the shaders contained in the
	/// BLS file.
	/// </summary>
	public class BLSHeader : IBinarySerializable
	{
		/// <summary>
		/// Binary data signature for a vertex shader container.
		/// </summary>
		public const string VertexShaderSignature = "GXVS";

		/// <summary>
		/// Binary data signature for a fragment shader container.
		/// </summary>
		public const string FragmentShaderSignature = "GXPS";

		/// <summary>
		/// The type of the container, that is, vertex or fragment.
		/// </summary>
		public ShaderContainerType ContainerType;

		/// <summary>
		/// The binary format version of the BLS file.
		/// </summary>
		public uint Version;

		/// <summary>
		/// The number of shader blocks stored in the file.
		/// </summary>
		public uint ShaderBlockCount;

		/// <summary>
		/// Creates a new <see cref="BLSHeader"/> object from supplied binary data, containing a serialized object.
		/// </summary>
		/// <param name="inData">The binary data containing the object.</param>
		/// <exception cref="FileLoadException">Thrown if no matching signatures could be found in the data.</exception>
		public BLSHeader(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
            {
            	using (BinaryReader br = new BinaryReader(ms))
	            {
		            string dataSignature = br.ReadBinarySignature();
		            if (dataSignature != VertexShaderSignature && dataSignature != FragmentShaderSignature)
		            {
			            throw new FileLoadException("BLS data must begin with a valid shader signature.");
		            }

		            if (dataSignature == VertexShaderSignature)
		            {
			            this.ContainerType = ShaderContainerType.Vertex;
		            }
		            else
		            {
			            this.ContainerType = ShaderContainerType.Fragment;
		            }

		            this.Version = br.ReadUInt32();
		            this.ShaderBlockCount = br.ReadUInt32();
	            }
            }
		}

		/// <summary>
		/// Serializes the current object into a byte array.
		/// </summary>
		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
		            if (this.ContainerType == ShaderContainerType.Vertex)
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

		/// <summary>
		/// Gets the static binary size of this class, that is, the absolute size in bytes of a serialized object.
		/// </summary>
		/// <returns></returns>
		public static int GetSize()
		{
			return 12;
		}
	}

	/// <summary>
	/// All the different container types there are of BLS files.
	/// </summary>
	public enum ShaderContainerType : byte
	{
		/// <summary>
		/// This BLS file is a vertex shader container.
		/// </summary>
		Vertex,

		/// <summary>
		/// This BLS file is a fragment shader container.
		/// </summary>
		Fragment
	}
}