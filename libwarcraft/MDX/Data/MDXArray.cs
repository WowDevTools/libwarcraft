//
//  MDXArray.cs
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Warcraft.Core;
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
        /// Constructs a new in-memory <see cref="MDXArray{T}"/> from a given set of values. This array will not carry
        /// any relevant information as a relative data store, and is instead considered as an abstract storage unit.
        /// </summary>
        /// <param name="values"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MDXArray(IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            Values.AddRange(values);
            Count = (uint)Values.Count;

            ElementsOffset = 0;
            IsFilled = true;
        }

        /// <summary>
        /// Deserializes the information header of an <see cref="MDXArray{T}"/> without reading its values. The values
        /// must be entered into the array using either <see cref="Fill(System.Collections.Generic.ICollection{T})"/> or
        /// <see cref="Fill(System.IO.BinaryReader)"/> before it can be used.
        /// </summary>
        /// <param name="data"></param>
        public MDXArray(byte[] data)
        {
            IsFilled = false;
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    Count = br.ReadUInt32();
                    ElementsOffset = br.ReadUInt32();
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
            Count = br.ReadUInt32();
            ElementsOffset = br.ReadUInt32();

            Fill(br);

            IsFilled = true;
        }

        /// <summary>
        /// Reads an <see cref="MDXArray{T}"/> using a given <see cref="BinaryReader"/>, and fills it with its
        /// referenced values.
        /// </summary>
        /// <param name="br">The reader to use.</param>
        /// <param name="version">The contextually version for the stored objects.</param>
        public MDXArray(BinaryReader br, WarcraftVersion version)
        {
            Count = br.ReadUInt32();
            ElementsOffset = br.ReadUInt32();

            Fill(br, version);

            IsFilled = true;
        }

        /// <summary>
        /// Gets all of the values contained in this <see cref="MDXArray{T}"/>. This copies all of the values
        /// to a new list.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetValues()
        {
            if (!IsFilled)
            {
                throw new InvalidOperationException("The values of the array cannot be accessed before it has been " +
                                                    "filled with its values.");
            }

            return new List<T>(Values);
        }

        /// <summary>
        /// Fills the array using a given collection of values. The collection must have the same number of elements
        /// as the stored number of elements in the <see cref="MDXArray{T}"/> information header.
        /// </summary>
        /// <param name="values"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Fill(ICollection<T> values)
        {
            if (values.Count != Count)
            {
                throw new ArgumentException("The number of elements in the filling collection must be equal to the " +
                                            "stored number of elements in the information header.");
            }

            Values.AddRange(values);
        }

        /// <summary>
        /// Fills the array using the given <see cref="BinaryReader"/>. The position of the reader will not be modified
        /// by this method.
        /// </summary>
        /// <param name="br"></param>
        public void Fill(BinaryReader br)
        {
            long initialPositionBeforeJumpToData = br.BaseStream.Position;
            br.BaseStream.Position = ElementsOffset;

            for (int i = 0; i < Count; ++i)
            {
                Values.Add(br.Read<T>());
            }

            br.BaseStream.Position = initialPositionBeforeJumpToData;

            IsFilled = true;
        }

        /// <summary>
        /// Fills the array with versioned objects using the given <see cref="BinaryReader"/>. The position of the
        /// reader will not be modified by this method.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="version">The contextually relevant version of the stored objects.</param>
        public void Fill(BinaryReader br, WarcraftVersion version)
        {
            long initialPositionBeforeJumpToData = br.BaseStream.Position;
            br.BaseStream.Position = ElementsOffset;

            for (int i = 0; i < Count; ++i)
            {
                Values.Add(br.Read<T>(version));
            }

            br.BaseStream.Position = initialPositionBeforeJumpToData;

            IsFilled = true;
        }

        /// <summary>
        /// Gets or sets a value in the array at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public T this[int index]
        {
            get
            {
                if (!IsFilled)
                {
                    throw new InvalidOperationException("The values of the array cannot be accessed before it has been " +
                                                        "filled with its values.");
                }

                return Values[index];
            }
            set
            {
                if (!IsFilled)
                {
                    throw new InvalidOperationException("The values of the array cannot be accessed before it has been " +
                                                        "filled with its values.");
                }

                Values[index] = value;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the underlying values.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (!IsFilled)
            {
                throw new InvalidOperationException("The enumerator of the array cannot be accessed before it has been " +
                                                    "filled with its values.");
            }
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

