//
//  DBCDefinition.cs
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

using System.Collections.Generic;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	/// <summary>
	/// A database record which holds some type of information.
	/// </summary>
	[DatabaseRecord]
	public abstract class DBCRecord : IDBCRecord
	{
		/// <summary>
		/// The record ID. This is the equivalent of a primary key in an SQL database, and is unique to the record.
		/// </summary>
		[RecordField(WarcraftVersion.Classic)]
		public uint ID
		{
			get;
			protected set;
		}

		/// <summary>
		/// The game version this record is valid for.
		/// </summary>
		/// <value>The version.</value>
		public WarcraftVersion Version
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a list of any string references in the record. Used for resolving them after they have been loaded.
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<StringReference> GetStringReferences()
		{
			yield break;
		}
	}
}

