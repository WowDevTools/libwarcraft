//
//  Exceptions.cs
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

namespace Warcraft.Core
{
	public class FileDeletedException : Exception
	{
		public string FilePath { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="FileDeletedException"/> class, along with a specified
		/// message.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		public FileDeletedException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="FileDeletedException"/> class, along with a specified
		/// message and file path.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="filePath"></param>
		public FileDeletedException(string message, string filePath)
			: base(message)
		{
			this.FilePath = filePath;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="FileDeletedException"/> class, along with a specified
		/// message and inner exception which caused this exception.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		/// <param name="innerException">The exception which caused this exception.</param>
		public FileDeletedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="FileDeletedException"/> class, along with a specified
		/// message, file path, and inner exception which caused this exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="filePath"></param>
		/// <param name="innerException"></param>
		public FileDeletedException(string message, string filePath, Exception innerException)
			: base(message, innerException)
		{
			this.FilePath = filePath;
		}
	}

	/// <summary>
	/// This exception thrown when an invalid or unknown chunk signature is found during parsing of binary data which
	/// is expected to be in valid RIFF format.
	/// </summary>
	public class InvalidChunkSignatureException : Exception
	{
		/// <summary>
		/// Creates a new instance of the <see cref="InvalidChunkSignatureException"/> class, along with a specified
		/// message.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		public InvalidChunkSignatureException(string message)
			:base(message)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="InvalidChunkSignatureException"/> class, along with a specified
		/// message and inner exception which caused this exception.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		/// <param name="innerException">The exception which caused this exception.</param>
		public InvalidChunkSignatureException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	/// <summary>
	/// This exception thrown when an invalid sector offset table is encountered during file extraction. Usually,
	/// it means the archive that the user is trying to extract the file from is invalid, corrupted, or has been
	/// maliciously zeroed at critical points.
	/// </summary>
	public class InvalidFileSectorTableException : Exception
	{
		/// <summary>
		/// Creates a new instance of the <see cref="InvalidFileSectorTableException"/> class, along with a specified
		/// message.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		public InvalidFileSectorTableException(string message)
			:base(message)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="InvalidFileSectorTableException"/> class, along with a specified
		/// message and inner exception which caused this exception.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		/// <param name="innerException">The exception which caused this exception.</param>
		public InvalidFileSectorTableException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class InvalidFieldAttributeException : Exception
	{
		/// <summary>
		/// Creates a new instance of the <see cref="InvalidFieldAttributeException"/> class, along with a specified
		/// message.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		public InvalidFieldAttributeException(string message)
			:base(message)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="InvalidFieldAttributeException"/> class, along with a specified
		/// message and inner exception which caused this exception.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		/// <param name="innerException">The exception which caused this exception.</param>
		public InvalidFieldAttributeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class IncompatibleRecordArrayTypeException : Exception
	{
		/// <summary>
		/// The incompatible type.
		/// </summary>
		public Type IncompatibType { get; }

		/// <summary>
		/// Creates a new instance of the <see cref="IncompatibleRecordArrayTypeException"/> class, along with a specified
		/// message.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		public IncompatibleRecordArrayTypeException(string message)
			:base(message)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="IncompatibleRecordArrayTypeException"/> class, along with a specified
		/// message.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		/// <param name="incompatibleType">The type that was incompatible.</param>
		public IncompatibleRecordArrayTypeException(string message, Type incompatibleType)
			:base(message)
		{
			this.IncompatibType = incompatibleType;
		}

		/// <summary>
		/// Creates a new instance of the <see cref="IncompatibleRecordArrayTypeException"/> class, along with a specified
		/// message and inner exception which caused this exception.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		/// <param name="innerException">The exception which caused this exception.</param>
		public IncompatibleRecordArrayTypeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Creates a new instance of the <see cref="IncompatibleRecordArrayTypeException"/> class, along with a specified
		/// message and inner exception which caused this exception.
		/// </summary>
		/// <param name="message">The message included in the exception.</param>
		/// <param name="incompatibleType">The type that was incompatible.</param>
		/// <param name="innerException">The exception which caused this exception.</param>
		public IncompatibleRecordArrayTypeException(string message, Type incompatibleType, Exception innerException)
			: base(message, innerException)
		{
			this.IncompatibType = incompatibleType;
		}
	}
}