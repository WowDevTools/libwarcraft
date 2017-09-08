//
//  SoundProviderPreferences.cs
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
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	[DatabaseRecord(DatabaseName.SoundProviderPreferences)]
	public class SoundProviderPreferencesRecord : DBCRecord
	{
		[RecordField(WarcraftVersion.Classic)]
		public StringReference Description { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint Flags { get; set; }

		[RecordField(WarcraftVersion.BurningCrusade)]
		public uint EAXEnvironmentSelection { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAXEffectVolume { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAXDecayTime { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX2EnvironmentSize { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX2EvironmentDiffusion { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint EAX2Room { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint EAX2RoomHF { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX2DecayHFRatio { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint EAX2Reflections { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX2ReflectionsDelay { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint EAX2Reverb { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX2ReverbDelay { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX2RoomRolloff { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX2AirAbsorption { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public uint EAX3RoomLF { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX3DecayLFRatio { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX3EchoTime { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX3EchoDepth { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX3ModulationTime { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX3ModulationDepth { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX3HFReference { get; set; }

		[RecordField(WarcraftVersion.Classic)]
		public float EAX3LFReference { get; set; }

		public override IEnumerable<StringReference> GetStringReferences()
		{
			yield return this.Description;
		}
	}
}