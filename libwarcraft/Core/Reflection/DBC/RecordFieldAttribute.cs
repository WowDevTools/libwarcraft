//
//  RecordField.cs
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
    /// Declares a property to be a record field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RecordFieldAttribute : Attribute
    {
        /// <summary>
        /// Gets the version that the field was introduced in.
        /// </summary>
        public WarcraftVersion IntroducedIn { get; }

        /// <summary>
        /// Gets or sets the version that the field was removed in. If the field has not been removed, then this will
        /// have a value of <see cref="WarcraftVersion.Unknown"/>.
        /// </summary>
        public WarcraftVersion RemovedIn { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordFieldAttribute"/> class.
        /// </summary>
        /// <param name="introducedIn">The version that the field was introduced in.</param>
        public RecordFieldAttribute(WarcraftVersion introducedIn)
        {
            IntroducedIn = introducedIn;
            RemovedIn = WarcraftVersion.Unknown;
        }
    }
}

