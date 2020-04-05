//
//  RecordDeserializationTests.cs
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
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.Unit.Reflection.DBC.TestData;
using static Warcraft.Unit.Reflection.DBC.TestData.RecordValues;

#pragma warning disable 1591, SA1600

namespace Warcraft.Integration.DBC.IO
{
    [TestFixture]
    public class RecordDeserializationTests
    {
        public class Enumeration
        {
            [Test]
            public void CanEnumerateDatabase()
            {
                var databaseName = DBCTestHelper.GetDatabaseNameFromRecordType(typeof(AnimationDataRecord));
                if (!DBCTestHelper.HasDatabaseFile(WarcraftVersion.Classic, databaseName))
                {
                    Assert.Ignore("Database file not present. Skipping.");
                }

                var database = DBCTestHelper.LoadDatabase<AnimationDataRecord>(WarcraftVersion.Classic, databaseName);

                try
                {
                    foreach (var record in database)
                    {
                        Assert.That(record, Is.Not.Null);
                    }
                }
                catch (Exception)
                {
                    Assert.Fail("Exception thrown during full enumeration.");
                }
            }
        }

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
