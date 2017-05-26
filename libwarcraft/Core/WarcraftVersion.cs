//
//  MDXFormat.cs
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

namespace Warcraft.Core
{
	/// <summary>
	/// World of Warcraft versions by expansion.
	/// </summary>
	public enum WarcraftVersion : uint
	{
		/// <summary>
		/// It's not known what version this is.
		/// </summary>
		Unknown 		= 0,

		/// <summary>
		/// Classic World of Warcraft, also referred to as "Vanilla".
		/// </summary>
		Classic 		= 1,

		/// <summary>
		/// World of Warcraft: The Burning Crusade
		/// </summary>
		BurningCrusade 	= 2,

		/// <summary>
		/// World of Warcraft: Wrath of the Lich King
		/// </summary>
		Wrath			= 3,

		/// <summary>
		/// World of Warcraft: Cataclysm
		/// </summary>
		Cataclysm 		= 4,

		/// <summary>
		/// World of Warcraft: Mists of Pandaria
		/// </summary>
		Mists 			= 5,

		/// <summary>
		/// World of Warcraft: Warlords of Draenor
		/// </summary>
		Warlords 		= 6,

		/// <summary>
		/// World of Warcraft: Legion
		/// </summary>
		Legion 			= 7
	}
}

