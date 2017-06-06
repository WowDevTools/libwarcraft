//
//  SoundAmbienceRecord.cs
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
using Warcraft.Core;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class SoundAmbienceRecord : DBCRecord
	{
		public const DatabaseName Database = DatabaseName.ZoneAmbience;

		/// <summary>
		/// The ambience sound to play during the day.
		/// </summary>
		public ForeignKey<uint> AmbienceDay;

		/// <summary>
		/// The ambience sound to play during the night.
		/// </summary>
		public ForeignKey<uint> AmbienceNight;

		/*
			Cataclysm +
		*/

		public uint Flags;

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

			this.AmbienceDay = new ForeignKey<uint>(DatabaseName.SoundEntries, "ID", reader.ReadUInt32());
			this.AmbienceNight = new ForeignKey<uint>(DatabaseName.SoundEntries, "ID", reader.ReadUInt32());

			if (this.Version > WarcraftVersion.Cataclysm)
			{
				this.Flags = reader.ReadUInt32();
			}

			this.HasLoadedRecordData = true;
		}

		public override List<StringReference> GetStringReferences()
		{
			return new List<StringReference>();
		}

		public override int FieldCount
		{
			get
			{
				if (this.Version > WarcraftVersion.Cataclysm)
				{
					return 4;
				}

				return 3;
			}
		}

		public override int RecordSize => sizeof(uint) * this.FieldCount;
	}
}