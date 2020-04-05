//
//  MDXTrack.cs
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
using Warcraft.Core.Interpolation;
using Warcraft.Core.Structures;
using Warcraft.MDX.Data;

namespace Warcraft.MDX.Animation
{
    /// <summary>
    /// Represents a time-indexed track of variable data.
    /// </summary>
    /// <typeparam name="T">The contained type.</typeparam>
    public class MDXTrack<T> : IVersionedClass
        where T : unmanaged
    {
        /// <summary>
        /// Gets a value indicating whether the timelines are as one composite timeline, or as separate timelines.
        /// </summary>
        public bool IsComposite { get; }

        /// <summary>
        /// Gets the interpolation type of the track.
        /// </summary>
        public InterpolationType Interpolationtype { get; }

        /// <summary>
        /// Gets the global sequence ID.
        /// </summary>
        public ushort GlobalSequenceID { get; }

        /*
            <= BC
        */

        /// <summary>
        /// Gets the composite interpolation ranges.
        /// </summary>
        public MDXArray<IntegerRange>? CompositeTimelineInterpolationRanges { get; }

        /// <summary>
        /// Gets the composite timestamps.
        /// </summary>
        public MDXArray<uint>? CompositeTimelineTimestamps { get; }

        /// <summary>
        /// Gets the composite values.
        /// </summary>
        public MDXArray<T>? CompositeTimelineValues { get; }

        /*
            >= Wrath
        */

        /// <summary>
        /// Gets the timestamps.
        /// </summary>
        public MDXArray<MDXArray<uint>>? Timestamps { get; }

        /// <summary>
        /// Gets the values.
        /// </summary>
        public MDXArray<MDXArray<T>>? Values { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.MDX.Animation.MDXTrack{T}"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        /// <param name="version">The version to read the instance in the context of.</param>
        /// <param name="valueless">
        /// If this value is true, it indicates that no values are associated with
        /// this track, and any value-related reading should be skipped.
        /// </param>
        public MDXTrack(BinaryReader br, WarcraftVersion version, bool valueless = false)
        {
            Interpolationtype = (InterpolationType)br.ReadUInt16();
            GlobalSequenceID = br.ReadUInt16();

            if (version < WarcraftVersion.Wrath)
            {
                IsComposite = true;
                CompositeTimelineInterpolationRanges = br.ReadMDXArray<IntegerRange>();
                CompositeTimelineTimestamps = br.ReadMDXArray<uint>();

                if (valueless)
                {
                    return;
                }

                // HACK: MDXTracks with quaternions need to have the version passed along
                if (typeof(T) == typeof(Quaternion))
                {
                    CompositeTimelineValues = br.ReadMDXArray<T>(version);
                }
                else
                {
                    CompositeTimelineValues = br.ReadMDXArray<T>();
                }
            }
            else
            {
                IsComposite = false;
                Timestamps = br.ReadMDXArray<MDXArray<uint>>();

                if (valueless)
                {
                    return;
                }

                // HACK: MDXTracks with quaternions need to have the version passed along
                if (typeof(T) == typeof(Quaternion))
                {
                    Values = br.ReadMDXArray<MDXArray<T>>(version);
                }
                else
                {
                    Values = br.ReadMDXArray<MDXArray<T>>();
                }
            }
        }
    }
}
