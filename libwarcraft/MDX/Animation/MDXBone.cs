//
//  MDXBone.cs
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
    public class MDXBone : IVersionedClass
    {
        public int SocketLookupTableIndex;
        public MDXBoneFlag Flags;
        public short ParentBone;
        public ushort SkinSectionID; // Likely not the correct name

        /*
            Only present in Version >= BC. Naming is most likely incorrect.
        */
        public ushort DistanceToFurtherDesc;
        public ushort ZRationOfBoneChain;

        // ...
        public MDXTrack<Vector3> Translation;
        public MDXTrack<Quaternion> Rotation;
        public MDXTrack<Vector3> Scale;

        public Vector3 PivotPoint;

        public MDXBone(BinaryReader br, WarcraftVersion version)
        {
            SocketLookupTableIndex = br.ReadInt32();
            Flags = (MDXBoneFlag) br.ReadUInt32();
            ParentBone = br.ReadInt16();
            SkinSectionID = br.ReadUInt16();

            if (version >= WarcraftVersion.BurningCrusade)
            {
                DistanceToFurtherDesc = br.ReadUInt16();
                ZRationOfBoneChain = br.ReadUInt16();
            }

            Translation = br.ReadMDXTrack<Vector3>(version);
            Rotation = br.ReadMDXTrack<Quaternion>(version);
            Scale = br.ReadMDXTrack<Vector3>(version);

            PivotPoint = br.ReadVector3();
        }
    }
}

