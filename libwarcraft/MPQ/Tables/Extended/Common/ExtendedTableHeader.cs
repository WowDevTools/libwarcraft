//
//  ExtendedTableHeader.cs
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

using System;
using System.IO;
using System.Text;
using Warcraft.Core.Interfaces;

namespace Warcraft.MPQ.Tables.Extended.Common
{
	/// <summary>
	/// The extended table header class represents a common header for the extended table types, used in the
	/// <see cref="MPQFormat.ExtendedV2"/> and <see cref="MPQFormat.ExtendedV3"/> formats. It contains a simple
	/// binary signature, a table format version, and a total size of the following table.
	/// </summary>
	public class ExtendedTableHeader : IBinarySerializable
	{
		/// <summary>
		/// The data signature of the extended table.
		/// </summary>
		public string Signature;

		/// <summary>
		/// The version of the extended table.
		/// </summary>
		public uint Version;

		/// <summary>
		/// The size of the following table data.
		/// </summary>
		public uint DataSize;

		/// <summary>
		/// Creates a new <see cref="ExtendedTableHeader"/> object from provided binary data.
		/// </summary>
		/// <param name="data">The data to deserialize the object from.</param>
		public ExtendedTableHeader(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Signature = BitConverter.ToString(br.ReadBytes(4));
					this.Version = br.ReadUInt32();
					this.DataSize = br.ReadUInt32();
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
					bw.Write(BitConverter.ToUInt32(Encoding.ASCII.GetBytes(this.Signature), 0));
					bw.Write(this.Version);
					bw.Write(this.DataSize);
				}
			}
		}
	}
}