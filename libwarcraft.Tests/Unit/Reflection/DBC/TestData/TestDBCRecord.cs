using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace libwarcraft.Tests.Unit.Reflection.DBC.TestData
{
    [DatabaseRecord]
    public class TestDBCRecord : DBCRecord
    {
        public uint TestNotRecordField { get; }

        [RecordField(WarcraftVersion.Classic)]
        public uint TestSimpleField { get; set; }

        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Cataclysm)]
        public uint TestAddedAndRemovedField { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.AnimationData, nameof(ID))]
        public ForeignKey<uint> TestForeignKeyField { get; set; }

        [RecordField(WarcraftVersion.Wrath)]
        public StringReference TestNewFieldInWrath { get; set; }
    }
}
