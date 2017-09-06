using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Reflection
{
	[TestFixture]
	public class DBCReflectionTests
	{
		private readonly string[] TestRecordPropertyNames =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecord.TestSimpleField),
			nameof(TestDBCRecord.TestAddedAndRemovedField),
			nameof(TestDBCRecord.TestForeignKeyField),
			nameof(TestDBCRecord.TestNewFieldInWrath),
		};

		private readonly string[] TestRecordClassicPropertyNames =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecord.TestSimpleField),
			nameof(TestDBCRecord.TestAddedAndRemovedField),
			nameof(TestDBCRecord.TestForeignKeyField),
		};

		private readonly string[] TestRecordCataPropertyNames =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecord.TestSimpleField),
			nameof(TestDBCRecord.TestForeignKeyField),
			nameof(TestDBCRecord.TestNewFieldInWrath),
		};

		[Test]
		public void GetRecordPropertiesGetsAValidPropertySet()
		{
			var recordProperties = DBCReflection.GetRecordProperties<TestDBCRecord>();

			var recordPropertyNames = recordProperties.Select(p => p.Name);

			Assert.That(recordPropertyNames, Is.EquivalentTo(this.TestRecordPropertyNames));
		}

		[Test]
		public void GetVersionRelevantPropertiesGetsAValidVersionedPropertySetForAddedProperties()
		{
			var recordProperties = DBCReflection.GetVersionRelevantProperties<TestDBCRecord>(WarcraftVersion.Classic);

			var recordPropertyNames = recordProperties.Select(p => p.Name);

			Assert.That(recordPropertyNames, Is.EquivalentTo(this.TestRecordClassicPropertyNames));
		}

		[Test]
		public void GetVersionRelevantPropertiesGetsAValidVersionedPropertySetForRemovedProperties()
		{
			var recordProperties = DBCReflection.GetVersionRelevantProperties<TestDBCRecord>(WarcraftVersion.Cataclysm);

			var recordPropertyNames = recordProperties.Select(p => p.Name);

			Assert.That(recordPropertyNames, Is.EquivalentTo(this.TestRecordCataPropertyNames));
		}

		[Test]
		public void IsPropertyForeignKeyReturnsTrueForForeignKeyPropertiesAndViceVersa()
		{
			var foreignKeyProperty = typeof(TestDBCRecord).GetProperties()
				.First(p => p.Name == nameof(TestDBCRecord.TestForeignKeyField));

			var otherProperty = typeof(TestDBCRecord).GetProperties()
				.First(p => p.Name == nameof(TestDBCRecord.TestSimpleField));

			Assert.True(DBCReflection.IsPropertyForeignKey(foreignKeyProperty));
			Assert.False(DBCReflection.IsPropertyForeignKey(otherProperty));
		}

		[Test]
		public void GetForeignKeyInfoOnAValidForeignKeyPropertyReturnsValidData()
		{
			var foreignKeyProperty = typeof(TestDBCRecord).GetProperties()
				.First(p => p.Name == nameof(TestDBCRecord.TestForeignKeyField));

			var foreignKeyInfo = DBCReflection.GetForeignKeyInfo(foreignKeyProperty);

			Assert.AreEqual(DatabaseName.AnimationData, foreignKeyInfo.Database);
			Assert.AreEqual(nameof(AnimationDataRecord.ID), foreignKeyInfo.Field);
		}

		[Test]
		public void GetForeignKeyInfoOnAPropertyThatIsNotAForeignKeyThrows()
		{
			var otherProperty = typeof(TestDBCRecord).GetProperties()
				.First(p => p.Name == nameof(TestDBCRecord.TestSimpleField));

			Assert.Throws<ArgumentException>(() => DBCReflection.GetForeignKeyInfo(otherProperty));
		}

		[Test]
		public void GetForeignKeyInfoOnAPropertyWithoutTheForeignKeyInfoAttributeThrows()
		{
			var invalidForeignKeyProperty = typeof(InvalidTestDBCRecord).GetProperties()
				.First(p => p.Name == nameof(InvalidTestDBCRecord.TestForeignKeyFieldMissingInfo));

			Assert.Throws<InvalidDataException>(() => DBCReflection.GetForeignKeyInfo(invalidForeignKeyProperty));
		}

		[Test]
		public void GetForeignKeyTypeOnAPropertyThatIsNotAForeignKeyThrows()
		{
			var otherProperty = typeof(TestDBCRecord).GetProperties()
				.First(p => p.Name == nameof(TestDBCRecord.TestSimpleField));

			Assert.Throws<ArgumentException>(() => DBCReflection.GetForeignKeyType(otherProperty));
		}

		[Test]
		public void GetForeignKeyTypeOnAForeignKeyReturnsCorrectType()
		{
			var foreignKeyProperty = typeof(TestDBCRecord).GetProperties()
				.First(p => p.Name == nameof(TestDBCRecord.TestForeignKeyField));

			Assert.AreEqual(typeof(uint), DBCReflection.GetForeignKeyType(foreignKeyProperty));
		}
	}
}
