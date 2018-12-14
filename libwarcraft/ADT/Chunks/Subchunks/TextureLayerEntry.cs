using System.IO;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// Texture layer entry, representing a ground texture in the chunk.
    /// </summary>
    public class TextureLayerEntry
    {
        /// <summary>
        /// Gets or sets index of the texture in the MTEX chunk.
        /// </summary>
        public uint TextureID { get; set; }

        /// <summary>
        /// Gets or sets flags for the texture. Used for animation data.
        /// </summary>
        public TextureLayerFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the offset into MCAL where the alpha map begins.
        /// </summary>
        public uint AlphaMapOffset { get; set; }

        /// <summary>
        /// Gets or sets the ground effect ID. This is a foreign key entry into GroundEffectTexture::ID.
        /// </summary>
        public ForeignKey<ushort> EffectID { get; set; }

        /// <summary>
        /// Gets or sets a currently unused value.
        /// </summary>
        public ushort Unused { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureLayerEntry"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        public TextureLayerEntry(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    TextureID = br.ReadUInt32();
                    Flags = (TextureLayerFlags)br.ReadUInt32();
                    AlphaMapOffset = br.ReadUInt32();

                    EffectID = new ForeignKey<ushort>(DatabaseName.GroundEffectTexture, nameof(DBCRecord.ID), br.ReadUInt16()); // TODO: Implement GroundEffectTextureRecord
                }
            }
        }

        /// <summary>
        /// Gets the size of a texture layer entry.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 16;
        }
    }
}
