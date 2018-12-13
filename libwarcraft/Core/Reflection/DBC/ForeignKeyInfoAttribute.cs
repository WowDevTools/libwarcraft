//
//  ForeignKeyInfoAttribute.cs
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
using Warcraft.DBC;

namespace Warcraft.Core.Reflection.DBC
{
    /// <summary>
    /// Applies information about a foreign key to a record property, such as the database name and the name of the
    /// field it points to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyInfoAttribute : Attribute
    {
        /// <summary>
        /// Gets the database that the key points to.
        /// </summary>
        public DatabaseName Database { get; }

        /// <summary>
        /// Gets the name of the column that the key points to in the database.
        /// </summary>
        public string Field { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyInfoAttribute"/> class.
        /// </summary>
        /// <param name="databaseName">The name of the database the key points to.</param>
        /// <param name="field">The name of the field that the key points to in the database.</param>
        public ForeignKeyInfoAttribute(DatabaseName databaseName, string field)
        {
            this.Database = databaseName;
            this.Field = field;
        }
    }
}
