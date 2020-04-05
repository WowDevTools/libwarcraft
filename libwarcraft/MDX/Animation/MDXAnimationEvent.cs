//
//  MDXAnimationEvent.cs
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

namespace Warcraft.MDX.Animation
{
    /// <summary>
    /// Represents an event that is triggered at some point during an animation.
    /// </summary>
    public class MDXAnimationEvent : IVersionedClass
    {
        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets generic data in the event.
        /// </summary>
        public uint Data { get; set; }

        /// <summary>
        /// Gets or sets the bone the event affects.
        /// </summary>
        public uint Bone { get; set; }

        /// <summary>
        /// Gets or sets the relative position from the bone of the event.
        /// </summary>
        public Vector3 RelativePosition { get; set; }

        /// <summary>
        /// Gets or sets a track of points where the event should be raised.
        /// </summary>
        public MDXTrack<bool> RaiseEvent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXAnimationEvent"/> class.
        /// </summary>
        /// <param name="br">The reader.</param>
        /// <param name="version">The version.</param>
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
