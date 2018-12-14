//
//  MDXLight.cs
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

namespace Warcraft.MDX.Visual
{
    public class MDXLight : IVersionedClass
    {
        public MDXLightType Type;
        public short Bone; // -1 if not attached to bone
        public Vector3 Position;
        public MDXTrack<RGB> AmbientColour;
        public MDXTrack<float> AmbientIntensity;
        public MDXTrack<RGB> DiffuseColour;
        public MDXTrack<float> DiffuseIntensity;
        public MDXTrack<float> AttenuationStart;
        public MDXTrack<float> AttenuationEnd;
        public MDXTrack<bool> Visibility;

        public MDXLight(BinaryReader br, WarcraftVersion version)
        {
            Type = (MDXLightType)br.ReadUInt16();
            Bone = br.ReadInt16();
            Position = br.ReadVector3();

            AmbientColour = br.ReadMDXTrack<RGB>(version);
            AmbientIntensity = br.ReadMDXTrack<float>(version);
            DiffuseColour = br.ReadMDXTrack<RGB>(version);
            DiffuseIntensity = br.ReadMDXTrack<float>(version);
            AttenuationStart = br.ReadMDXTrack<float>(version);
            AttenuationEnd = br.ReadMDXTrack<float>(version);

            Visibility = br.ReadMDXTrack<bool>(version);
        }
    }
}
