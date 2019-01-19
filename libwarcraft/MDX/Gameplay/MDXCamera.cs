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
    /// <summary>
    /// Represents a camera in a model.
    /// </summary>
    public class MDXCamera : IVersionedClass
    {
        /// <summary>
        /// Gets or sets the type lookup index.
        /// </summary>
        public uint TypeLookupIndex { get; set; }

        /// <summary>
        /// Gets or sets the field of view of the camera. This is only present in versions below Cataclysm.
        /// </summary>
        public float FieldOfView { get; set; }

        /// <summary>
        /// Gets or sets the far clipping plane distance.
        /// </summary>
        public float FarClip { get; set; }

        /// <summary>
        /// Gets or sets the near clipping plane distance.
        /// </summary>
        public float NearClip { get; set; }

        /// <summary>
        /// Gets or sets the relative camera position track.
        /// </summary>
        public MDXTrack<SplineKey<Vector3>> Positions { get; set; }

        /// <summary>
        /// Gets or sets the base position of the camera.
        /// </summary>
        public Vector3 PositionBase { get; set; }

        /// <summary>
        /// Gets or sets the relative camera target position track.
        /// </summary>
        public MDXTrack<SplineKey<Vector3>> TargetPositions { get; set; }

        /// <summary>
        /// Gets or sets the base target position.
        /// </summary>
        public Vector3 TargetPositionBase { get; set; }

        /// <summary>
        /// Gets or sets the camera roll track.
        /// </summary>
        public MDXTrack<SplineKey<float>> Roll { get; set; }

        /// <summary>
        /// Gets or sets the camera FOV track. This is only present in Cataclysm and above.
        /// </summary>
        public MDXTrack<SplineKey<float>> AnimatedFOV { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXCamera"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        /// <param name="version">The version to read the instance in the context of.</param>
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
