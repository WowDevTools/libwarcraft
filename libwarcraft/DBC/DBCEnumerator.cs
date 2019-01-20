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
        private readonly DBC<TRecord> _parentDatabase;
        private readonly BinaryReader _databaseReader;
        private readonly long _stringBlockOffset;

        private int _recordIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBCEnumerator{TRecord}"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="data">The data to load.</param>
        /// <param name="stringBlockOffset">The offset of the string block.</param>
        public DBCEnumerator(DBC<TRecord> database, byte[] data, long stringBlockOffset)
        {
            _parentDatabase = database;
            _stringBlockOffset = stringBlockOffset;
            _databaseReader = new BinaryReader(new MemoryStream(data));
            _recordIndex = 0;

            // Seek to the start of the record block
            _databaseReader.BaseStream.Seek(DBCHeader.GetSize(), SeekOrigin.Begin);
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            long recordBlockEnd = _stringBlockOffset;
            if (_databaseReader.BaseStream.Position >= recordBlockEnd)
            {
                return false;
            }

            if (_parentDatabase.HasCachedRecordAtIndex(_recordIndex))
            {
                Current = _parentDatabase[_recordIndex];
                _databaseReader.BaseStream.Position += _parentDatabase.RecordSize;
            }
            else
            {
                Current = _databaseReader.ReadRecord<TRecord>(_parentDatabase.FieldCount, _parentDatabase.RecordSize, _parentDatabase.Version);

                foreach (var stringReference in Current.GetStringReferences())
                {
                    _parentDatabase.ResolveStringReference(stringReference);
                }

                _parentDatabase.CacheRecordAtIndex(Current, _recordIndex);
            }

            ++_recordIndex;

            return _databaseReader.BaseStream.Position != recordBlockEnd;
        }

        /// <inheritdoc />
        public void Reset()
        {
            _databaseReader.BaseStream.Seek(DBCHeader.GetSize(), SeekOrigin.Begin);
            _recordIndex = 0;
            Current = null;
        }

        /// <inheritdoc />
        public TRecord Current { get; private set; }

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Dispose()
        {
            _databaseReader.Dispose();
        }
    }
}
