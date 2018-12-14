//
//  Rotator.cs
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
        private Vector3 Values;

        /// <summary>
        /// Pitch of the rotator
        /// </summary>
        public float Pitch
        {
            get => Values.X;
            set => Values.X = value;
        }

        /// <summary>
        /// Yaw of the rotator
        /// </summary>
        public float Yaw
        {
            get => Values.Y;
            set => Values.Y = value;
        }

        /// <summary>
        /// Roll of the rotator
        /// </summary>
        public float Roll
        {
            get => Values.Z;
            set => Values.Z = value;
        }

        /// <summary>
        /// Creates a new rotator object from three floats.
        /// </summary>
        /// <param name="inPitch">Pitch</param>
        /// <param name="inYaw">Yaw</param>
        /// <param name="inRoll">Roll</param>
        public Rotator(float inPitch, float inYaw, float inRoll)
        {
            Values = new Vector3(inPitch, inYaw, inRoll);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rotator"/> struct.
        /// </summary>
        /// <param name="inVector">In vector.</param>
        public Rotator(Vector3 inVector)
            :this(inVector.X, inVector.Y, inVector.Z)
        {

        }

        /// <summary>
        /// Creates a string representation of the current instance.
        /// </summary>
        /// <returns>A string representation of the current instance.</returns>
        public override string ToString()
        {
            return $"Pitch: {Pitch}, Yaw: {Yaw}, Roll: {Roll}";
        }

        public IReadOnlyCollection<float> Flatten()
        {
            return Values.Flatten();
        }
    }
}
