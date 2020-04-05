//
//  DBCInspectorTests.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.Unit.Reflection.DBC.TestData;
using static Warcraft.Unit.Reflection.DBC.TestData.FieldNameLists;

#pragma warning disable 1591, SA1600, SA1649, SA1402

namespace Warcraft.Unit.Reflection.DBC
{
    [TestFixture]
    public class DBCInspectorTests
    {
        public class GetMovedProperties
        {
            [Test]
            public void ReturnsCorrectPropertiesForSingleField()
            {
                var recordProperties = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithSingleMovedField)).ToList();

                var movedPropertiesClassic = DBCInspector.GetMovedProperties(WarcraftVersion.Classic, recordProperties).Select(p => p.Key.Name);
                var movedPropertiesBC = DBCInspector.GetMovedProperties(WarcraftVersion.BurningCrusade, recordProperties).Select(p => p.Key.Name);

                Assert.That(movedPropertiesClassic, Is.Empty);
                Assert.That(movedPropertiesBC, Is.EquivalentTo(SingleMovedFieldRecordNamesMovingFields));
            }

            [Test]
            public void ReturnsCorrectPropertiesForMultipleFields()
            {
                var recordProperties = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields)).ToList();

                var movedPropertiesClassic = DBCInspector.GetMovedProperties(WarcraftVersion.Classic, recordProperties).Select(p => p.Key.Name);
                var movedPropertiesBC = DBCInspector.GetMovedProperties(WarcraftVersion.BurningCrusade, recordProperties).Select(p => p.Key.Name);
                var movedPropertiesWrath = DBCInspector.GetMovedProperties(WarcraftVersion.Wrath, recordProperties).Select(p => p.Key.Name);

                Assert.That(movedPropertiesClassic, Is.Empty);
                Assert.That(movedPropertiesBC, Is.EquivalentTo(MultipleMovedFieldRecordNamesMovingFieldsBC));
                Assert.That(movedPropertiesWrath, Is.EquivalentTo(MultipleMovedFieldRecordNamesMovingFieldsWrath));
            }
        }

        public class GetMostRecentPropertyMove
        {
            [Test]
            public void ReturnsCorrectAttributeForSingleVersionMove()
            {
                var movedProperty = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithSingleMovedField))
                    .First(p => p.Name == nameof(TestDBCRecordWithSingleMovedField.FieldC));

                var attribute = DBCInspector.GetMostRecentPropertyMove(WarcraftVersion.BurningCrusade, movedProperty);

                Assert.AreEqual(WarcraftVersion.BurningCrusade, attribute.MovedIn);
                Assert.AreEqual(nameof(DBCRecord.ID), attribute.ComesAfter);
            }

            [Test]
            public void ReturnsCorrectAttributeForMultiVersionMove()
            {
                var movedProperty = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithSingleMovedFieldMultipleVersions))
                    .First(p => p.Name == nameof(TestDBCRecordWithSingleMovedFieldMultipleVersions.FieldC));

                var attributeBC = DBCInspector.GetMostRecentPropertyMove(WarcraftVersion.BurningCrusade, movedProperty);
                var attributeWrath = DBCInspector.GetMostRecentPropertyMove(WarcraftVersion.Wrath, movedProperty);

                Assert.AreEqual(WarcraftVersion.BurningCrusade, attributeBC.MovedIn);
                Assert.AreEqual(nameof(DBCRecord.ID), attributeBC.ComesAfter);

                Assert.AreEqual(WarcraftVersion.Wrath, attributeWrath.MovedIn);
                Assert.AreEqual(nameof(TestDBCRecordWithSingleMovedFieldMultipleVersions.FieldA), attributeWrath.ComesAfter);
            }

            [Test]
            public void ThrowsIfPropertyHasNotMoved()
            {
                var movedPropertyButNotInVersion = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithSingleMovedFieldMultipleVersions))
                    .First(p => p.Name == nameof(TestDBCRecordWithSingleMovedFieldMultipleVersions.FieldC));

                Assert.Throws<ArgumentException>(() => DBCInspector.GetMostRecentPropertyMove(WarcraftVersion.Classic, movedPropertyButNotInVersion));
            }
        }

        public class HasPropertyMovedInVersion
        {
            [Test]
            public void ReturnsTrueForMovedField()
            {
                var movedProperty = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithSingleMovedField))
                    .First(p => p.Name == nameof(TestDBCRecordWithSingleMovedField.FieldC));

                Assert.IsTrue(DBCInspector.HasPropertyMovedInVersion(WarcraftVersion.BurningCrusade, movedProperty));
            }

            [Test]
            public void ReturnsFalseForNotMovedField()
            {
                var notMovedProperty = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithSingleMovedField))
                    .First(p => p.Name == nameof(TestDBCRecordWithSingleMovedField.FieldA));

                Assert.IsFalse(DBCInspector.HasPropertyMovedInVersion(WarcraftVersion.BurningCrusade, notMovedProperty));
            }

            [Test]
            public void ReturnsFalseForMovedFieldButNotInTheSpecifiedVersion()
            {
                var movedProperty = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithSingleMovedField))
                    .First(p => p.Name == nameof(TestDBCRecordWithSingleMovedField.FieldC));

                Assert.IsFalse(DBCInspector.HasPropertyMovedInVersion(WarcraftVersion.Classic, movedProperty));
            }
        }

        public class GetVersionRelevantProperties
        {
            [Test]
            public void GetsAValidVersionedPropertySetForAddedProperties()
            {
                var recordProperties = DBCInspector.GetVersionRelevantProperties(WarcraftVersion.Classic, typeof(TestDBCRecord));

                var recordPropertyNames = recordProperties.Select(p => p.Name);

                Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordClassicPropertyNames));
            }

            [Test]
            public void GetsAValidVersionedPropertySetForRemovedProperties()
            {
                var recordProperties = DBCInspector.GetVersionRelevantProperties(WarcraftVersion.Cataclysm, typeof(TestDBCRecord));

                var recordPropertyNames = recordProperties.Select(p => p.Name);

                Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordCataPropertyNames));
            }

            [Test]
            public void IncludesArrayFieldsWhenMultipleVersionsArePresent()
            {
                var recordProperties = DBCInspector.GetVersionRelevantProperties(WarcraftVersion.Classic, typeof(TestDBCRecordWithVersionedArray));

                var recordPropertyNames = recordProperties.Select(p => p.Name);

                Assert.That(recordPropertyNames, Is.EquivalentTo(VersionedArrayRecordNames));
            }
        }

        public class GetRecordProperties
        {
            [Test]
            public void GetsAValidPropertySet()
            {
                var recordProperties = DBCInspector.GetRecordProperties(typeof(TestDBCRecord));

                var recordPropertyNames = recordProperties.Select(p => p.Name);

                Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordPropertyNames));
            }

            [Test]
            public void IncludesArrayFields()
            {
                var recordProperties = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithArray));

                var recordPropertyNames = recordProperties.Select(p => p.Name);

                Assert.That(recordPropertyNames, Is.EquivalentTo(TestRecordWithArrayPropertyNames));
            }

            [Test]
            public void ThrowsIfAnIncompatiblePropertyIsDecoratedWithRecordFieldArray()
            {
                Assert.Throws<IncompatibleRecordArrayTypeException>(() => DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithInvalidArray)));
            }
        }

        public class GetForeignKeyInfo
        {
            [Test]
            public void OnAValidForeignKeyPropertyReturnsValidData()
            {
                var foreignKeyProperty = typeof(TestDBCRecord).GetProperties()
                    .First(p => p.Name == nameof(TestDBCRecord.TestForeignKeyField));

                var foreignKeyInfo = DBCInspector.GetForeignKeyInfo(foreignKeyProperty);

                Assert.AreEqual(DatabaseName.AnimationData, foreignKeyInfo.Database);
                Assert.AreEqual(nameof(AnimationDataRecord.ID), foreignKeyInfo.Field);
            }

            [Test]
            public void OnAPropertyThatIsNotAForeignKeyThrows()
            {
                var otherProperty = typeof(TestDBCRecord).GetProperties()
                    .First(p => p.Name == nameof(TestDBCRecord.TestSimpleField));

                Assert.Throws<ArgumentException>(() => DBCInspector.GetForeignKeyInfo(otherProperty));
            }

            [Test]
            public void OnAPropertyWithoutTheForeignKeyInfoAttributeThrows()
            {
                var invalidForeignKeyProperty = typeof(TestDBCRecordInvalidForeignKeyField).GetProperties()
                    .First(p => p.Name == nameof(TestDBCRecordInvalidForeignKeyField.TestForeignKeyFieldMissingInfo));

                Assert.Throws<InvalidDataException>(() => DBCInspector.GetForeignKeyInfo(invalidForeignKeyProperty));
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

                Assert.True(DBCInspector.IsPropertyForeignKey(foreignKeyProperty));
                Assert.False(DBCInspector.IsPropertyForeignKey(otherProperty));
            }
        }

        public class GetRecordSize
        {
            [Test]
            public void ReturnsCorrectSizeOnVersionedRecords()
            {
                long recordSizeClassic = DBCInspector.GetRecordSize(WarcraftVersion.Classic, typeof(TestDBCRecord));
                long recordSizeWrath = DBCInspector.GetRecordSize(WarcraftVersion.Wrath, typeof(TestDBCRecord));
                long recordSizeCata = DBCInspector.GetRecordSize(WarcraftVersion.Cataclysm, typeof(TestDBCRecord));

                Assert.AreEqual(16, recordSizeClassic);
                Assert.AreEqual(20, recordSizeWrath);
                Assert.AreEqual(16, recordSizeCata);
            }

            [Test]
            public void ReturnsCorrectSizeOnRecordsWithArrays()
            {
                Assert.AreEqual(24, DBCInspector.GetRecordSize(WarcraftVersion.Classic, typeof(TestDBCRecordWithArray)));
            }
        }

        public class GetPropertyCount
        {
            [Test]
            public void ReturnsCorrectCountOnRecordsOnVersionedRecords()
            {
                long propCountClassic = DBCInspector.GetPropertyCount(WarcraftVersion.Classic, typeof(TestDBCRecord));
                long propCountWrath = DBCInspector.GetPropertyCount(WarcraftVersion.Wrath, typeof(TestDBCRecord));
                long propCountCata = DBCInspector.GetPropertyCount(WarcraftVersion.Cataclysm, typeof(TestDBCRecord));

                Assert.AreEqual(4, propCountClassic);
                Assert.AreEqual(5, propCountWrath);
                Assert.AreEqual(4, propCountCata);
            }

            [Test]
            public void ReturnsCorrectCountForRecordsWithArrays()
            {
                Assert.AreEqual(6, DBCInspector.GetPropertyCount(WarcraftVersion.Classic, typeof(TestDBCRecordWithArray)));
            }

            [Test]
            public void ReturnsCorrectCountForRecordsWithVersionedArrays()
            {
                Assert.AreEqual(3, DBCInspector.GetPropertyCount(WarcraftVersion.Classic, typeof(TestDBCRecordWithVersionedArray)));
                Assert.AreEqual(5, DBCInspector.GetPropertyCount(WarcraftVersion.Wrath, typeof(TestDBCRecordWithVersionedArray)));
                Assert.AreEqual(7, DBCInspector.GetPropertyCount(WarcraftVersion.Cataclysm, typeof(TestDBCRecordWithVersionedArray)));
            }
        }

        public class GetVersionRelevantPropertyFieldArrayAttribute
        {
            [Test]
            public void ReturnsAValidAttributeForAMarkedProperty()
            {
                var arrayProperty = typeof(TestDBCRecordWithArray).GetProperties()
                    .First(p => p.Name == nameof(TestDBCRecordWithArray.ArrayField));

                RecordFieldAttribute? attribute = null;
                Assert.DoesNotThrow(() => attribute = DBCInspector.GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion.Classic, arrayProperty));

                Assert.AreEqual(WarcraftVersion.Classic, attribute?.IntroducedIn);
            }

            [Test]
            public void ThrowsForAnUnmarkedProperty()
            {
                var simpleProperty = typeof(TestDBCRecordWithArray).GetProperties()
                    .First(p => p.Name == nameof(TestDBCRecordWithArray.SimpleField));

                Assert.Throws<ArgumentException>(() => DBCInspector.GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion.Classic, simpleProperty));
            }

            [Test]
            public void ReturnsCorrectAttributeForPropertyWithMultipleAttributes()
            {
                var arrayProperty = typeof(TestDBCRecordWithVersionedArray).GetProperties()
                    .First(p => p.Name == nameof(TestDBCRecordWithVersionedArray.VersionedArray));

                var classicAttribute =
                    DBCInspector.GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion.Classic, arrayProperty);

                var wrathAttribute =
                    DBCInspector.GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion.Wrath, arrayProperty);

                var cataAttribute =
                    DBCInspector.GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion.Cataclysm, arrayProperty);

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

                Assert.False(DBCInspector.IsPropertyFieldArray(arrayProperty));
            }

            [Test]
            public void ReturnsTrueForARecordFieldArrayProperty()
            {
                var arrayProperty = typeof(TestDBCRecordWithArray).GetProperties()
                    .First(p => p.Name == nameof(TestDBCRecordWithArray.ArrayField));

                Assert.True(DBCInspector.IsPropertyFieldArray(arrayProperty));
            }

            [Test]
            public void ReturnsFalseForAnUnmarkedProperty()
            {
                var arrayProperty = typeof(TestDBCRecord).GetProperties()
                    .First(p => p.Name == nameof(TestDBCRecord.TestNotRecordField));

                Assert.False(DBCInspector.IsPropertyFieldArray(arrayProperty));
            }
        }
    }
}
