//
//  TerrainLiquidInstance.cs
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
using System.IO;
using Warcraft.Core.Structures;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// Represents an instance of a liquid layer in a terrain tile.
    /// </summary>
    public class TerrainLiquidInstance
    {
        /// <summary>
        /// Gets or sets the type of the liquid. Foreign key reference to LiquidTypeRec::ID.
        /// </summary>
        public ForeignKey<ushort> LiquidType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the liquid object. Foreign key reference to LiquidObjectRec::ID.
        /// </summary>
        public ForeignKey<ushort> LiquidObject { get; set; } = null!;

        /// <summary>
        /// Gets or sets the height level range.
        /// </summary>
        public Range HeightLevelRange { get; set; }

        /// <summary>
        /// Gets or sets the X offset of the liquid.
        /// </summary>
        public byte XLiquidOffset { get; set; }

        /// <summary>
        /// Gets or sets the Y offset of the liquid.
        /// </summary>
        public byte YLiquidOffset { get; set; }

        /// <summary>
        /// Gets or sets the width of the liquid.
        /// </summary>
        public byte Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the liquid.
        /// </summary>
        public byte Height { get; set; }

        /// <summary>
        /// Gets or sets the offset to an 8 by 8 map of bit boolean values that determine whether or not an instance is
        /// filled or not. If the offset is 0, all tiles are filled.
        /// </summary>
        public uint LiquidBooleanMapOffset { get; set; }

        /// <summary>
        /// Gets or sets the fill map.
        /// </summary>
        public List<List<bool>> BooleanMap { get; set; } = new List<List<bool>>();

        /// <summary>
        /// Gets or sets the offset to the vertex data of this liquid instance. The format of the data is determined
        /// in LiquidMaterialRec::LiquidVertexFormat via LiquidTypeRec::MaterialID. As a side note,
        /// oceans (<see cref="LiquidType"/> = 2) have a hardcoded check that the LiquidVertexFormat also equals 2.
        ///
        /// Since the vertex data format is undetermined (and as such the length is also undetermined) without
        /// a DBC entry, it's up to external programs to read the correct values. You can also use the GameWorld wrapper
        /// class.
        /// </summary>
        public uint VertexDataOffset { get; set; }

        /// <summary>
        /// Gets or sets the vertex data.
        /// </summary>
        public LiquidVertexData? VertexData { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainLiquidInstance"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        public TerrainLiquidInstance(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    LiquidType = new ForeignKey<ushort>(DatabaseName.LiquidType, nameof(DBCRecord.ID), br.ReadUInt16());
                    LiquidObject = new ForeignKey<ushort>(DatabaseName.LiquidObject, nameof(DBCRecord.ID), br.ReadUInt16());

                    HeightLevelRange = new Range(br.ReadSingle(), br.ReadSingle());

                    XLiquidOffset = br.ReadByte();
                    YLiquidOffset = br.ReadByte();
                    Width = br.ReadByte();
                    Height = br.ReadByte();

                    LiquidBooleanMapOffset = br.ReadUInt32();
                    VertexDataOffset = br.ReadUInt32();

                    // TODO: Read boolean map

                    // TODO: [#9] Accept DBC liquid type and read vertex data
                }
            }
        }

        /// <summary>
        /// Gets the size of a liquid instance block.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 24;
        }
    }
}
