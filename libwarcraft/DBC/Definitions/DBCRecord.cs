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
using System.IO;
using Warcraft.Core;
using Warcraft.Core.Interfaces;

namespace Warcraft.DBC.Definitions
{
	/// <summary>
	/// A database record which holds some type of information.
	/// </summary>
	public abstract class DBCRecord : IPostLoad<byte[]>, IDBCRecord, IDeferredDeserialize
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
			set;
		}

		/// <summary>
		/// Whether or not this record has had its data loaded.
		/// </summary>
		private bool HasLoadedRecordData = false;

		/// <summary>
		/// Sets the version this record is valid for.
		/// </summary>
		public virtual void SetVersion(WarcraftVersion inVersion)
		{
			this.Version = inVersion;
		}

		/// <summary>
		/// Loads and parses the provided data.
		/// </summary>
		/// <param name="data">ExtendedData.</param>
		public abstract void PostLoad(byte[] data);

		/// <summary>
		/// Gets the field count for this record at.
		/// </summary>
		/// <returns>The field count.</returns>
		public virtual int FieldCount => -1;

		/// <summary>
		/// Gets the size of the record.
		/// </summary>
		/// <returns>The record size.</returns>
		public virtual int RecordSize => -1;

		/// <summary>
		/// Determines whether or not this object has finished loading.
		/// </summary>
		/// <returns><value>true</value> if the object has finished loading; otherwise, <value>false</value>.</returns>
		public virtual bool HasFinishedLoading()
		{
			return this.HasLoadedRecordData;
		}

		/// <summary>
		/// Deserializes the data of the object using the provided <see cref="BinaryReader"/>.
		/// </summary>
		/// <param name="reader"></param>
		public virtual void DeserializeSelf(BinaryReader reader)
		{
			if (this.Version == WarcraftVersion.Unknown)
			{
				throw new InvalidOperationException("The record data cannot be loaded before the version has been set.");
			}

			
			this.ID = reader.ReadUInt32();
		}
	}
}

