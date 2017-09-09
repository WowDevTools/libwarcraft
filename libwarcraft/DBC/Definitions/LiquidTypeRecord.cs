//
//  LiquidTypeRecord.cs
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

using System.Collections.Generic;
using Warcraft.DBC.SpecialFields;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;

namespace Warcraft.DBC.Definitions
{
	[DatabaseRecord(DatabaseName.LiquidType)]
	public class LiquidTypeRecord : DBCRecord
	{
		/// <summary>
		/// The name of the liquid.
		/// </summary>
		[RecordField(WarcraftVersion.Classic)]
		public StringReference Name { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public uint Flags { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public LiquidType Type
		{
			get => TranslateLiquidType();
			set => this.TypeInternal = value;
		}
		private LiquidType TypeInternal;

		[RecordField(WarcraftVersion.Wrath)]
		[ForeignKeyInfo(DatabaseName.SoundEntries, nameof(ID))]
		public ForeignKey<uint> Sound { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.Spell, nameof(ID))]
		public ForeignKey<uint> SpellEffect { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public float MaxDarkenDepth { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public float FogDarkenIntensity { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public float AmbientDarkenIntensity { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public float DirectDarkenIntensity { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public uint LightID { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public float ParticleScale { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public uint ParticleMovement { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public uint ParticleTextureSlots { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public uint MaterialID { get; set; }

		[RecordFieldArray(WarcraftVersion.Wrath, Count = 6)]
		public StringReference[] Textures { get; set; }

		[RecordFieldArray(WarcraftVersion.Wrath, Count = 2)]
		public int[] Colour { get; set; }

		[RecordFieldArray(WarcraftVersion.Wrath, Count = 18)]
		public float[] Unknown1 { get; set; }

		[RecordFieldArray(WarcraftVersion.Wrath, Count = 4)]
		public int[] Unknown2 { get; set; }

		/// <inheritdoc />
		public override IEnumerable<StringReference> GetStringReferences()
		{
			yield return this.Name;
		}

		public LiquidType TranslateLiquidType()
		{
			if (this.Version >= WarcraftVersion.Wrath)
			{
				return this.TypeInternal;
			}

			int baseValue = (int)this.TypeInternal;
			switch (baseValue)
			{
				case 0:
				{
					return LiquidType.Magma;
				}
				case 2:
				{
					return LiquidType.Slime;
				}
				case 3:
				{
					return LiquidType.Water;
				}
				default:
				{
					return LiquidType.Water;
				}
			}
		}
	}

	public enum LiquidType
	{
		// Originally 3, now 0
		Water = 0,
		// Originally 3 (same as water), now 1
		Ocean = 1,
		// Originally 0, now 2
		Magma = 2,
		// Originally 2, now 3
		Slime = 3
	}
}

