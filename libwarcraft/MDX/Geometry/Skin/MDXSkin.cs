//
//  MDXView.cs
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

using System.IO;
using Warcraft.Core.Interfaces;
using Warcraft.MDX.Data;
using Warcraft.MDX.Visual;

namespace Warcraft.MDX.Geometry.Skin
{
    /// <summary>
    /// An <see cref="MDXSkin"/> is a container object for a complete model configuration inside an <see cref="MDX"/>
    /// model. Each skin acts as a LOD (level of detail) instance. Skins, due to the way MDX files are serialized,
    /// are read using an extension to the builtin <see cref="BinaryReader"/>.
    /// </summary>
    public class MDXSkin : IVersionedClass
    {
        /// <summary>
        /// A list of vertices from the global vertex list which are used in this skin.
        /// </summary>
        public MDXArray<ushort> VertexIndices;

        /// <summary>
        /// A list of indices into the <see cref="VertexIndices"/> list, which constitute the triangles in the skin.
        /// Each triplet of indices in this list corresponds to one triangle.
        /// </summary>
        public MDXArray<ushort> Triangles;

        /// <summary>
        /// A list of bone index quantets for the vertices in the skin. The given indices are indexed into the
        /// <see cref="MDX.BoneLookupTable"/>.
        /// </summary>
        public MDXArray<MDXVertexProperty> VertexProperties;

        /// <summary>
        /// A list of <see cref="MDXSkinSection"/> objects, which constitute the parts of the whole skin. These
        /// separations act to divide the skin into different shading zones.
        /// </summary>
        public MDXArray<MDXSkinSection> Sections;

        /// <summary>
        /// A list of <see cref="MDXRenderBatch"/> objects. These render batches act as shader layers for skin sections.
        /// </summary>
        public MDXArray<MDXRenderBatch> RenderBatches;

        /// <summary>
        /// The maximum number of bones in each draw call.
        /// </summary>
        public uint BoneCountMax;
    }
}

