//
//  StringReference.cs
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

namespace Warcraft.DBC.SpecialFields
{
    /// <summary>
    /// Represents a reference to a string in the database.
    /// </summary>
    public class StringReference
    {
        /// <summary>
        /// Gets the relative offset into the database file's string block.
        /// </summary>
        public uint Offset
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the actual string.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringReference"/> class.
        /// </summary>
        /// <param name="inOffset">The relative offset into the database file's string block.</param>
        public StringReference(uint inOffset)
        {
            Offset = inOffset;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }
    }
}
