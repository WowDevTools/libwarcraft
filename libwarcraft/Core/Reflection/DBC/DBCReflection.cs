using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Warcraft.Core.Extensions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.Core.Reflection.DBC
{
	public static class DBCReflection
	{
		public static void DeserializeRecord<T>(BinaryReader reader, T record, WarcraftVersion version)
		{
			var databaseProperties = GetVersionRelevantProperties<T>(version);
			foreach (var databaseProperty in databaseProperties)
			{
				if (!databaseProperty.CanWrite)
				{
					throw new ArgumentException("Property setter not found. Record properties must have a setter.");
				}

				object fieldValue;
				var fieldType = databaseProperty.PropertyType;
				if (IsPropertyForeignKey(databaseProperty))
				{
					// Get the foreign key information
					var foreignKeyAttribute = GetForeignKeyInfo(databaseProperty);

					// Get the inner type
					var keyType = GetForeignKeyType(databaseProperty);
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
				else
				{
					fieldValue = reader.Read(fieldType);
				}

				databaseProperty.SetValue(record, fieldValue);
			}
		}

		public static IEnumerable<PropertyInfo> GetRecordProperties<T>()
		{
			return typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Where(p => p.IsDefined(typeof(RecordFieldAttribute)));
		}

		public static IEnumerable<PropertyInfo> GetVersionRelevantProperties<T>(WarcraftVersion version)
		{
			// Order the properties by their field order.
			var orderedProperties = GetRecordProperties<T>().OrderBy(
				p => (p.GetCustomAttributes().First(a => a is RecordFieldAttribute) as RecordFieldAttribute)?.Order);

			foreach (var recordProperty in orderedProperties)
			{
				var versionAttribute = recordProperty.GetCustomAttributes().First(a => a is RecordFieldAttribute) as RecordFieldAttribute;

				if (versionAttribute == null)
				{
					throw new InvalidDataException("Somehow, a property had a version attribute defined but did not actually have one. Call a priest.");
				}

				// Field is not present in the version we're reading, skip it
				if (versionAttribute.IntroducedIn > version)
				{
					continue;
				}

				// Field has been removed in the version we're reading, skip it
				if (versionAttribute.RemovedIn.HasValue && versionAttribute.RemovedIn.Value <= version)
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
	}
}
