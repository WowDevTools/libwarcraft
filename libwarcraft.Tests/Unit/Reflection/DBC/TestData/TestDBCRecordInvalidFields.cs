//
//  TestDBCRecordInvalidFields.cs
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
using Warcraft.DBC.SpecialFields;

#pragma warning disable 1591, SA1600, SA1649, SA1402

namespace Warcraft.Unit.Reflection.DBC.TestData
{
    [DatabaseRecord]
    public class TestDBCRecordInvalidForeignKeyField : DBCRecord
    {
        [RecordField(WarcraftVersion.Classic)]
        public ForeignKey<uint> TestForeignKeyFieldMissingInfo { get; set; } = null!;

        [RecordField(WarcraftVersion.Classic)]
        public uint TestFieldWithoutSetter { get; }
    }
}
