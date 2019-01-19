//
//  AreaInfoFlags.cs
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

namespace Warcraft.WDT.Chunks
{
    /// <summary>
    /// Defines various area information flags.
    /// </summary>
    public enum AreaInfoFlags : uint
    {
        /// <summary>
        /// This tile has terrain in it.
        /// </summary>
        HasTerrainData = 1,

        /// <summary>
        /// This tile is loaded. This flag is never set in serialized files, and is only a runtime construct.
        /// </summary>
        IsLoaded = 2,
    }
}
