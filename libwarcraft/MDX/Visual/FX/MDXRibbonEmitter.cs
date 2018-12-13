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
            this.RibbonID = br.ReadUInt32();
            this.BoneIndex = br.ReadUInt32();
            this.RelativePosition = br.ReadVector3();

            this.Textures = br.ReadMDXArray<ushort>();
            this.Materials = br.ReadMDXArray<ushort>();

            this.Colour = br.ReadMDXTrack<RGB>(version);
            this.Alpha = br.ReadMDXTrack<short>(version);
            this.HeightAbove = br.ReadMDXTrack<float>(version);
            this.HeightBelow = br.ReadMDXTrack<float>(version);

            this.EdgesPerSecond = br.ReadSingle();
            this.EdgeLifetime = br.ReadSingle();
            this.Gravity = br.ReadSingle();

            this.TextureTileX = br.ReadUInt16();
            this.TextureTileY = br.ReadUInt16();

            this.TextureSlot = br.ReadMDXTrack<ushort>(version);
            this.Visibility = br.ReadMDXTrack<bool>(version);

            if (version >= WarcraftVersion.Wrath)
            {
                this.PriorityPlane = br.ReadInt16();
                this.Unknown = br.ReadInt16();
            }
        }
    }
}

