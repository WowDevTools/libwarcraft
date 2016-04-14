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
		BLF_IsFile = 0x80000000,
		BLF_HasChecksums = 0x04000000,
		BLF_IsDeleted = 0x02000000,
		BLF_IsSingleUnit = 0x01000000,
		BLF_HasAdjustedEncryptionKey = 0x00020000,
		BLF_IsEncrypted = 0x00010000,
		BLF_IsCompressed = 0x00000200,
		BLF_IsImploded = 0x00000100
	}
}

