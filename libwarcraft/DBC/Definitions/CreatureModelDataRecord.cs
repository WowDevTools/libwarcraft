//
//  CreatureModelDataRecord.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Reflection.DBC;
using Warcraft.Core.Structures;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	[DatabaseRecord(DatabaseName.CreatureModelData)]
	public class CreatureModelDataRecord : DBCRecord
	{
		[RecordField(WarcraftVersion.Classic)]
		public uint Flags { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public StringReference ModelPath { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint SizeClass { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float ModelScale { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.UnitBloodLevels, nameof(ID))]
		public ForeignKey<uint> BloodLevel { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.FootprintTextures, nameof(ID))]
		public ForeignKey<uint> FootprintDecal { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float FootprintDecalLength { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float FootprintDecalWidth { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float FootprintDecalParticleScale { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint FoleyMaterialID { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.CameraShakes, nameof(ID))]
		public ForeignKey<uint> FootstepShakeSize { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		[ForeignKeyInfo(DatabaseName.CameraShakes, nameof(ID))]
		public ForeignKey<uint> DeathThudShakeSize { get; set; }

		[RecordField(WarcraftVersion.BurningCrusade)]
		[ForeignKeyInfo(DatabaseName.CreatureSoundData, nameof(ID))]
		public ForeignKey<uint> Sound { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float CollisionWidth { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float CollisionHeight { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float MountHeight { get; set; }

		[RecordField(WarcraftVersion.BurningCrusade)]
		public Box BoundingBox { get; set; }

		[RecordField(WarcraftVersion.BurningCrusade)]
		public float WorldEffectScale { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public float AttachedEffectScale { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public float MissileCollisionRadius { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public float MissileCollisionPush { get; set; }

		[RecordField(WarcraftVersion.Wrath)]
		public float MissileCollisionRaise { get; set; }

		/// <inheritdoc />
		public override IEnumerable<StringReference> GetStringReferences()
		{
			yield return this.ModelPath;
		}
	}
}