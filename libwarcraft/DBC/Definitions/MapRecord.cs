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
	public class MapRecord : DBCRecord
	{
		public StringReference Directory;
		public uint InstanceType;
		public uint PVP;
		public LocalizedStringReference MapName;
		public uint MinLevel;
		public uint MaxLevel;
		public uint MaxPlayers;
		public uint Unknown1;
		public uint Unknown2;
		public uint Unknown3;
		public UInt32ForeignKey AreaTableID;
		public LocalizedStringReference MapDescription1;
		public LocalizedStringReference MapDescription2;
		public UInt32ForeignKey LoadingScreenID;
		public uint RaidOffset;
		public uint Unknown4;
		public uint Unknown5;

		public override void LoadData(byte[] data)
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