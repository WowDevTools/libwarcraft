//
//  ZoneMusicRecord.cs
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
	public class CharHairGeosetsRecord : DBCRecord
	{
		public const DatabaseName Database = DatabaseName.CreatureDisplayInfo;

		public ForeignKey<uint> Race;
		public bool IsFemale;
		public uint VariationID;
		public uint GeosetID;
		public bool ShowScalp;

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

			this.Race = new ForeignKey<uint>(DatabaseName.ChrRaces, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.IsFemale = (reader.ReadUInt32() > 0);
			this.VariationID = reader.ReadUInt32();
			this.GeosetID = reader.ReadUInt32();
			this.ShowScalp = (reader.ReadUInt32() > 0);

			this.HasLoadedRecordData = true;
		}

		public override IEnumerable<StringReference> GetStringReferences()
		{
			yield break;
		}

		public override int FieldCount => 6;

		public override int RecordSize => sizeof(uint) * this.FieldCount;
	}
}