//
//  MDXRibbonEmitter.cs
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
using Warcraft.MDX.Data;

namespace Warcraft.MDX.Visual.FX
{
    public class MDXRibbonEmitter : IVersionedClass
    {
        public uint RibbonID;
        public uint BoneIndex;
        public Vector3 RelativePosition;
        public MDXArray<ushort> Textures;
        public MDXArray<ushort> Materials;
        public MDXTrack<RGB> Colour;
        public MDXTrack<short> Alpha;
        public MDXTrack<float> HeightAbove;
        public MDXTrack<float> HeightBelow;
        public float EdgesPerSecond;
        public float EdgeLifetime;
        public float Gravity;
        public ushort TextureTileX;
        public ushort TextureTileY;
        public MDXTrack<ushort> TextureSlot;
        public MDXTrack<bool> Visibility;

        // >= Wrath (probably, needs verification)
        public short PriorityPlane;
        public short Unknown;

        public MDXRibbonEmitter(BinaryReader br, WarcraftVersion version)
        {
            RibbonID = br.ReadUInt32();
            BoneIndex = br.ReadUInt32();
            RelativePosition = br.ReadVector3();

            Textures = br.ReadMDXArray<ushort>();
            Materials = br.ReadMDXArray<ushort>();

            Colour = br.ReadMDXTrack<RGB>(version);
            Alpha = br.ReadMDXTrack<short>(version);
            HeightAbove = br.ReadMDXTrack<float>(version);
            HeightBelow = br.ReadMDXTrack<float>(version);

            EdgesPerSecond = br.ReadSingle();
            EdgeLifetime = br.ReadSingle();
            Gravity = br.ReadSingle();

            TextureTileX = br.ReadUInt16();
            TextureTileY = br.ReadUInt16();

            TextureSlot = br.ReadMDXTrack<ushort>(version);
            Visibility = br.ReadMDXTrack<bool>(version);

            if (version >= WarcraftVersion.Wrath)
            {
                PriorityPlane = br.ReadInt16();
                Unknown = br.ReadInt16();
            }
        }
    }
}

