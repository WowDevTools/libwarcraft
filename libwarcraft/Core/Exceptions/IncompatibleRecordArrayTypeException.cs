//
//  IncompatibleRecordArrayTypeException.cs
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

namespace Warcraft.Core
{
    /// <summary>
    /// Represents an invalid state arising from an incompatible record array type.
    /// </summary>
    public class IncompatibleRecordArrayTypeException : Exception
    {
        /// <summary>
        /// Gets the incompatible type.
        /// </summary>
        public Type IncompatibType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompatibleRecordArrayTypeException"/> class, along with a specified
        /// message.
        /// </summary>
        /// <param name="message">The message included in the exception.</param>
        public IncompatibleRecordArrayTypeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompatibleRecordArrayTypeException"/> class, along with a specified
        /// message.
        /// </summary>
        /// <param name="message">The message included in the exception.</param>
        /// <param name="incompatibleType">The type that was incompatible.</param>
        public IncompatibleRecordArrayTypeException(string message, Type incompatibleType)
            : base(message)
        {
            IncompatibType = incompatibleType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompatibleRecordArrayTypeException"/> class, along with a specified
        /// message and inner exception which caused this exception.
        /// </summary>
        /// <param name="message">The message included in the exception.</param>
        /// <param name="innerException">The exception which caused this exception.</param>
        public IncompatibleRecordArrayTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompatibleRecordArrayTypeException"/> class, along with a specified
        /// message and inner exception which caused this exception.
        /// </summary>
        /// <param name="message">The message included in the exception.</param>
        /// <param name="incompatibleType">The type that was incompatible.</param>
        /// <param name="innerException">The exception which caused this exception.</param>
        public IncompatibleRecordArrayTypeException(string message, Type incompatibleType, Exception innerException)
            : base(message, innerException)
        {
            IncompatibType = incompatibleType;
        }
    }
}
