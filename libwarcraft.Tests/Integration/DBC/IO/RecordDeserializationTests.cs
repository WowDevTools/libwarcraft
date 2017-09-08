using System.IO;
using libwarcraft.Tests.Reflection;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;

namespace libwarcraft.Tests.Integration.DBC.IO
{
	[TestFixture]
	public class RecordDeserializationTests
	{
		private readonly byte[] TestDBCRecordBytesClassic =
		{
			1, 0, 0, 0, // ID
			1, 0, 0, 0, // TestSimpleField
			2, 0, 0, 0, // TestAddedAndRemovedField
			6, 0, 0, 0, // TestForeignKeyField
		};

		private readonly byte[] TestDBCRecordBytesWrath =
		{
			1, 0, 0, 0, // ID
			1, 0, 0, 0, // TestSimpleField
			2, 0, 0, 0, // TestAddedAndRemovedField
			6, 0, 0, 0, // TestForeignKeyField
			4, 0, 0, 0, // TestNewFieldInWrath
		};

		private readonly byte[] TestDBCRecordBytesCata =
		{
			1, 0, 0, 0, // ID
			1, 0, 0, 0, // TestSimpleField
			6, 0, 0, 0, // TestForeignKeyField
			4, 0, 0, 0, // TestNewFieldInWrath
		};

		[Test]
		public void DeserializingTestDBCRecordSetsThePropertiesToTheCorrectValues()
		{
			var testVersion = WarcraftVersion.Classic;

			TestDBCRecord record = new TestDBCRecord();
			record.Version = testVersion;

			using (var ms = new MemoryStream(this.TestDBCRecordBytesClassic))
			{
				using (var br = new BinaryReader(ms))
				{
					DBCDeserializer.DeserializeRecord(br, record, testVersion);
				}
			}

			Assert.AreEqual(1, record.ID);
			Assert.AreEqual(1, record.TestSimpleField);
			Assert.AreEqual(2, record.TestAddedAndRemovedField);
			Assert.AreEqual(6, record.TestForeignKeyField.Key);
		}

		[Test]
		public void DeserializingTestDBCRecordSetsThePropertiesToTheCorrectValuesForAddedFields()
		{
			var testVersion = WarcraftVersion.Wrath;

			TestDBCRecord record = new TestDBCRecord();
			record.Version = testVersion;

			using (var ms = new MemoryStream(this.TestDBCRecordBytesWrath))
			{
				using (var br = new BinaryReader(ms))
				{
					DBCDeserializer.DeserializeRecord(br, record, testVersion);
				}
			}

			Assert.AreEqual(1, record.ID);
			Assert.AreEqual(1, record.TestSimpleField);
			Assert.AreEqual(2, record.TestAddedAndRemovedField);
			Assert.AreEqual(6, record.TestForeignKeyField.Key);
			Assert.AreEqual(4, record.TestNewFieldInWrath.Offset);
		}

		[Test]
		public void DeserializingTestDBCRecordSetsThePropertiesToTheCorrectValuesForRemovedFields()
		{
			var testVersion = WarcraftVersion.Cataclysm;

			TestDBCRecord record = new TestDBCRecord();
			record.Version = testVersion;

			using (var ms = new MemoryStream(this.TestDBCRecordBytesCata))
			{
				using (var br = new BinaryReader(ms))
				{
					DBCDeserializer.DeserializeRecord(br, record, testVersion);
				}
			}

			Assert.AreEqual(1, record.ID);
			Assert.AreEqual(1, record.TestSimpleField);
			Assert.AreEqual(6, record.TestForeignKeyField.Key);
			Assert.AreEqual(4, record.TestNewFieldInWrath.Offset);
		}
	}
}
