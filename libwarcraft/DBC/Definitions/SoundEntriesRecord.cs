//
//  SoundEntriesRecord.cs
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
    [DatabaseRecord(DatabaseName.SoundEntries)]
    public class SoundEntriesRecord : DBCRecord
    {
        [RecordField(WarcraftVersion.Classic)]
        public SoundType Type { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public StringReference Name { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 10)]
        public List<StringReference> SoundFiles { get; set; }

        [RecordFieldArray(WarcraftVersion.Classic, Count = 10)]
        public List<uint> PlayFrequencies { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public StringReference DirectoryBase { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public float Volume { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public uint Flags { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public float MinDistance { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        public float DistanceCutoff { get; set; }

        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.SoundProviderPreferences, nameof(ID))]
        public ForeignKey<uint> EAXDefinition { get; set; }

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
