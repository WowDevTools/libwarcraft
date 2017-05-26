//
//  MPQFileInfo.cs
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

using Warcraft.MPQ.Tables.Hash;
using Warcraft.MPQ.Tables.Block;
using Warcraft.Core.Locale;
using Warcraft.MPQ.Attributes;

namespace Warcraft.MPQ.FileInfo
{
	public class MPQFileInfo
	{
		/// <summary>
		/// The path of the file in the archive.
		/// </summary>
		/// <value>The path.</value>
		public string Path
		{
			get;
			private set;
		}

		/// <summary>
		/// The hash entry that points to this file.
		/// </summary>
		/// <value>A hash entry.</value>
		public HashTableEntry HashEntry
		{
			get;
			private set;
		}

		/// <summary>
		/// The block entry describing the storage of the file.
		/// </summary>
		/// <value>The block entry.</value>
		public BlockTableEntry BlockEntry
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the file attributes.
		/// </summary>
		/// <value>The attributes.</value>
		public FileAttributes Attributes
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets a value indicating whether this reference is deleted in the archive.
		/// </summary>
		/// <value><c>true</c> if this reference is deleted; otherwise, <c>false</c>.</value>
		public bool IsDeleted
		{
			get
			{
				return GetFlags().HasFlag(BlockFlags.IsDeletionMarker);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.MPQ.FileInfo.MPQFileInfo"/> class.
		/// </summary>
		/// <param name="inPath">In path.</param>
		/// <param name="inHashEntry">In hash entry.</param>
		/// <param name="inBlockEntry">In block entry.</param>
		/// <param name="inAttributes">In attributes.</param>
		public MPQFileInfo(string inPath, HashTableEntry inHashEntry, BlockTableEntry inBlockEntry, FileAttributes inAttributes)
			: this(inPath, inHashEntry, inBlockEntry)
		{
			this.Attributes = inAttributes;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.MPQ.FileInfo.MPQFileInfo"/> class.
		/// </summary>
		/// <param name="inPath">In path.</param>
		/// <param name="inHashEntry">In hash entry.</param>
		/// <param name="inBlockEntry">In block entry.</param>
		public MPQFileInfo(string inPath, HashTableEntry inHashEntry, BlockTableEntry inBlockEntry)
		{
			this.Path = inPath;
			this.HashEntry = inHashEntry;
			this.BlockEntry = inBlockEntry;
		}

		/// <summary>
		/// Gets the locale of the file. The actual values of the enum are Windows LANGID values, but
		/// files are most commonly set to 0 (default or neutral locale).
		/// </summary>
		/// <returns>The locale.</returns>
		public LocaleID GetLocale()
		{
			return this.HashEntry.GetLocalizationID();
		}

		/// <summary>
		/// Gets the platform that the file is meant for. A 0 indicates default or platform-neutral.
		/// </summary>
		/// <returns>The platform.</returns>
		public ushort GetPlatform()
		{
			return this.HashEntry.GetPlatformID();
		}

		/// <summary>
		/// Gets the size of the file as it is in the archive. This is usually smaller 
		/// than <see cref="GetActualSize"/>.
		/// </summary>
		/// <returns>The stored size.</returns>
		public long GetStoredSize()
		{
			return this.BlockEntry.GetBlockSize();
		}

		/// <summary>
		/// Gets the actual size of the file, once it's been decrypted and decompressed.
		/// </summary>
		/// <returns>The actual size.</returns>
		public long GetActualSize()
		{
			return this.BlockEntry.GetFileSize();
		}

		/// <summary>
		/// Gets a set of flags describing the way the file is stored in the archive.
		/// </summary>
		/// <returns>The flags.</returns>
		public BlockFlags GetFlags()
		{
			return this.BlockEntry.GetFlags();
		}
	}
}

