//
//  SoundProviderPreferencesRecord.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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
    /// <summary>
    /// Defines reverb settings for a location, using the Environmental Effects Extension.
    /// </summary>
    [DatabaseRecord(DatabaseName.SoundProviderPreferences)]
    public class SoundProviderPreferencesRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the description of the reverb settings.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference Description { get; set; }

        /// <summary>
        /// Gets or sets the flags of the reverb settings.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Flags { get; set; }

        /// <summary>
        /// Gets or sets the environment selection.
        /// </summary>
        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint EAXEnvironmentSelection { get; set; }

        /// <summary>
        /// Gets or sets the effect volume.
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.BurningCrusade)]
        public float EAXEffectVolume { get; set; }

        /// <summary>
        /// Gets or sets the decay time.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAXDecayTime { get; set; }

        /// <summary>
        /// Gets or sets the environment size.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX2EnvironmentSize { get; set; }

        /// <summary>
        /// Gets or sets the environment diffusion.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX2EvironmentDiffusion { get; set; }

        /// <summary>
        /// Gets or sets the room type.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint EAX2Room { get; set; }

        /// <summary>
        /// Gets or sets the high-frequency room type.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint EAX2RoomHF { get; set; }

        /// <summary>
        /// Gets or sets the high-frequency decay ratio.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX2DecayHFRatio { get; set; }

        /// <summary>
        /// Gets or sets the reflection level.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint EAX2Reflections { get; set; }

        /// <summary>
        /// Gets or sets the reflection delay.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX2ReflectionsDelay { get; set; }

        /// <summary>
        /// Gets or sets the reverb level.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint EAX2Reverb { get; set; }

        /// <summary>
        /// Gets or sets the reverb delay.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX2ReverbDelay { get; set; }

        /// <summary>
        /// Gets or sets the rolloff factor of the room.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX2RoomRolloff { get; set; }

        /// <summary>
        /// Gets or sets the air absorption level.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX2AirAbsorption { get; set; }

        /// <summary>
        /// Gets or sets the low-frequency room.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint EAX3RoomLF { get; set; }

        /// <summary>
        /// Gets or sets the low-frequency decay ratio.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX3DecayLFRatio { get; set; }

        /// <summary>
        /// Gets or sets the echo time.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX3EchoTime { get; set; }

        /// <summary>
        /// Gets or sets the echo depth.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX3EchoDepth { get; set; }

        /// <summary>
        /// Gets or sets the modulation time.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX3ModulationTime { get; set; }

        /// <summary>
        /// Gets or sets the modulation depth.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX3ModulationDepth { get; set; }

        /// <summary>
        /// Gets or sets the reference level for high frequencies.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX3HFReference { get; set; }

        /// <summary>
        /// Gets or sets the reference level for low frequencies.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float EAX3LFReference { get; set; }

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            yield return Description;
        }
    }
}
