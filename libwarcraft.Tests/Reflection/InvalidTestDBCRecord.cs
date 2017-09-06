using System.Collections.Generic;
using Warcraft.Core;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace libwarcraft.Tests.Reflection
{
	[DatabaseRecord]
	public class InvalidTestDBCRecord : DBCRecord
	{
		[RecordField(WarcraftVersion.Classic, 1)]
		public ForeignKey<uint> TestForeignKeyFieldMissingInfo { get; set; }

		[RecordField(WarcraftVersion.Classic, 2)]
		public uint TestFieldWithoutSetter { get; }

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
