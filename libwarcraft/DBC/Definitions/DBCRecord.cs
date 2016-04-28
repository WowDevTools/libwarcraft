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
using System;
using Warcraft.Core;

namespace Warcraft.DBC.Definitions
{
	public abstract class DBCRecord
	{
		/// <summary>
		/// The record ID. This is the equivalent of a primary key in an SQL database, and is unique to the record.
		/// </summary>
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
			protected set;
		} = WarcraftVersion.Unknown;

		/// <summary>
		/// Sets the version this record is valid for.
		/// </summary>
		public virtual void SetVersion(WarcraftVersion InVersion)
		{
			this.Version = InVersion;
		}

		/// <summary>
		/// Loads and parses the provided data.
		/// </summary>
		/// <param name="data">Data.</param>
		public abstract void LoadData(byte[] data);

		/// <summary>
		/// Gets the field count for this record at.
		/// </summary>
		/// <returns>The field count.</returns>
		public abstract int GetFieldCount();

		/// <summary>
		/// Gets the size of the record.
		/// </summary>
		/// <returns>The record size.</returns>
		public abstract int GetRecordSize();
	}
}

