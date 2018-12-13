//
//  AxisConfiguration.cs
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

namespace Warcraft.Core.Structures
{
    /// <summary>
    /// An axis configuration, that is, how vector data should be interpreted.
    /// </summary>
    public enum AxisConfiguration
    {
        /// <summary>
        /// No assumptions should be made about the vector storage format, and should be read as XYZ.
        /// </summary>
        Native,

        /// <summary>
        /// Assume that the data is stored as Y-up.
        /// </summary>
        YUp,

        /// <summary>
        /// Assume that the data is stored as Z-up.
        /// </summary>
        ZUp
    }
}
