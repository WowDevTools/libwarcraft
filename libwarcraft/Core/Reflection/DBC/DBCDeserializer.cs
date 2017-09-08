//
//  DBCDeserializer.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Warcraft.Core.Extensions;
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
			var databaseProperties = GetVersionRelevantProperties(version, typeof(T));
			foreach (var databaseProperty in databaseProperties)
			{
				if (!databaseProperty.CanWrite)
				{
					throw new ArgumentException("Property setter not found. Record properties must have a setter.");
				}

				object propertyValue;
				if (IsPropertyFieldArray(databaseProperty))
				{
					var elementType = GetFieldArrayPropertyElementType(databaseProperty.PropertyType);
					var arrayAttribute = GetPropertyFieldArrayAttribute(databaseProperty);

					List<object> values = new List<object>();
					for (int i = 0; i < arrayAttribute.Count; ++i)
					{
						values.Add(ReadPropertyValue(reader, databaseProperty, elementType, version));
					}

					propertyValue = GetAssignableCollectionObject(values, databaseProperty.PropertyType, elementType);
				}
				else
				{
					propertyValue = ReadPropertyValue(reader, databaseProperty, databaseProperty.PropertyType, version);
				}

				databaseProperty.SetValue(record, propertyValue);
			}
		}

		/// <summary>
		/// Gets the underlying element type of a field array property.
		/// </summary>
		/// <param name="propertyType">The type to get the underlying type of</param>
		/// <returns>The underlying type.</returns>
		/// <exception cref="ArgumentException">Thrown if no underlying type could be deduced.</exception>
		public static Type GetFieldArrayPropertyElementType(Type propertyType)
		{
			if (propertyType.IsArray)
			{
				return propertyType.GetElementType();
			}

			if (propertyType.IsGenericType)
			{
				return propertyType.GetGenericArguments().First();
			}

			throw new ArgumentException($"No inner type could be deduced for a property of type {propertyType}", nameof(propertyType));
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
		public static object ReadPropertyValue(BinaryReader reader, PropertyInfo property, Type elementType, WarcraftVersion version)
		{
			object fieldValue;
			if (IsPropertyForeignKey(property))
			{
				// Get the foreign key information
				var foreignKeyAttribute = GetForeignKeyInfo(property);

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
		/// Gets the <see cref="RecordFieldArrayAttribute"/> that the given property is decorated with.
		/// </summary>
		/// <param name="propertyInfo">The property to check.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if the property is not an array type.
		/// Thrown if the property does not have a field array attribute.
		/// </exception>
		public static RecordFieldArrayAttribute GetPropertyFieldArrayAttribute(PropertyInfo propertyInfo) // TODO: Write tests
		{
			if (!IsPropertyFieldArray(propertyInfo))
			{
				throw new ArgumentException("The property is not an array. Use GetPropertyFieldAttribute instead, or decorate it with RecordFieldArray.", nameof(propertyInfo));
			}

			var arrayAttribute = propertyInfo.GetCustomAttributes().First(p => p is RecordFieldArrayAttribute) as RecordFieldArrayAttribute;

			return arrayAttribute;
		}

		/// <summary>
		/// Gets the <see cref="RecordFieldAttribute"/> that the given property is decorated with.
		/// </summary>
		/// <param name="propertyInfo">The property to check.</param>
		/// <returns></returns>
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
					if (!IsPropertyTypeCompatibleWithArrayAttribute(property.PropertyType))
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
		/// Gets the record field properties that are relevant for the given version, that is, those that exist in the
		/// given version. The properties are guaranteed to be ordered by inheritance and declaration order.
		/// </summary>
		/// <param name="version">The version which the property should be relevant for.</param>
		/// <param name="recordType">The type where the properties are.</param>
		/// <returns>An ordered set of properties.</returns>
		public static IEnumerable<PropertyInfo> GetVersionRelevantProperties(WarcraftVersion version, Type recordType)
		{
			// Order the properties by their field order.
			var orderedProperties = GetRecordProperties(recordType)
				.OrderBy(p => p.DeclaringType, new InheritanceChainComparer())
				.ThenBy(p => p.MetadataToken).ToList();

			foreach (var recordProperty in orderedProperties)
			{
				var versionAttribute = GetPropertyFieldAttribute(recordProperty);

				// Field is not present in the version we're reading, skip it
				if (versionAttribute.IntroducedIn > version)
				{
					continue;
				}

				// Field has been removed in the version we're reading, skip it
				if (versionAttribute.RemovedIn <= version && versionAttribute.RemovedIn != WarcraftVersion.Unknown)
				{
					continue;
				}

				yield return recordProperty;
			}
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
		/// <returns></returns>
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
			int size = 0;
			foreach (var recordProperty in GetVersionRelevantProperties(version, recordType))
			{
				switch (recordProperty.PropertyType)
				{
					// Single-field types
					case Type foreignKeyType when foreignKeyType.IsGenericType && foreignKeyType.GetGenericTypeDefinition() == typeof(ForeignKey<>):
					case Type stringRefType when stringRefType == typeof(StringReference):
					case Type enumType when enumType.IsEnum:
					{
						var underlyingType = GetUnderlyingStoredPrimitiveType(recordProperty.PropertyType);

						size += Marshal.SizeOf(underlyingType);
						break;
					}
					// Multi-field types
					case Type genericListType when genericListType.IsGenericType && genericListType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>)):
					case Type arrayType when arrayType.IsArray:
					{
						var elementSize = Marshal.SizeOf(GetUnderlyingStoredPrimitiveType(recordProperty.PropertyType));
						var arrayInfoAttribute = GetPropertyFieldArrayAttribute(recordProperty);

						size += (int)(elementSize * arrayInfoAttribute.Count);

						break;
					}
					// Special version-variant length handling
					case Type locStringRefType when locStringRefType == typeof(LocalizedStringReference):
					{
						size += LocalizedStringReference.GetFieldCount(version) * sizeof(uint);
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

		/// <summary>
		/// Gets the number of properties marked with <see cref="RecordFieldAttribute"/> (or subclasses of it) in the
		/// given type.
		/// </summary>
		/// <param name="version">The version of the record.</param>
		/// <param name="recordType">The type with properties.</param>
		/// <returns>The number of properties in the type.</returns>
		public static int GetPropertyCount(WarcraftVersion version, Type recordType)
		{
			int count = 0;
			foreach (var recordProperty in GetVersionRelevantProperties(version, recordType))
			{
				switch (recordProperty.PropertyType)
				{
					case Type _ when IsPropertyFieldArray(recordProperty):
					{
						var arrayInfoAttribute = GetPropertyFieldArrayAttribute(recordProperty);
						count += (int)arrayInfoAttribute.Count;

						break;
					}
					case Type locStringRefType when locStringRefType == typeof(LocalizedStringReference):
					{
						count += LocalizedStringReference.GetFieldCount(version);
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
