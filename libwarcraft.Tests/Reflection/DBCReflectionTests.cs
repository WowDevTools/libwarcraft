using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			public void GetsAValidVersionedPropertySetForAddedProperties()
			{
				var recordProperties = DBCDeserializer.GetVersionRelevantProperties(WarcraftVersion.Classic, typeof(TestDBCRecord));

				var recordPropertyNames = recordProperties.Select(p => p.Name);

				Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordClassicPropertyNames));
			}

			[Test]
			public void GetsAValidVersionedPropertySetForRemovedProperties()
			{
				var recordProperties = DBCDeserializer.GetVersionRelevantProperties(WarcraftVersion.Cataclysm, typeof(TestDBCRecord));

				var recordPropertyNames = recordProperties.Select(p => p.Name);

				Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordCataPropertyNames));
			}
		}

		public class GetRecordProperties
		{
			[Test]
			public void GetsAValidPropertySet()
			{
				var recordProperties = DBCDeserializer.GetRecordProperties(typeof(TestDBCRecord));

				var recordPropertyNames = recordProperties.Select(p => p.Name);

				Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordPropertyNames));
			}

			[Test]
			public void IncludesArrayFields()
			{
				var recordProperties = DBCDeserializer.GetRecordProperties(typeof(TestDBCRecordWithArray));

				var recordPropertyNames = recordProperties.Select(p => p.Name);

				Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordWithArrayPropertyNames));
			}

			[Test]
			public void ThrowsIfAnIncompatiblePropertyIsDecoratedWithRecordFieldArray()
			{
				Assert.Throws<IncompatibleRecordArrayTypeException>(() => DBCDeserializer.GetRecordProperties(typeof(TestDBCRecordWithInvalidArray)));
			}
		}

		public class GetForeignKeyInfo
		{
			[Test]
			public void OnAValidForeignKeyPropertyReturnsValidData()
			{
				var foreignKeyProperty = typeof(TestDBCRecord).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecord.TestForeignKeyField));

				var foreignKeyInfo = DBCDeserializer.GetForeignKeyInfo(foreignKeyProperty);

				Assert.AreEqual(DatabaseName.AnimationData, foreignKeyInfo.Database);
				Assert.AreEqual(nameof(AnimationDataRecord.ID), foreignKeyInfo.Field);
			}

			[Test]
			public void OnAPropertyThatIsNotAForeignKeyThrows()
			{
				var otherProperty = typeof(TestDBCRecord).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecord.TestSimpleField));

				Assert.Throws<ArgumentException>(() => DBCDeserializer.GetForeignKeyInfo(otherProperty));
			}

			[Test]
			public void OnAPropertyWithoutTheForeignKeyInfoAttributeThrows()
			{
				var invalidForeignKeyProperty = typeof(InvalidTestDBCRecord).GetProperties()
					.First(p => p.Name == nameof(InvalidTestDBCRecord.TestForeignKeyFieldMissingInfo));

				Assert.Throws<InvalidDataException>(() => DBCDeserializer.GetForeignKeyInfo(invalidForeignKeyProperty));
			}
		}

		public class IsPropertyForeignKey
		{
			[Test]
			public void ReturnsTrueForForeignKeyPropertiesAndViceVersa()
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
			public void ReturnsCorrectSizeOnVersionedRecords()
			{
				long recordSizeClassic = DBCDeserializer.GetRecordSize(WarcraftVersion.Classic, typeof(TestDBCRecord));
				long recordSizeWrath = DBCDeserializer.GetRecordSize(WarcraftVersion.Wrath, typeof(TestDBCRecord));
				long recordSizeCata = DBCDeserializer.GetRecordSize(WarcraftVersion.Cataclysm, typeof(TestDBCRecord));

				Assert.AreEqual(16, recordSizeClassic);
				Assert.AreEqual(20, recordSizeWrath);
				Assert.AreEqual(16, recordSizeCata);
			}

			[Test]
			public void ReturnsCorrectSizeOnRecordsWithArrays()
			{
				Assert.AreEqual(24, DBCDeserializer.GetRecordSize(WarcraftVersion.Classic, typeof(TestDBCRecordWithArray)));
			}
		}

		public class GetPropertyCount
		{
			[Test]
			public void ReturnsCorrectCountOnRecordsOnVersionedRecords()
			{
				long propCountClassic = DBCDeserializer.GetPropertyCount(WarcraftVersion.Classic, typeof(TestDBCRecord));
				long propCountWrath = DBCDeserializer.GetPropertyCount(WarcraftVersion.Wrath, typeof(TestDBCRecord));
				long propCountCata = DBCDeserializer.GetPropertyCount(WarcraftVersion.Cataclysm, typeof(TestDBCRecord));

				Assert.AreEqual(4, propCountClassic);
				Assert.AreEqual(5, propCountWrath);
				Assert.AreEqual(4, propCountCata);
			}

			[Test]
			public void ReturnsCorrectCountForRecordsWithArrays()
			{
				Assert.AreEqual(6, DBCDeserializer.GetPropertyCount(WarcraftVersion.Classic, typeof(TestDBCRecordWithArray)));
			}
		}

		public class GetVersionRelevantPropertyFieldArrayAttribute
		{
			[Test]
			public void ReturnsAValidAttributeForAMarkedProperty()
			{
				var arrayProperty = typeof(TestDBCRecordWithArray).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecordWithArray.ArrayField));

				Assert.DoesNotThrow(() => DBCDeserializer.GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion.Classic, arrayProperty));
			}

			[Test]
			public void ThrowsForAnUnmarkedProperty()
			{
				var simpleProperty = typeof(TestDBCRecordWithArray).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecordWithArray.SimpleField));

				Assert.Throws<ArgumentException>(() => DBCDeserializer.GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion.Classic, simpleProperty));
			}

			[Test]
			public void ReturnsCorrectAttributeForPropertyWithMultipleAttributes()
			{
				var arrayProperty = typeof(TestDBCRecordWithVersionedArray).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecordWithVersionedArray.VersionedArray));

				var classicAttribute =
					DBCDeserializer.GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion.Classic, arrayProperty);

				var wrathAttribute =
					DBCDeserializer.GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion.Wrath, arrayProperty);

				var cataAttribute =
					DBCDeserializer.GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion.Cataclysm, arrayProperty);

				Assert.AreEqual(2, classicAttribute.Count);
				Assert.AreEqual(4, wrathAttribute.Count);
				Assert.AreEqual(6, cataAttribute.Count);
			}
		}

		public class IsPropertyFieldArray
		{
			[Test]
			public void ReturnsFalseForARecordFieldProperty()
			{
				var arrayProperty = typeof(TestDBCRecordWithArray).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecordWithArray.SimpleField));

				Assert.False(DBCDeserializer.IsPropertyFieldArray(arrayProperty));
			}

			[Test]
			public void ReturnsTrueForARecordFieldArrayProperty()
			{
				var arrayProperty = typeof(TestDBCRecordWithArray).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecordWithArray.ArrayField));

				Assert.True(DBCDeserializer.IsPropertyFieldArray(arrayProperty));
			}

			[Test]
			public void ReturnsFalseForAnUnmarkedProperty()
			{
				var arrayProperty = typeof(TestDBCRecord).GetProperties()
					.First(p => p.Name == nameof(TestDBCRecord.TestNotRecordField));

				Assert.False(DBCDeserializer.IsPropertyFieldArray(arrayProperty));
			}
		}

		public class GetUnderlyingStoredPrimitiveType
		{
			[Test]
			public void ReturnsCorrectTypeForPrimitiveArrays()
			{
				var complexType = typeof(uint[]);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void ReturnsCorrectTypeForForeignKeys()
			{
				var complexType = typeof(ForeignKey<uint>);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void ReturnsCorrectTypeForIListOfPrimitive()
			{
				var complexType = typeof(IList<uint>);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void ReturnsCorrectTypeForArraysOfGenericType()
			{
				var complexType = typeof(ForeignKey<uint>[]);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void ReturnsCorrectTypeForIListOfGenericType()
			{
				var complexType = typeof(IList<ForeignKey<uint>>);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void ReturnsCorrectTypeForStringReferenceSpecialType()
			{
				var complexType = typeof(StringReference);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void ReturnsCorrectTypeForLocalizedStringReferenceSpecialType()
			{
				var complexType = typeof(LocalizedStringReference);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void ReturnsCorrectTypeForArrayOfStringReferenceSpecialType()
			{
				var complexType = typeof(StringReference[]);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void ReturnsCorrectTypeForArrayOfLocalizedStringReferenceSpecialType()
			{
				var complexType = typeof(LocalizedStringReference[]);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void ReturnsCorrectTypeForIListOfStringReferenceSpecialType()
			{
				var complexType = typeof(IList<StringReference>);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}

			[Test]
			public void ReturnsCorrectTypeForIListOfLocalizedStringReferenceSpecialType()
			{
				var complexType = typeof(IList<LocalizedStringReference>);

				Assert.AreEqual(typeof(uint), DBCDeserializer.GetUnderlyingStoredPrimitiveType(complexType));
			}
		}
	}
}
