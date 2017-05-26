//
//  BaseSkinSectionIdentifier.cs
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

		public string MajorType { get; }
		public string MinorType { get; }

		/// <summary>
		/// Creates a new <see cref="BaseSkinSectionIdentifier"/> object from an underlying data value.
		/// </summary>
		/// <param name="inValue"></param>
		public BaseSkinSectionIdentifier(ushort inValue)
		{
			if (inValue > 9999)
			{
				throw new ArgumentOutOfRangeException(nameof(inValue), "The input value may not exceed four digits.");
			}

			this.InternalValue = inValue;

			this.MajorType = GetMajorType();
			this.MinorType = GetMinorType(this.MajorType);
		}

		/// <summary>
		/// Gets the major type of the skin section.
		/// </summary>
		/// <returns>The major type.</returns>
		protected virtual string GetMajorType()
		{
			if (this.InternalValue == 0)
			{
				return "Skin";
			}
			if (this.InternalValue > 0 && this.InternalValue < 100)
			{
				return "Hair";
			}
			if (this.InternalValue >= 100 && this.InternalValue < 200)
			{
				return "Beard";
			}
			if (this.InternalValue >= 200 && this.InternalValue < 300)
			{
				return "Moustache";
			}
			if (this.InternalValue >= 300 && this.InternalValue < 400)
			{
				return "Sideburns";
			}
			if (this.InternalValue >= 400 && this.InternalValue < 500)
			{
				return "Glove";
			}
			if (this.InternalValue >= 500 && this.InternalValue < 600)
			{
				return "Boots";
			}
			if (this.InternalValue >= 600 && this.InternalValue < 700)
			{
				return "None";
			}
			if (this.InternalValue >= 700 && this.InternalValue < 800)
			{
				return "Ears";
			}
			if (this.InternalValue >= 800 && this.InternalValue < 900)
			{
				return "Wristbands";
			}
			if (this.InternalValue >= 900 && this.InternalValue < 1000)
			{
				return "Kneepads";
			}
			if (this.InternalValue >= 1000 && this.InternalValue < 1100)
			{
				return "Chest";
			}
			if (this.InternalValue >= 1100 && this.InternalValue < 1200)
			{
				return "Pants";
			}
			if (this.InternalValue >= 1200 && this.InternalValue < 1300)
			{
				return "Tabard";
			}
			if (this.InternalValue >= 1300 && this.InternalValue < 1400)
			{
				return "Trousers";
			}
			if (this.InternalValue >= 1400 && this.InternalValue < 1500)
			{
				return "None";
			}
			if (this.InternalValue >= 1500 && this.InternalValue < 1600)
			{
				return "Cloak";
			}
			if (this.InternalValue >= 1600 && this.InternalValue < 1700)
			{
				return "None";
			}
			if (this.InternalValue >= 1700 && this.InternalValue < 1800)
			{
				return "Eyeglow";
			}
			if (this.InternalValue >= 1800 && this.InternalValue < 1900)
			{
				return "Belt";
			}
			if (this.InternalValue >= 1900 && this.InternalValue < 2000)
			{
				return "Tail";
			}
			if (this.InternalValue >= 2000 && this.InternalValue < 2100)
			{
				return "Feet";
			}
			if (this.InternalValue >= 2100 && this.InternalValue < 2200)
			{
				return "None";
			}
			if (this.InternalValue >= 2200 && this.InternalValue < 2300)
			{
				return "None";
			}
			if (this.InternalValue >= 2300 && this.InternalValue < 2400)
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
			int minorTypeValue = int.Parse(this.InternalValue.ToString("D4").Substring(2));

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
			return $"{this.MajorType} - {this.MinorType}";
		}
	}
}