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
    /// <typeparam name="T"></typeparam>
    public class DBCEnumerator<T> : IEnumerator<T> where T : DBCRecord, new()
    {
        private readonly DBC<T> ParentDatabase;
        private readonly BinaryReader DatabaseReader;
        private readonly long StringBlockOffset;

        private int RecordIndex;

        /// <summary>
        /// Initialize a new <see cref="DBCEnumerator{T}"/> from a given database, its data, and where the string block
        /// begins in it.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="data"></param>
        /// <param name="stringBlockOffset"></param>
        public DBCEnumerator(DBC<T> database, byte[] data, long stringBlockOffset)
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
                Current = DatabaseReader.ReadRecord<T>(ParentDatabase.FieldCount, ParentDatabase.RecordSize, ParentDatabase.Version);

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
        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        /// <inheritdoc />
        public void Dispose()
        {
            DatabaseReader.Dispose();
        }
    }
}
