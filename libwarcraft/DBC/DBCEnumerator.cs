//
//  DBCEnumerator.cs
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

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Extensions;
using Warcraft.DBC.Definitions;

namespace Warcraft.DBC
{
    /// <summary>
    /// Enumerator object of a DBC object.
    /// </summary>
    /// <typeparam name="TRecord">The record type.</typeparam>
    public class DBCEnumerator<TRecord> : IEnumerator<TRecord> where TRecord : DBCRecord, new()
    {
        private readonly DBC<TRecord> ParentDatabase;
        private readonly BinaryReader DatabaseReader;
        private readonly long StringBlockOffset;

        private int RecordIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBCEnumerator{TRecord}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="data">The data to load.</param>
        /// <param name="stringBlockOffset">The offset of the string block.</param>
        public DBCEnumerator(DBC<TRecord> database, byte[] data, long stringBlockOffset)
        {
            ParentDatabase = database;
            StringBlockOffset = stringBlockOffset;
            DatabaseReader = new BinaryReader(new MemoryStream(data));
            RecordIndex = 0;

            // Seek to the start of the record block
            DatabaseReader.BaseStream.Seek(DBCHeader.GetSize(), SeekOrigin.Begin);
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            long recordBlockEnd = StringBlockOffset;
            if (DatabaseReader.BaseStream.Position >= recordBlockEnd)
            {
                return false;
            }

            if (ParentDatabase.HasCachedRecordAtIndex(RecordIndex))
            {
                Current = ParentDatabase[RecordIndex];
                DatabaseReader.BaseStream.Position += ParentDatabase.RecordSize;
            }
            else
            {
                Current = DatabaseReader.ReadRecord<TRecord>(ParentDatabase.FieldCount, ParentDatabase.RecordSize, ParentDatabase.Version);

                foreach (var stringReference in Current.GetStringReferences())
                {
                    ParentDatabase.ResolveStringReference(stringReference);
                }

                ParentDatabase.CacheRecordAtIndex(Current, RecordIndex);
            }

            ++RecordIndex;

            return DatabaseReader.BaseStream.Position != recordBlockEnd;
        }

        /// <inheritdoc />
        public void Reset()
        {
            DatabaseReader.BaseStream.Seek(DBCHeader.GetSize(), SeekOrigin.Begin);
            RecordIndex = 0;
            Current = null;
        }

        /// <inheritdoc />
        public TRecord Current { get; private set; }

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Dispose()
        {
            DatabaseReader.Dispose();
        }
    }
}
