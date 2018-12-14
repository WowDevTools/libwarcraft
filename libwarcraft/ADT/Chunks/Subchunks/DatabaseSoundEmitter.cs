//
//  DatabaseSoundEmitter.cs
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
using System.Numerics;
using Warcraft.Core.Interfaces;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// Represents a sound emitter that gets its data from a database entry.
    /// </summary>
    public class DatabaseSoundEmitter : SoundEmitter, IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the ID of the sound entry in the database.
        /// </summary>
        public ForeignKey<uint> SoundEntryID { get; set; }

        /// <summary>
        /// Gets or sets the position of the emitter.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the size of the emitter.
        /// </summary>
        public Vector3 Size { get; set; }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
