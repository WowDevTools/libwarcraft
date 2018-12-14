//
//  MDXCamera.cs
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
using Warcraft.Core.Structures;
using Warcraft.MDX.Animation;

namespace Warcraft.MDX.Gameplay
{
    public class MDXCamera : IVersionedClass
    {
        public uint TypeLookupIndex;

        // < Cataclysm only
        public float FieldOfView;

        public float FarClip;
        public float NearClip;
        public MDXTrack<SplineKey<Vector3>> Positions;
        public Vector3 PositionBase;

        public MDXTrack<SplineKey<Vector3>> TargetPositions;
        public Vector3 TargetPositionBase;

        public MDXTrack<SplineKey<float>> Roll;

        // => Cataclysm only
        public MDXTrack<SplineKey<float>> AnimatedFOV;

        public MDXCamera(BinaryReader br, WarcraftVersion version)
        {
            TypeLookupIndex = br.ReadUInt32();

            if (version < WarcraftVersion.Cataclysm)
            {
                FieldOfView = br.ReadSingle();
            }

            FarClip = br.ReadSingle();
            NearClip = br.ReadSingle();

            Positions = br.ReadMDXTrack<SplineKey<Vector3>>(version);
            PositionBase = br.ReadVector3();

            TargetPositions = br.ReadMDXTrack<SplineKey<Vector3>>(version);
            TargetPositionBase = br.ReadVector3();

            Roll = br.ReadMDXTrack<SplineKey<float>>(version);

            if (version >= WarcraftVersion.Cataclysm)
            {
                AnimatedFOV = br.ReadMDXTrack<SplineKey<float>>(version);
            }
        }
    }
}
