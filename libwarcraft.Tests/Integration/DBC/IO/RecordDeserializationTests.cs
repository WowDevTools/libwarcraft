using System.IO;
using libwarcraft.Tests.Unit.Reflection.DBC.TestData;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using static libwarcraft.Tests.Unit.Reflection.DBC.TestData.RecordValues;

namespace libwarcraft.Tests.Integration.DBC.IO
{
	[TestFixture]
	public class RecordDeserializationTests
	{
		public class DeserializeRecord
		{
			[Test]
			public void SetsCorrectValuesForMovedFields()
			{
				var testVersion = WarcraftVersion.Classic;

				var record = new TestDBCRecordWithMultipleMovedFields();
				record.Version = testVersion;

				using (var ms = new MemoryStream(MultiMoveClassicBytes))
				{
					using (var br = new BinaryReader(ms))
					{
						DBCDeserializer.DeserializeRecord(br, record, testVersion);
					}
				}

				Assert.AreEqual(1, record.ID);
				Assert.AreEqual(2, record.FieldA);
				Assert.AreEqual(4, record.FieldB);
				Assert.AreEqual(8, record.FieldC);
				Assert.AreEqual(16, record.FieldD);
				Assert.AreEqual(32, record.FieldE);
			}

			[Test]
			public void SetsCorrectValuesForComplexMovedFieldsSingleVersion()
			{
				var testVersion = WarcraftVersion.BurningCrusade;

				var record = new TestDBCRecordWithMultipleMovedFields();
				record.Version = testVersion;

				using (var ms = new MemoryStream(MultiMoveBCBytes))
				{
					using (var br = new BinaryReader(ms))
					{
						DBCDeserializer.DeserializeRecord(br, record, testVersion);
					}
				}

				Assert.AreEqual(1, record.ID);
				Assert.AreEqual(2, record.FieldA);
				Assert.AreEqual(4, record.FieldB);
				Assert.AreEqual(8, record.FieldC);
				Assert.AreEqual(16, record.FieldD);
				Assert.AreEqual(32, record.FieldE);
			}

			[Test]
			public void SetsCorrectValuesForComplexMovedFieldsMultiVersion()
			{
				var testVersion = WarcraftVersion.Wrath;

				var record = new TestDBCRecordWithMultipleMovedFields();
				record.Version = testVersion;

				using (var ms = new MemoryStream(MultiMoveWrathBytes))
				{
					using (var br = new BinaryReader(ms))
					{
						DBCDeserializer.DeserializeRecord(br, record, testVersion);
					}
				}

				Assert.AreEqual(1, record.ID);
				Assert.AreEqual(2, record.FieldA);
				Assert.AreEqual(4, record.FieldB);
				Assert.AreEqual(8, record.FieldC);
				Assert.AreEqual(16, record.FieldD);
				Assert.AreEqual(32, record.FieldE);
			}

			[Test]
			public void SetsCorrectValuesForSimpleRecord()
			{
				var testVersion = WarcraftVersion.Classic;

				TestDBCRecord record = new TestDBCRecord();
				record.Version = testVersion;

				using (var ms = new MemoryStream(SimpleClassicBytes))
				{
					using (var br = new BinaryReader(ms))
					{
						DBCDeserializer.DeserializeRecord(br, record, testVersion);
					}
				}

				Assert.AreEqual(1, record.ID);
				Assert.AreEqual(2, record.TestSimpleField);
				Assert.AreEqual(4, record.TestAddedAndRemovedField);
				Assert.AreEqual(8, record.TestForeignKeyField.Key);
			}

			[Test]
			public void SetsCorrectValuesForAddedFields()
			{
				var testVersion = WarcraftVersion.Wrath;

				TestDBCRecord record = new TestDBCRecord();
				record.Version = testVersion;

				using (var ms = new MemoryStream(SimpleWrathBytes))
				{
					using (var br = new BinaryReader(ms))
					{
						DBCDeserializer.DeserializeRecord(br, record, testVersion);
					}
				}

				Assert.AreEqual(1, record.ID);
				Assert.AreEqual(2, record.TestSimpleField);
				Assert.AreEqual(4, record.TestAddedAndRemovedField);
				Assert.AreEqual(8, record.TestForeignKeyField.Key);
				Assert.AreEqual(16, record.TestNewFieldInWrath.Offset);
			}

			[Test]
			public void SetsCorrectValuesForRemovedFields()
			{
				var testVersion = WarcraftVersion.Cataclysm;

				TestDBCRecord record = new TestDBCRecord();
				record.Version = testVersion;

				using (var ms = new MemoryStream(SimpleCataBytes))
				{
					using (var br = new BinaryReader(ms))
					{
						DBCDeserializer.DeserializeRecord(br, record, testVersion);
					}
				}

				Assert.AreEqual(1, record.ID);
				Assert.AreEqual(2, record.TestSimpleField);
				Assert.AreEqual(8, record.TestForeignKeyField.Key);
				Assert.AreEqual(16, record.TestNewFieldInWrath.Offset);
			}
		}
	}
}
