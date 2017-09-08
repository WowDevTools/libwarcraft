using System.Collections.Generic;
using Warcraft.Core;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace libwarcraft.Tests.Reflection
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
		[ForeignKeyInfo(DatabaseName.AnimationData, nameof(AnimationDataRecord.ID))]
		public ForeignKey<uint> TestForeignKeyField { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public StringReference TestNewFieldInWrath { get; set; }

		public override IEnumerable<StringReference> GetStringReferences()
		{
			throw new System.NotImplementedException();
		}
	}
}
