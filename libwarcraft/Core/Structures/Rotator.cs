//
//  Rotator.cs
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

using System.Collections.Generic;
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.Core.Structures
{
    /// <summary>
    /// A structure representing a three-dimensional collection of euler angles.
    /// </summary>
    public struct Rotator : IFlattenableData<float>
    {
        private Vector3 _values;

        /// <summary>
        /// Gets or sets the pitch of the rotator.
        /// </summary>
        public float Pitch
        {
            readonly get => _values.X;
            set => _values.X = value;
        }

        /// <summary>
        /// Gets or sets the yaw of the rotator.
        /// </summary>
        public float Yaw
        {
            readonly get => _values.Y;
            set => _values.Y = value;
        }

        /// <summary>
        /// Gets or sets the roll of the rotator.
        /// </summary>
        public float Roll
        {
            readonly get => _values.Z;
            set => _values.Z = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rotator"/> struct.
        /// </summary>
        /// <param name="inPitch">The pitch.</param>
        /// <param name="inYaw">The yaw.</param>
        /// <param name="inRoll">The roll.</param>
        public Rotator(float inPitch, float inYaw, float inRoll)
        {
            _values = new Vector3(inPitch, inYaw, inRoll);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rotator"/> struct.
        /// </summary>
        /// <param name="inVector">In vector.</param>
        public Rotator(Vector3 inVector)
            : this(inVector.X, inVector.Y, inVector.Z)
        {
        }

        /// <inheritdoc />
        public override readonly string ToString()
        {
            return $"Pitch: {Pitch}, Yaw: {Yaw}, Roll: {Roll}";
        }

        /// <inheritdoc />
        public readonly IReadOnlyCollection<float> Flatten()
        {
            return _values.Flatten();
        }
    }
}
