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
        private readonly ushort InternalValue;

        public string MajorType => GetMajorType();
        public string MinorType => GetMinorType(MajorType);

        /// <summary>
        /// Creates a new <see cref="BaseSkinSectionIdentifier"/> object from an underlying data value.
        /// </summary>
        /// <param name="inValue"></param>
        public BaseSkinSectionIdentifier(ushort inValue)
        {
            if (inValue > 9999)
            {
                throw new ArgumentOutOfRangeException(nameof(inValue), $"The input value may not exceed four digits. Given value was {inValue}");
            }

            InternalValue = inValue;
        }

        /// <summary>
        /// Gets the major type of the skin section.
        /// </summary>
        /// <returns>The major type.</returns>
        protected virtual string GetMajorType()
        {
            if (InternalValue == 0)
            {
                return "Skin";
            }
            if (InternalValue > 0 && InternalValue < 100)
            {
                return "Hair";
            }
            if (InternalValue >= 100 && InternalValue < 200)
            {
                return "Beard";
            }
            if (InternalValue >= 200 && InternalValue < 300)
            {
                return "Moustache";
            }
            if (InternalValue >= 300 && InternalValue < 400)
            {
                return "Sideburns";
            }
            if (InternalValue >= 400 && InternalValue < 500)
            {
                return "Glove";
            }
            if (InternalValue >= 500 && InternalValue < 600)
            {
                return "Boots";
            }
            if (InternalValue >= 600 && InternalValue < 700)
            {
                return "None";
            }
            if (InternalValue >= 700 && InternalValue < 800)
            {
                return "Ears";
            }
            if (InternalValue >= 800 && InternalValue < 900)
            {
                return "Wristbands";
            }
            if (InternalValue >= 900 && InternalValue < 1000)
            {
                return "Kneepads";
            }
            if (InternalValue >= 1000 && InternalValue < 1100)
            {
                return "Chest";
            }
            if (InternalValue >= 1100 && InternalValue < 1200)
            {
                return "Pants";
            }
            if (InternalValue >= 1200 && InternalValue < 1300)
            {
                return "Tabard";
            }
            if (InternalValue >= 1300 && InternalValue < 1400)
            {
                return "Trousers";
            }
            if (InternalValue >= 1400 && InternalValue < 1500)
            {
                return "None";
            }
            if (InternalValue >= 1500 && InternalValue < 1600)
            {
                return "Cloak";
            }
            if (InternalValue >= 1600 && InternalValue < 1700)
            {
                return "None";
            }
            if (InternalValue >= 1700 && InternalValue < 1800)
            {
                return "Eyeglow";
            }
            if (InternalValue >= 1800 && InternalValue < 1900)
            {
                return "Belt";
            }
            if (InternalValue >= 1900 && InternalValue < 2000)
            {
                return "Tail";
            }
            if (InternalValue >= 2000 && InternalValue < 2100)
            {
                return "Feet";
            }
            if (InternalValue >= 2100 && InternalValue < 2200)
            {
                return "None";
            }
            if (InternalValue >= 2200 && InternalValue < 2300)
            {
                return "None";
            }
            if (InternalValue >= 2300 && InternalValue < 2400)
            {
                return "Elf Hands";
            }

            return "Unknown";
        }

        /// <summary>
        /// Gets the minor type of this skin section.
        /// </summary>
        /// <returns>The minor type.</returns>
        protected virtual string GetMinorType(string majorType)
        {
            int minorTypeValue = int.Parse(InternalValue.ToString("D4").Substring(2));

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

        /// <summary>
        /// Converts the object into a string, representing its values in a human-readable format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{MajorType} - {MinorType}";
        }
    }
}
