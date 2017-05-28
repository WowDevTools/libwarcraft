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
using System.Collections.Generic;
using System.IO;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	/// <summary>
	/// A database record defining properties for maps.
	/// </summary>
	public class MapRecord : DBCRecord
	{
		public const DatabaseName Database = DatabaseName.Map;
		
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
		public ForeignKey<uint> AreaTableID;

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
		public ForeignKey<uint> LoadingScreenID;

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

		/// <summary>
		/// Loads and parses the provided data.
		/// </summary>
		/// <param name="data">ExtendedData.</param>
		public override void PostLoad(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					DeserializeSelf(br);
				}
			}
		}

		/// <summary>
		/// Deserializes the data of the object using the provided <see cref="BinaryReader"/>.
		/// </summary>
		/// <param name="reader"></param>
		public override void DeserializeSelf(BinaryReader reader)
		{
			base.DeserializeSelf(reader);
			
			throw new NotImplementedException();
			this.HasLoadedRecordData = true;
		}
		
		public override List<StringReference> GetStringReferences()
		{
			return new List<StringReference>(); // TODO: Localization handling
		}

		public override int FieldCount => throw new System.NotImplementedException();

		public override int RecordSize => throw new System.NotImplementedException();
	}
}