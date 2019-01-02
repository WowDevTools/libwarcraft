//
//  SplineKey.cs
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
using Warcraft.Core.Extensions;

namespace Warcraft.Core.Structures
{
    /// <summary>
    /// Represents a key value in a spline.
    /// </summary>
    /// <typeparam name="T">The type of the key.</typeparam>
    public class SplineKey<T>
    {
        /// <summary>
        /// Gets or sets the value of the key.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets or sets the in tangent of the key.
        /// </summary>
        public T InTangent { get; set; }

        /// <summary>
        /// Gets or sets the out tangent of the key.
        /// </summary>
        public T OutTangent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplineKey{T}"/> class.
        /// </summary>
        /// <param name="br">The reader to use when reading the key.</param>
        public SplineKey(BinaryReader br)
        {
            Value = br.Read<T>();
            InTangent = br.Read<T>();
            OutTangent = br.Read<T>();
        }
    }
}
