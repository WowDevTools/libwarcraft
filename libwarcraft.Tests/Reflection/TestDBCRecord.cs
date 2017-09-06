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

		[RecordField(WarcraftVersion.Classic, 1)]
		public uint TestSimpleField { get; set; }

		[RecordField(WarcraftVersion.Classic, WarcraftVersion.Cataclysm, 2)]
		public uint TestAddedAndRemovedField { get; set; }

		[RecordField(WarcraftVersion.Classic, 3)]
		[ForeignKeyInfo(DatabaseName.AnimationData, nameof(AnimationDataRecord.ID))]
		public ForeignKey<uint> TestForeignKeyField { get; set; }

		[RecordField(WarcraftVersion.Wrath, 4)]
		public uint TestNewFieldInWrath { get; set; }

		public override void PostLoad(byte[] data)
		{
			throw new System.NotImplementedException();
		}

		public override IEnumerable<StringReference> GetStringReferences()
		{
			throw new System.NotImplementedException();
		}
	}
}
