//
//  MDX.cs
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Warcraft.MDX.Visual;
using Warcraft.MDX.Geometry;
using Warcraft.MDX.Animation;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Shading.Blending;
using Warcraft.Core.Structures;
using Warcraft.MDX.Data;
using Warcraft.MDX.Gameplay;
using Warcraft.MDX.Geometry.Skin;
using Warcraft.MDX.Visual.FX;

namespace Warcraft.MDX
{
    public class MDX
    {
        public const string Signature = "MD20";
        public WarcraftVersion Version;
        public string Name;

        public ModelObjectFlags GlobalModelFlags;

        public MDXArray<uint> GlobalSequenceTimestamps;
        public MDXArray<MDXAnimationSequence> AnimationSequences;
        public MDXArray<ushort> AnimationSequenceLookupTable;

        public MDXArray<MDXPlayableAnimationLookupTableEntry> PlayableAnimationLookupTable;

        public MDXArray<MDXBone> Bones;
        public MDXArray<ushort> BoneSocketLookupTable;

        public MDXArray<MDXVertex> Vertices;
        public MDXArray<MDXSkin> Skins;
        public uint SkinCount;

        public MDXArray<MDXColourAnimation> ColourAnimations;
        public MDXArray<MDXTexture> Textures;
        public MDXArray<MDXTextureWeight> TransparencyAnimations;
        public MDXArray<MDXTextureTransform> TextureTransformations;
        public MDXArray<short> ReplaceableTextureLookupTable;
        public MDXArray<MDXMaterial> Materials;

        public MDXArray<short> BoneLookupTable;
        public MDXArray<short> TextureLookupTable;
        public MDXArray<EMDXTextureMappingType> TextureMappingLookupTable;
        public MDXArray<short> TransparencyLookupTable;
        public MDXArray<short> TextureTransformationLookupTable;

        public Box BoundingBox;
        public float BoundingSphereRadius;

        public Box CollisionBox;
        public float CollisionSphereRadius;

        public MDXArray<ushort> CollisionTriangles;
        public MDXArray<Vector3> CollisionVertices;
        public MDXArray<Vector3> CollisionNormals;

        public MDXArray<MDXAttachment> Attachments;
        public MDXArray<MDXAttachmentType> AttachmentLookupTable;

        public MDXArray<MDXAnimationEvent> AnimationEvents;

        public MDXArray<MDXLight> Lights;

        public MDXArray<MDXCamera> Cameras;
        public MDXArray<MDXCameraType> CameraTypeLookupTable;

        public MDXArray<MDXRibbonEmitter> RibbonEmitters;
        // ribbon emitters
        // particle emitters

        // cond: wrath & blendmap overrides
        public MDXArray<BlendingMode> BlendMapOverrides;

        public MDX(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                LoadFromStream(ms);
            }
        }

        public MDX(Stream dataStream)
        {
            LoadFromStream(dataStream);
        }

