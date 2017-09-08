using System.Collections.Generic;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Reflection
{
	public class TestDBCRecordWithInvalidArray : DBCRecord
	{
		[RecordField(WarcraftVersion.Classic)]
		public uint SimpleField { get; set; }

		[RecordFieldArray(WarcraftVersion.Classic, Count = 4)]
		public IEnumerable<uint> ArrayField { get; set; }
	}
}
