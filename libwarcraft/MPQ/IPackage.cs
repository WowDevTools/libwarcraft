//
//  IPackage.cs
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
using System.Collections.Generic;
using Warcraft.MPQ.FileInfo;

namespace Warcraft.MPQ
{
	/// <summary>
	/// Interface for file-providing packages. A package must be able to provide a file list and output data
	/// for files stored in it.
	/// </summary>
	public interface IPackage
	{
		/// <summary>
		/// Extract the file at <paramref name="filePath"/> from the archive.
		/// </summary>
		/// <returns>The file as a byte array, or null if the file could not be found.</returns>
		/// <param name="filePath">Path to the file in the archive.</param>
		byte[] ExtractFile(string filePath);

		/// <summary>
		/// Determines whether this archive has a listfile.
		/// </summary>
		/// <returns><c>true</c> if this archive has a listfile; otherwise, <c>false</c>.</returns>
		bool HasFileList();

		/// <summary>
		/// Gets the best available listfile from the archive. If an external listfile has been provided, 
		/// that one is prioritized over the one stored in the archive.
		/// </summary>
		/// <returns>The listfile.</returns>
		List<string> GetFileList();


		/// <summary>
		/// Checks if the specified file path exists in the archive.
		/// </summary>
		/// <returns><c>true</c>, if the file exists, <c>false</c> otherwise.</returns>
		/// <param name="filePath">File path.</param>
		bool ContainsFile(string filePath);

		/// <summary>
		/// Gets the file info of the provided path.
		/// </summary>
		/// <returns>The file info, or null if the file doesn't exist in the archive.</returns>
		/// <param name="filePath">File path.</param>
		MPQFileInfo GetFileInfo(string filePath);
	}
}

