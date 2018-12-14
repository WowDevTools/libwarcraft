using System;
using Warcraft.DBC;

namespace Warcraft.Core
{
    public class DatabaseRecordAttribute : Attribute
    {
        public DatabaseName Database { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseRecordAttribute"/> class.
        /// </summary>
        public DatabaseRecordAttribute()
        {
        }

        public DatabaseRecordAttribute(DatabaseName database)
        {
            Database = database;
        }
    }
}