        private void LoadFromStream(Stream dataStream)
        {
            using (BinaryReader br = new BinaryReader(dataStream))
            {
                string dataSignature = new string(br.ReadBinarySignature().Reverse().ToArray());
                if (dataSignature != Signature)
                {
                    throw new ArgumentException("The provided data stream does not contain a valid MDX signature. " +
                                                "It might be a Legion file, or you may have omitted the signature, which should be \"MD20\".");
                }

                Version = GetModelVersion(br.ReadUInt32());
                Name = new string(br.ReadMDXArray<char>().GetValues().ToArray());
                GlobalModelFlags = (ModelObjectFlags) br.ReadUInt32();

                GlobalSequenceTimestamps = br.ReadMDXArray<uint>();
                AnimationSequences = br.ReadMDXArray<MDXAnimationSequence>(Version);
                AnimationSequenceLookupTable = br.ReadMDXArray<ushort>();

                if (Version < WarcraftVersion.Wrath)
                {
                    PlayableAnimationLookupTable = br.ReadMDXArray<MDXPlayableAnimationLookupTableEntry>();
                }

                Bones = br.ReadMDXArray<MDXBone>(Version);
                BoneSocketLookupTable = br.ReadMDXArray<ushort>();
                Vertices = br.ReadMDXArray<MDXVertex>();

                if (Version < WarcraftVersion.Wrath)
                {
                    Skins = br.ReadMDXArray<MDXSkin>(Version);
                }
                else
                {
                    // Skins are stored out of file, figure out a clean solution
                    SkinCount = br.ReadUInt32();
                }

                ColourAnimations = br.ReadMDXArray<MDXColourAnimation>(Version);
                Textures = br.ReadMDXArray<MDXTexture>();
                TransparencyAnimations = br.ReadMDXArray<MDXTextureWeight>(Version);

                if (Version <= WarcraftVersion.BurningCrusade)
                {
                    // There's an array of something here, but we've no idea what type of data it is. Thus, we'll skip
                    // over it.
                    br.BaseStream.Position += 8;
                }

                TextureTransformations = br.ReadMDXArray<MDXTextureTransform>(Version);
                ReplaceableTextureLookupTable = br.ReadMDXArray<short>();
                Materials = br.ReadMDXArray<MDXMaterial>(Version);

                BoneLookupTable = br.ReadMDXArray<short>();
                TextureLookupTable = br.ReadMDXArray<short>();
                TextureMappingLookupTable = br.ReadMDXArray<EMDXTextureMappingType>();
                TransparencyLookupTable = br.ReadMDXArray<short>();
                TextureTransformationLookupTable = br.ReadMDXArray<short>();

                BoundingBox = br.ReadBox();
                BoundingSphereRadius = br.ReadSingle();

                CollisionBox = br.ReadBox();
                CollisionSphereRadius = br.ReadSingle();

                CollisionTriangles = br.ReadMDXArray<ushort>();
                CollisionVertices = br.ReadMDXArray<Vector3>();
                CollisionNormals = br.ReadMDXArray<Vector3>();

                Attachments = br.ReadMDXArray<MDXAttachment>(Version);
                AttachmentLookupTable = br.ReadMDXArray<MDXAttachmentType>();

                AnimationEvents = br.ReadMDXArray<MDXAnimationEvent>(Version);
                Lights = br.ReadMDXArray<MDXLight>(Version);

                Cameras = br.ReadMDXArray<MDXCamera>(Version);
                CameraTypeLookupTable = br.ReadMDXArray<MDXCameraType>();

                RibbonEmitters = br.ReadMDXArray<MDXRibbonEmitter>(Version);

                // TODO: Particle Emitters
                // Skip for now
                br.BaseStream.Position += 8;

                if (Version >= WarcraftVersion.Wrath && GlobalModelFlags.HasFlag(ModelObjectFlags.HasBlendModeOverrides))
                {
                    BlendMapOverrides = br.ReadMDXArray<BlendingMode>();
                }
            }
        }

        /// <summary>
        /// Sets the skins used in this model.
        /// </summary>
        /// <param name="skins"></param>
        public void SetSkins(IEnumerable<MDXSkin> skins)
        {
            var skinArray = new MDXArray<MDXSkin>(skins);
            if (skinArray.Count != SkinCount)
            {
                throw new ArgumentException("The number of skins did not match the skin count for the model.", nameof(skins));
            }

            Skins = skinArray;
        }

        /// <summary>
        /// Translates a given numerical model version number into its equivalent <see cref="WarcraftVersion"/>.
        /// </summary>
        /// <param name="version">The numerical model version.</param>
        /// <returns>An equivalent Warcraft version.</returns>
        public static WarcraftVersion GetModelVersion(uint version)
        {
            if (version <= 256)
            {
                return WarcraftVersion.Classic;
            }

            if (version <= 263)
            {
                return WarcraftVersion.BurningCrusade;
            }

            if (version == 264)
            {
                return WarcraftVersion.Wrath;
            }

            if (version <= 272)
            {
                return WarcraftVersion.Cataclysm;
            }

            if (version < 274)
            {
                // It should be noted that this is a guess based on the newer and older
                // model versions. If it works, great - YMMV
                return WarcraftVersion.Warlords;
            }

            if (version >= 274)
            {
                return WarcraftVersion.Legion;
            }

            return WarcraftVersion.Unknown;
        }
    }
}

