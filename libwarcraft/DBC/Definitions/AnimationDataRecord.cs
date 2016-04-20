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
using Warcraft.Core;
using System.IO;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class AnimationDataRecord : DBCRecord
	{
		public const string RecordName = "AnimationData";

		public StringReference Name;
		public WeaponAnimationFlags WeaponFlags;
		public uint BodyFlags;
		public uint Flags;
		public Int32ForeignKey FallbackAnimation;
		public Int32ForeignKey BehaviourAnimation;
		public uint BehaviourTier;

		public AnimationDataRecord()
		{
		}

		public override void LoadData(byte[] data)
		{
			if (this.Version == WarcraftVersion.Unknown)
			{
				throw new InvalidOperationException("The record data cannot be loaded before SetVersion has been called.");
			}
			
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.ID = br.ReadUInt32();
					this.Name = new StringReference(br.ReadUInt32());

					if (Version >= WarcraftVersion.Warlords)
					{
						this.WeaponFlags = (WeaponAnimationFlags)br.ReadUInt32();
						this.BodyFlags = br.ReadUInt32();
					}

					this.Flags = br.ReadUInt32();

					this.FallbackAnimation = new Int32ForeignKey(AnimationDataRecord.RecordName, "ID", br.ReadUInt32());
					this.BehaviourAnimation = new Int32ForeignKey(AnimationDataRecord.RecordName, "ID", br.ReadUInt32());

					if (Version >= WarcraftVersion.Wrath)
					{
						this.BehaviourTier = br.ReadUInt32();
					}
				}
			}
		}

		public override int GetRecordSize()
		{
			if (this.Version == WarcraftVersion.Unknown)
			{
				throw new InvalidOperationException("The record information cannot be accessed before SetVersion has been called.");
			}

			switch (Version)
			{
				case WarcraftVersion.Classic:
					return 28;
				case WarcraftVersion.BurningCrusade:
					return 28;
				case WarcraftVersion.Wrath:
					return 32;
				case WarcraftVersion.Cataclysm:
					return 32;
				case WarcraftVersion.Mists:
					return 32;
				case WarcraftVersion.Warlords:
					return 24;
				case WarcraftVersion.Legion:
					return 24;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public override int GetFieldCount()
		{
			if (this.Version == WarcraftVersion.Unknown)
			{
				throw new InvalidOperationException("The record information cannot be accessed before SetVersion has been called.");
			}

			switch (Version)
			{
				case WarcraftVersion.Classic:
					return 7;
				case WarcraftVersion.BurningCrusade:
					return 7;
				case WarcraftVersion.Wrath:
					return 8;
				case WarcraftVersion.Cataclysm:
					return 8;
				case WarcraftVersion.Mists:
					return 8;
				case WarcraftVersion.Warlords:
					return 6;
				case WarcraftVersion.Legion:
					return 6;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	public enum WeaponAnimationFlags : uint
	{
		None = 0,
		Sheathe = 4,
		Sheathe2 = 16,
		Unsheathe = 32
	}
}

