using System.Collections.Generic;
using System.IO;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// Terrain liquid chunk. Contains information about water and other liquids in a map tile.
    /// </summary>
    public class TerrainLiquidChunk
    {
        public uint WaterInstanceOffset;
        public uint LayerCount;
        public uint AttributesOffset;

        public List<TerrainLiquidInstance> LiquidInstances = new List<TerrainLiquidInstance>();
        public TerrainLiquidAttributes LiquidAttributes;

        public TerrainLiquidChunk(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    WaterInstanceOffset = br.ReadUInt32();
                    LayerCount = br.ReadUInt32();
                    AttributesOffset = br.ReadUInt32();
                }
            }
        }

        /// <summary>
        /// Gets the size of a chunk.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 12;
        }
    }
}
