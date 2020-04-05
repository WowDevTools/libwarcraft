//
//  LocalizedStringReference.cs
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
        /// Gets or sets the reference to the English version of the string.
        /// </summary>
        public StringReference English { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to the Korean version of the string.
        /// </summary>
        public StringReference Korean { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to the French version of the string.
        /// </summary>
        public StringReference French { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to the German version of the string.
        /// </summary>
        public StringReference German { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to the Chinese version of the string.
        /// </summary>
        public StringReference Chinese { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to the Taiwan version of the string.
        /// </summary>
        public StringReference Taiwan { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to the Spanish version of the string.
        /// </summary>
        public StringReference Spanish { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to the Mexican Spanish version of the string.
        /// </summary>
        public StringReference SpanishMexican { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to the Russian version of the string.
        /// </summary>
        public StringReference Russian { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to an unknown version of the string.
        /// </summary>
        public StringReference Unknown1 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to the Portugese version of the string.
        /// </summary>
        public StringReference Portugese { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to the Italian version of the string.
        /// </summary>
        public StringReference Italian { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to an unknown version of the string.
        /// </summary>
        public StringReference Unknown2 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to an unknown version of the string.
        /// </summary>
        public StringReference Unknown3 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to an unknown version of the string.
        /// </summary>
        public StringReference Unknown4 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reference to an unknown version of the string.
        /// </summary>
        public StringReference Unknown5 { get; set; } = null!;

        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        public uint Flags { get; set; }

        /// <summary>
        /// Gets or sets the locale of the client.
        /// </summary>
        public StringReference ClientLocale { get; set; } = null!;

        /// <summary>
        /// Gets the actual localized references.
        /// </summary>
        /// <returns>The references.</returns>
        public IEnumerable<StringReference> GetReferences()
        {
            return new[]
            {
                English,
                Korean,
                French,
                German,
                Chinese,
                Taiwan,
                Spanish,
                SpanishMexican,
                Russian,
                Unknown1,
                Portugese,
                Italian,
                Unknown2,
                Unknown3,
                Unknown4,
                Unknown5,
                ClientLocale
            }.Where(s => s != null);
        }

        /// <summary>
        /// Gets the number of fields expected to be in a reference by version. This is equal to LanguageCount + 1,
        /// that is, including the flag field.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>The count.</returns>
        public static int GetFieldCount(WarcraftVersion version)
        {
            if (version >= WarcraftVersion.Cataclysm)
            {
                return 1;
            }

            switch (version)
            {
                case WarcraftVersion.Wrath:
                case WarcraftVersion.BurningCrusade:
                {
                    return 17;
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
