//
//  DBCDeserializerTests.cs
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

using System.Collections.Generic;
using NUnit.Framework;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

#pragma warning disable 1591, SA1600, SA1649, SA1402

namespace Warcraft.Unit.Reflection.DBC
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
