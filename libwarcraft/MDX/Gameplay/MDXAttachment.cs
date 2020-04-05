//
//  MDXAttachment.cs
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
using Warcraft.MDX.Animation;

namespace Warcraft.MDX.Gameplay
{
    /// <summary>
    /// Represents an attachment to a model.
    /// </summary>
    public class MDXAttachment : IVersionedClass
    {
        /// <summary>
        /// Gets or sets the attachment lookup index.
        /// </summary>
        public uint AttachmentIDLookupIndex { get; set; }

        /// <summary>
        /// Gets or sets the bone the attachment is attached to.
        /// </summary>
        public ushort Bone { get; set; }

        /// <summary>
        /// Gets or sets an unknown value.
        /// </summary>
        public ushort Unknown { get; set; }

        /// <summary>
        /// Gets or sets the relative position of the attachment.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets a track which indicates if the attachment should be animated.
        /// </summary>
        public MDXTrack<bool> AnimateAttached { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXAttachment"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        /// <param name="version">The version to read the instance in the context of.</param>
        public MDXAttachment(BinaryReader br, WarcraftVersion version)
        {
            AttachmentIDLookupIndex = br.ReadUInt32();
            Bone = br.ReadUInt16();
            Unknown = br.ReadUInt16();
            Position = br.ReadVector3();
            AnimateAttached = br.ReadMDXTrack<bool>(version);
        }
    }
}
