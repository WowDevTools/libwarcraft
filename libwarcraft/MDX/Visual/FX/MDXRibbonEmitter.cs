//
//  MDXRibbonEmitter.cs
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
using Warcraft.Core.Structures;
using Warcraft.MDX.Animation;
using Warcraft.MDX.Data;

namespace Warcraft.MDX.Visual.FX
{
    /// <summary>
    /// Represents a ribbon emitter.
    /// </summary>
    public class MDXRibbonEmitter : IVersionedClass
    {
        /// <summary>
        /// Gets or sets the ribbon ID.
        /// </summary>
        public uint RibbonID { get; set; }

        /// <summary>
        /// Gets or sets the bone index.
        /// </summary>
        public uint BoneIndex { get; set; }

        /// <summary>
        /// Gets or sets the relative position of the ribbon.
        /// </summary>
        public Vector3 RelativePosition { get; set; }

        /// <summary>
        /// Gets or sets the texture IDs of the ribbon.
        /// </summary>
        public MDXArray<ushort> Textures { get; set; }

        /// <summary>
        /// Gets or sets the materials of the ribbon.
        /// </summary>
        public MDXArray<ushort> Materials { get; set; }

        /// <summary>
        /// Gets or sets the ribbon's colour animation track.
        /// </summary>
        public MDXTrack<RGB> Colour { get; set; }

        /// <summary>
        /// Gets or sets the ribbon's opacity animation track.
        /// </summary>
        public MDXTrack<short> Alpha { get; set; }

        /// <summary>
        /// Gets or sets the ribbon's height above its position.
        /// </summary>
        public MDXTrack<float> HeightAbove { get; set; }

        /// <summary>
        /// Gets or sets the ribbon's height below its position.
        /// </summary>
        public MDXTrack<float> HeightBelow { get; set; }

        /// <summary>
        /// Gets or sets the edges per second of the ribbon.
        /// </summary>
        public float EdgesPerSecond { get; set; }

        /// <summary>
        /// Gets or sets the lifetime of an edge.
        /// </summary>
        public float EdgeLifetime { get; set; }

        /// <summary>
        /// Gets or sets the gravitational constant for the ribbon.
        /// </summary>
        public float Gravity { get; set; }

        /// <summary>
        /// Gets or sets the number of texture tiles in the X direction.
        /// </summary>
        public ushort TextureTileX { get; set; }

        /// <summary>
        /// Gets or sets the number of texture tiles in the Y direction.
        /// </summary>
        public ushort TextureTileY { get; set; }

        /// <summary>
        /// Gets or sets the ribbon's texture slot track.
        /// </summary>
        public MDXTrack<ushort> TextureSlot { get; set; }

        /// <summary>
        /// Gets or sets the ribbon's visibility track.
        /// </summary>
        public MDXTrack<bool> Visibility { get; set; }

        /// <summary>
        /// Gets or sets the priority plane of the ribbon. Probably introduced in Wrath.
        /// </summary>
        public short PriorityPlane { get; set; }

        /// <summary>
        /// Gets or sets an unknown field.
        /// </summary>
        public short Unknown { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXRibbonEmitter"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        /// <param name="version">The version to read the instance in the context of.</param>
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

            if (version < WarcraftVersion.Wrath)
            {
                return;
            }

            PriorityPlane = br.ReadInt16();
            Unknown = br.ReadInt16();
        }
    }
}
