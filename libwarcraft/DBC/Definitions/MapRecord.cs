//
//  MapRecord.cs
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
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	/// <summary>
	/// A database record defining properties for maps.
	/// </summary>
	public class MapRecord : DBCRecord
	{
		/// <summary>
		/// The directory under which the map is stored.
		/// </summary>
		public StringReference Directory;

		/// <summary>
		/// The type of instance this map is.
		/// </summary>
		public uint InstanceType;

		/// <summary>
		/// What sort of PvP the map allows.
		/// </summary>
		public uint PvP;

		/// <summary>
		/// The name of the map.
		/// </summary>
		public LocalizedStringReference MapName;

		/// <summary>
		/// The minimum level of the map.
		/// </summary>
		public uint MinLevel;

		/// <summary>
		/// The maximum level of the map.
		/// </summary>
		public uint MaxLevel;

		/// <summary>
		/// The maximum number of players that can be in the map at any one time.
		/// </summary>
		public uint MaxPlayers;

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		public uint Unknown1;

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		public uint Unknown2;

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		public uint Unknown3;

		/// <summary>
		/// The ID of the area table entry for this map, which contains more information.
		/// </summary>
		public UInt32ForeignKey AreaTableID;

		/// <summary>
		/// TODO: Unknown behaviour, improve comment
		/// The description of the map.
		/// </summary>
		public LocalizedStringReference MapDescription1;

		/// <summary>
		/// TODO: Unknown behaviour, improve comment
		/// The description of the map.
		/// </summary>
		public LocalizedStringReference MapDescription2;

		/// <summary>
		/// The ID of the loading screen for this map.
		/// </summary>
		public UInt32ForeignKey LoadingScreenID;

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		public uint RaidOffset;

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		public uint Unknown4;

		/// <summary>
		/// TODO: Unknown behaviour
		/// </summary>
		public uint Unknown5;

		public override void PostLoad(byte[] data)
		{
			throw new NotImplementedException();
		}

		public override int GetFieldCount()
		{
			throw new NotImplementedException();
		}

		public override int GetRecordSize()
		{
			throw new NotImplementedException();
		}
	}
}