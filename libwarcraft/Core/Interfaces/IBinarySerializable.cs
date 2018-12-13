//
//  IBinarySerializable.cs
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

namespace Warcraft.Core.Interfaces
{
    /// <summary>
    /// Classes which implement this interface promise to be able to serialize themselves into a binary format,
    /// which can then be used to reconstruct the object fully by passing the same data as a byte array to its
    /// constructor.
    /// </summary>
    public interface IBinarySerializable
    {
        /// <summary>
        /// Serializes the current object into a byte array.
        /// </summary>
        byte[] Serialize();
    }
}
