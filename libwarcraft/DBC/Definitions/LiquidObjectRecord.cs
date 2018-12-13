//
//  AnimationDataRecord.cs
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

using Warcraft.Core;
using Warcraft.Core.Reflection.DBC;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// A database record defining how an in-world liquid behaves.
    /// </summary>
    [DatabaseRecord(DatabaseName.LiquidObject)]
    public class LiquidObjectRecord : DBCRecord
    {
        /// <summary>
        /// The direction in which the liquid flows.
        /// </summary>
        [RecordField(WarcraftVersion.Cataclysm)]
        public float FlowDirection { get; set; }

        /// <summary>
        /// The speed with which the liquid flows.
        /// </summary>
        [RecordField(WarcraftVersion.Cataclysm)]
        public float FlowSpeed { get; set; }

        /// <summary>
        /// The type of liquid. This is a foreign reference to another table.
        /// </summary>
        [RecordField(WarcraftVersion.Cataclysm)]
        [ForeignKeyInfo(DatabaseName.LiquidType, nameof(ID))]
        public ForeignKey<uint> LiquidType { get; set; }

        /// <summary>
        /// Whether or not this liquid is fishable.
        /// </summary>
        [RecordField(WarcraftVersion.Cataclysm)]
        public uint Fishable { get; set; }

        /// <summary>
        /// TODO: Unconfirmed behaviour
        /// The amount light this liquid reflects.
        /// </summary>
        [RecordField(WarcraftVersion.Cataclysm)]
        public uint Reflection { get; set; }
    }
}
