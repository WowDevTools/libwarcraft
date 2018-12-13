//
//  RecordFieldOrderAttribute.cs
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

namespace Warcraft.Core.Reflection.DBC
{
    /// <summary>
    /// Represents an information tag, describing relative movement of a record field in a particular version.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RecordFieldOrderAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the version that the associated field was moved in.
        /// </summary>
        public WarcraftVersion MovedIn { get; set; }

        /// <summary>
        /// Gets or sets the name of the previous field in the record, which the associated field should come after.
        /// </summary>
        public string ComesAfter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordFieldOrderAttribute"/> class.
        /// </summary>
        /// <param name="movedIn">The version that the field moved in.</param>
        public RecordFieldOrderAttribute(WarcraftVersion movedIn)
        {
            this.MovedIn = movedIn;
        }
    }
}
