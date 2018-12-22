//
//  ModelPlacementEntry.cs
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

using System.IO;
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Structures;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// An entry struct containing information about the model.
    /// </summary>
    public class ModelPlacementEntry
    {
        /// <summary>
        /// Gets or sets the specifies which model to use via the MMID chunk.
        /// </summary>
        public uint ModelEntryIndex { get; set; }

        /// <summary>
        /// Gets or sets the a unique actor ID for the model. Blizzard implements this as game global, but it's only
        /// checked in loaded tile.
        /// </summary>
        public uint UniqueID { get; set; }

        /// <summary>
        /// Gets or sets the position of the model.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the model.
        /// </summary>
        public Rotator Rotation { get; set; }

        /// <summary>
        /// Gets or sets the scale of the model. 1024 is the default value, equating to 1.0f. There is no uneven scaling
        /// of objects.
        /// </summary>
        public ushort ScalingFactor { get; set; }

        /// <summary>
        /// Gets or sets the MMDF flags for the object.
        /// </summary>
        public ModelPlacementFlags Flags { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.ModelPlacementEntry"/> class.
        /// </summary>
        /// <param name="data">ExtendedData.</param>
        public ModelPlacementEntry(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    ModelEntryIndex = br.ReadUInt32();
                    UniqueID = br.ReadUInt32();
                    Position = br.ReadVector3();
                    Rotation = br.ReadRotator();

                    ScalingFactor = br.ReadUInt16();
                    Flags = (ModelPlacementFlags)br.ReadUInt16();
                }
            }
        }

        /// <summary>
        /// Gets the size of an entry.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 36;
        }
    }
}
