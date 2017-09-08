using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace libwarcraft.Tests.Reflection
{
	[TestFixture]
	public class DBCReflectionTests
	{
		private static readonly string[] TestRecordPropertyNames =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecord.TestSimpleField),
			nameof(TestDBCRecord.TestAddedAndRemovedField),
			nameof(TestDBCRecord.TestForeignKeyField),
			nameof(TestDBCRecord.TestNewFieldInWrath),
		};

		private static readonly string[] TestRecordClassicPropertyNames =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecord.TestSimpleField),
			nameof(TestDBCRecord.TestAddedAndRemovedField),
			nameof(TestDBCRecord.TestForeignKeyField),
		};

		private static readonly string[] TestRecordCataPropertyNames =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecord.TestSimpleField),
			nameof(TestDBCRecord.TestForeignKeyField),
			nameof(TestDBCRecord.TestNewFieldInWrath),
		};

		private static readonly string[] TestRecordWithArrayPropertyNames =
		{
			nameof(DBCRecord.ID),
			nameof(TestDBCRecordWithArray.SimpleField),
			nameof(TestDBCRecordWithArray.ArrayField)
		};

		public class GetVersionRelevantProperties
		{
			[Test]
			public void GetVersionRelevantPropertiesGetsAValidVersionedPropertySetForAddedProperties()
			{
				var recordProperties = DBCDeserializer.GetVersionRelevantProperties(WarcraftVersion.Classic, typeof(TestDBCRecord));

				var recordPropertyNames = recordProperties.Select(p => p.Name);

				Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordClassicPropertyNames));
			}

			[Test]
			public void GetVersionRelevantPropertiesGetsAValidVersionedPropertySetForRemovedProperties()
			{
				var recordProperties = DBCDeserializer.GetVersionRelevantProperties(WarcraftVersion.Cataclysm, typeof(TestDBCRecord));

				var recordPropertyNames = recordProperties.Select(p => p.Name);

				Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordCataPropertyNames));
			}
		}

		public class GetRecordProperties
		{
			[Test]
			public void GetRecordPropertiesGetsAValidPropertySet()
			{
				var recordProperties = DBCDeserializer.GetRecordProperties(typeof(TestDBCRecord));

				var recordPropertyNames = recordProperties.Select(p => p.Name);

				Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordPropertyNames));
			}

			[Test]
			public void GetRecordPropertiesIncludesArrayFields()
			{
				var recordProperties = DBCDeserializer.GetRecordProperties(typeof(TestDBCRecordWithArray));

				var recordPropertyNames = recordProperties.Select(p => p.Name);

				Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordWithArrayPropertyNames));
			}

			[Test]
			public void GetRecordPropertiesThrowsIfAnIncompatiblePropertyIsDecoratedWithRecordFieldArray()
			{
				Assert.Throws<IncompatibleRecordArrayTypeException>(() => DBCDeserializer.GetRecordProperties(typeof(TestDBCRecordWithInvalidArray)));
			}
		}

		public class GetForeignKeyInfo
		{
			[Test]
			public void GetForeignKeyInfoOnAValidForeignKeyPropertyReturnsValidData()
			{
				var foreignKeyProperty = typeof(TestDBCRecord).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecord.TestForeignKeyField));

				var foreignKeyInfo = DBCDeserializer.GetForeignKeyInfo(foreignKeyProperty);

				Assert.AreEqual(DatabaseName.AnimationData, foreignKeyInfo.Database);
				Assert.AreEqual(nameof(AnimationDataRecord.ID), foreignKeyInfo.Field);
			}

			[Test]
			public void GetForeignKeyInfoOnAPropertyThatIsNotAForeignKeyThrows()
			{
				var otherProperty = typeof(TestDBCRecord).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecord.TestSimpleField));

				Assert.Throws<ArgumentException>(() => DBCDeserializer.GetForeignKeyInfo(otherProperty));
			}

			[Test]
			public void GetForeignKeyInfoOnAPropertyWithoutTheForeignKeyInfoAttributeThrows()
			{
				var invalidForeignKeyProperty = typeof(InvalidTestDBCRecord).GetProperties()
					.First(p => p.Name == nameof(InvalidTestDBCRecord.TestForeignKeyFieldMissingInfo));

				Assert.Throws<InvalidDataException>(() => DBCDeserializer.GetForeignKeyInfo(invalidForeignKeyProperty));
			}
		}

		public class IsPropertyForeignKey
		{
			[Test]
			public void IsPropertyForeignKeyReturnsTrueForForeignKeyPropertiesAndViceVersa()
			{
				var foreignKeyProperty = typeof(TestDBCRecord).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecord.TestForeignKeyField));

				var otherProperty = typeof(TestDBCRecord).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecord.TestSimpleField));

				Assert.True(DBCDeserializer.IsPropertyForeignKey(foreignKeyProperty));
				Assert.False(DBCDeserializer.IsPropertyForeignKey(otherProperty));
			}
		}

		public class GetRecordSize
		{
			[Test]
			public void GetRecordSizeReturnsCorrectSizeOnVersionedRecords()
			{
				long recordSizeClassic = DBCDeserializer.GetRecordSize(WarcraftVersion.Classic, typeof(TestDBCRecord));
				long recordSizeWrath = DBCDeserializer.GetRecordSize(WarcraftVersion.Wrath, typeof(TestDBCRecord));
				long recordSizeCata = DBCDeserializer.GetRecordSize(WarcraftVersion.Cataclysm, typeof(TestDBCRecord));

				Assert.AreEqual(16, recordSizeClassic);
				Assert.AreEqual(20, recordSizeWrath);
				Assert.AreEqual(16, recordSizeCata);
			}

			[Test]
			public void GetRecordSizeReturnsCorrectSizeOnRecordsWithArrays()
			{
				Assert.AreEqual(24, DBCDeserializer.GetRecordSize(WarcraftVersion.Classic, typeof(TestDBCRecordWithArray)));
			}
		}

		public class GetPropertyCount
		{
			[Test]
			public void GetPropertyCountReturnsCorrectCountOnRecordsOnVersionedRecords()
			{
				long propCountClassic = DBCDeserializer.GetPropertyCount(WarcraftVersion.Classic, typeof(TestDBCRecord));
				long propCountWrath = DBCDeserializer.GetPropertyCount(WarcraftVersion.Wrath, typeof(TestDBCRecord));
				long propCountCata = DBCDeserializer.GetPropertyCount(WarcraftVersion.Cataclysm, typeof(TestDBCRecord));

				Assert.AreEqual(4, propCountClassic);
				Assert.AreEqual(5, propCountWrath);
				Assert.AreEqual(4, propCountCata);
			}

			[Test]
			public void GetPropertyCountReturnsCorrectCountForRecordsWithArrays()
			{
				Assert.AreEqual(6, DBCDeserializer.GetPropertyCount(WarcraftVersion.Classic, typeof(TestDBCRecordWithArray)));
			}
		}

		public class GetPropertyFieldArrayAttribute
		{
			[Test]
			public void GetPropertyFieldArrayAttributeReturnsAValidAttributeForAMarkedProperty()
			{
				var arrayProperty = typeof(TestDBCRecordWithArray).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecordWithArray.ArrayField));

				Assert.DoesNotThrow(() => DBCDeserializer.GetPropertyFieldArrayAttribute(arrayProperty));
			}

			[Test]
			public void GetPropertyFieldArrayAttributeThrowsForAnUnmarkedProperty()
			{
				var simpleProperty = typeof(TestDBCRecordWithArray).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecordWithArray.SimpleField));

				Assert.Throws<ArgumentException>(() => DBCDeserializer.GetPropertyFieldArrayAttribute(simpleProperty));
			}
		}

		public class IsPropertyArray
		{
			[Test]
			public void IsPropertyArrayReturnsFalseForARecordFieldProperty()
			{
				var arrayProperty = typeof(TestDBCRecordWithArray).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecordWithArray.SimpleField));

				Assert.False(DBCDeserializer.IsPropertyFieldArray(arrayProperty));
			}

			[Test]
			public void IsPropertyArrayReturnsTrueForARecordFieldArrayProperty()
			{
				var arrayProperty = typeof(TestDBCRecordWithArray).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecordWithArray.ArrayField));

				Assert.True(DBCDeserializer.IsPropertyFieldArray(arrayProperty));
			}

			[Test]
			public void IsPropertyArrayReturnsFalseForAnUnmarkedProperty()
			{
				var arrayProperty = typeof(TestDBCRecord).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecord.TestNotRecordField));

				Assert.False(DBCDeserializer.IsPropertyFieldArray(arrayProperty));
			}
		}

		public class GetUnderlyingStoredType
		{
			[Test]
			public void GetUnderlyingStoredTypeWorksForPrimitiveArrays()
			{
				var complexType = typeof(uint[]);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void GetUnderlyingStoredTypeWorksForForeignKeys()
			{
				var complexType = typeof(ForeignKey<uint>);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void GetUnderlyingStoredTypeWorksForIListOfPrimitive()
			{
				var complexType = typeof(IList<uint>);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void GetUnderlyingStoredTypeWorksForArraysOfGenericType()
			{
				var complexType = typeof(ForeignKey<uint>[]);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void GetUnderlyingStoredTypeWorksForIListOfGenericType()
			{
				var complexType = typeof(IList<ForeignKey<uint>>);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void GetUnderlyingStoredTypeWorksForStringReferenceSpecialType()
			{
				var complexType = typeof(StringReference);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void GetUnderlyingStoredTypeWorksForLocalizedStringReferenceSpecialType()
			{
				var complexType = typeof(LocalizedStringReference);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void GetUnderlyingStoredTypeWorksForArrayOfStringReferenceSpecialType()
			{
				var complexType = typeof(StringReference[]);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void GetUnderlyingStoredTypeWorksForArrayOfLocalizedStringReferenceSpecialType()
			{
				var complexType = typeof(LocalizedStringReference[]);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void GetUnderlyingStoredTypeWorksForIListOfStringReferenceSpecialType()
			{
				var complexType = typeof(IList<StringReference>);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void GetUnderlyingStoredTypeWorksForIListOfLocalizedStringReferenceSpecialType()
			{
				var complexType = typeof(IList<LocalizedStringReference>);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}
		}
	}
}
