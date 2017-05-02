//
//  MDXArray.cs
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Extensions;

namespace Warcraft.MDX.Data
{
	/// <summary>
	/// Represents an array of values, referenced by an element count and a byte offset to where the elements are
	/// stored.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MDXArray<T> : IEnumerable<T>
	{
		/// <summary>
		/// The number of elements in the array.
		/// </summary>
		public uint Count;

		/// <summary>
		/// The byte offset where the elements are stored.
		/// </summary>
		public uint ElementsOffset;

		/// <summary>
		/// The values of the array.
		/// </summary>
		private readonly List<T> Values = new List<T>();

		/// <summary>
		/// Whether or not the array has been filled with its values.
		/// </summary>
		private bool IsFilled;

		/// <summary>
		/// Deserializes the information header of an <see cref="MDXArray{T}"/> without reading its values. The values
		/// must be entered into the array using either <see cref="Fill(System.Collections.Generic.ICollection{T})"/> or
		/// <see cref="Fill(System.IO.BinaryReader)"/> before it can be used.
		/// </summary>
		/// <param name="data"></param>
		public MDXArray(byte[] data)
		{
			this.IsFilled = false;
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Count = br.ReadUInt32();
					this.ElementsOffset = br.ReadUInt32();
				}
			}
		}

		/// <summary>
		/// Reads an <see cref="MDXArray{T}"/> using a given <see cref="BinaryReader"/>, and fills it with its
		/// referenced values.
		/// </summary>
		/// <param name="br">The reader to use.</param>
		public MDXArray(BinaryReader br)
		{
			this.Count = br.ReadUInt32();
			this.ElementsOffset = br.ReadUInt32();

			Fill(br);

			this.IsFilled = true;
		}

		/// <summary>
		/// Fills the array using a given collection of values. The collection must have the same number of elements
		/// as the stored number of elements in the <see cref="MDXArray{T}"/> information header.
		/// </summary>
		/// <param name="values"></param>
		/// <exception cref="ArgumentException"></exception>
		public void Fill(ICollection<T> values)
		{
			if (values.Count != this.Count)
			{
				throw new ArgumentException("The number of elements in the filling collection must be equal to the " +
				                            "stored number of elements in the information header.");
			}

			this.Values.AddRange(values);
		}

		/// <summary>
		/// Fills the array using the given <see cref="BinaryReader"/>. The position of the reader will not be modified
		/// by this method.
		/// </summary>
		/// <param name="br"></param>
		public void Fill(BinaryReader br)
		{
			long initialPositionBeforeJumpToData = br.BaseStream.Position;
			br.BaseStream.Position = this.ElementsOffset;

			for (int i = 0; i < this.Count; ++i)
			{
				this.Values.Add(br.Read<T>());
			}

			br.BaseStream.Position = initialPositionBeforeJumpToData;

			this.IsFilled = true;
		}

		/// <summary>
		/// Gets or sets a value in the array at the specified index.
		/// </summary>
		/// <param name="index"></param>
		public T this[int index]
		{
			get
			{
				if (!this.IsFilled)
				{
					throw new InvalidOperationException("The values of the array cannot be accessed before it has been " +
					                                    "filled with its values.");
				}

				return this.Values[index];
			}
			set
			{
				if (!this.IsFilled)
				{
					throw new InvalidOperationException("The values of the array cannot be accessed before it has been " +
					                                    "filled with its values.");
				}

				this.Values[index] = value;
			}
		}

		/// <summary>
		/// Returns an enumerator that iterates through the underlying values.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			if (!this.IsFilled)
			{
				throw new InvalidOperationException("The enumerator of the array cannot be accessed before it has been " +
				                                    "filled with its values.");
			}
			return this.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

