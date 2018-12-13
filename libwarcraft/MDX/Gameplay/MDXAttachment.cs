//
//  MDXAttachment.cs
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
using System.Numerics;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.MDX.Animation;

namespace Warcraft.MDX.Gameplay
{
    public class MDXAttachment : IVersionedClass
    {
        public uint AttachmentIDLookupIndex;
        public ushort Bone;
        public ushort Unknown;
        public Vector3 Position;
        public MDXTrack<bool> AnimateAttached;

        public MDXAttachment(BinaryReader br, WarcraftVersion version)
        {
            this.AttachmentIDLookupIndex = br.ReadUInt32();
            this.Bone = br.ReadUInt16();
            this.Unknown = br.ReadUInt16();
            this.Position = br.ReadVector3();
            this.AnimateAttached = br.ReadMDXTrack<bool>(version);
        }
    }
}
