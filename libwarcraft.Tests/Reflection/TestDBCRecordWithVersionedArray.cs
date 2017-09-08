using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Reflection
{
	public class TestDBCRecordWithVersionedArray : DBCRecord
	{
		[RecordFieldArray(WarcraftVersion.Cataclysm, Count = 6)]
		[RecordFieldArray(WarcraftVersion.Classic, Count = 2)]
		[RecordFieldArray(WarcraftVersion.Wrath, Count = 4)]
		public uint[] VersionedArray { get; set; }
	}
}