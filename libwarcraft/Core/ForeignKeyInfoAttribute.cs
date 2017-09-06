using System;
using Warcraft.DBC;

namespace Warcraft.Core
{
	[AttributeUsage(AttributeTargets.Property)]
	public class ForeignKeyInfoAttribute : Attribute
	{
		public DatabaseName Database { get; }
		public string Field { get; }

		public ForeignKeyInfoAttribute(DatabaseName databaseName, string field)
		{
			this.Database = databaseName;
			this.Field = field;
		}
	}
}
