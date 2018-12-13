//
//  MapChunkAlphaMaps.cs
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
using System.IO;
using Warcraft.Core.Interfaces;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.ADT.Chunks.Subchunks
{
    /// <summary>
    /// MCLY Chunk - Contains definitions for the alpha map layers.
    /// </summary>
    public class MapChunkTextureLayers : IIFFChunk
    {
        public const string Signature = "MCLY";

        /// <summary>
        /// An array of alpha map layers in this MCNK.
        /// </summary>
        public List<TextureLayerEntry> Layers = new List<TextureLayerEntry>();

        public MapChunkTextureLayers()
        {

        }

        public MapChunkTextureLayers(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (MemoryStream ms = new MemoryStream(inData))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    long nLayers = br.BaseStream.Length / TextureLayerEntry.GetSize();
                    for (int i = 0; i < nLayers; i++)
                    {
                        this.Layers.Add(new TextureLayerEntry(br.ReadBytes(TextureLayerEntry.GetSize())));
                    }
                }
            }
        }

        public string GetSignature()
        {
            return Signature;
        }
    }

    /// <summary>
    /// Texture layer entry, representing a ground texture in the chunk.
    /// </summary>
    public class TextureLayerEntry
    {
        /// <summary>
        /// Index of the texture in the MTEX chunk
        /// </summary>
        public uint TextureID;

        /// <summary>
        /// Flags for the texture. Used for animation data.
        /// </summary>
        public TextureLayerFlags Flags;

        /// <summary>
        /// Offset into MCAL where the alpha map begins.
        /// </summary>
        public uint AlphaMapOffset;

        /// <summary>
        /// Ground effect ID. This is a foreign key entry into GroundEffectTexture::ID.
        /// </summary>
        public ForeignKey<ushort> EffectID;

        /// <summary>
        /// A currently unused value.
        /// </summary>
        public ushort Unused;

        public TextureLayerEntry(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    this.TextureID = br.ReadUInt32();
                    this.Flags = (TextureLayerFlags)br.ReadUInt32();
                    this.AlphaMapOffset = br.ReadUInt32();

                    this.EffectID = new ForeignKey<ushort>(DatabaseName.GroundEffectTexture, nameof(DBCRecord.ID), br.ReadUInt16()); // TODO: Implement GroundEffectTextureRecord
                }
            }
        }

        /// <summary>
        /// Gets the size of a texture layer entry.
        /// </summary>
        /// <returns>The size.</returns>
        public static int GetSize()
        {
            return 16;
        }
    }

    /// <summary>
    /// Chunk flags
    /// </summary>
    [Flags]
    public enum TextureLayerFlags : uint
    {
        Animated45RotationPerTick = 0x001,

        Animated90RotationPerTick = 0x002,

        Animated180RotationPerTick = 0x004,

        AnimSpeed1 = 0x008,

        AnimSpeed2 = 0x010,

        AnimSpeed3 = 0x020,

        AnimationEnabled = 0x040,

        EmissiveLayer = 0x080,

        UseAlpha = 0x100,

        CompressedAlpha = 0x200,

        UseCubeMappedReflection = 0x400,

        // 19 unused bits
    }
}

