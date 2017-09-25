//
//  FieldInformationCache.cs
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

namespace Warcraft.Core.Reflection.DBC
{
	/// <summary>
	/// Caches reflection information about DBC records.
	/// </summary>
	public class FieldInformationCache
	{
		private static readonly Lazy<FieldInformationCache> LazyInstance =
			new Lazy<FieldInformationCache>(() => new FieldInformationCache());

		/// <summary>
		/// Gets the singleton instance of the information cache.
		/// </summary>
		public static FieldInformationCache Instance => LazyInstance.Value;

		/// <summary>
		/// The cache dictionary containing the reflection information.
		/// </summary>
		private readonly Dictionary<(Type Type, WarcraftVersion Version), RecordFieldInformation> InformationCache;

		private FieldInformationCache()
		{
			this.InformationCache = new Dictionary<(Type Type, WarcraftVersion Version), RecordFieldInformation>();
		}

		/// <summary>
		/// Gets the reflection information for a given record type and version from the cache, creating it if it is not
		/// already present.
		/// </summary>
		/// <param name="recordType">The record type.</param>
		/// <param name="version">The version to get.</param>
		/// <returns>A <see cref="RecordFieldInformation"/> relevant for the given version.</returns>
		public RecordFieldInformation GetRecordInformation(Type recordType, WarcraftVersion version)
		{
			var infoKey = (recordType, version);
			if (!this.InformationCache.ContainsKey(infoKey))
			{
				var recordInfo = new RecordFieldInformation(recordType, version);
				this.InformationCache.Add(infoKey, recordInfo);
			}

			return this.InformationCache[infoKey];
		}
	}
}
