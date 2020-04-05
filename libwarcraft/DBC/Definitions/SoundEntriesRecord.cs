//
//  SoundEntriesRecord.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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
    /// Defines a database entry containing properties of a set of sounds.
    /// </summary>
    [DatabaseRecord(DatabaseName.SoundEntries)]
    public class SoundEntriesRecord : DBCRecord
    {
        /// <summary>
        /// Gets or sets the type of the sound set.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public SoundType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the sound set.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the sounds included in the sound set.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 10)]
        public List<StringReference> SoundFiles { get; set; } = null!;

        /// <summary>
        /// Gets or sets the playback frequencies of the sounds.
        /// </summary>
        [RecordFieldArray(WarcraftVersion.Classic, Count = 10)]
        public List<uint> PlayFrequencies { get; set; } = null!;

        /// <summary>
        /// Gets or sets the base directory of the sound references.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference DirectoryBase { get; set; } = null!;

        /// <summary>
        /// Gets or sets the volume of the sound set.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float Volume { get; set; }

        /// <summary>
        /// Gets or sets the flags of the sound.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint Flags { get; set; }

        /// <summary>
        /// Gets or sets the minimum distance that the sound is audible at.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float MinDistance { get; set; }

        /// <summary>
        /// Gets or sets the cutoff distance of the sound.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public float DistanceCutoff { get; set; }

        /// <summary>
        /// Gets or sets the extended audio effects definition.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundProviderPreferences, nameof(ID))]
        public ForeignKey<uint> EAXDefinition { get; set; } = null!;

        /// <summary>
        /// Gets or sets the advanced properties ID.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint SoundEntriesAdvancedID { get; set; }

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            yield return Name;
            yield return DirectoryBase;
            foreach (var soundFile in SoundFiles)
            {
                yield return soundFile;
            }
        }
    }
}
