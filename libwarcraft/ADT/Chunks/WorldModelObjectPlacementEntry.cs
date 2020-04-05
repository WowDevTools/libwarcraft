//
//  WorldModelObjectPlacementEntry.cs
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

using System.IO;
using System.Numerics;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;

namespace Warcraft.ADT.Chunks
{
    /// <summary>
    /// An entry struct containing information about the WMO.
    /// </summary>
    public class WorldModelObjectPlacementEntry : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the specifies which WMO to use via the MMID chunk.
        /// </summary>
        public uint WorldModelObjectEntryIndex { get; set; }

        /// <summary>
        /// Gets or sets the a unique actor ID for the model. Blizzard implements this as game global, but it's only
        /// checked in loaded tiles. When not in use, it's set to -1.
        /// </summary>
        public int UniqueID { get; set; }

        /// <summary>
        /// Gets or sets the position of the WMO.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the model.
        /// </summary>
        public Rotator Rotation { get; set; }

        /// <summary>
        /// Gets or sets the the bounding box of the model.
        /// </summary>
        public Box BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the flags of the model.
        /// </summary>
        public WorldModelObjectFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the doodadset of the model.
        /// </summary>
        public ushort DoodadSet { get; set; }

        /// <summary>
        /// Gets or sets the nameset of the model.
        /// </summary>
        public ushort NameSet { get; set; }

        /// <summary>
        /// Gets or sets the an unused value. Seems to have data in Legion tiles.
        /// </summary>
        public ushort Unused { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Warcraft.ADT.Chunks.WorldModelObjectPlacementEntry"/> class.
        /// </summary>
        /// <param name="data">ExtendedData.</param>
        public WorldModelObjectPlacementEntry(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    WorldModelObjectEntryIndex = br.ReadUInt32();
                    UniqueID = br.ReadInt32();

                    Position = br.ReadVector3();
                    Rotation = br.ReadRotator();
                    BoundingBox = br.ReadBox();

                    Flags = (WorldModelObjectFlags)br.ReadUInt16();
                    DoodadSet = br.ReadUInt16();
                    NameSet = br.ReadUInt16();
                    Unused = br.ReadUInt16();
                }
            }
        }

        /// <summary>
        /// Gets the size of a placement entry.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 64;
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(WorldModelObjectEntryIndex);
                    bw.Write(UniqueID);

                    bw.WriteVector3(Position);
                    bw.WriteRotator(Rotation);
                    bw.WriteBox(BoundingBox);

                    bw.Write((ushort)Flags);
                    bw.Write(DoodadSet);
                    bw.Write(NameSet);
                    bw.Write(Unused);
                }

                return ms.ToArray();
            }
        }
    }
}
