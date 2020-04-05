//
//  PlainSoundEmitter.cs
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

using System;
using System.Numerics;
using Warcraft.Core.Interfaces;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// Represents a sound emitter.
    /// </summary>
    public class PlainSoundEmitter : SoundEmitter, IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the ID of the sound.
        /// </summary>
        public uint SoundID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the sound's name.
        /// </summary>
        public uint SoundNameID { get; set; }

        /// <summary>
        /// Gets or sets the position of the sound.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the inner attenuation radius.
        /// </summary>
        public float AttenuationRadiusStart { get; set; }

        /// <summary>
        /// Gets or sets the outer attenuation radius.
        /// </summary>
        public float AttenuationRadiusEnd { get; set; }

        /// <summary>
        /// Gets or sets the absolute cutoff distance.
        /// </summary>
        public float CutoffDistance { get; set; }

        /// <summary>
        /// Gets or sets the start time of the emitter.
        /// </summary>
        public ushort StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the emitter.
        /// </summary>
        public ushort EndTime { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the sound group's silence.
        /// </summary>
        public ushort GroupSilenceMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the sound group's silence.
        /// </summary>
        public ushort GroupSilenceMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the playing instance count.
        /// </summary>
        public ushort PlayInstancesMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the playing instance count.
        /// </summary>
        public ushort PlayInstancesMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of times the emitter will loop.
        /// </summary>
        public ushort LoopCountMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of times the emitter will loop.
        /// </summary>
        public ushort LoopCountMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum time gap between play cycles.
        /// </summary>
        public ushort InterSoundGapMin { get; set; }

        /// <summary>
        /// Gets or sets the maximum time gap between play cycles.
        /// </summary>
        public ushort InterSoundMapMax { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
