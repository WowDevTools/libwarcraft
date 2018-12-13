using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace libwarcraft.Tests.Unit.Reflection.DBC
{
    [TestFixture]
    public class DBCDeserializerTests
    {
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
