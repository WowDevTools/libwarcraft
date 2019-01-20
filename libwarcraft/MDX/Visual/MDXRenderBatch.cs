//
//  MDXRenderBatch.cs
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

using System.IO;
using Warcraft.MDX.Geometry.Skin;

namespace Warcraft.MDX.Visual
{
    /// <summary>
    /// An <see cref="MDXRenderBatch"/> is a layer in the shading pipeline of an <see cref="MDX"/> model, associated
    /// with a single <see cref="MDXSkinSection"/>. Multiple batches may be associated with a single skin, and will
    /// then be layered on top of each other, ordered by <see cref="MaterialLayer"/>.
    /// </summary>
    public class MDXRenderBatch
    {
        /// <summary>
        /// Gets or sets a set of flags, defining operations on this layer.
        /// </summary>
        public MDXRenderBatchFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the rendering priority plane of this layer.
        /// </summary>
        public sbyte PriorityPlane { get; set; }

        /// <summary>
        /// Gets or sets the shader ID for this layer.
        /// </summary>
        public ushort ShaderID { get; set; }

        /// <summary>
        /// Gets or sets the index of the skin section which this batch belongs to.
        /// </summary>
        public ushort SkinSectionIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the geoset which this batch belongs to.
        /// </summary>
        public ushort GeosetIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of a colour animation track which this batch should use. -1 denotes no colour.
        /// This is an index into <see cref="MDX.ColourAnimations"/>.
        /// </summary>
        public short ColorIndex { get; set; }

        /// <summary>
        /// Gets or sets the index of the material to use for this batch. This is an index into <see cref="MDX.Materials"/>.
        /// </summary>
        public ushort MaterialIndex { get; set; }

        /// <summary>
        /// Gets or sets the layer that this batch is on. Used when multiple batches are assigned to one skin section to determine
        /// the order in which the batches should be applied.
        /// </summary>
        public ushort MaterialLayer { get; set; }

        /// <summary>
        /// Gets or sets the number of textures used in this batch. This is also the number of textures to load, starting at
        /// <see cref="TextureLookupTableIndex"/> in <see cref="MDX.TextureLookupTable"/>.
        /// </summary>
        public ushort TextureCount { get; set; }

        /// <summary>
        /// Gets or sets the start index of the set of textures associated in this batch.
        /// This is an index into <see cref="MDX.TextureLookupTable"/>.
        /// </summary>
        public ushort TextureLookupTableIndex { get; set; }

        /// <summary>
        /// Gets or sets the start index of the shader texture slot to attach the textures to. This is usually -1, 0 or 1. -1 denotes
        /// environment mapping. This is an index into <see cref="MDX.TextureMappingLookupTable"/>.
        /// </summary>
        public ushort TextureMappingLookupTableIndex { get; set; }

        /// <summary>
        /// Gets or sets the start index of the texture weights used for the textures. This is an index into
        /// <see cref="MDX.TransparencyLookupTable"/>.
        /// </summary>
        public ushort TransparencyLookupTableIndex { get; set; }

        /// <summary>
        /// Gets or sets the start index of the UV coordinate animation tracks used for the textures. This is an index into
        /// <see cref="MDX.TextureTransformationLookupTable"/>.
        /// </summary>
        public ushort TextureTransformLookupTableIndex { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXRenderBatch"/> class.
        /// </summary>
        /// <param name="data">The binary data containing the batch.</param>
        public MDXRenderBatch(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Flags = (MDXRenderBatchFlags)br.ReadByte();
                    PriorityPlane = br.ReadSByte();

                    ShaderID = br.ReadUInt16();

                    SkinSectionIndex = br.ReadUInt16();
                    GeosetIndex = br.ReadUInt16();
                    ColorIndex = br.ReadInt16();
                    MaterialIndex = br.ReadUInt16();
                    MaterialLayer = br.ReadUInt16();
                    TextureCount = br.ReadUInt16();

                    TextureLookupTableIndex = br.ReadUInt16();
                    TextureMappingLookupTableIndex = br.ReadUInt16();
                    TransparencyLookupTableIndex = br.ReadUInt16();
                    TextureTransformLookupTableIndex = br.ReadUInt16();
                }
            }
        }

        /// <summary>
        /// Gets the absolute byte size of a serialized object.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 24;
        }
    }
}
