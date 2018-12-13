using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace libwarcraft.Tests.Unit.Reflection.DBC.TestData
{
    [DatabaseRecord]
    public class TestDBCRecordInvalidForeignKeyField : DBCRecord
    {
        [RecordField(WarcraftVersion.Classic)]
        public ForeignKey<uint> TestForeignKeyFieldMissingInfo { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint TestFieldWithoutSetter { get; }
    }
}
