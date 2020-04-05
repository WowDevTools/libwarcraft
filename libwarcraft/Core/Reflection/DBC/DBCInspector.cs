//
//  DBCInspector.cs
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using Warcraft.Core.Structures;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.Core.Reflection.DBC
{
    /// <summary>
    /// Holds functions for inspecting DBC records at runtime to deduce layout, types, storage size requirements and
    /// field counts.
    /// </summary>
    public static class DBCInspector
    {
        /// <summary>
        /// Contains field count information about custom registered record field types, allowing for field types like
        /// Box, Vector3, etc.
        /// </summary>
        private static readonly Dictionary<Type, int> CustomFieldTypeFieldCounts = new Dictionary<Type, int>
        {
            { typeof(Vector2), 2 },
            { typeof(Vector3), 3 },
            { typeof(Vector4), 4 },
            { typeof(Box), 6 },
            { typeof(BGRA), 1 },
            { typeof(ARGB), 1 }
        };

        /// <summary>
        /// Contains byte size information about custom registered record field types, allowing for field types like
        /// Box, Vector3, etc. There's no need to register a struct type here, unless the marshaller is reporting an
        /// incorrect value.
        /// </summary>
        private static readonly Dictionary<Type, int> CustomFieldTypeStorageSizes = new Dictionary<Type, int>();

        /// <summary>
        /// Register a custom type with the inspector, such that it properly recognizes it and can use it to determine
        /// the layout of records. If the type is not a marshallable struct, then a storage size must be supplied.
        /// </summary>
        /// <param name="type">The type of the field.</param>
        /// <param name="fieldCount">The element count of the field.</param>
        /// <param name="storageSize">The absolute storage size of the type.</param>
        public static void RegisterFieldType(Type type, int fieldCount, int? storageSize = null)
        {
            if (!type.IsValueType && !storageSize.HasValue)
            {
                throw new ArgumentException("A storage size must be specified for types that are not value types.", nameof(type));
            }

            if (fieldCount <= 0)
            {
                throw new ArgumentException("The type must have at least one field.", nameof(fieldCount));
            }

            if (storageSize <= 0)
            {
                throw new ArgumentException("The type must be at least one byte.", nameof(storageSize));
            }

            // Register the type
            CustomFieldTypeFieldCounts.Add(type, fieldCount);

            if (storageSize.HasValue)
            {
                CustomFieldTypeStorageSizes.Add(type, fieldCount);
            }
        }

        /// <summary>
        /// Gets the underlying element type of a field array property.
        /// </summary>
        /// <param name="propertyType">The type to get the underlying type of.</param>
        /// <returns>The underlying type.</returns>
        /// <exception cref="ArgumentException">Thrown if no underlying type could be deduced.</exception>
        public static Type GetFieldArrayPropertyElementType(Type propertyType)
        {
            if (propertyType.IsArray)
            {
                return propertyType.GetElementType()!;
            }

            if (propertyType.IsGenericType)
            {
                return propertyType.GetGenericArguments().First();
            }

            throw new ArgumentException($"No inner type could be deduced for a property of type {propertyType}", nameof(propertyType));
        }

        /// <summary>
        /// Determines whether or not the given property is a field array. This is done by checking for an instance of
        /// <see cref="RecordFieldArrayAttribute"/>.
        /// </summary>
        /// <param name="propertyInfo">The property to check.</param>
        /// <returns>true if the property is a field array; otherwise, false.</returns>
        public static bool IsPropertyFieldArray(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes().Any(p => p is RecordFieldArrayAttribute);
        }

        /// <summary>
        /// Gets the relevant <see cref="RecordFieldArrayAttribute"/> that the given property is decorated with.
        /// </summary>
        /// <param name="version">The version that the attribute should be relevant for.</param>
        /// <param name="propertyInfo">The property to check.</param>
        /// <returns>A field array attribute.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the property is not an array type.
        /// Thrown if the property does not have a field array attribute.
        /// </exception>
        public static RecordFieldArrayAttribute GetVersionRelevantPropertyFieldArrayAttribute(WarcraftVersion version, PropertyInfo propertyInfo) // TODO: Write tests
        {
            if (!IsPropertyFieldArray(propertyInfo))
            {
                throw new ArgumentException("The property is not an array. Use GetPropertyFieldAttribute instead, or decorate it with RecordFieldArray.", nameof(propertyInfo));
            }

            var attributes = propertyInfo
                .GetCustomAttributes()
                .Where(p => p is RecordFieldArrayAttribute)
                .Cast<RecordFieldArrayAttribute>()
                .OrderBy(a => a.IntroducedIn);

            return attributes.Last(a => IsPropertyRelevantForVersion(version, a));
        }

        /// <summary>
        /// Gets the <see cref="RecordFieldAttribute"/> that the given property is decorated with.
        /// </summary>
        /// <param name="propertyInfo">The property to check.</param>
        /// <returns>A field attribute.</returns>
        /// <exception cref="InvalidDataException">Thrown if the property does not have a field attribute.</exception>
        public static RecordFieldAttribute GetPropertyFieldAttribute(PropertyInfo propertyInfo) // TODO: Write tests
        {
            var fieldAttribute = propertyInfo.GetCustomAttributes().FirstOrDefault(a => a is RecordFieldAttribute) as RecordFieldAttribute;

            if (fieldAttribute == null)
            {
                throw new ArgumentException("The property did not have a RecordField attribute attached to it.");
            }

            return fieldAttribute;
        }

        /// <summary>
        /// Gets all properties decorated with <see cref="RecordFieldAttribute"/> from the given type.
        /// </summary>
        /// <param name="recordType">The type to get the properties from.</param>
        /// <returns>An unordered set of properties.</returns>
        /// <exception cref="IncompatibleRecordArrayTypeException">
        /// Thrown if a property which does not support a <see cref="RecordFieldArrayAttribute"/> is decorated with one.
        /// </exception>
        /// <exception cref="InvalidFieldAttributeException">
        /// Thrown if a property is decorated with an invalid <see cref="RecordFieldAttribute"/>.
        /// </exception>
        public static IEnumerable<PropertyInfo> GetRecordProperties(Type recordType)
        {
            var recordProperties = recordType.GetProperties
                (
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.FlattenHierarchy
                )
                .Where(p => p.IsDefined(typeof(RecordFieldAttribute)))
                .ToList();

            // Do a bit of error checking
            foreach (var property in recordProperties)
            {
                if (property.GetCustomAttributes().Any(a => a is RecordFieldArrayAttribute))
                {
                    if (!DBCDeserializer.IsPropertyTypeCompatibleWithArrayAttribute(property.PropertyType))
                    {
                        throw new IncompatibleRecordArrayTypeException
                        (
                            "Incompatible property definition decorated with RecordFieldArray. Use an array or something that implements IList<T>.", property.PropertyType
                        );
                    }
                }

                var versionAttribute = GetPropertyFieldAttribute(property);
                if ((versionAttribute.RemovedIn < versionAttribute.IntroducedIn) && versionAttribute.RemovedIn != WarcraftVersion.Unknown)
                {
                    throw new InvalidFieldAttributeException("The field was marked as having been removed before it was introduced.");
                }
            }

            return recordProperties;
        }

        /// <summary>
        /// Gets the record field properties that are relevant for the given version, that is, those that exist in the
        /// given version. No order is guaranteed.
        /// </summary>
        /// <param name="version">The version which the property should be relevant for.</param>
        /// <param name="recordType">The type where the properties are.</param>
        /// <returns>An ordered set of properties.</returns>
        public static IEnumerable<PropertyInfo> GetVersionRelevantProperties(WarcraftVersion version, Type recordType)
        {
            foreach (var recordProperty in GetRecordProperties(recordType))
            {
                RecordFieldAttribute versionAttribute;
                if (IsPropertyFieldArray(recordProperty))
                {
                    versionAttribute = GetVersionRelevantPropertyFieldArrayAttribute(version, recordProperty);

                    if (versionAttribute == null)
                    {
                        // There was no property defined for the version.
                        continue;
                    }
                }
                else
                {
                    versionAttribute = GetPropertyFieldAttribute(recordProperty);
                }

                if (!IsPropertyRelevantForVersion(version, versionAttribute))
                {
                    continue;
                }

                yield return recordProperty;
            }
        }

        /// <summary>
        /// Determines whether or not a property is relevant for the given version, based on its <see cref="RecordFieldAttribute"/>.
        /// </summary>
        /// <param name="version">The version to check against.</param>
        /// <param name="versionAttribute">The attribute containing version information.</param>
        /// <returns>true if the property is relevant; otherwise, false.</returns>
        public static bool IsPropertyRelevantForVersion(WarcraftVersion version, RecordFieldAttribute versionAttribute)
        {
            // Field is not present in this version
            if (versionAttribute.IntroducedIn > version)
            {
                return false;
            }

            // Field has been removed in this or a previous version
            if (versionAttribute.RemovedIn <= version && versionAttribute.RemovedIn != WarcraftVersion.Unknown)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether or not the given property has moved in the specified version. If the property moved in
        /// a previous version, it is also considered as having moved in the current version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="property">The property.</param>
        /// <returns>true if the property has moved; otherwise, false.</returns>
        public static bool HasPropertyMovedInVersion(WarcraftVersion version, PropertyInfo property)
        {
            var orderAttribute = property.GetCustomAttributes().FirstOrDefault(a => a is RecordFieldOrderAttribute) as RecordFieldOrderAttribute;

            if (orderAttribute == null)
            {
                return false;
            }

            if (orderAttribute.MovedIn > version)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the properties that have moved in the given version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="properties">The properties.</param>
        /// <returns>The properties that have moved.</returns>
        public static Dictionary<PropertyInfo, RecordFieldOrderAttribute> GetMovedProperties(WarcraftVersion version, IEnumerable<PropertyInfo> properties)
        {
            var movingProperties = new Dictionary<PropertyInfo, RecordFieldOrderAttribute>();

            foreach (var property in properties)
            {
                if (!HasPropertyMovedInVersion(version, property))
                {
                    continue;
                }

                var orderAttribute = GetMostRecentPropertyMove(version, property);
                movingProperties.Add(property, orderAttribute);
            }

            return movingProperties;
        }

        /// <summary>
        /// Gets the most recent field order property, relative to the given version.
        /// </summary>
        /// <param name="version">The most recent version allowed.</param>
        /// <param name="property">The property to get the attribute from.</param>
        /// <returns>A field order attribute.</returns>
        /// <exception cref="ArgumentException">Thrown if the property has not moved.</exception>
        public static RecordFieldOrderAttribute GetMostRecentPropertyMove(WarcraftVersion version, PropertyInfo property)
        {
            if (!HasPropertyMovedInVersion(version, property))
            {
                throw new ArgumentException("The property has not moved.", nameof(property));
            }

            // Grab all of the order properties
            var lastAttribute = property
                .GetCustomAttributes()
                .Where(a => a is RecordFieldOrderAttribute)
                .Cast<RecordFieldOrderAttribute>()
                .OrderBy(a => a.MovedIn)
                .Last(a => a.MovedIn <= version);

            return lastAttribute;
        }

        /// <summary>
        /// Determines whether or not the given property is a foreign key.
        /// </summary>
        /// <param name="propertyInfo">The property to check.</param>
        /// <returns>true if the property is a foreign key; otherwise, false.</returns>
        public static bool IsPropertyForeignKey(PropertyInfo propertyInfo)
        {
            return
                propertyInfo.PropertyType.IsGenericType &&
                propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(ForeignKey<>);
        }

        /// <summary>
        /// Gets the <see cref="ForeignKeyInfoAttribute"/> attached to the given property.
        /// </summary>
        /// <param name="foreignKey">The foreign key property.</param>
        /// <returns>The foreign key info.</returns>
        /// <exception cref="ArgumentException">Thrown if the property is not a foreign key.</exception>
        /// <exception cref="InvalidDataException">Thrown if the property is not decorated with a <see cref="ForeignKeyInfoAttribute"/>.</exception>
        public static ForeignKeyInfoAttribute GetForeignKeyInfo(PropertyInfo foreignKey)
        {
            if (!IsPropertyForeignKey(foreignKey))
            {
                throw new ArgumentException("The given property was not a foreign key.", nameof(foreignKey));
            }

            var foreignKeyAttribute = foreignKey.GetCustomAttributes().FirstOrDefault(a => a is ForeignKeyInfoAttribute) as ForeignKeyInfoAttribute;

            if (foreignKeyAttribute == null)
            {
                throw new InvalidDataException("ForeignKey properties must be decorated with the ForeignKeyInfo attribute.");
            }

            return foreignKeyAttribute;
        }

        /// <summary>
        /// Gets the absolute size in bytes of the given record type.
        /// </summary>
        /// <param name="version">The version of the record.</param>
        /// <param name="recordType">The type to get the size of.</param>
        /// <returns>The absolute size in bytes of the record.</returns>
        public static int GetRecordSize(WarcraftVersion version, Type recordType)
        {
            var size = 0;
            foreach (var recordProperty in GetVersionRelevantProperties(version, recordType))
            {
                switch (recordProperty.PropertyType)
                {
                    // Single-field types
                    case Type foreignKeyType when foreignKeyType.IsGenericType && foreignKeyType.GetGenericTypeDefinition() == typeof(ForeignKey<>):
                    case Type stringRefType when stringRefType == typeof(StringReference):
                    case Type enumType when enumType.IsEnum:
                    {
                        var underlyingType = DBCDeserializer.GetUnderlyingStoredPrimitiveType(recordProperty.PropertyType);

                        size += Marshal.SizeOf(underlyingType);
                        break;
                    }

                    // Multi-field types
                    case Type genericListType when genericListType.IsGenericType && genericListType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>)):
                    case Type arrayType when arrayType.IsArray:
                    {
                        var elementSize = Marshal.SizeOf(DBCDeserializer.GetUnderlyingStoredPrimitiveType(recordProperty.PropertyType));
                        var arrayInfoAttribute = GetVersionRelevantPropertyFieldArrayAttribute(version, recordProperty);

                        size += (int)(elementSize * arrayInfoAttribute.Count);

                        break;
                    }

                    // Special version-variant length handling
                    case Type locStringRefType when locStringRefType == typeof(LocalizedStringReference):
                    {
                        size += LocalizedStringReference.GetFieldCount(version) * sizeof(uint);
                        break;
                    }

                    case Type registeredType when CustomFieldTypeStorageSizes.ContainsKey(registeredType):
                    {
                        size += CustomFieldTypeStorageSizes[registeredType];
                        break;
                    }

                    default:
                    {
                        size += Marshal.SizeOf(recordProperty.PropertyType);
                        break;
                    }
                }
            }

            return size;
        }

        /// <summary>
        /// Gets the number of properties marked with <see cref="RecordFieldAttribute"/> (or subclasses of it) in the
        /// given type.
        /// </summary>
        /// <param name="version">The version of the record.</param>
        /// <param name="recordType">The type with properties.</param>
        /// <returns>The number of properties in the type.</returns>
        public static int GetPropertyCount(WarcraftVersion version, Type recordType)
        {
            var count = 0;
            var properties = GetVersionRelevantProperties(version, recordType);
            foreach (var recordProperty in properties)
            {
                switch (recordProperty.PropertyType)
                {
                    case Type _ when IsPropertyFieldArray(recordProperty):
                    {
                        var arrayInfoAttribute = GetVersionRelevantPropertyFieldArrayAttribute(version, recordProperty);
                        count += (int)arrayInfoAttribute.Count;

                        break;
                    }

                    case Type locStringRefType when locStringRefType == typeof(LocalizedStringReference):
                    {
                        count += LocalizedStringReference.GetFieldCount(version);
                        break;
                    }

                    case Type registeredType when CustomFieldTypeFieldCounts.ContainsKey(registeredType):
                    {
                        count += CustomFieldTypeFieldCounts[registeredType];
                        break;
                    }

                    default:
                    {
                        ++count;
                        break;
                    }
                }
            }

            return count;
        }
    }
}
