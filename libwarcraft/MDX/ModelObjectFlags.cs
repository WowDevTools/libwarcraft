//
//  MDXFlags.cs
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

namespace Warcraft.MDX
{
    [Flags]
    public enum ModelObjectFlags : uint
    {
        TiltX                        = 0x1,
        TiltY                        = 0x2,
        HasBlendModeOverrides        = 0x8,
        HasPhysicsData                = 0x20,
        HasSkinLODs                    = 0x80,
        UnknownCameraFlag            = 0x100
    }
}

