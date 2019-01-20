//
//  MDXTextureType.cs
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

namespace Warcraft.MDX.Visual
{
    /// <summary>
    /// Defines the various texture types.
    /// </summary>
    public enum MDXTextureType : uint
    {
        /// <summary>
        /// A regular object texture.
        /// </summary>
        Regular = 0,

        /// <summary>
        /// A skin texture for a character.
        /// </summary>
        CharacterSkin = 1,

        /// <summary>
        /// A skin texture for some type of equipment.
        /// </summary>
        EquipmentSkin = 2,

        /// <summary>
        /// A reflection from a weapon blade.
        /// </summary>
        WeaponBladeReflection = 3,

        /// <summary>
        /// The handle of a weapon.
        /// </summary>
        WeaponHandle = 4,

        /// <summary>
        /// An environment mapping.
        /// </summary>
        Environment = 5,

        /// <summary>
        /// A hair texture.
        /// </summary>
        Hair = 6,

        /// <summary>
        /// A facial hair texture.
        /// </summary>
        FacialHair = 7,

        /// <summary>
        /// Additional skin textures.
        /// </summary>
        SkinExtra = 8,

        /// <summary>
        /// A UI skin,
        /// </summary>
        UIModelSkin = 9,

        /// <summary>
        /// A tauren mane.
        /// </summary>
        TaurenMane = 10,

        /// <summary>
        /// A monster skin.
        /// </summary>
        MonsterSkin1 = 11,

        /// <summary>
        /// A monster skin.
        /// </summary>
        MonsterSkin2 = 12,

        /// <summary>
        /// A monster skin.
        /// </summary>
        MonsterSkin3 = 13,

        /// <summary>
        /// An item icon.
        /// </summary>
        ItemIcon = 14,

        /// <summary>
        /// The background colour of a guild tabard.
        /// </summary>
        GuildBackgroundColor = 15,

        /// <summary>
        /// The embled colour of a guild tabard.
        /// </summary>
        GuildEmblemColor = 16,

        /// <summary>
        /// The border colour of a guild tabard.
        /// </summary>
        GuildBorderColor = 17,

        /// <summary>
        /// The embled on a guild tabard.
        /// </summary>
        GuildEmblem = 18
    }
}
