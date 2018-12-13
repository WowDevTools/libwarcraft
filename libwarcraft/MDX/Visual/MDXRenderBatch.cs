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
        /// A set of flags, defining operations on this layer.
        /// </summary>
        public EMDXRenderBatchFlags Flags;

        /// <summary>
        /// The rendering priority plane of this layer.
        /// </summary>
        public sbyte PriorityPlane;

        /// <summary>
        /// The shader ID for this layer.
        /// </summary>
        public ushort ShaderID;

        /// <summary>
        /// The index of the skin section which this batch belongs to.
        /// </summary>
        public ushort SkinSectionIndex;

        /// <summary>
        /// The index of the geoset which this batch belongs to.
        /// </summary>
        public ushort GeosetIndex;

        /// <summary>
        /// The index of a colour animation track which this batch should use. -1 denotes no colour.
        /// This is an index into <see cref="MDX.ColourAnimations"/>
        /// </summary>
        public short ColorIndex;

        /// <summary>
        /// The index of the material to use for this batch. This is an index into <see cref="MDX.Materials"/>.
        /// </summary>
        public ushort MaterialIndex;

        /// <summary>
        /// The layer that this batch is on. Used when multiple batches are assigned to one skin section to determine
        /// the order in which the batches should be applied.
        /// </summary>
        public ushort MaterialLayer;

        /// <summary>
        /// The number of textures used in this batch. This is also the number of textures to load, starting at
        /// <see cref="TextureLookupTableIndex"/> in <see cref="MDX.TextureLookupTable"/>.
        /// </summary>
        public ushort TextureCount;

        /// <summary>
        /// The start index of the set of textures associated in this batch.
        /// This is an index into <see cref="MDX.TextureLookupTable"/>.
        /// </summary>
        public ushort TextureLookupTableIndex;

        /// <summary>
        /// The start index of the shader texture slot to attach the textures to. This is usually -1, 0 or 1. -1 denotes
        /// environment mapping. This is an index into <see cref="MDX.TextureMappingLookupTable"/>.
        /// </summary>
        public ushort TextureMappingLookupTableIndex;

        /// <summary>
        /// The start index of the texture weights used for the textures. This is an index into
        /// <see cref="MDX.TransparencyLookupTable"/>.
        /// </summary>
        public ushort TransparencyLookupTableIndex;

        /// <summary>
        /// The start index of the UV coordinate animation tracks used for the textures. This is an index into
        /// <see cref="MDX.TextureTransformLookupTable"/>
        /// </summary>
        public ushort TextureTransformLookupTableIndex;

        /// <summary>
        /// Deserializes a new <see cref="MDXRenderBatch"/> object from given binary data.
        /// </summary>
        /// <param name="data">The binary data containing the batch.</param>
        public MDXRenderBatch(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    this.Flags = (EMDXRenderBatchFlags)br.ReadByte();
                    this.PriorityPlane = br.ReadSByte();

                    this.ShaderID = br.ReadUInt16();

                    this.SkinSectionIndex = br.ReadUInt16();
                    this.GeosetIndex = br.ReadUInt16();
                    this.ColorIndex = br.ReadInt16();
                    this.MaterialIndex = br.ReadUInt16();
                    this.MaterialLayer = br.ReadUInt16();
                    this.TextureCount = br.ReadUInt16();

                    this.TextureLookupTableIndex = br.ReadUInt16();
                    this.TextureMappingLookupTableIndex = br.ReadUInt16();
                    this.TransparencyLookupTableIndex = br.ReadUInt16();
                    this.TextureTransformLookupTableIndex = br.ReadUInt16();
                }
            }
        }

        /// <summary>
        /// Gets the absolute byte size of a serialized object.
        /// </summary>
        /// <returns></returns>
        public static int GetSize()
        {
            return 24;
        }
    }
}

