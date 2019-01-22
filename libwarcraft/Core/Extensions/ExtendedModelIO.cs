//
//  ExtendedModelIO.cs
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
using System.IO;
using System.Linq;
using System.Numerics;
using Warcraft.Core.Interfaces;
using Warcraft.MDX.Animation;
using Warcraft.MDX.Data;
using Warcraft.MDX.Gameplay;
using Warcraft.MDX.Geometry;
using Warcraft.MDX.Geometry.Skin;
using Warcraft.MDX.Visual;
using Warcraft.MDX.Visual.FX;

namespace Warcraft.Core.Extensions
{
    /// <summary>
    /// Extension methods used internally in the library for data IO. This class is specialized and only deals with
    /// MDX and WMO classes.
    /// </summary>
    public static class ExtendedModelIO
    {
        /// <summary>
        /// Reads an <see cref="MDXSkin"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader to use.</param>
        /// <param name="version">The contextually relevant version to target.</param>
        /// <returns>A fully read skin.</returns>
        public static MDXSkin ReadMDXSkin(this BinaryReader binaryReader, WarcraftVersion version)
        {
            var skin = new MDXSkin
            {
                VertexIndices = binaryReader.ReadMDXArray<ushort>(),
                Triangles = binaryReader.ReadMDXArray<ushort>(),
                VertexProperties = binaryReader.ReadMDXArray<MDXVertexProperty>(),
                Sections = binaryReader.ReadMDXArray<MDXSkinSection>(version),
                RenderBatches = binaryReader.ReadMDXArray<MDXRenderBatch>(),
                BoneCountMax = binaryReader.ReadUInt32()
            };

            return skin;
        }

        /// <summary>
        /// Reads an <see cref="MDXArray{T}"/> of type <typeparamref name="T"/> from the data stream.
        /// This advances the position of the reader by 8 bytes.
        /// </summary>
        /// <param name="binaryReader">binaryReader.</param>
        /// <typeparam name="T">The type which the array encapsulates.</typeparam>
        /// <returns>A new array, filled with the values it references.</returns>
        public static MDXArray<T> ReadMDXArray<T>(this BinaryReader binaryReader)
        {
            if (typeof(T).GetInterfaces().Contains(typeof(IVersionedClass)))
            {
                throw new InvalidOperationException("Versioned classes must be provided with a target version.");
            }

            return new MDXArray<T>(binaryReader);
        }

        /// <summary>
        /// Reads an <see cref="MDXArray{T}"/> of type <typeparamref name="T"/> from the data stream.
        /// This advances the position of the reader by 8 bytes.
        /// </summary>
        /// <param name="binaryReader">binaryReader.</param>
        /// <param name="version">The contextually relevant version of the stored objects.</param>
        /// <typeparam name="T">The type which the array encapsulates.</typeparam>
        /// <returns>A new array, filled with the values it references.</returns>
        public static MDXArray<T> ReadMDXArray<T>(this BinaryReader binaryReader, WarcraftVersion version)
        {
            // Quaternion hack, since it's packed into 16 bits in some versions
            var containsQuaternion = FindInnermostGenericType(typeof(T)) == typeof(Quaternion);
            if (!typeof(T).GetInterfaces().Contains(typeof(IVersionedClass)) && !containsQuaternion)
            {
                return new MDXArray<T>(binaryReader);
            }

            return new MDXArray<T>(binaryReader, version);
        }

        /// <summary>
        /// Finds the innermost generic type argument of the given type.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <returns>The innermost type.</returns>
        public static Type FindInnermostGenericType(Type t)
        {
            if (!t.IsGenericType)
            {
                return t;
            }

            foreach (var genericT in t.GenericTypeArguments)
            {
                return FindInnermostGenericType(genericT);
            }

            return t;
        }

        /// <summary>
        /// Reads an <see cref="MDXAttachment"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXAttachment ReadMDXAttachment(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXAttachment(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXAttachmentType"/> value from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <returns>The value.</returns>
        public static MDXAttachmentType ReadMDXAttachmentType(this BinaryReader binaryReader)
        {
            return (MDXAttachmentType)binaryReader.ReadInt16();
        }

