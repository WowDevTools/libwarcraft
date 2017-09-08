using System.Collections.Generic;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Unit.Reflection.DBC.TestData
{
	public class TestDBCRecordWithArray : DBCRecord
	{
		[RecordField(WarcraftVersion.Classic)]
		public uint SimpleField { get; set; }

		[RecordFieldArray(WarcraftVersion.Classic, Count = 4)]
		public List<uint> ArrayField { get; set; }
	}
}
