//
//  MDXAnimationEvent.cs
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

namespace Warcraft.MDX.Animation
{
    public class MDXAnimationEvent : IVersionedClass
    {
        public string EventName;
        public uint Data;
        public uint Bone;
        public Vector3 RelativePosition;
        public MDXTrack<bool> RaiseEvent;

        public MDXAnimationEvent(BinaryReader br, WarcraftVersion version)
        {
            EventName = new string(br.ReadChars(4));
            Data = br.ReadUInt32();
            Bone = br.ReadUInt32();
            RelativePosition = br.ReadVector3();
            RaiseEvent = br.ReadMDXTrack<bool>(version, true);
        }
    }
}
