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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC
{
	/// <summary>
	/// DBC file handler. Parses and presents DBC files in a statically typed, easy to use fashion.
	/// </summary>
	public class DBC<T> : IDBC, IReadOnlyCollection<T>, IEnumerator<T> where T : DBCRecord, new()
	{
		/// <summary>
		/// The header of the database file. Describes the sizes and field counts of the records in the database.
		/// </summary>
		public readonly DBCHeader Header;

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
		/// The <see cref="BinaryReader"/> which holds the data of the database.
		/// </summary>
		private readonly BinaryReader DatabaseReader;

		/// <summary>
		/// The strings in the DBC file.
		/// </summary>
		public readonly Dictionary<long, string> Strings = new Dictionary<long, string>();

		/// <summary>
		/// Initializes a new instance of the <see cref="DBC"/> class.
		/// </summary>
		/// <param name="inVersion">In version.</param>
		/// <param name="data">ExtendedData.</param>
		public DBC(WarcraftVersion inVersion, byte[] data)
		{
			this.Version = inVersion;

			this.DatabaseReader = new BinaryReader(new MemoryStream(data));
			this.Header = new DBCHeader(this.DatabaseReader.ReadBytes(DBCHeader.GetSize()));

			// Seek to and read the string block
			this.DatabaseReader.BaseStream.Seek(this.Header.RecordCount * this.Header.RecordSize, SeekOrigin.Current);
			long stringBlockBaseOffset = this.DatabaseReader.BaseStream.Position;
			while (this.DatabaseReader.BaseStream.Position != this.DatabaseReader.BaseStream.Length)
			{
				this.Strings.Add(this.DatabaseReader.BaseStream.Position - stringBlockBaseOffset, this.DatabaseReader.ReadNullTerminatedString());
			}
		}

		/// <summary>
		/// Gets a record from the database by its primary key ID.
		/// </summary>
		/// <returns>The record</returns>
		/// <param name="id">Primary key ID.</param>
		public T GetRecordByID(int id)
		{
			long recordOffset = DBCHeader.GetSize() + this.Header.RecordSize * id;
			this.DatabaseReader.BaseStream.Seek(recordOffset, SeekOrigin.Begin);

			T record = this.DatabaseReader.ReadRecord<T>((int)this.Header.FieldCount, (int)this.Header.RecordSize, this.Version);

			foreach (var stringReference in record.GetStringReferences())
			{
				ResolveStringReference(stringReference);
			}
			
			return record;
		}

		/// <summary>
		/// Resolves a string reference. String references are stored as offsets into the string data block
		/// of the database. This function looks up the matching string and stores it in the reference object.
		/// </summary>
		/// <returns>The string reference.</returns>
		/// <param name="reference">Reference.</param>
		public void ResolveStringReference(StringReference reference)
		{
			if (this.Strings.ContainsKey(reference.Offset))
			{
				reference.Value = this.Strings[reference.Offset];
				return;
			}

			reference.Value = "";
		}

		/*
			Enumeration implementation
		*/

		public IEnumerator<T> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count => (int)this.Header.RecordCount;
		public bool MoveNext()
		{
			if (this.DatabaseReader.BaseStream.Position >= this.DatabaseReader.BaseStream.Length)
			{
				return false;
			}

			this.Current = this.DatabaseReader.ReadRecord<T>((int)this.Header.FieldCount, (int)this.Header.RecordSize, this.Version);
			return this.DatabaseReader.BaseStream.Position != this.DatabaseReader.BaseStream.Length;
		}

		public void Reset()
		{
			this.DatabaseReader.BaseStream.Position = DBCHeader.GetSize();
			this.Current = null;
		}

		public T Current { get; private set; }

		object IEnumerator.Current => this.Current;

		public void Dispose()
		{
			this.DatabaseReader.Dispose();
		}
		
		/*
			Indexing implementation
		*/
		public T this[int i] => GetRecordByID(i);
	}
}

