//
//  MovingFields.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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

using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.Definitions;

#pragma warning disable 1591, SA1600, SA1649, SA1402

namespace Warcraft.Unit.Reflection.DBC.TestData
{
    [DatabaseRecord]
    public class TestDBCRecordWithSingleMovedField : DBCRecord
    {
        [RecordField(WarcraftVersion.Classic)]
        public uint FieldA { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint FieldB { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.BurningCrusade, ComesAfter = nameof(ID))]
        public uint FieldC { get; set; }
    }

    [DatabaseRecord]
    public class TestDBCRecordWithSingleMovedFieldMultipleVersions : DBCRecord
    {
        [RecordField(WarcraftVersion.Classic)]
        public uint FieldA { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint FieldB { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.BurningCrusade, ComesAfter = nameof(ID))]
        [RecordFieldOrder(WarcraftVersion.Wrath, ComesAfter = nameof(FieldA))]
        public uint FieldC { get; set; }
    }

    [DatabaseRecord]
    public class TestDBCRecordWithMultipleMovedFields : DBCRecord
    {
        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.BurningCrusade, ComesAfter = nameof(FieldC))]
        public uint FieldA { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint FieldB { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.BurningCrusade, ComesAfter = nameof(ID))]
        public uint FieldC { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.Wrath, ComesAfter = nameof(ID))]
        public uint FieldD { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.BurningCrusade, ComesAfter = nameof(FieldA))]
        public uint FieldE { get; set; }
    }

    [DatabaseRecord]
    public class TestDBCRecordWithInvalidReentrantMovedFields : DBCRecord
    {
        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.BurningCrusade, ComesAfter = nameof(FieldC))]
        public uint FieldA { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint FieldB { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.BurningCrusade, ComesAfter = nameof(FieldA))]
        public uint FieldC { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.BurningCrusade, ComesAfter = nameof(ID))]
        public uint FieldD { get; set; }
    }
}
