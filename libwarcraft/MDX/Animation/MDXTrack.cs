//
//  MDXTrack.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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

using Warcraft.Core.Interpolation;
using System.IO;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.MDX.Data;

namespace Warcraft.MDX.Animation
{
	public class MDXTrack<T> : IVersionedClass
	{
		public InterpolationType Interpolationtype;
		public short GlobalSequenceID;

		/*
			<= BC
		*/

		public readonly MDXArray<IntegerRange> CompositeTimelineInterpolationRanges;
		public readonly MDXArray<uint> CompositeTimelineTimestamps;
		public readonly MDXArray<T> CompositeTimelineValues;

		/*
			>= Wrath
		*/

		public readonly MDXArray<MDXArray<uint>> Timestamps;
		public readonly MDXArray<MDXArray<T>> Values;

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.MDX.Animation.MDXTrack{T}"/> class.
		/// </summary>
		/// <param name="br"></param>
		/// <param name="version">Format.</param>
		public MDXTrack(BinaryReader br, WarcraftVersion version)
		{
			this.Interpolationtype = (InterpolationType)br.ReadInt16();
			this.GlobalSequenceID = br.ReadInt16();

			if (version < WarcraftVersion.Wrath)
			{
				this.CompositeTimelineInterpolationRanges = br.ReadMDXArray<IntegerRange>();
				this.CompositeTimelineTimestamps = br.ReadMDXArray<uint>();
				this.CompositeTimelineValues = br.ReadMDXArray<T>();
			}
			else
			{
				this.Timestamps = br.ReadMDXArray<MDXArray<uint>>();
				this.Values = br.ReadMDXArray<MDXArray<T>>();
			}
		}
	}
}

