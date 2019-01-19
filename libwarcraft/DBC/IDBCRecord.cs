//
//  IDBCRecord.cs
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

using System.Collections.Generic;
using Warcraft.Core;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC
{
    /// <summary>
    /// Database record interface.
    /// </summary>
    public interface IDBCRecord
    {
        /// <summary>
        /// Gets the ID of the record.
        /// </summary>
        uint ID { get; }

        /// <summary>
        /// Gets or sets the game version the record is valid for.
        /// </summary>
        WarcraftVersion Version { get; set; }

        /// <summary>
        /// Gets the string references (if any) in the record.
        /// </summary>
        /// <returns>The references.</returns>
        IEnumerable<StringReference> GetStringReferences();
    }
}
