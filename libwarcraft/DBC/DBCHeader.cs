//
//  DBCHeader.cs
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

namespace Warcraft.DBC
{
	/// <summary>
	/// The header of a DBC file.
	/// </summary>
	public class DBCHeader
	{
		/// <summary>
		/// The data signature of a DBC file.
		/// </summary>
		public const string Signature = "WDBC";

		/// <summary>
		/// The number of records in the database.
		/// </summary>
		public uint RecordCount;

		/// <summary>
		/// The field count in the database.
		/// </summary>
		public uint FieldCount;

		/// <summary>
		/// The size of a single record in the database.
		/// </summary>
		public uint RecordSize;

		/// <summary>
		/// The size of the string block in the database. This block is always stored at the end of the database.
		/// </summary>
		public uint StringBlockSize;

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.DBC.DBCHeader"/> class.
		/// </summary>
		/// <param name="data">Data.</param>
		public DBCHeader(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					string DataSignature = new string(br.ReadChars(4));
					if (Signature != DataSignature)
					{
						throw new FileLoadException("The loaded data did not have a valid DBC signature.");
					}

					this.RecordCount = br.ReadUInt32();
					this.FieldCount = br.ReadUInt32();
					this.RecordSize = br.ReadUInt32();
					this.StringBlockSize = br.ReadUInt32();
				}
			}
		}

		/// <summary>
		/// Gets the size of a DBC header in bytes.
		/// </summary>
		/// <returns>The size.</returns>
		public static int GetSize()
		{
			return 20;
		}
	}
}

