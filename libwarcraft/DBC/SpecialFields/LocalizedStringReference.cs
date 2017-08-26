//
//  MapRecord.cs
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
using System.Collections.Generic;
using System.Linq;
using Warcraft.Core;

namespace Warcraft.DBC.SpecialFields
{
	/// <summary>
	/// A localize (that is, translated) reference to a string in the database record.
	/// </summary>
	public class LocalizedStringReference
	{
		/// <summary>
		/// The reference to the English version of the string.
		/// </summary>
		public StringReference English;

		/// <summary>
		/// The reference to the Korean version of the string.
		/// </summary>
		public StringReference Korean;

		/// <summary>
		/// The reference to the French version of the string.
		/// </summary>
		public StringReference French;

		/// <summary>
		/// The reference to the German version of the string.
		/// </summary>
		public StringReference German;

		/// <summary>
		/// The reference to the Chinese version of the string.
		/// </summary>
		public StringReference Chinese;

		/// <summary>
		/// The reference to the Taiwan version of the string.
		/// </summary>
		public StringReference Taiwan;

		/// <summary>
		/// The reference to the Spanish version of the string.
		/// </summary>
		public StringReference Spanish;

		/// <summary>
		/// The reference to the Mexican Spanish version of the string.
		/// </summary>
		public StringReference SpanishMexican;

		/// <summary>
		/// The reference to the Russian version of the string.
		/// </summary>
		public StringReference Russian;

		/// <summary>
		/// The reference to an unknown version of the string.
		/// </summary>
		public StringReference Unknown1;

		/// <summary>
		/// The reference to the Portugese version of the string.
		/// </summary>
		public StringReference Portugese;

		/// <summary>
		/// The reference to the Italian version of the string.
		/// </summary>
		public StringReference Italian;

		/// <summary>
		/// The reference to an unknown version of the string.
		/// </summary>
		public StringReference Unknown2;

		/// <summary>
		/// The reference to an unknown version of the string.
		/// </summary>
		public StringReference Unknown3;

		/// <summary>
		/// The reference to an unknown version of the string.
		/// </summary>
		public StringReference Unknown4;

		/// <summary>
		/// The reference to an unknown version of the string.
		/// </summary>
		public StringReference Unknown5;

		public uint Flags;

		public StringReference ClientLocale;

		/// <summary>
		/// Gets the actual localized references.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<StringReference> GetReferences()
		{
			return new[]
			{
				this.English,
				this.Korean,
				this.French,
				this.German,
				this.Chinese,
				this.Taiwan,
				this.Spanish,
				this.SpanishMexican,
				this.Russian,
				this.Unknown1,
				this.Portugese,
				this.Italian,
				this.Unknown2,
				this.Unknown3,
				this.Unknown4,
				this.Unknown5,
				this.ClientLocale
			}.Where(s => s != null);
		}

		/// <summary>
		/// Gets the number of fields expected to be in a reference by version. This is equal to LanguageCount + 1,
		/// that is, including the flag field.
		/// </summary>
		/// <param name="version"></param>
		/// <returns></returns>
		public static int GetFieldCount(WarcraftVersion version)
		{
			if (version >= WarcraftVersion.Cataclysm)
			{
				return 1;
			}

			switch (version)
			{
				case WarcraftVersion.Wrath:
				{
					return 17;
				}
				case WarcraftVersion.BurningCrusade:
				{
					return 10;
				}
				case WarcraftVersion.Classic:
				{
					return 9;
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(version));
				}
			}
		}
	}
}