//
//  TextureBlobFlags.cs
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

namespace Warcraft.TEX
{
	/// <summary>
	/// A set of flags, which describe alternate behaviour of the compression in the texture blob.
	/// </summary>
	[Flags]
	public enum TextureBlobFlags : byte
	{
		/// <summary>
		/// Tells the compression algorithm to prefer ARGB1555 textures, if DXT1 compression
		/// is not available. This flag is only valid if <see cref="TextureBlobDataEntryTextureBlobDataEntrypressionType"/> is
		///
		/// </summary>
		PreferARGB1555IfDXT1IsNotAvailable = 1 << 0
	}
}