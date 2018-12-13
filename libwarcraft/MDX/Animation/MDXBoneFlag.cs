//
//  MDXBoneFlag.cs
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

namespace Warcraft.MDX.Animation
{
    public enum MDXBoneFlag : uint
    {
        SphericalBillboard            = 0x8,
        CylindricalBillboardLockedX    = 0x10,
        CylindricalBillboardLockedY    = 0x20,
        CylindricalBillboardLockedZ    = 0x40,
        Transformed                    = 0x200,
        KinematicBone                = 0x400,
        ScaledAnimation                = 0x1000
    }
}
