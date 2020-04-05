//
//  RecordFieldArrayAttribute.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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
    /// Declares a property to be a record field property that is an array of elements. Multiple instances
    /// of this attribute can be applied to a property if the field count changes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RecordFieldArrayAttribute : RecordFieldAttribute
    {
        /// <summary>
        /// Gets or sets the number of elements in the array.
        /// </summary>
        public uint Count { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordFieldArrayAttribute"/> class.
        /// </summary>
        /// <param name="introducedIn">The version that the field was introduced in.</param>
        public RecordFieldArrayAttribute(WarcraftVersion introducedIn)
            : base(introducedIn)
        {
        }
    }
}
