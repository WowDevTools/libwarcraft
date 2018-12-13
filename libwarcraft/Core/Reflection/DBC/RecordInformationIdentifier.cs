//
//  FieldInformationCache.cs
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
    /// Used as a singular identifier for a <see cref="RecordFieldInformation"/> instance.
    /// </summary>
    public class RecordInformationIdentifier : IEquatable<RecordInformationIdentifier>
    {
        /// <summary>
        /// Gets the type identifying the field information set.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the version identifying the field information set.
        /// </summary>
        public WarcraftVersion Version { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordInformationIdentifier"/> class.
        /// </summary>
        /// <param name="type">The type identifying the field information set.</param>
        /// <param name="version">The version identifying the field information set.</param>
        public RecordInformationIdentifier(Type type, WarcraftVersion version)
        {
            Type = type;
            Version = version;
        }

        /// <inheritdoc />
        public bool Equals(RecordInformationIdentifier other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Type, other.Type) && Version == other.Version;
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((RecordInformationIdentifier)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (int)Version;
            }
        }
    }
}
