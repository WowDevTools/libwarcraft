//
//  MDXArray.cs
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
    /// <typeparam name="T">The contained type.</typeparam>
    public class MDXArray<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets or sets the number of elements in the array.
        /// </summary>
        public uint Count { get; set; }

        /// <summary>
        /// Gets or sets the byte offset where the elements are stored.
        /// </summary>
        public uint ElementsOffset { get; set; }

        /// <summary>
        /// The values of the array.
        /// </summary>
        private readonly List<T> _values = new List<T>();

        /// <summary>
        /// Whether or not the array has been filled with its values.
        /// </summary>
        private bool _isFilled;

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXArray{T}"/> class.
        /// Constructs a new in-memory <see cref="MDXArray{T}"/> from a given set of values. This array will not carry
        /// any relevant information as a relative data store, and is instead considered as an abstract storage unit.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <exception cref="ArgumentNullException">Thrown if the values are null.</exception>
        public MDXArray(IEnumerable<T> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            _values.AddRange(values);
            Count = (uint)_values.Count;

            ElementsOffset = 0;
            _isFilled = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXArray{T}"/> class.
        /// Deserializes the information header of an <see cref="MDXArray{T}"/> without reading its values. The values
        /// must be entered into the array using either <see cref="Fill(System.Collections.Generic.ICollection{T})"/> or
        /// <see cref="Fill(System.IO.BinaryReader)"/> before it can be used.
        /// </summary>
        /// <param name="data">The data.</param>
        public MDXArray(byte[] data)
        {
            _isFilled = false;
            using var ms = new MemoryStream(data);
            using var br = new BinaryReader(ms);
            Count = br.ReadUInt32();
            ElementsOffset = br.ReadUInt32();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXArray{T}"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        public MDXArray(BinaryReader br)
        {
            Count = br.ReadUInt32();
            ElementsOffset = br.ReadUInt32();

            Fill(br);

            _isFilled = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXArray{T}"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        /// <param name="version">The version to read the instance in the context of.</param>
        public MDXArray(BinaryReader br, WarcraftVersion version)
        {
            Count = br.ReadUInt32();
            ElementsOffset = br.ReadUInt32();

            Fill(br, version);

            _isFilled = true;
        }

        /// <summary>
        /// Gets all of the values contained in this <see cref="MDXArray{T}"/>. This copies all of the values
        /// to a new list.
        /// </summary>
        /// <returns>The values.</returns>
        public IEnumerable<T> GetValues()
        {
            if (!_isFilled)
            {
                throw new InvalidOperationException("The values of the array cannot be accessed before it has been " +
                                                    "filled with its values.");
            }

            return new List<T>(_values);
        }

        /// <summary>
        /// Fills the array using a given collection of values. The collection must have the same number of elements
        /// as the stored number of elements in the <see cref="MDXArray{T}"/> information header.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <exception cref="ArgumentException">Thrown if the element count does not match.</exception>
        public void Fill(ICollection<T> values)
        {
            if (values.Count != Count)
            {
                throw new ArgumentException("The number of elements in the filling collection must be equal to the " +
                                            "stored number of elements in the information header.");
            }

            _values.AddRange(values);
        }

        /// <summary>
        /// Fills the array using the given <see cref="BinaryReader"/>. The position of the reader will not be modified
        /// by this method.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        public void Fill(BinaryReader br)
        {
            var initialPositionBeforeJumpToData = br.BaseStream.Position;
            br.BaseStream.Position = ElementsOffset;

            for (var i = 0; i < Count; ++i)
            {
                _values.Add(br.Read<T>());
            }

            br.BaseStream.Position = initialPositionBeforeJumpToData;

            _isFilled = true;
        }

        /// <summary>
        /// Fills the array with versioned objects using the given <see cref="BinaryReader"/>. The position of the
        /// reader will not be modified by this method.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        /// <param name="version">The version to read the instance in the context of.</param>
        public void Fill(BinaryReader br, WarcraftVersion version)
        {
            var initialPositionBeforeJumpToData = br.BaseStream.Position;
            br.BaseStream.Position = ElementsOffset;

            for (var i = 0; i < Count; ++i)
            {
                _values.Add(br.Read<T>(version));
            }

            br.BaseStream.Position = initialPositionBeforeJumpToData;

            _isFilled = true;
        }

        /// <summary>
        /// Gets or sets a value in the array at the specified index.
        /// </summary>
        /// <param name="index">The element index.</param>
        public T this[int index]
        {
            get
            {
                if (!_isFilled)
                {
                    throw new InvalidOperationException("The values of the array cannot be accessed before it has been " +
                                                        "filled with its values.");
                }

                return _values[index];
            }

            set
            {
                if (!_isFilled)
                {
                    throw new InvalidOperationException("The values of the array cannot be accessed before it has been " +
                                                        "filled with its values.");
                }

                _values[index] = value;
            }
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            if (!_isFilled)
            {
                throw new InvalidOperationException("The enumerator of the array cannot be accessed before it has been " +
                                                    "filled with its values.");
            }

            return _values.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
