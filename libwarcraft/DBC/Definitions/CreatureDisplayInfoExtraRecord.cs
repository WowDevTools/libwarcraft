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
using Warcraft.Core.Extensions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class CreatureDisplayInfoExtraRecord : DBCRecord
	{
		public const DatabaseName Database = DatabaseName.CreatureDisplayInfoExtra;

		public ForeignKey<uint> Race;
		public bool IsFemale;
		public uint Skin;
		public uint Face;
		public ForeignKey<uint> HairType;
		public ForeignKey<uint> HairVariation;
		public uint BeardType;
		public ForeignKey<uint> Helmet;
		public ForeignKey<uint> Shoulder;
		public ForeignKey<uint> Shirt;
		public ForeignKey<uint> Cuirass;
		public ForeignKey<uint> Belt;
		public ForeignKey<uint> Legs;
		public ForeignKey<uint> Boots;
		public ForeignKey<uint> Wrist;
		public ForeignKey<uint> Gloves;
		public ForeignKey<uint> Tabard;
		public ForeignKey<uint> Cape;
		public StringReference BakedName;

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

			// 0 means male, 1 means female
			this.IsFemale = (reader.ReadUInt32() > 0);
			this.Skin = reader.ReadUInt32();
			this.Face = reader.ReadUInt32();

			this.HairType = new ForeignKey<uint>(DatabaseName.CharHairGeosets, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.HairVariation = new ForeignKey<uint>(DatabaseName.CharSections, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.BeardType = reader.ReadUInt32();

			this.Helmet = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.Shoulder = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.Shirt = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.Cuirass = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.Belt = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.Legs = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.Boots = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.Wrist = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.Gloves = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.Tabard = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.Cape = new ForeignKey<uint>(DatabaseName.ItemDisplayInfo, nameof(DBCRecord.ID), reader.ReadUInt32());

			this.BakedName = reader.ReadStringReference();

			this.HasLoadedRecordData = true;
		}

		public override List<StringReference> GetStringReferences()
		{
			return new List<StringReference>{ this.BakedName };
		}

		public override int FieldCount => 19;

		public override int RecordSize => sizeof(uint) * this.FieldCount;
	}
}