//
//  MDXSkinSection.cs
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

using System.IO;
using System.Numerics;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.MDX.Geometry.Skin
{
    /// <summary>
    /// A section of an <see cref="MDXSkin"/>. Each section is associated with a single material or distinct rendering
    /// operation.
    /// </summary>
    public class MDXSkinSection : IVersionedClass
    {
        /// <summary>
        /// Gets or sets an identifier which specifies what type of skin section this is. It differentiates standard parts, such as
        /// the shell of a pot, from nonstandard parts, such as hair styles, horns, equipment geometry, etc.
        /// </summary>
        public BaseSkinSectionIdentifier SkinSectionID { get; set; }

        /// <summary>
        /// Gets or sets an index modifier. This value acts as the upper 16 bits of a 32-bit index value for some of the indexing
        /// values in the skin section. This is done as a storage saving operation.
        ///
        /// The affected fields are
        ///
        /// <see cref="StartVertexIndex"/>
        /// <see cref="StartTriangleIndex"/>
        /// <see cref="StartBoneIndex"/>
        /// <see cref="CenterBoneIndex"/>
        ///
        /// To combine the level modifier with the value, the following formula is used.
        ///
        /// int finalIndex = (Level &lt;&lt; 16) | originalIndex.
        /// </summary>
        public ushort Level { get; set; }

        /// <summary>
        /// Gets or sets the index of the vertex where this skin section starts. This index is into the local skin vertex list.
        /// </summary>
        public ushort StartVertexIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of vertices used in this skin section.
        /// </summary>
        public ushort VertexCount { get; set; }

        /// <summary>
        /// Gets or sets the index of the triangle where this skin section starts. This index is into the local skin triangle list.
        /// </summary>
        public ushort StartTriangleIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of triangles in this skin section.
        /// </summary>
        public ushort TriangleCount { get; set; }

        /// <summary>
        /// Gets or sets the number of bones in the lookup table which are relevant for this skin section.
        /// </summary>
        public ushort BoneCount { get; set; }

        /// <summary>
        /// Gets or sets the starting index in the <see cref="MDX.Bones"/> where the bones affecting this skin section
        /// are.
        /// </summary>
        public ushort StartBoneIndex { get; set; }

        /// <summary>
        /// Gets or sets the number of bones up the parent chain which influence this skin section.
        /// </summary>
        public ushort InfluencingBonesCount { get; set; }

        /// <summary>
        /// Gets or sets the index of the center bone (the "root" bone) in the bone lookup table.
        /// </summary>
        public ushort CenterBoneIndex { get; set; }

        /// <summary>
        /// Gets or sets the center position of this skin section, in model space.
        /// </summary>
        public Vector3 CenterPosition { get; set; }

        /*
            The following fields are present >= Burning Crusade
        */

        /// <summary>
        /// Gets or sets the center position of this skin section, in model space, which is used for draw sorting.
        /// </summary>
        public Vector3 SortCenterPosition { get; set; }

        /// <summary>
        /// Gets or sets the distance to the furthest vertex from the <see cref="SortCenterPosition"/>.
        /// </summary>
        public float SortRadius { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXSkinSection"/> class. The data is expected to be
        /// 32 or 48 bytes long, depending on the version of the originating file.
        /// </summary>
        /// <param name="data">The data containing the skin section.</param>
        public MDXSkinSection(byte[] data)
        {
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            SkinSectionID = new BaseSkinSectionIdentifier(br.ReadUInt16());
            Level = br.ReadUInt16();
            StartVertexIndex = br.ReadUInt16();
            VertexCount = br.ReadUInt16();
            StartTriangleIndex = br.ReadUInt16();
            TriangleCount = br.ReadUInt16();
            BoneCount = br.ReadUInt16();
            StartBoneIndex = br.ReadUInt16();
            InfluencingBonesCount = br.ReadUInt16();
            CenterBoneIndex = br.ReadUInt16();
            CenterPosition = br.ReadVector3();

            if (br.BaseStream.Length > 32)
            {
                SortCenterPosition = br.ReadVector3();
                SortRadius = br.ReadSingle();
            }
        }

        /// <summary>
        /// Gets the absolute serialized byte size of this class.
        /// </summary>
        /// <param name="version">The version that is contextually relevant.</param>
        /// <returns>The size of a serialized object.</returns>
        public static int GetSize(WarcraftVersion version)
        {
            if (version >= WarcraftVersion.BurningCrusade)
            {
                return 48;
            }

            return 32;
        }
    }
}
