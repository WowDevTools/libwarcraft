//
//  MDX.cs
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Shading.Blending;
using Warcraft.Core.Structures;

using Warcraft.MDX.Animation;
using Warcraft.MDX.Data;
using Warcraft.MDX.Gameplay;
using Warcraft.MDX.Geometry;
using Warcraft.MDX.Geometry.Skin;
using Warcraft.MDX.Visual;
using Warcraft.MDX.Visual.FX;

namespace Warcraft.MDX
{
    /// <summary>
    /// Represents a game model.
    /// </summary>
    public class MDX
    {
        /// <summary>
        /// Holds the binary chunk signature.
        /// </summary>
        public const string Signature = "MD20";

        /// <summary>
        /// Gets or sets the game version the model is for.
        /// </summary>
        public WarcraftVersion Version { get; set; }

        /// <summary>
        /// Gets or sets the name of the model.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the model flags.
        /// </summary>
        public ModelObjectFlags GlobalModelFlags { get; set; }

        /// <summary>
        /// Gets or sets the global sequence timestamps.
        /// </summary>
        public MDXArray<uint>? GlobalSequenceTimestamps { get; set; }

        /// <summary>
        /// Gets or sets the animation sequences.
        /// </summary>
        public MDXArray<MDXAnimationSequence>? AnimationSequences { get; set; }

        /// <summary>
        /// Gets or sets the animation sequence lookup table.
        /// </summary>
        public MDXArray<ushort>? AnimationSequenceLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the playable animation lookup table.
        /// </summary>
        public MDXArray<MDXPlayableAnimationLookupTableEntry>? PlayableAnimationLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the bones of the model.
        /// </summary>
        public MDXArray<MDXBone>? Bones { get; set; }

        /// <summary>
        /// Gets or sets the key bone lookup table.
        /// </summary>
        public MDXArray<ushort>? KeyBoneLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the vertices of the model.
        /// </summary>
        public MDXArray<MDXVertex>? Vertices { get; set; }

        /// <summary>
        /// Gets or sets the skins of the model.
        /// </summary>
        public MDXArray<MDXSkin>? Skins { get; set; }

        /// <summary>
        /// Gets or sets the number of skins in the model.
        /// </summary>
        public uint SkinCount { get; set; }

        /// <summary>
        /// Gets or sets the colour animations.
        /// </summary>
        public MDXArray<MDXColourAnimation>? ColourAnimations { get; set; }

        /// <summary>
        /// Gets or sets the model's textures.
        /// </summary>
        public MDXArray<MDXTexture>? Textures { get; set; }

        /// <summary>
        /// Gets or sets the transparency animations.
        /// </summary>
        public MDXArray<MDXTextureWeight>? TransparencyAnimations { get; set; }

        /// <summary>
        /// Gets or sets the texture transformations.
        /// </summary>
        public MDXArray<MDXTextureTransform>? TextureTransformations { get; set; }

        /// <summary>
        /// Gets or sets the replaceable texture lookup table.
        /// </summary>
        public MDXArray<short>? ReplaceableTextureLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the model's materials.
        /// </summary>
        public MDXArray<MDXMaterial>? Materials { get; set; }

        /// <summary>
        /// Gets or sets the bone lookup table.
        /// </summary>
        public MDXArray<short>? BoneLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the texture lookup table.
        /// </summary>
        public MDXArray<short>? TextureLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the texture mapping lookup table.
        /// </summary>
        public MDXArray<MDXTextureMappingType>? TextureMappingLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the transparency lookup table.
        /// </summary>
        public MDXArray<short>? TransparencyLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the texture transformation lookup table.
        /// </summary>
        public MDXArray<short>? TextureTransformationLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the model's bounding box.
        /// </summary>
        public Box BoundingBox { get; set; }

        /// <summary>
        /// Gets or sets the bounding sphere radius.
        /// </summary>
        public float BoundingSphereRadius { get; set; }

