//
//  BaseSkinSectionIdentifier.cs
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

namespace Warcraft.MDX.Geometry.Skin
{
    /// <summary>
    /// Encapsulates a skin section identifier value, and presents its meaning in a more human-readable format. Value
    /// definitions are taken from <a href="https://wowdev.wiki/M2/.skin#Submeshes"/>, and carry generic meanings in
    /// this class.
    ///
    /// The class can be inherited from, and override the meanings of the numeric values to ease human understanding of
    /// nongeneric section identifiers.
    /// </summary>
    public class BaseSkinSectionIdentifier
    {
        private readonly ushort _internalValue;

        /// <summary>
        /// Gets the major type of the skin.
        /// </summary>
        public string MajorType => GetMajorType();

        /// <summary>
        /// Gets the minor type of the skin.
        /// </summary>
        public string MinorType => GetMinorType(MajorType);

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSkinSectionIdentifier"/> class.
        /// </summary>
        /// <param name="inValue">The value.</param>
        public BaseSkinSectionIdentifier(ushort inValue)
        {
            if (inValue > 9999)
            {
                throw new ArgumentOutOfRangeException(nameof(inValue), $"The input value may not exceed four digits. Given value was {inValue}");
            }

            _internalValue = inValue;
        }

        /// <summary>
        /// Gets the major type of the skin section.
        /// </summary>
        /// <returns>The major type.</returns>
        protected virtual string GetMajorType()
        {
            if (_internalValue == 0)
            {
                return "Skin";
            }

            if (_internalValue > 0 && _internalValue < 100)
            {
                return "Hair";
            }

            if (_internalValue >= 100 && _internalValue < 200)
            {
                return "Beard";
            }

            if (_internalValue >= 200 && _internalValue < 300)
            {
                return "Moustache";
            }

            if (_internalValue >= 300 && _internalValue < 400)
            {
                return "Sideburns";
            }

            if (_internalValue >= 400 && _internalValue < 500)
            {
                return "Glove";
            }

            if (_internalValue >= 500 && _internalValue < 600)
            {
                return "Boots";
            }

            if (_internalValue >= 600 && _internalValue < 700)
            {
                return "None";
            }

            if (_internalValue >= 700 && _internalValue < 800)
            {
                return "Ears";
            }

            if (_internalValue >= 800 && _internalValue < 900)
            {
                return "Wristbands";
            }

            if (_internalValue >= 900 && _internalValue < 1000)
            {
                return "Kneepads";
            }

            if (_internalValue >= 1000 && _internalValue < 1100)
            {
                return "Chest";
            }

            if (_internalValue >= 1100 && _internalValue < 1200)
            {
                return "Pants";
            }

            if (_internalValue >= 1200 && _internalValue < 1300)
            {
                return "Tabard";
            }

            if (_internalValue >= 1300 && _internalValue < 1400)
            {
                return "Trousers";
            }

            if (_internalValue >= 1400 && _internalValue < 1500)
            {
                return "None";
            }

            if (_internalValue >= 1500 && _internalValue < 1600)
            {
                return "Cloak";
            }

            if (_internalValue >= 1600 && _internalValue < 1700)
            {
                return "None";
            }

            if (_internalValue >= 1700 && _internalValue < 1800)
            {
                return "Eyeglow";
            }

            if (_internalValue >= 1800 && _internalValue < 1900)
            {
                return "Belt";
            }

            if (_internalValue >= 1900 && _internalValue < 2000)
            {
                return "Tail";
            }

            if (_internalValue >= 2000 && _internalValue < 2100)
            {
                return "Feet";
            }

            if (_internalValue >= 2100 && _internalValue < 2200)
            {
                return "None";
            }

            if (_internalValue >= 2200 && _internalValue < 2300)
            {
                return "None";
            }

            if (_internalValue >= 2300 && _internalValue < 2400)
            {
                return "Elf Hands";
            }

            return "Unknown";
        }

        /// <summary>
        /// Gets the minor type of this skin section.
        /// </summary>
        /// <param name="majorType">The major type.</param>
        /// <returns>The minor type.</returns>
        protected virtual string GetMinorType(string majorType)
        {
            var minorTypeValue = int.Parse(_internalValue.ToString("D4").Substring(2));

            switch (majorType)
            {
                case "Skin":
                {
                    return "Simple";
                }

                case "Cloak":
                case "Hair":
                case "Beard":
                case "Glove":
                case "Boots":
                case "Elf Hands":
                case "Tail":
                {
                    break;
                }

                case "Moustache":
                case "Sideburns":
                case "Chest":
                case "Ears":
                {
                    if (minorTypeValue == 1)
                    {
                        return "None";
                    }

                    break;
                }

                case "Wristbands":
                {
                    if (minorTypeValue == 1)
                    {
                        return "None";
                    }

                    if (minorTypeValue == 2)
                    {
                        return "Normal";
                    }

                    if (minorTypeValue == 3)
                    {
                        return "Ruffled";
                    }

                    break;
                }

                case "Kneepads":
                {
                    if (minorTypeValue == 1)
                    {
                        return "None";
                    }

                    if (minorTypeValue == 2)
                    {
                        return "Long";
                    }

                    if (minorTypeValue == 3)
                    {
                        return "Short";
                    }

                    break;
                }

                case "Pants":
                {
                    if (minorTypeValue == 1)
                    {
                        return "Regular";
                    }

                    if (minorTypeValue == 2)
                    {
                        return "Short Skirt";
                    }

                    if (minorTypeValue == 4)
                    {
                        return "Armored";
                    }

                    break;
                }

                case "Tabard":
                {
                    if (minorTypeValue == 1)
                    {
                        return "None";
                    }

                    if (minorTypeValue == 2)
                    {
                        return "Full";
                    }

                    break;
                }

                case "Trousers":
                {
                    if (minorTypeValue == 1)
                    {
                        return "Legs";
                    }

                    if (minorTypeValue == 2)
                    {
                        return "Dress";
                    }

                    break;
                }

                case "Eyeglows":
                {
                    if (minorTypeValue == 1)
                    {
                        return "None";
                    }

                    if (minorTypeValue == 2)
                    {
                        return "Elf";
                    }

                    if (minorTypeValue == 3)
                    {
                        return "Death Knight";
                    }

                    break;
                }

                case "Belt":
                {
                    if (minorTypeValue == 1)
                    {
                        return "None";
                    }

                    if (minorTypeValue == 2)
                    {
                        return "Bulky";
                    }

                    break;
                }

                case "Feet":
                {
                    if (minorTypeValue == 1)
                    {
                        return "None";
                    }

                    if (minorTypeValue == 2)
                    {
                        return "Feet";
                    }

                    break;
                }

                default:
                {
                    return "Unknown";
                }
            }

            return $"{majorType} {minorTypeValue}";
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{MajorType} - {MinorType}";
        }
    }
}
