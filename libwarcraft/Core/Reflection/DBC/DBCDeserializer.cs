//
//  DBCDeserializer.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Warcraft.Core.Extensions;
using Warcraft.Core.Structures;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.Core.Reflection.DBC
{
    /// <summary>
    /// Deserialization class for DBC records. Contains a lot of reflection-based code for inspecting the record type
    /// definitions in order to determine their layouts, sizes, and formats.
    /// </summary>
    public static class DBCDeserializer
    {
        /// <summary>
        /// Deserializes the values of a DBC record.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> containing the data of the record.</param>
        /// <param name="record">An instance of the record class.</param>
        /// <param name="version">The version of the record to deserialize.</param>
        /// <typeparam name="T">The type of record to deserialize.</typeparam>
        /// <exception cref="ArgumentException">Thrown if the record contains field properties without setters.</exception>
        public static void DeserializeRecord<T>(BinaryReader reader, T record, WarcraftVersion version)
        {
            var reflectionInfo = RecordInformationCache.Instance.GetRecordInformation(typeof(T), version);

            foreach (var databaseProperty in reflectionInfo.VersionRelevantProperties)
            {
                object propertyValue;
                if (reflectionInfo.IsPropertyFieldArray(databaseProperty))
                {
                    var elementType = reflectionInfo.PropertyFieldArrayElementTypes[databaseProperty];
                    var arrayAttribute = reflectionInfo.PropertyFieldArrayAttributes[databaseProperty];

                    List<object> values = new List<object>();
                    for (int i = 0; i < arrayAttribute.Count; ++i)
                    {
                        values.Add(ReadPropertyValue(reader, reflectionInfo, databaseProperty, elementType, version));
                    }

                    propertyValue = GetAssignableCollectionObject(values, databaseProperty.PropertyType, elementType);
                }
                else
                {
                    propertyValue = ReadPropertyValue(reader, reflectionInfo, databaseProperty, databaseProperty.PropertyType, version);
                }

                databaseProperty.SetValue(record, propertyValue);
            }
        }

        /// <summary>
        /// Converts a generic list of objects to something that can be assigned to the given property with the given
        /// element type.
        /// </summary>
        /// <param name="values">The values to convert.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="elementType">The type of the elements.</param>
        /// <typeparam name="T">The current type of the values.</typeparam>
        /// <returns>An object which can be assigned to a property of type <paramref name="propertyType"/>.</returns>
        /// <exception cref="ArgumentException">Thrown if no compatible object could be created.</exception>
        public static object GetAssignableCollectionObject<T>(List<T> values, Type propertyType, Type elementType)
        {
            if (propertyType.IsArray)
            {
                var valueArray = values.ToArray();
                var outputArray = Array.CreateInstance(elementType, values.Count);

                Array.Copy(valueArray, outputArray, valueArray.Length);

                return outputArray;
            }

            // Create type info for a list of the property's type
            Type specificListType = typeof(List<>).MakeGenericType(elementType);

            if (propertyType == specificListType)
            {
                IList list = (IList)Activator.CreateInstance(propertyType);
                foreach (var value in values)
                {
                    list.Add(value);
                }

                return list;
            }

            throw new ArgumentException($"No compatible object could be created for a property of type {propertyType}", nameof(propertyType));
        }

        /// <summary>
        /// Reads a property value from the given <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="reader">The reader, containing the data.</param>
        /// <param name="property">The property which will contain the data.</param>
        /// <param name="elementType">The element type of the field. This is primarily used for reading arrays.</param>
        /// <param name="version">The version of the record.</param>
        /// <returns>The value that should be assigned to the property.</returns>
        public static object ReadPropertyValue(BinaryReader reader, RecordFieldInformation recordInfo, PropertyInfo property, Type elementType, WarcraftVersion version)
        {
            object fieldValue;
            if (DBCInspector.IsPropertyForeignKey(property))
            {
                // Get the foreign key information
                var foreignKeyAttribute = recordInfo.PropertyForeignKeyAttributes[property];

                // Get the inner type
                var keyType = GetUnderlyingStoredPrimitiveType(elementType);
                var keyValue = reader.Read(keyType);

                // Create the specific ForeignKey type
                var genericKeyFieldType = typeof(ForeignKey<>);
                var specificKeyFieldType = genericKeyFieldType.MakeGenericType(keyType);

                fieldValue = Activator.CreateInstance
                (
                    specificKeyFieldType,
                    foreignKeyAttribute.Database,
                    foreignKeyAttribute.Field,
                    keyValue
                );
            }
            else if (elementType.IsEnum)
            {
                // Get the underlying type of the enum
                var enumType = elementType.GetEnumUnderlyingType();
                fieldValue = reader.Read(enumType);
            }
            else
            {
                if (elementType == typeof(LocalizedStringReference))
                {
                    fieldValue = reader.Read(elementType, version);
                }
                else
                {
                    fieldValue = reader.Read(elementType);
                }
            }

            return fieldValue;
        }

        /// <summary>
        /// Determines if the given property type is compatible with a <see cref="RecordFieldArrayAttribute"/>. At
        /// present, this is limited to array types and types implementing <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="propertyType">The type to check.</param>
        /// <returns>true if the property is compatible; otherwise, false.</returns>
        public static bool IsPropertyTypeCompatibleWithArrayAttribute(Type propertyType)
        {
            bool isArray = propertyType.IsArray;
            bool implementsGenericIList = propertyType.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));

            return isArray || implementsGenericIList;
        }

        /// <summary>
        /// Gets the primitive type of the value that the specified type is stored as.
        /// </summary>
        /// <param name="wrappingType">The type that wraps a primitive type.</param>
        /// <returns>A primitive type.</returns>
        public static Type GetUnderlyingStoredPrimitiveType(Type wrappingType)
        {
            if (wrappingType.IsPrimitive)
            {
                return wrappingType;
            }

            Type innerType = null;
            if (wrappingType.IsGenericType)
            {
                // Something implementing IList<T>, or a ForeignKey
                innerType = wrappingType.GetGenericArguments().FirstOrDefault();
            }

            if (wrappingType.IsArray)
            {
                // Whatever the array element type is
                innerType = wrappingType.GetElementType();
            }

            if (wrappingType.IsEnum)
            {
                innerType = wrappingType.GetEnumUnderlyingType();
            }

            if (wrappingType == typeof(StringReference) || wrappingType == typeof(LocalizedStringReference))
            {
                return typeof(uint);
            }

            return GetUnderlyingStoredPrimitiveType(innerType);
        }
    }
}
