using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Structures;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.ADT.Chunks
{
    public class TerrainLiquidInstance
    {
        /// <summary>
        /// The type of the liquid. Foreign key reference to LiquidTypeRec::ID.
        /// </summary>
        public ForeignKey<ushort> LiquidType;

        /// <summary>
        /// The liquid object. Foreign key reference to LiquidObjectRec::ID.
        /// </summary>
        public ForeignKey<ushort> LiquidObject;

        public Range HeightLevelRange;

        public byte XLiquidOffset;
        public byte YLiquidOffset;
        public byte Width;
        public byte Height;

        /// <summary>
        /// Offset to an 8 by 8 map of bit boolean values that determine whether or not
        /// an instance is filled or not. If the offset is 0, all tiles are filled.
        /// </summary>
        public uint LiquidBooleanMapOffset;
        public List<List<bool>> BooleanMap = new List<List<bool>>();

        /// <summary>
        /// Offset to the vertex data of this liquid instance. The format of the data is determined
        /// in LiquidMaterialRec::LiquidVertexFormat via LiquidTypeRec::MaterialID. As a side note,
        /// oceans (<see cref="LiquidType"/> = 2) have a hardcoded check that the LiquidVertexFormat also equals 2.
        ///
        /// Since the vertex data format is undetermined (and as such the length is also undetermined) without
        /// a DBC entry, it's up to external programs to read the correct values. You can also use the GameWorld wrapper
        /// class.
        /// </summary>
        public uint VertexDataOffset;
        public LiquidVertexData VertexData;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainLiquidInstance"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        public TerrainLiquidInstance(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
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
