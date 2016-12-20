//
//  DBC.cs
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
using System.Collections.Generic;
using System.IO;
using Warcraft.Core;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC
{
	/// <summary>
	/// DBC file handler. Parses and presents DBC files in a statically typed, easy to use fashion.
	/// </summary>
	public class DBC<T> where T : DBCRecord
	{
		/// <summary>
		/// The header of the database file. Describes the sizes and field counts of the records in the database.
		/// </summary>
		public DBCHeader Header;

		/// <summary>
		/// The game version this database is valid for. This affects how the records are parsed, and is vital for
		/// getting correct data.
		/// </summary>
		public WarcraftVersion Version
		{
			get;
			private set;
		}

		/// <summary>
		/// The records contained in the database. The records are not guaranteed to be in order by ID.
		/// In order to access records by their primary key, use <see cref="GetRecordByID"/>.
		/// </summary>
		public List<T> Records = new List<T>();

		/// <summary>
		/// The strings in the DBC file.
		/// </summary>
		public List<string> Strings = new List<string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.DBC.DBC"/> class.
		/// </summary>
		/// <param name="InVersion">In version.</param>
		/// <param name="data">Data.</param>
		public DBC(WarcraftVersion InVersion, byte[] data)
		{
			this.Version = InVersion;

			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Header = new DBCHeader(br.ReadBytes(DBCHeader.GetSize()));

					for (int i = 0; i < this.Header.RecordCount; ++i)
					{
						byte[] rawRecord = br.ReadBytes((int)this.Header.RecordSize);

						T record = Activator.CreateInstance<T>();
						record.SetVersion(InVersion);

						// If the record is of the UnknownRecord type,
						// this DBC file will just load the data without sanity checking it.
						if (!(record is UnknownRecord))
						{
							// Make sure the provided record type is valid for this database file
							if (record.GetRecordSize() != this.Header.RecordSize)
							{
								throw new ArgumentException("The provided record type is not valid for this database file.");
							}
							if (record.GetFieldCount() != this.Header.FieldCount)
							{
								throw new ArgumentException("The provided record type is not valid for this database file.");
							}
						}

						record.LoadData(rawRecord);

						this.Records.Add(record);
					}

					while (br.BaseStream.Position != br.BaseStream.Length)
					{
						this.Strings.Add(br.ReadNullTerminatedString());
					}
				}
			}
		}

		/// <summary>
		/// Gets a record from the database by its primary key ID.
		/// </summary>
		/// <returns>The record</returns>
		/// <param name="ID">Primary key ID.</param>
		public T GetRecordByID(uint ID)
		{
			foreach (T Record in this.Records)
			{
				if (Record.ID == ID)
				{
					return Record;
				}
			}

			return null;
		}

		/// <summary>
		/// Resolves a string reference. String references are stored as offsets into the string data block
		/// of the database. This function goes through the strings until the right offset is found, and then
		/// returns the string.
		/// </summary>
		/// <returns>The string reference.</returns>
		/// <param name="reference">Reference.</param>
		public string ResolveStringReference(StringReference reference)
		{
			int blockOffset = 0;
			foreach (string blockString in this.Strings)
			{
				if (blockOffset == reference.StringOffset)
				{
					return blockString;
				}

				blockOffset += blockString.Length + 1;
			}

			return "";
		}
	}
}

