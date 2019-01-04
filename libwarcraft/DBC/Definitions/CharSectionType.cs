//
//  CharSectionType.cs
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

namespace Warcraft.DBC.Definitions
{
    /// <summary>
    /// Defines the general type of the section.
    /// </summary>
    public enum CharSectionType
    {
        /// <summary>
        /// The section defines the base skin.
        /// </summary>
        BaseSkin,

        /// <summary>
        /// The section defines the face.
        /// </summary>
        Face,

        /// <summary>
        /// The section defines facial hair.
        /// </summary>
        FacialHair,

        /// <summary>
        /// The section defines hair.
        /// </summary>
        Hair,

        /// <summary>
        /// The section defines underwear.
        /// </summary>
        Underwear,

        /// <summary>
        /// The section defines the base skin, in high resolution.
        /// </summary>
        BaseSkinHiDef,

        /// <summary>
        /// The section defines the face, in high resolution.
        /// </summary>
        FaceHiDef,

        /// <summary>
        /// The section defines facial hair, in high resolution.
        /// </summary>
        FacialHairHiDef,

        /// <summary>
        /// The section defines hair, in high resolution.
        /// </summary>
        HairHiDef,

        /// <summary>
        /// The section defines underwear, in high resolution.
        /// </summary>
        UnderwearHiDef,

        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown1,

        /// <summary>
        /// The section defines a demon hunter tattoo.
        /// </summary>
        DemonHunterTattoo
    }
}
