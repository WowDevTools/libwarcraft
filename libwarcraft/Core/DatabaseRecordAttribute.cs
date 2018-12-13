using System;
using Warcraft.DBC;

namespace Warcraft.Core
{
    public class DatabaseRecordAttribute : Attribute
    {
        public DatabaseName Database { get; }

        public DatabaseRecordAttribute()
        {

        }

        public DatabaseRecordAttribute(DatabaseName database)
        {
            Database = database;
        }
    }
}