        /// <summary>
        /// Gets or sets the model's collision box.
        /// </summary>
        public Box CollisionBox { get; set; }

        /// <summary>
        /// Gets or sets the collision sphere radius.
        /// </summary>
        public float CollisionSphereRadius { get; set; }

        /// <summary>
        /// Gets or sets the triangle indexes of the convex collision hull.
        /// </summary>
        public MDXArray<ushort>? CollisionTriangles { get; set; }

        /// <summary>
        /// Gets or sets the vertices of the convex collision hull.
        /// </summary>
        public MDXArray<Vector3>? CollisionVertices { get; set; }

        /// <summary>
        /// Gets or sets the normals of the convex collision hull.
        /// </summary>
        public MDXArray<Vector3>? CollisionNormals { get; set; }

        /// <summary>
        /// Gets or sets the attachments of the model.
        /// </summary>
        public MDXArray<MDXAttachment>? Attachments { get; set; }

        /// <summary>
        /// Gets or sets the attachment lookup table.
        /// </summary>
        public MDXArray<MDXAttachmentType>? AttachmentLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the animation events.
        /// </summary>
        public MDXArray<MDXAnimationEvent>? AnimationEvents { get; set; }

        /// <summary>
        /// Gets or sets the lights in the model.
        /// </summary>
        public MDXArray<MDXLight>? Lights { get; set; }

        /// <summary>
        /// Gets or sets the cameras in the model.
        /// </summary>
        public MDXArray<MDXCamera>? Cameras { get; set; }

        /// <summary>
        /// Gets or sets the camera type lookup table.
        /// </summary>
        public MDXArray<MDXCameraType>? CameraTypeLookupTable { get; set; }

        /// <summary>
        /// Gets or sets the ribbon emitters in the model.
        /// </summary>
        public MDXArray<MDXRibbonEmitter>? RibbonEmitters { get; set; }

        /// <summary>
        /// Gets or sets the particle emitters in the model.
        /// </summary>
        public MDXArray<MDXParticleEmitter>? ParticleEmitters { get; set; }

        /// <summary>
        /// Gets or sets the blending mode overrides. This is only present if the model is from Wrath or above, and has
        /// the blending map override flag set.
        /// </summary>
        // cond: wrath & blendmap overrides
        public MDXArray<BlendingMode>? BlendMapOverrides { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDX"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        public MDX(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                LoadFromStream(ms);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDX"/> class.
        /// </summary>
        /// <param name="dataStream">The stream to load the model from.</param>
        public MDX(Stream dataStream)
        {
            LoadFromStream(dataStream);
        }

        private void LoadFromStream(Stream dataStream)
        {
            using (var br = new BinaryReader(dataStream))
            {
                var dataSignature = new string(br.ReadBinarySignature().Reverse().ToArray());
                if (dataSignature != Signature)
                {
                    throw new ArgumentException("The provided data stream does not contain a valid MDX signature. " +
                                                "It might be a Legion file, or you may have omitted the signature, which should be \"MD20\".");
                }

                Version = GetModelVersion(br.ReadUInt32());
                Name = new string(br.ReadMDXArray<char>().GetValues().ToArray());
                GlobalModelFlags = (ModelObjectFlags)br.ReadUInt32();

                GlobalSequenceTimestamps = br.ReadMDXArray<uint>();
                AnimationSequences = br.ReadMDXArray<MDXAnimationSequence>(Version);
                AnimationSequenceLookupTable = br.ReadMDXArray<ushort>();

                if (Version < WarcraftVersion.Wrath)
                {
                    PlayableAnimationLookupTable = br.ReadMDXArray<MDXPlayableAnimationLookupTableEntry>();
                }

                Bones = br.ReadMDXArray<MDXBone>(Version);
                KeyBoneLookupTable = br.ReadMDXArray<ushort>();
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
                TextureMappingLookupTable = br.ReadMDXArray<MDXTextureMappingType>();
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
        /// <param name="skins">The skins.</param>
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
