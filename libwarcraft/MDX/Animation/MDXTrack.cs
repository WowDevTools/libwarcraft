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
using System;
using Warcraft.Core.Interpolation;
using System.Collections.Generic;
using System.IO;
using Warcraft.Core;

namespace Warcraft.MDX.Animation
{
	public class MDXTrack<T>
	{
		public InterpolationType Interpolationtype;
		public short GlobalSequenceID;

		/*
			<= BC
			Read these values and fill in the lists with
			the data.
		*/

		public readonly MDXArray<KeyValuePair<int, int>> InterpolationRanges;

		/*
			>= Wrath
			Read the lists directly.
		*/
		public readonly MDXArray<MDXArray<int>> Timestamps;
		public readonly MDXArray<MDXArray<T>> Values;

		/// <summary>
		/// Initializes a new instance of the <see cref="Warcraft.MDX.Animation.MDXTrack</c>"/> class.
		/// This class fires off a new BinaryReader, as it outreferences values elsewhere in the file.
		/// The value references in the track are not filled, as they can be a number of different types.
		/// When used, you must fill the values yourself after the creation of the track.
		/// </summary>
		/// <param name="data">Data.</param>
		/// <param name="filePath">File path to the M2 file.</param>
		/// <param name="Format">Format.</param>
		public MDXTrack(byte[] data, string filePath, WarcraftVersion Format)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					Interpolationtype = (InterpolationType)br.ReadInt16();
					GlobalSequenceID = br.ReadInt16();

					if (Format < WarcraftVersion.Wrath)
					{
						InterpolationRanges = new MDXArray<KeyValuePair<int, int>>(br.ReadBytes(8));
						Timestamps = new MDXArray<MDXArray<int>>(br.ReadBytes(8));
						Values = new MDXArray<MDXArray<T>>(br.ReadBytes(8));
					}
					else
					{
						throw new NotImplementedException();
					}
				}
			}
		}
	}
}

