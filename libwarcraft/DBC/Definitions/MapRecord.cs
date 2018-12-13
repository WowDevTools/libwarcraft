//
//  MapRecord.cs
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
using System.Numerics;
using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// A database record defining properties for maps.
    /// </summary>
    [DatabaseRecord(DatabaseName.Map)]
    public class MapRecord : DBCRecord
    {
        /// <summary>
        /// The directory under which the map is stored.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public StringReference Directory { get; set; }

        /// <summary>
        /// The type of instance this map is.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint InstanceType { get; set; }

        /// <summary>
        /// Gets or sets the flags for this map record.
        /// </summary>
        [RecordField(WarcraftVersion.Wrath)]
        public uint Flags { get; set; }

        /// <summary>
        /// What sort of PvP the map allows.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public uint PvP { get; set; }

        /// <summary>
        /// The name of the map.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference MapName { get; set; }

        /// <summary>
        /// The minimum level of the map.
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath)]
        public uint MinLevel { get; set; }

        /// <summary>
        /// The maximum level of the map.
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath)]
        public uint MaxLevel { get; set; }

        /// <summary>
        /// The maximum number of players that can be in the map at any one time.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.Wrath, ComesAfter = nameof(RaidOffset))]
        public uint MaxPlayers { get; set; }

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath)]
        public uint Unknown1 { get; set; }

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath)]
        public uint Unknown2 { get; set; }

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath)]
        public uint Unknown3 { get; set; }

        /// <summary>
        /// The ID of the area table entry for this map, which contains more information.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.AreaTable, nameof(ID))]
        public ForeignKey<uint> AreaTableID { get; set; }

        /// <summary>
        /// TODO: Unknown behaviour, improve comment
        /// The description of the map.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference MapDescription1 { get; set; }

        /// <summary>
        /// TODO: Unknown behaviour, improve comment
        /// The description of the map.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        public LocalizedStringReference MapDescription2 { get; set; }

        /// <summary>
        /// The ID of the loading screen for this map.
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [ForeignKeyInfo(DatabaseName.LoadingScreens, nameof(ID))]
        public ForeignKey<uint> LoadingScreenID { get; set; }

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        [RecordField(WarcraftVersion.Classic)]
        [RecordFieldOrder(WarcraftVersion.Wrath, ComesAfter = nameof(ExpansionID))]
        public uint RaidOffset { get; set; }

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.Wrath)]
        public uint Unknown4 { get; set; }

        /// <summary>
        /// TODO: Unknown behaviour
        /// </summary>
        [RecordField(WarcraftVersion.Classic, RemovedIn = WarcraftVersion.BurningCrusade)]
        public uint Unknown5 { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        public float MinimapIconScale { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade, RemovedIn = WarcraftVersion.Wrath)]
        public LocalizedStringReference RequirementText { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade, RemovedIn = WarcraftVersion.Wrath)]
        public LocalizedStringReference HeroicText { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade, RemovedIn = WarcraftVersion.Wrath)]
        public LocalizedStringReference EmptyText2 { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        [ForeignKeyInfo(DatabaseName.Map, nameof(ID))]
        public ForeignKey<int> ParentMap { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        public Vector2 MapEntranceCoordinates { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade, RemovedIn = WarcraftVersion.Wrath)]
        public uint ResetTimeRaid { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade, RemovedIn = WarcraftVersion.Wrath)]
        public uint ResetTimeHeroic { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade, RemovedIn = WarcraftVersion.Wrath)]
        public uint Unknown6 { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint TimeOfDayOveride { get; set; }

        [RecordField(WarcraftVersion.BurningCrusade)]
        public uint ExpansionID { get; set; }

        /// <inheritdoc />
        public override IEnumerable<StringReference> GetStringReferences()
        {
            yield return this.Directory;

            foreach (var localizedMapName in this.MapName.GetReferences())
            {
                yield return localizedMapName;
            }

            foreach (var mapDescription in this.MapDescription1.GetReferences())
            {
                yield return mapDescription;
            }

            foreach (var mapDescription in this.MapDescription2.GetReferences())
            {
                yield return mapDescription;
            }

            if (this.RequirementText != null)
            {
                foreach (var text in this.RequirementText.GetReferences())
                {
                    yield return text;
                }
            }

            if (this.HeroicText != null)
            {
                foreach (var text in this.HeroicText.GetReferences())
                {
                    yield return text;
                }
            }

            if (this.EmptyText2 != null)
            {
                foreach (var text in this.EmptyText2.GetReferences())
                {
                    yield return text;
                }
            }
        }
    }
}
