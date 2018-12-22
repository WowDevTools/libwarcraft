//
//  ModelPlacementFlags.cs
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

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// Flags for the model.
    /// </summary>
    [Flags]
    public enum ModelPlacementFlags : ushort
    {
        /// <summary>
        /// Biodome. Perhaps a skybox?
        /// </summary>
        Biodome = 1,

        /// <summary>
        /// Possibly used for vegetation and grass.
        /// </summary>
        Shrubbery = 2,
    }
}
