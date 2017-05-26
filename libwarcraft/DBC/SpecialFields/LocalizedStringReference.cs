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
		StringReference English;

		/// <summary>
		/// The reference to the Korean version of the string.
		/// </summary>
		StringReference Korean;

		/// <summary>
		/// The reference to the French version of the string.
		/// </summary>
		StringReference French;

		/// <summary>
		/// The reference to the German version of the string.
		/// </summary>
		StringReference German;

		/// <summary>
		/// The reference to the Chinese version of the string.
		/// </summary>
		StringReference Chinese;

		/// <summary>
		/// The reference to the Taiwan version of the string.
		/// </summary>
		StringReference Taiwan;

		/// <summary>
		/// The reference to the Spanish version of the string.
		/// </summary>
		StringReference Spanish;

		/// <summary>
		/// The reference to the Mexican Spanish version of the string.
		/// </summary>
		StringReference SpanishMexican;

		/// <summary>
		/// The reference to the Russian version of the string.
		/// </summary>
		StringReference Russian;
	}
}