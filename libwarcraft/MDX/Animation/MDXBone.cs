//
//  MDXBone.cs
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

namespace Warcraft.MDX.Animation
{
    /// <summary>
    /// Represents a bone in a model.
    /// </summary>
    public class MDXBone : IVersionedClass
    {
        /// <summary>
        /// Gets or sets the socket lookup table index.
        /// </summary>
        public int SocketLookupTableIndex { get; set; }

        /// <summary>
        /// Gets or sets the bone flags.
        /// </summary>
        public MDXBoneFlag Flags { get; set; }

        /// <summary>
        /// Gets or sets the parent bone, or -1 if it is the root bone.
        /// </summary>
        public short ParentBone { get; set; }

        /// <summary>
        /// Gets or sets the skin section the bone belongs to.
        /// </summary>
        public ushort SkinSectionID { get; set; } // Likely not the correct name

        /*
            Only present in Version >= BC. Naming is most likely incorrect.
        */

        /// <summary>
        /// Gets or sets the CRC of the bone name.
        /// </summary>
        public uint BoneNameCRC { get; set; }

        // ...

        /// <summary>
        /// Gets or sets the translation track of the bone.
        /// </summary>
        public MDXTrack<Vector3> Translation { get; set; }

        /// <summary>
        /// Gets or sets the rotation track of the bone.
        /// </summary>
        public MDXTrack<Quaternion> Rotation { get; set; }

        /// <summary>
        /// Gets or sets the scale track of the bone.
        /// </summary>
        public MDXTrack<Vector3> Scale { get; set; }

        /// <summary>
        /// Gets or sets the pivot point of the bone.
        /// </summary>
        public Vector3 PivotPoint { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXBone"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        /// <param name="version">The version to read the instance in the context of.</param>
        public MDXBone(BinaryReader br, WarcraftVersion version)
        {
            SocketLookupTableIndex = br.ReadInt32();
            Flags = (MDXBoneFlag)br.ReadUInt32();
            ParentBone = br.ReadInt16();
            SkinSectionID = br.ReadUInt16();

            if (version >= WarcraftVersion.BurningCrusade)
            {
                BoneNameCRC = br.ReadUInt32();
            }

            Translation = br.ReadMDXTrack<Vector3>(version);
            Rotation = br.ReadMDXTrack<Quaternion>(version);
            Scale = br.ReadMDXTrack<Vector3>(version);

            PivotPoint = br.ReadVector3();
        }
    }
}
