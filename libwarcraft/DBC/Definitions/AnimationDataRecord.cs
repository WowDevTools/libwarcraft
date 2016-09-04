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
	/// <summary>
	/// Animation data record. This database defines the different animations models can have, and
	/// is referenced by M2 and MDX files.
	/// </summary>
	public class AnimationDataRecord : DBCRecord
	{
		public const string RecordName = "AnimationData";

		/// <summary>
		/// The name of the animation.
		/// </summary>
		public StringReference Name;

		/// <summary>
		/// The weapon flags. This affects how the model's weapons are held during the animation.
		/// </summary>
		[FieldVersion(WarcraftVersion.Warlords)]
		public WeaponAnimationFlags WeaponFlags;

		/// <summary>
		/// The body flags.
		/// </summary>
		[FieldVersion(WarcraftVersion.Warlords)]
		public uint BodyFlags;

		/// <summary>
		/// General animation flags.
		/// </summary>
		public uint Flags;

		/// <summary>
		/// The fallback animation that precedes this one.
		/// </summary>
		public UInt32ForeignKey FallbackAnimation;

		/// <summary>
		/// The top-level behaviour animation that this animation is a child of.
		/// </summary>
		public UInt32ForeignKey BehaviourAnimation;

		/// <summary>
		/// The behaviour tier of the animation. In most cases, this indicates whether or not the animation
		/// is used for flying characters.
		/// </summary>
		[FieldVersion(WarcraftVersion.Wrath)]
		public uint BehaviourTier;

		/// <summary>
		/// Loads and parses the provided data.
		/// </summary>
		/// <param name="data">Data.</param>
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

					this.FallbackAnimation = new UInt32ForeignKey(AnimationDataRecord.RecordName, "ID", br.ReadUInt32());
					this.BehaviourAnimation = new UInt32ForeignKey(AnimationDataRecord.RecordName, "ID", br.ReadUInt32());

					if (Version >= WarcraftVersion.Wrath)
					{
						this.BehaviourTier = br.ReadUInt32();
					}
				}
			}
		}

		/// <summary>
		/// Gets the size of the record.
		/// </summary>
		/// <returns>The record size.</returns>
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

		/// <summary>
		/// Gets the field count for this record at.
		/// </summary>
		/// <returns>The field count.</returns>
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

	/// <summary>
	/// Weapon animation flags.
	/// </summary>
	public enum WeaponAnimationFlags : uint
	{
		/// <summary>
		/// Ignores the current state of the character's weapons.
		/// </summary>
		None = 0,

		/// <summary>
		/// Sheathes the weapons for the duration of the animation.
		/// </summary>
		Sheathe = 4,

		/// <summary>
		/// Sheathes the weapons for the duration of the animation.
		/// </summary>
		Sheathe2 = 16,

		/// <summary>
		/// Unsheathes the weapons for the duration of the animation.
		/// </summary>
		Unsheathe = 32
	}
}

