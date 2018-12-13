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
using System.Collections.Generic;

namespace Warcraft.Core.Reflection.DBC
{
    /// <summary>
    /// Caches reflection information about DBC records.
    /// </summary>
    public class RecordInformationCache
    {
        private static readonly Lazy<RecordInformationCache> LazyInstance =
            new Lazy<RecordInformationCache>(() => new RecordInformationCache());

        /// <summary>
        /// Gets the singleton instance of the information cache.
        /// </summary>
        public static RecordInformationCache Instance => LazyInstance.Value;

        /// <summary>
        /// The cache dictionary containing the reflection information.
        /// </summary>
        private readonly Dictionary<RecordInformationIdentifier, RecordFieldInformation> InformationCache;

        private RecordInformationCache()
        {
            InformationCache = new Dictionary<RecordInformationIdentifier, RecordFieldInformation>();
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
            var infoKey = new RecordInformationIdentifier(recordType, version);
            if (!InformationCache.ContainsKey(infoKey))
            {
                var recordInfo = new RecordFieldInformation(recordType, version);
                InformationCache.Add(infoKey, recordInfo);
            }

            return InformationCache[infoKey];
        }
    }
}
