//
//  DBC.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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
using System.Linq;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC
{
    /// <summary>
    /// DBC file handler. Parses and presents DBC files in a statically typed, easy to use fashion.
    /// </summary>
    /// <typeparam name="TRecord">The record type.</typeparam>
    public class DBC<TRecord> : IDBC, IReadOnlyList<TRecord> where TRecord : DBCRecord, new()
    {
        /// <summary>
        /// The header of the database file. Describes the sizes and field counts of the records in the database.
        /// </summary>
        private readonly DBCHeader Header;

        /// <summary>
        /// Gets the number of held records.
        /// </summary>
        public int RecordCount => (int)Header.RecordCount;

        /// <summary>
        /// Gets the number of fields in each record.
        /// </summary>
        public int FieldCount => (int)Header.FieldCount;

        /// <summary>
        /// Gets the absolute size of each record.
        /// </summary>
        public int RecordSize => (int)Header.RecordSize;

        /// <summary>
        /// Gets the absolute size of the string block.
        /// </summary>
        public int StringBlockSize => (int)Header.StringBlockSize;

        /// <summary>
        /// Gets the game version this database is valid for. This affects how the records are parsed, and is vital for
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
        private readonly byte[] DatabaseContents;

        private readonly long StringBlockOffset;

        /// <summary>
        /// Gets the strings in the DBC file.
        /// </summary>
        public Dictionary<long, string> Strings { get; } = new Dictionary<long, string>();

        /// <summary>
        /// The records in the database. Used as a procedural cache.
        /// </summary>
        private readonly List<TRecord> Records;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBC{TRecord}"/> class.
        /// </summary>
        /// <param name="inVersion">In version.</param>
        /// <param name="data">ExtendedData.</param>
        public DBC(WarcraftVersion inVersion, byte[] data)
        {
            Version = inVersion;
            DatabaseContents = data;

            using (BinaryReader databaseReader = new BinaryReader(new MemoryStream(DatabaseContents)))
            {
                Header = new DBCHeader(databaseReader.ReadBytes(DBCHeader.GetSize()));

                // Seek to and read the string block
                databaseReader.BaseStream.Seek(Header.RecordCount * Header.RecordSize, SeekOrigin.Current);
                StringBlockOffset = databaseReader.BaseStream.Position;
                while (databaseReader.BaseStream.Position != databaseReader.BaseStream.Length)
                {
                    Strings.Add(databaseReader.BaseStream.Position - StringBlockOffset, databaseReader.ReadNullTerminatedString());
                }
            }

            Records = new List<TRecord>(Count);

            // Initialize the record list with null values
            for (int i = 0; i < Count; ++i)
            {
                Records.Add(null);
            }
        }

        /// <summary>
        /// Gets a record from the database by its primary key ID.
        /// </summary>
        /// <returns>The record.</returns>
        /// <param name="id">Primary key ID.</param>
        public TRecord GetRecordByID(int id)
        {
            return this.FirstOrDefault(record => record.ID == id);
        }

        /// <summary>
        /// Resolves a string reference. String references are stored as offsets into the string data block
        /// of the database. This function looks up the matching string and stores it in the reference object.
        /// </summary>
        /// <param name="reference">Reference.</param>
        public void ResolveStringReference(StringReference reference)
        {
            if (Strings.ContainsKey(reference.Offset))
            {
                reference.Value = Strings[reference.Offset];
                return;
            }

            reference.Value = string.Empty;
        }

        /// <summary>
        /// Determines whether or not the record at the given index has been cached.
        /// </summary>
        /// <param name="index">The index of the record.</param>
        /// <returns>true if the record has been cached; otherwise, false.</returns>
        internal bool HasCachedRecordAtIndex(int index)
        {
            return Records[index] != null;
        }

        /// <summary>
        /// Caches the given record at the given index.
        /// </summary>
        /// <param name="record">The record to cache.</param>
        /// <param name="index">The index where to cache the record.</param>
        /// <exception cref="InvalidOperationException">Thrown if there is already a record cached at the given index.</exception>
        internal void CacheRecordAtIndex(TRecord record, int index)
        {
            if (HasCachedRecordAtIndex(index))
            {
                throw new InvalidOperationException("A record was already cached at the given index.");
            }

            Records[index] = record;
        }

        /*
            Enumeration implementation
        */

        /// <inheritdoc />
        public int Count => RecordCount;

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<TRecord> GetEnumerator()
        {
            return new DBCEnumerator<TRecord>(this, DatabaseContents, StringBlockOffset);
        }

        /*
            Indexing implementation
        */

        /// <inheritdoc />
        public TRecord this[int i]
        {
            get
            {
                if (HasCachedRecordAtIndex(i))
                {
                    return Records[i];
                }

                using (BinaryReader databaseReader = new BinaryReader(new MemoryStream(DatabaseContents)))
                {
                    long recordOffset = DBCHeader.GetSize() + (Header.RecordSize * i);
                    databaseReader.BaseStream.Seek(recordOffset, SeekOrigin.Begin);

                    TRecord record = databaseReader.ReadRecord<TRecord>((int)Header.FieldCount, (int)Header.RecordSize, Version);

                    foreach (var stringReference in record.GetStringReferences())
                    {
                        ResolveStringReference(stringReference);
                    }

                    Records[i] = record;

                    return record;
                }
            }
        }
    }
}