        /// <summary>
        /// Reads an <see cref="MDXMaterial"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXMaterial ReadMDXMaterial(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXMaterial(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXColourAnimation"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXColourAnimation ReadMDXColourAnimation(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXColourAnimation(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXTextureTransform"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXTextureTransform ReadMDXTextureTransform(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXTextureTransform(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXTextureWeight"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXTextureWeight ReadMDXTextureWeight(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXTextureWeight(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXTexture"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <returns>The value.</returns>
        public static MDXTexture ReadMDXTexture(this BinaryReader binaryReader)
        {
            return new MDXTexture(binaryReader);
        }

        /// <summary>
        /// Reads an <see cref="MDXRibbonEmitter"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXRibbonEmitter ReadMDXRibbonEmitter(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXRibbonEmitter(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXCamera"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXCamera ReadMDXCamera(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXCamera(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXCameraType"/> value from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <returns>The value.</returns>
        public static MDXCameraType ReadMDXCameraType(this BinaryReader binaryReader)
        {
            return (MDXCameraType)binaryReader.ReadInt16();
        }

        /// <summary>
        /// Reads an <see cref="MDXLight"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXLight ReadMDXLight(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXLight(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXAnimationEvent"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXAnimationEvent ReadMDXAnimationEvent(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXAnimationEvent(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXTrack{T}"/> of type <typeparamref name="T"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <param name="valueless">
        /// If this value is true, it indicates that no values are associated with
        /// this track, and any value-related reading should be skipped.
        /// </param>
        /// <typeparam name="T">The type of the track.</typeparam>
        /// <returns>The value.</returns>
        public static MDXTrack<T> ReadMDXTrack<T>(this BinaryReader binaryReader, WarcraftVersion version, bool valueless = false)
        {
            return new MDXTrack<T>(binaryReader, version, valueless);
        }

        /// <summary>
        /// Reads an <see cref="MDXBone"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXBone ReadMDXBone(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXBone(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXPlayableAnimationLookupTableEntry"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <returns>The value.</returns>
        public static MDXPlayableAnimationLookupTableEntry ReadMDXPlayableAnimationLookupTableEntry(
            this BinaryReader binaryReader)
        {
            return new MDXPlayableAnimationLookupTableEntry(binaryReader.ReadInt16(), (MDXPlayableAnimationFlags)binaryReader.ReadUInt16());
        }

        /// <summary>
        /// Reads an <see cref="MDXVertex"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <returns>The value.</returns>
        public static MDXVertex ReadMDXVertex(this BinaryReader binaryReader)
        {
            return new MDXVertex(binaryReader.ReadBytes(MDXVertex.GetSize()));
        }

        /// <summary>
        /// Reads an <see cref="MDXAnimationSequence"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXAnimationSequence ReadMDXAnimationSequence(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXAnimationSequence(binaryReader, version);
        }

        /// <summary>
        /// Reads an <see cref="MDXVertexProperty"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <returns>The value.</returns>
        public static MDXVertexProperty ReadMDXVertexProperty(this BinaryReader binaryReader)
        {
            return new MDXVertexProperty
            (
                binaryReader.ReadByte(),
                binaryReader.ReadByte(),
                binaryReader.ReadByte(),
                binaryReader.ReadByte()
            );
        }

        /// <summary>
        /// Reads an <see cref="MDXSkinSection"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="version">The version.</param>
        /// <returns>The value.</returns>
        public static MDXSkinSection ReadMDXSkinSection(this BinaryReader binaryReader, WarcraftVersion version)
        {
            return new MDXSkinSection(binaryReader.ReadBytes(MDXSkinSection.GetSize(version)));
        }

        /// <summary>
        /// Reads an <see cref="MDXRenderBatch"/> from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <returns>The value.</returns>
        public static MDXRenderBatch ReadMDXRenderBatch(this BinaryReader binaryReader)
        {
            return new MDXRenderBatch(binaryReader.ReadBytes(MDXRenderBatch.GetSize()));
        }
    }
}
