//
//  MDXBoneFlag.cs
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

namespace Warcraft.MDX.Animation
{
    /// <summary>
    /// Defines various bone flags.
    /// </summary>
    public enum MDXBoneFlag : uint
    {
        /// <summary>
        /// The bone is a spherical billboard.
        /// </summary>
        SphericalBillboard = 0x8,

        /// <summary>
        /// The bone is a cylindrical billboard, rotating around the X axis.
        /// </summary>
        CylindricalBillboardLockedX = 0x10,

        /// <summary>
        /// The bone is a cylindrical billboard, rotating around the Y axis.
        /// </summary>
        CylindricalBillboardLockedY = 0x20,

        /// <summary>
        /// The bone is a cylindrical billboard, rotating around the Z axis.
        /// </summary>
        CylindricalBillboardLockedZ = 0x40,

        /// <summary>
        /// The bone is transformed.
        /// </summary>
        Transformed = 0x200,

        /// <summary>
        /// The bone is a kinematic bone.
        /// </summary>
        KinematicBone = 0x400,

        /// <summary>
        /// The bone has scaled animation.
        /// </summary>
        ScaledAnimation = 0x1000
    }
}
