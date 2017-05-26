//
//  AnimationDataRecord.cs
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
using Warcraft.Core.Interfaces;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	/// <summary>
	/// A database record defining how an in-world liquid behaves.
	/// </summary>
	public class LiquidObjectRecord : DBCRecord, IPostLoad<byte[]>
	{
		/// <summary>
		/// The direction in which the liquid flows.
		/// </summary>
		public float FlowDirection;

		/// <summary>
		/// The speed with which the liquid flows.
		/// </summary>
		public float FlowSpeed;

		/// <summary>
		/// The type of liquid. This is a foreign reference to another table.
		/// </summary>
		public UInt32ForeignKey LiquidType;

		/// <summary>
		/// Whether or not this liquid is fishable.
		/// </summary>
		public uint Fishable;

		/// <summary>
		/// TODO: Unconfirmed behaviour
		/// The amount light this liquid reflects.
		/// </summary>
		public uint Reflection;

		/// <summary>
		/// Loads and parses the provided data.
		/// </summary>
		/// <param name="data">ExtendedData.</param>
		public override void PostLoad(byte[] data)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the field count for this record.
		/// </summary>
		/// <returns>The field count.</returns>
		public override int GetFieldCount()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the size of the record.
		/// </summary>
		/// <returns>The record size.</returns>
		public override int GetRecordSize()
		{
			throw new NotImplementedException();
		}
	}
}