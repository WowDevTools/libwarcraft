using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Unit.Reflection.DBC.TestData
{
	public class FieldNameLists
	{
		public static readonly string[] SingleMovedFieldNamesBeforeMove =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecordWithSingleMovedField.FieldA),
			nameof(TestDBCRecordWithSingleMovedField.FieldB),
			nameof(TestDBCRecordWithSingleMovedField.FieldC)
		};

		public static readonly string[] SingleMovedFieldBCAfterMove =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecordWithSingleMovedField.FieldC),
			nameof(TestDBCRecordWithSingleMovedField.FieldA),
			nameof(TestDBCRecordWithSingleMovedField.FieldB)
		};


		public static readonly string[] SingleMovedFieldWrathAfterMove =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecordWithSingleMovedField.FieldA),
			nameof(TestDBCRecordWithSingleMovedField.FieldC),
			nameof(TestDBCRecordWithSingleMovedField.FieldB)
		};

		public static readonly string[] MultiMovedFieldsBeforeMove =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldA),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldB),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldC),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldD),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldE),
		};

		public static readonly string[] MultiMovedFieldsBCAfterMove =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldC),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldA),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldE),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldB),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldD),
		};

		public static readonly string[] MultiMovedFieldsWrathAfterMove =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldD),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldC),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldA),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldE),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldB),
		};

		public static readonly string[] SingleMovedFieldRecordNamesMovingFields =
		{
			nameof(TestDBCRecordWithSingleMovedField.FieldC)
		};

		public static readonly string[] SimpleMoveDependencyChainFieldC =
		{
			nameof(DBCRecord.ID)
		};

		public static readonly string[] MultiMoveBCDependencyChainFieldC =
		{
			nameof(DBCRecord.ID)
		};

		public static readonly string[] MultiMoveBCDependencyChainFieldE =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldC),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldA),
		};

		public static readonly string[] MultiMoveBCDependencyChainFieldA =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecordWithMultipleMovedFields.FieldC)
		};

		public static readonly string[] MultiMoveWrathDependencyChainFieldD =
		{
			nameof(DBCRecord.ID)
		};

		public static readonly string[] MultiVersionDependencyChainFieldE =
		{
			nameof(DBCRecord.ID)
		};
	}
}
