using System.Collections.Generic;
using Warcraft.Core;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace libwarcraft.Tests.Reflection
{
	[DatabaseRecord]
	public class InvalidTestDBCRecord : DBCRecord
	{
		[RecordField(WarcraftVersion.Classic)]
		public ForeignKey<uint> TestForeignKeyFieldMissingInfo { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint TestFieldWithoutSetter { get; }

		public override IEnumerable<StringReference> GetStringReferences()
		{
			throw new System.NotImplementedException();
		}
	}
}
