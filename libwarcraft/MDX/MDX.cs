//
//  MDX.cs
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
using System.IO;
using System.Linq;
using System.Numerics;
using Warcraft.MDX.Visual;
using Warcraft.MDX.Geometry;
using Warcraft.MDX.Animation;
using Warcraft.Core;
using Warcraft.Core.Extensions;
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
		public MDXArray<short> TextureSlotLookupTable;
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
		public MDXArray<ushort> BlendMapOverrides;

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

				this.Version = GetModelVersion(br.ReadUInt32());
				this.Name = new string(br.ReadMDXArray<char>().GetValues().ToArray());
				this.GlobalModelFlags = (ModelObjectFlags) br.ReadUInt32();

				this.GlobalSequenceTimestamps = br.ReadMDXArray<uint>();
				this.AnimationSequences = br.ReadMDXArray<MDXAnimationSequence>(this.Version);
				this.AnimationSequenceLookupTable = br.ReadMDXArray<ushort>();

				if (this.Version < WarcraftVersion.Wrath)
				{
					this.PlayableAnimationLookupTable = br.ReadMDXArray<MDXPlayableAnimationLookupTableEntry>();
				}

				this.Bones = br.ReadMDXArray<MDXBone>(this.Version);
				this.BoneSocketLookupTable = br.ReadMDXArray<ushort>();
				this.Vertices = br.ReadMDXArray<MDXVertex>();

				if (this.Version < WarcraftVersion.Wrath)
				{
					this.Skins = br.ReadMDXArray<MDXSkin>(this.Version);
				}
				else
				{
					// Skins are stored out of file, figure out a clean solution
					this.SkinCount = br.ReadUInt32();
				}

				this.ColourAnimations = br.ReadMDXArray<MDXColourAnimation>(this.Version);
				this.Textures = br.ReadMDXArray<MDXTexture>();
				this.TransparencyAnimations = br.ReadMDXArray<MDXTextureWeight>(this.Version);

				if (this.Version <= WarcraftVersion.BurningCrusade)
				{
					// There's an array of something here, but we've no idea what type of data it is. Thus, we'll skip
					// over it.
					br.BaseStream.Position += 8;
				}

				this.TextureTransformations = br.ReadMDXArray<MDXTextureTransform>(this.Version);
				this.ReplaceableTextureLookupTable = br.ReadMDXArray<short>();
				this.Materials = br.ReadMDXArray<MDXMaterial>();

				this.BoneLookupTable = br.ReadMDXArray<short>();
				this.TextureLookupTable = br.ReadMDXArray<short>();
				this.TextureSlotLookupTable = br.ReadMDXArray<short>();
				this.TransparencyLookupTable = br.ReadMDXArray<short>();
				this.TextureTransformationLookupTable = br.ReadMDXArray<short>();

				this.BoundingBox = br.ReadBox();
				this.BoundingSphereRadius = br.ReadSingle();

				this.CollisionBox = br.ReadBox();
				this.CollisionSphereRadius = br.ReadSingle();

				this.CollisionTriangles = br.ReadMDXArray<ushort>();
				this.CollisionVertices = br.ReadMDXArray<Vector3>();
				this.CollisionNormals = br.ReadMDXArray<Vector3>();

				this.Attachments = br.ReadMDXArray<MDXAttachment>(this.Version);
				this.AttachmentLookupTable = br.ReadMDXArray<MDXAttachmentType>();

				this.AnimationEvents = br.ReadMDXArray<MDXAnimationEvent>(this.Version);
				this.Lights = br.ReadMDXArray<MDXLight>(this.Version);

				this.Cameras = br.ReadMDXArray<MDXCamera>(this.Version);
				this.CameraTypeLookupTable = br.ReadMDXArray<MDXCameraType>();

				this.RibbonEmitters = br.ReadMDXArray<MDXRibbonEmitter>(this.Version);

				// TODO: Particle Emitters
				// Skip for now
				br.BaseStream.Position += 8;

				if (this.Version >= WarcraftVersion.Wrath && this.GlobalModelFlags.HasFlag(ModelObjectFlags.HasBlendModeOverrides))
				{
					this.BlendMapOverrides = br.ReadMDXArray<ushort>();
				}
			}
		}

		public static WarcraftVersion GetModelVersion(uint version)
		{
			if (version <= 256)
			{
				return WarcraftVersion.Classic;
			}

			if (version <= 263 && version > 256)
			{
				return WarcraftVersion.BurningCrusade;
			}

			if (version == 264)
			{
				return WarcraftVersion.Wrath;
			}

			if (version <= 272 && version > 264)
			{
				return WarcraftVersion.Cataclysm;
			}

			if (version < 274 && version > 272)
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

