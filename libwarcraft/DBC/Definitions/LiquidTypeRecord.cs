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
using System;
using System.Collections.Generic;
using System.IO;
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

		private LiquidType TypeInternal;

		[RecordField(WarcraftVersion.Classic)]
		public LiquidType Type
		{
			get => TranslateLiquidType();
			set => this.TypeInternal = value;
		}

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.Spell, nameof(ID))]
		public ForeignKey<uint> SpellEffect { get; set; }

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

