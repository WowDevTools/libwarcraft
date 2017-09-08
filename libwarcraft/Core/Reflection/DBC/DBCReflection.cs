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
	public static class DBCReflection
	{
		public static void DeserializeRecord<T>(BinaryReader reader, T record, WarcraftVersion version)
		{
			var databaseProperties = GetVersionRelevantProperties(version, typeof(T));
			foreach (var databaseProperty in databaseProperties)
			{
				if (!databaseProperty.CanWrite)
				{
					throw new ArgumentException("Property setter not found. Record properties must have a setter.");
				}

				object propertyValue = null;
				if (IsPropertyArray(databaseProperty))
				{
					var elementType = GetPropertyArrayElementType(databaseProperty.PropertyType);
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

		public static Type GetPropertyArrayElementType(Type propertyType)
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

			throw new ArgumentException($"No compatible collection could be created for a property of type {propertyType}", nameof(propertyType));
		}

		public static object ReadPropertyValue(BinaryReader reader, PropertyInfo property, Type propertyType, WarcraftVersion version)
		{
			object fieldValue;
			if (IsPropertyForeignKey(property))
			{
				// Get the foreign key information
				var foreignKeyAttribute = GetForeignKeyInfo(property);

				// Get the inner type
				var keyType = GetForeignKeyType(property);
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
			else if (propertyType.IsEnum)
			{
				// Get the underlying type of the enum
				var enumType = propertyType.GetEnumUnderlyingType();
				fieldValue = reader.Read(enumType);
			}
			else
			{
				if (propertyType == typeof(LocalizedStringReference))
				{
					fieldValue = reader.Read(propertyType, version);
				}
				else
				{
					fieldValue = reader.Read(propertyType);
				}
			}

			return fieldValue;
		}

		public static bool IsPropertyArray(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetCustomAttributes().Any(p => p is RecordFieldArrayAttribute);
		}

		public static RecordFieldArrayAttribute GetPropertyFieldArrayAttribute(PropertyInfo propertyInfo) // TODO: Write tests
		{
			if (!IsPropertyArray(propertyInfo))
			{
				throw new ArgumentException("The property is not an array. Use GetPropertyFieldAttribute instead.", nameof(propertyInfo));
			}

			var arrayAttribute = propertyInfo.GetCustomAttributes().FirstOrDefault(p => p is RecordFieldArrayAttribute) as RecordFieldArrayAttribute;

			if (arrayAttribute == null)
			{
				throw new InvalidDataException("Somehow, a property had an array attribute defined but did not actually have one. Call a priest.");
			}

			return arrayAttribute;
		}

		public static RecordFieldAttribute GetPropertyFieldAttribute(PropertyInfo propertyInfo) // TODO: Write tests
		{
			var fieldAttribute = propertyInfo.GetCustomAttributes().FirstOrDefault(a => a is RecordFieldAttribute) as RecordFieldAttribute;

			if (fieldAttribute == null)
			{
				throw new InvalidDataException("Somehow, a property had a version attribute defined but did not actually have one. Call a priest.");
			}

			return fieldAttribute;
		}

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

		public static bool IsPropertyTypeCompatibleWithArrayAttribute(Type propertyType)
		{
			bool isArray = propertyType.IsArray;
			bool implementsGenericIList = propertyType.GetInterfaces()
				.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>));


			return isArray || implementsGenericIList;
		}

		public static IEnumerable<PropertyInfo> GetVersionRelevantProperties(WarcraftVersion version, Type recordType)
		{
			// Order the properties by their field order.
			var orderedProperties = GetRecordProperties(recordType).OrderByDescending(p => p.DeclaringType, new InheritanceChainComparer())
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

		public static bool IsPropertyForeignKey(PropertyInfo propertyInfo)
		{
			return
				propertyInfo.PropertyType.IsGenericType &&
				propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(ForeignKey<>);
		}

		public static ForeignKeyInfoAttribute GetForeignKeyInfo(PropertyInfo foreignKeyInfo)
		{
			if (!IsPropertyForeignKey(foreignKeyInfo))
			{
				throw new ArgumentException("The given property was not a foreign key.", nameof(foreignKeyInfo));
			}

			var foreignKeyAttribute = foreignKeyInfo.GetCustomAttributes().FirstOrDefault(a => a is ForeignKeyInfoAttribute) as ForeignKeyInfoAttribute;

			if (foreignKeyAttribute == null)
			{
				throw new InvalidDataException("ForeignKey properties must be decorated with the ForeignKeyInfo attribute.");
			}

			return foreignKeyAttribute;
		}

		public static Type GetForeignKeyType(PropertyInfo foreignKeyInfo)
		{
			if (!IsPropertyForeignKey(foreignKeyInfo))
			{
				throw new ArgumentException("The given property was not a foreign key.", nameof(foreignKeyInfo));
			}

			return foreignKeyInfo.PropertyType.GetGenericArguments().First();
		}

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
						var underlyingType = GetUnderlyingStoredType(recordProperty.PropertyType);

						size += Marshal.SizeOf(underlyingType);
						break;
					}
					// Multi-field types
					case Type genericListType when genericListType.IsGenericType && genericListType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>)):
					case Type arrayType when arrayType.IsArray:
					{
						var elementSize = Marshal.SizeOf(GetUnderlyingStoredType(recordProperty.PropertyType));
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

		public static Type GetUnderlyingStoredType(Type wrappingType)
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

			return GetUnderlyingStoredType(innerType);
		}

		public static int GetPropertyCount(WarcraftVersion version, Type recordType)
		{
			int count = 0;
			foreach (var recordProperty in GetVersionRelevantProperties(version, recordType))
			{
				switch (recordProperty.PropertyType)
				{
					case Type _ when IsPropertyArray(recordProperty):
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

	public class InheritanceChainComparer : IComparer<Type>
	{
		/// <inheritdoc />
		public int Compare(Type x, Type y)
		{
			if (x.IsSubclassOf(y))
			{
				return -1;
			}

			if (y.IsSubclassOf(x))
			{
				return 1;
			}

			return 0;
		}
	}
}
