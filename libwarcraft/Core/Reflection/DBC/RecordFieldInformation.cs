//
//  RecordFieldInformation.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Warcraft.DBC.Definitions;

namespace Warcraft.Core.Reflection.DBC
{
	/// <summary>
	/// Builds and represents the reflected information about the structure of a database record.
	/// </summary>
	public class RecordFieldInformation : IEquatable<RecordFieldInformation>
	{
		/// <summary>
		/// Gets the record type that this information graph is valid for.
		/// </summary>
		public Type Type { get; }

		/// <summary>
		/// Gets the <see cref="WarcraftVersion"/> that this information graph is valid for.
		/// </summary>
		public WarcraftVersion Version { get; }

		/// <summary>
		/// Gets the version-relevant properties of the record.
		/// </summary>
		public IReadOnlyList<PropertyInfo> VersionRelevantProperties { get; }

		/// <summary>
		/// Gets the record field attribute of a given property.
		/// </summary>
		public Dictionary<PropertyInfo, RecordFieldAttribute> PropertyFieldAttributes { get; }

		/// <summary>
		/// Gets the record field array of a given array property.
		/// </summary>
		public Dictionary<PropertyInfo, RecordFieldArrayAttribute> PropertyFieldArrayAttributes { get; }

		/// <summary>
		/// Gets the record field array element type of a given array property.
		/// </summary>
		public Dictionary<PropertyInfo, Type> PropertyFieldArrayElementTypes { get; }

		/// <summary>
		/// Gets the foreign key information of a given property.
		/// </summary>
		public Dictionary<PropertyInfo, ForeignKeyInfoAttribute> PropertyForeignKeyAttributes { get; }

		/// <summary>
		/// Gets the field count of the record.
		/// </summary>
		public int FieldCount { get; }

		/// <summary>
		/// Gets the size of the record.
		/// </summary>
		public int Size { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="RecordFieldInformation"/> class.
		/// </summary>
		/// <param name="recordType">The type of record to build the reflection data for.</param>
		/// <param name="version">The version to build data for.</param>
		/// <exception cref="ArgumentException">Thrown if the specified type is not a record type.</exception>
		public RecordFieldInformation(Type recordType, WarcraftVersion version)
		{
			if (!recordType.IsSubclassOf(typeof(DBCRecord)))
			{
				throw new ArgumentException("The specified type was not a record type.", nameof(recordType));
			}

			if (!recordType.GetCustomAttributes().Any(a => a is DatabaseRecordAttribute))
			{
				throw new ArgumentException($"The record type {recordType.Name} was not decorated with the \"DatabaseRecord\" attribute.");
			}

			this.Type = recordType;
			this.Version = version;
			var orderer = new FieldOrderer(this.Version, DBCInspector.GetVersionRelevantProperties(this.Version, this.Type).ToList());

			this.VersionRelevantProperties = orderer.ReorderProperties().ToList();

			this.PropertyFieldAttributes = new Dictionary<PropertyInfo, RecordFieldAttribute>();
			this.PropertyFieldArrayAttributes = new Dictionary<PropertyInfo, RecordFieldArrayAttribute>();
			this.PropertyFieldArrayElementTypes = new Dictionary<PropertyInfo, Type>();
			this.PropertyForeignKeyAttributes = new Dictionary<PropertyInfo, ForeignKeyInfoAttribute>();

			foreach (var property in this.VersionRelevantProperties)
			{
				if (!property.CanWrite)
				{
					throw new ArgumentException("Property setter not found. Record properties must have a setter.");
				}

				var fieldInfoAttribute = DBCInspector.GetPropertyFieldAttribute(property);
				this.PropertyFieldAttributes.Add(property, fieldInfoAttribute);

				if (DBCInspector.IsPropertyFieldArray(property))
				{
					this.PropertyFieldArrayAttributes.Add(property, DBCInspector.GetVersionRelevantPropertyFieldArrayAttribute(this.Version, property));
					this.PropertyFieldArrayElementTypes.Add(property, DBCInspector.GetFieldArrayPropertyElementType(property.PropertyType));
				}

				if (DBCInspector.IsPropertyForeignKey(property))
				{
					this.PropertyForeignKeyAttributes.Add(property, DBCInspector.GetForeignKeyInfo(property));
				}
			}

			this.FieldCount = DBCInspector.GetPropertyCount(this.Version, this.Type);
			this.Size = DBCInspector.GetRecordSize(this.Version, this.Type);
		}

		/// <summary>
		/// Determines whether or not a given property is recognized as a field array by this information set.
		/// </summary>
		/// <param name="property">The property to check.</param>
		/// <returns>true if the property is a field array; otherwise, false.</returns>
		public bool IsPropertyFieldArray(PropertyInfo property)
		{
			return this.PropertyFieldArrayAttributes.ContainsKey(property);
		}

		/// <inheritdoc />
		public bool Equals(RecordFieldInformation other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Equals(this.Type, other.Type) && this.Version == other.Version;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != this.GetType())
			{
				return false;
			}

			return Equals((RecordFieldInformation)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				return ((this.Type != null ? this.Type.GetHashCode() : 0) * 397) ^ (int)this.Version;
			}
		}
	}
}
