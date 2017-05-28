//
//  UnknownRecord.cs
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
using Warcraft.Core;
using System.IO;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	/// <summary>
	/// Unknown record. This class serves as a general-purpose container for unimplemented records, and
	/// simplifies inspection of those records.
	/// </summary>
	public class UnknownRecord : DBCRecord
	{
		/// <summary>
		/// The record data, stored as raw bytes.
		/// </summary>
		public byte[] RecordData;

		/// <summary>
		/// Loads and parses the provided data.
		/// </summary>
		/// <param name="data">ExtendedData.</param>
		public override void PostLoad(byte[] data)
		{
			if (this.Version == WarcraftVersion.Unknown)
			{
				throw new InvalidOperationException("The record data cannot be loaded before SetVersion has been called.");
			}

			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					// No matter what, any unknown records will have an ID primary key column.
					this.ID = br.ReadUInt32();
				}
			}

			/*byte[] dataWithoutID = new byte[data.Length - 4];
			Buffer.BlockCopy(data, 4, dataWithoutID, 0, dataWithoutID.Length);*/

			//this.RecordData = dataWithoutID;
			this.RecordData = data;
			this.HasLoadedRecordData = true;
		}
		
		public override List<StringReference> GetStringReferences()
		{
			return new List<StringReference>();
		}

		/// <summary>
		/// Gets the size of the record.
		/// </summary>
		/// <returns>The record size.</returns>
		public override int RecordSize
		{
			get
			{
				if (this.Version == WarcraftVersion.Unknown)
				{
					throw new InvalidOperationException("The record information cannot be accessed before the version has been set.");
				}

				return -1;
			}
		}

		/// <summary>
		/// Gets the field count for this record at.
		/// </summary>
		/// <returns>The field count.</returns>
		public override int FieldCount
		{
			get
			{
				if (this.Version == WarcraftVersion.Unknown)
				{
					throw new InvalidOperationException("The record information cannot be accessed before the version has been set.");
				}

				return -1;
			}
		}
	}
}
