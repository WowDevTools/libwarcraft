//
//  CreatureDisplayInfoRecord.cs
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
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	[DatabaseRecord(DatabaseName.CreatureDisplayInfo)]
	public class CreatureDisplayInfoRecord : DBCRecord
	{
		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.CreatureModelData, nameof(ID))]
		public ForeignKey<uint> Model { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.CreatureSoundData, nameof(ID))]
		public ForeignKey<uint> Sound { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.CreatureDisplayInfoExtra, nameof(ID))]
		public ForeignKey<uint> ExtraDisplayInformation { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float Scale { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint Opacity { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public StringReference TextureVariation1 { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public StringReference TextureVariation2 { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public StringReference TextureVariation3 { get; set; }

		[RecordField(WarcraftVersion.BurningCrusade)]
		public StringReference PortraitTexture { get; set; }

		[RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath)]
		public uint SizeClass { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		[ForeignKeyInfo(DatabaseName.UnitBloodLevels, nameof(ID))]
		public ForeignKey<uint> BloodLevel { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.UnitBlood, nameof(ID))]
		public ForeignKey<uint> Blood { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.NPCSounds, nameof(ID))]
		public ForeignKey<uint> NPCSound { get; set; }

		[RecordField(WarcraftVersion.BurningCrusade)]
		public uint ParticleColour { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public uint CreatureGeosetData { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public uint ObjectEffectPackage { get; set; }

		public IReadOnlyList<StringReference> TextureVariations => new[] { this.TextureVariation1, this.TextureVariation2, this.TextureVariation3 };

		/// <inheritdoc />
		public override IEnumerable<StringReference> GetStringReferences()
		{
			var references =  new List<StringReference>
			{
				this.TextureVariation1,
				this.TextureVariation2,
				this.TextureVariation3
			};

			if (this.Version >= WarcraftVersion.BurningCrusade)
			{
				references.Add(this.PortraitTexture);
			}

			return references;
		}
	}
}