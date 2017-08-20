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
	public class CreatureModelDataRecord : DBCRecord
	{
		public const DatabaseName Database = DatabaseName.CreatureModelData;

		public uint Flags;
		public StringReference ModelPath;
		public uint SizeClass;
		public float ModelScale;
		public ForeignKey<uint> BloodLevel;
		public ForeignKey<uint> FootprintDecal;
		public float FootprintDecalLength;
		public float FootprintDecalWidth;
		public float FootprintDecalScale;
		public uint FoleyMaterialID;
		public ForeignKey<uint> FootstepShakeSize;
		public ForeignKey<uint> DeathThudShakeSize;
		public ForeignKey<uint> SoundData;

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

			this.Flags = reader.ReadUInt32();
			this.ModelPath = reader.ReadStringReference();
			this.SizeClass = reader.ReadUInt32();
			this.ModelScale = reader.ReadSingle();
			this.BloodLevel = new ForeignKey<uint>(DatabaseName.UnitBloodLevels, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.FootprintDecal = new ForeignKey<uint>(DatabaseName.FootprintTextures, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.FootprintDecalLength = reader.ReadSingle();
			this.FootprintDecalWidth = reader.ReadSingle();
			this.FootprintDecalScale = reader.ReadSingle();
			this.FoleyMaterialID = reader.ReadUInt32();
			this.FootstepShakeSize = new ForeignKey<uint>(DatabaseName.CameraShakes, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.DeathThudShakeSize = new ForeignKey<uint>(DatabaseName.CameraShakes, nameof(DBCRecord.ID), reader.ReadUInt32());
			this.SoundData = new ForeignKey<uint>(DatabaseName.CreatureSoundData, nameof(DBCRecord.ID), reader.ReadUInt32());

			this.HasLoadedRecordData = true;
		}

		public override List<StringReference> GetStringReferences()
		{
			return new List<StringReference>{ this.ModelPath };
		}

		public override int FieldCount => 14;

		public override int RecordSize => sizeof(uint) * this.FieldCount;
	}
}