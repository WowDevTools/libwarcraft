using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using libwarcraft.Tests.Unit.Reflection.DBC.TestData;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.Definitions;
using static libwarcraft.Tests.Unit.Reflection.DBC.TestData.FieldNameLists;

namespace libwarcraft.Tests.Unit.Reflection.DBC
{
    [TestFixture]
    public class FieldOrdererTests
    {
        public class GetPrecedenceChain
        {
            [Test]
            public void GetsCorrectChainForSimpleMove()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithSingleMovedField))
                        .ToList()
                );

                var movedProperty = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithSingleMovedField))
                    .First(p => p.Name == nameof(TestDBCRecordWithSingleMovedField.FieldC));

                var moveAttribute = DBCInspector.GetMostRecentPropertyMove(WarcraftVersion.BurningCrusade, movedProperty);

                var chain = orderer.GetPrecendenceChain(moveAttribute, new List<PropertyInfo>()).Select(p => p.Name);

                Assert.That(SimpleMoveDependencyChainFieldC, Is.EquivalentTo(chain));
            }

            [Test]
            public void GetsCorrectChainFor2TierMove()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithMultipleMovedFields))
                        .ToList()
                );

                var movedProperty = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithMultipleMovedFields.FieldA));

                var moveAttribute = DBCInspector.GetMostRecentPropertyMove(WarcraftVersion.BurningCrusade, movedProperty);

                var chain = orderer.GetPrecendenceChain(moveAttribute, new List<PropertyInfo>()).Select(p => p.Name);

                Assert.That(MultiMoveBCDependencyChainFieldA, Is.EquivalentTo(chain));
            }

            [Test]
            public void GetsCorrectChainFor3TierMove()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithMultipleMovedFields))
                        .ToList()
                );

                var movedProperty = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithMultipleMovedFields.FieldE));

                var moveAttribute = DBCInspector.GetMostRecentPropertyMove(WarcraftVersion.BurningCrusade, movedProperty);

                var chain = orderer.GetPrecendenceChain(moveAttribute, new List<PropertyInfo>()).Select(p => p.Name);

                Assert.That(MultiMoveBCDependencyChainFieldE, Is.EquivalentTo(chain));
            }

            [Test]
            public void GetsCorrectChainForMultiVersionMove()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.Wrath,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.Wrath, typeof(TestDBCRecordWithMultipleMovedFields))
                        .ToList()
                );

                var movedProperty = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithMultipleMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithMultipleMovedFields.FieldD));

                var moveAttribute = DBCInspector.GetMostRecentPropertyMove(WarcraftVersion.Wrath, movedProperty);

                var chain = orderer.GetPrecendenceChain(moveAttribute, new List<PropertyInfo>()).Select(p => p.Name);

                Assert.That(MultiVersionDependencyChainFieldE, Is.EquivalentTo(chain));
            }
        }

        public class HasCyclicMoveDependency
        {
            [Test]
            public void ReturnsTrueForCyclicDependency()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithInvalidReentrantMovedFields))
                        .ToList()
                );

                var propertyA = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithInvalidReentrantMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithInvalidReentrantMovedFields.FieldA));

                var propertyC = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithInvalidReentrantMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithInvalidReentrantMovedFields.FieldC));

                Assert.IsTrue(orderer.HasCyclicMoveDependency(propertyA));
                Assert.IsTrue(orderer.HasCyclicMoveDependency(propertyC));
            }

            [Test]
            public void ReturnsFalseForLinearDependency()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithInvalidReentrantMovedFields))
                        .ToList()
                );

                var propertyD = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithInvalidReentrantMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithInvalidReentrantMovedFields.FieldD));

                Assert.IsFalse(orderer.HasCyclicMoveDependency(propertyD));
            }

            [Test]
            public void ReturnsFalseForNonMoved()
            {
                var orderer = new FieldOrderer
                (
                    WarcraftVersion.BurningCrusade,
                    DBCInspector.GetVersionRelevantProperties(WarcraftVersion.BurningCrusade, typeof(TestDBCRecordWithInvalidReentrantMovedFields))
                        .ToList()
                );

                var propertyB = DBCInspector.GetRecordProperties(typeof(TestDBCRecordWithInvalidReentrantMovedFields))
                    .First(p => p.Name == nameof(TestDBCRecordWithInvalidReentrantMovedFields.FieldB));

                Assert.IsFalse(orderer.HasCyclicMoveDependency(propertyB));
            }
        }
    }
}
