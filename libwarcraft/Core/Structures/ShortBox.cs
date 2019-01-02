//
//  ShortBox.cs
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

using System.Collections.Generic;
using System.Linq;
using Warcraft.Core.Interfaces;

namespace Warcraft.Core.Structures
{
    /// <summary>
    /// A structure representing an axis-aligned bounding box, comprised of two <see cref="Vector3s"/> objects
    /// defining the bottom and top corners of the box.
    /// </summary>
    public struct ShortBox : IFlattenableData<short>
    {
        /// <summary>
        /// The bottom corner of the bounding box.
        /// </summary>
        public Vector3s BottomCorner;

        /// <summary>
        /// The top corner of the bounding box.
        /// </summary>
        public Vector3s TopCorner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortBox"/> struct from a top and bottom corner.
        /// </summary>
        /// <param name="inBottomCorner">The bottom corner of the box.</param>
        /// <param name="inTopCorner">The top corner of the box.</param>
        /// <returns>A new <see cref="Box"/> object.</returns>
        public ShortBox(Vector3s inBottomCorner, Vector3s inTopCorner)
        {
            BottomCorner = inBottomCorner;
            TopCorner = inTopCorner;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<short> Flatten()
        {
            return BottomCorner.Flatten().Concat(TopCorner.Flatten()).ToArray();
        }
    }
}
