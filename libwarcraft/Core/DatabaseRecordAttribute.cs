//
//  DatabaseRecordAttribute.cs
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
using Warcraft.DBC;

namespace Warcraft.Core
{
    /// <summary>
    /// Tags a record as belonging to a specific database.
    /// </summary>
    public class DatabaseRecordAttribute : Attribute
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        public DatabaseName Database { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseRecordAttribute"/> class.
        /// </summary>
        public DatabaseRecordAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseRecordAttribute"/> class.
        /// </summary>
        /// <param name="database">The name of the database.</param>
        public DatabaseRecordAttribute(DatabaseName database)
        {
            Database = database;
        }
    }
}
