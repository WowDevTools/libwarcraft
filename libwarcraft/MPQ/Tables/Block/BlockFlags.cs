//
//  BlockFlags.cs
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

namespace Warcraft.MPQ.Tables.Block
{
	/// <summary>
	/// Possible flags for a block entry, designating different flags for a stored file.
	/// </summary>
	[Flags]
	public enum BlockFlags : uint
	{
		/// <summary>
		/// This block is compressed with a single round of PKWARE IMPLODE compression.
		/// </summary>
		IsImploded = 				0x00000100,

		/// <summary>
		/// This block is compressed with multiple rounds of varying compression algorithms.
		/// </summary>
		IsCompressedMultiple = 		0x00000200,

		/// <summary>
		/// This block is compressed with a single round of TODO: unknown compression.
		/// </summary>
		IsCompressed = 				0x0000FF00,

		/// <summary>
		/// This block is encrypted.
		/// </summary>
		IsEncrypted = 				0x00010000,

		/// <summary>
		/// This block has an encryption key which is adjusted by its offset.
		/// </summary>
		HasAdjustedEncryptionKey = 	0x00020000,

		/// <summary>
		/// This block is a patch file for BSDIFF40, which should be applied to whatever it overrides.
		/// </summary>
		IsPatchFile = 				0x00100000,

		/// <summary>
		/// The file in this block is stored as a single unit and does not use sectors.
		/// </summary>
		IsSingleUnit = 				0x01000000,

		/// <summary>
		/// This block marks a deleted file.
		/// </summary>
		IsDeletionMarker = 			0x02000000,

		/// <summary>
		/// This file has CRC checksums appended at the end of the file data.
		/// </summary>
		HasCRCChecksums = 			0x04000000,

		/// <summary>
		/// This file exists.
		/// </summary>
		Exists = 					0x80000000,
	}
}

