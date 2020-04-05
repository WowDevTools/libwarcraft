//
//  FieldOrdererTests.cs
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
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.Unit.Reflection.DBC.TestData;
using static Warcraft.Unit.Reflection.DBC.TestData.FieldNameLists;

#pragma warning disable 1591, SA1600

namespace Warcraft.Integration.DBC
{
    [TestFixture]
    public class FieldOrdererTests
    {
        public class Constructor
        {
            [Test]
            public void AssignsVersion()
            {
                var orderer = new FieldOrderer(WarcraftVersion.Classic, new PropertyInfo[] { });

                Assert.AreEqual(WarcraftVersion.Classic, orderer.Version);
            }

            [Test]
            public void AssignsOriginalProperties()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.Classic,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.Classic, typeof(TestDBCRecordWithSingleMovedField))
                    .ToList()
                );

                var assignedPropertyNames = orderer.OriginalProperties.Select(p => p.Name);

                Assert.That(assignedPropertyNames, Is.EquivalentTo(SingleMovedFieldNamesBeforeMove));
            }

            [Test]
            public void AssignsMovingProperties()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithSingleMovedField))
                        .ToList()
                );

                var assignedPropertyNames = orderer.MovingProperties.Select(p => p.Key.Name);

                Assert.That(assignedPropertyNames, Is.EquivalentTo(SingleMovedFieldRecordNamesMovingFields));
            }
        }

        public class BuildPrecedenceChains
        {
            [Test]
            public void AssignsCorrectChainsSimpleVersion()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithMultipleMovedFields))
                        .ToList()
                );

                var propertyA = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithMultipleMovedFields.FieldA));

                var propertyC = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithMultipleMovedFields.FieldC));

                var propertyE = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithMultipleMovedFields.FieldE));

                Assert.That(orderer.PrecedenceChains[propertyA].Select(p => p.Name), Is.EquivalentTo(MultiMoveBCDependencyChainFieldA));
                Assert.That(orderer.PrecedenceChains[propertyC].Select(p => p.Name), Is.EquivalentTo(MultiMoveBCDependencyChainFieldC));
                Assert.That(orderer.PrecedenceChains[propertyE].Select(p => p.Name), Is.EquivalentTo(MultiMoveBCDependencyChainFieldE));
            }

            [Test]
            public void AssignsCorrectChainsMultiVersion()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.Wrath,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.Wrath, typeof(TestDBCRecordWithMultipleMovedFields))
                        .ToList()
                );

                var propertyA = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithMultipleMovedFields.FieldA));

                var propertyC = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithMultipleMovedFields.FieldC));

                var propertyD = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithMultipleMovedFields.FieldD));

                var propertyE = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithMultipleMovedFields.FieldE));

                Assert.That(orderer.PrecedenceChains[propertyA].Select(p => p.Name), Is.EquivalentTo(MultiMoveBCDependencyChainFieldA));
                Assert.That(orderer.PrecedenceChains[propertyC].Select(p => p.Name), Is.EquivalentTo(MultiMoveBCDependencyChainFieldC));
                Assert.That(orderer.PrecedenceChains[propertyD].Select(p => p.Name), Is.EquivalentTo(MultiMoveWrathDependencyChainFieldD));
                Assert.That(orderer.PrecedenceChains[propertyE].Select(p => p.Name), Is.EquivalentTo(MultiMoveBCDependencyChainFieldE));
            }
        }

        public class ReorderProperties
        {
            [Test]
            public void ReturnsCorrectOrderForSimpleMove()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithSingleMovedField))
                        .ToList()
                );

                Assert.That(orderer.ReorderProperties().Select(p => p.Name), Is.EquivalentTo(SingleMovedFieldBCAfterMove));
            }

            [Test]
            public void ReturnsCorrectOrderForMultiMove()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithMultipleMovedFields))
                        .ToList()
                );

                Assert.That(orderer.ReorderProperties().Select(p => p.Name), Is.EquivalentTo(MultiMovedFieldsBCAfterMove));
            }

            [Test]
            public void ReturnsCorrectOrderForMultiVersionMove()
            {
                var ordererBC = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithSingleMovedFieldMultipleVersions))
                        .ToList()
                );

                var ordererWrath = new FieldOrderer
                (
                    WarcraftVersion.Wrath,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.Wrath, typeof(TestDBCRecordWithSingleMovedFieldMultipleVersions))
                        .ToList()
                );

                Assert.That(ordererBC.ReorderProperties().Select(p => p.Name), Is.EquivalentTo(SingleMovedFieldBCAfterMove));
                Assert.That(ordererWrath.ReorderProperties().Select(p => p.Name), Is.EquivalentTo(SingleMovedFieldWrathAfterMove));
            }

            [Test]
            public void ReturnsCorrectOrderForMultiMoveMultiVersion()
            {
                var ordererBC = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithMultipleMovedFields))
                        .ToList()
                );

                var ordererWrath = new FieldOrderer
                (
                    WarcraftVersion.Wrath,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.Wrath, typeof(TestDBCRecordWithMultipleMovedFields))
                        .ToList()
                );

                Assert.That(ordererBC.ReorderProperties().Select(p => p.Name), Is.EquivalentTo(MultiMovedFieldsBCAfterMove));
                Assert.That(ordererWrath.ReorderProperties().Select(p => p.Name), Is.EquivalentTo(MultiMovedFieldsWrathAfterMove));
            }

            [Test]
            public void ThrowsWhenThereAreCyclicalDependencies()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithInvalidReentrantMovedFields))
                        .ToList()
                );

                Assert.Throws<InvalidOperationException>(() => orderer.ReorderProperties());
            }

            [Test]
            public void DoesNotThrowWhenThereAreNoCyclicalDependencies()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithMultipleMovedFields))
                        .ToList()
                );

                Assert.DoesNotThrow(() => orderer.ReorderProperties());
            }
        }
    }
}
