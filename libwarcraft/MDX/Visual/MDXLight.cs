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
    /// <summary>
    /// Represents a light source in a model.
    /// </summary>
    public class MDXLight : IVersionedClass
    {
        /// <summary>
        /// Gets or sets the light type.
        /// </summary>
        public MDXLightType Type { get; set; }

        /// <summary>
        /// Gets or sets the bone the light is attached to. -1 means no attachment.
        /// </summary>
        public short Bone { get; set; }

        /// <summary>
        /// Gets or sets the relative position of the light.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the ambient colour track.
        /// </summary>
        public MDXTrack<RGB> AmbientColour { get; set; }

        /// <summary>
        /// Gets or sets the ambient intensity track.
        /// </summary>
        public MDXTrack<float> AmbientIntensity { get; set; }

        /// <summary>
        /// Gets or sets the diffuse colour track.
        /// </summary>
        public MDXTrack<RGB> DiffuseColour { get; set; }

        /// <summary>
        /// Gets or sets the diffuse intensity track.
        /// </summary>
        public MDXTrack<float> DiffuseIntensity { get; set; }

        /// <summary>
        /// Gets or sets the attenuation start distance track.
        /// </summary>
        public MDXTrack<float> AttenuationStart { get; set; }

        /// <summary>
        /// Gets or sets the attenuation end distance track.
        /// </summary>
        public MDXTrack<float> AttenuationEnd { get; set; }

        /// <summary>
        /// Gets or sets the visibility track.
        /// </summary>
        public MDXTrack<bool> Visibility { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXLight"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        /// <param name="version">The version to read the instance in the context of.</param>
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
