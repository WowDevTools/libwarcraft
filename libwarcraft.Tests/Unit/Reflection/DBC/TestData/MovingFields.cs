using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Unit.Reflection.DBC.TestData
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
