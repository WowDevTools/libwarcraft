//
//  MDXHeader.cs
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
using Warcraft.Core;
using System.IO;

namespace Warcraft.MDX
{
	public class MDXHeader
	{
		public const string Signature = "MD20";

		public uint Version;
		public uint NameLength;
		public uint NameOffset;
		public ModelObjectFlags GlobalModelFlags;
		public uint GlobalSequenceCount;
		public uint GlobalSequencesOffset;
		public uint AnimationSequenceCount;
		public uint AnimationSequencesOffset;
		public uint AnimationLookupTableEntryCount;
		public uint AnimationLookupTableOffset;

		// Pre-Wrath
		public uint PlayableAnimationLookupTableEntryCount;
		public uint PlayableAnimationLookupTableOffset;
		// End Pre-Wrath

		public uint BoneCount;
		public uint BonesOffset;
		public uint KeyedBoneLookupTableCount;
		public uint KeyedBoneLookupTablesOffset;
		public uint VertexCount;
		public uint VerticesOffset;
		public uint LODViewsCount;

		// Pre-Wrath
		public uint LODViewsOffset;
		// End Pre-Wrath

		public uint SubmeshColourAnimationCount;
		public uint SubmeshColourAnimationsOffset;
		public uint TextureCount;
		public uint TexturesOffset;
		public uint TransparencyAnimationCount;
		public uint TransparencyAnimationsOffset;

		// Pre-Wrath
		// Seems to always be 0
		public uint UnknownCount;
		public uint UnknownOffset;
		// End Pre-Wrath

		public uint UVTextureAnimationCount;
		public uint UVTextureAnimationsOffset;
		public uint ReplaceableTextureCount;
		public uint ReplaceableTexturesOffset;
		public uint RenderFlagCount;
		public uint RenderFlagsOffset;

		public uint BoneLookupTableCount;
		public uint BoneLookupTablesOffset;
		public uint TextureLookupTableCount;
		public uint TextureLookupTablesOffset;
		public uint TextureUnitCount;
		public uint TextureUnitsOffset;
		public uint TransparencyLookupTableCount;
		public uint TransparencyLookupTablesOffset;
		public uint UVTextureAnimationLookupTableCount;
		public uint UVTextureAnimationLookupTablesOffset;

		public Box BoundingBox;
		public float BoundingSphereRadius;
		public Box CollisionBox;
		public float CollisionSphereRadius;

		public uint BoundingTriangleCount;
		public uint BoundingTrianglesOffset;
		public uint BoundingVertexCount;
		public uint BoundingVerticesOffset;
		public uint BoundingNormalCount;
		public uint BoundingNormalsOffset;

		public uint AttachmentsCount;
		public uint AttachmentsOffset;
		public uint AttachmentsLookupTableCount;
		public uint AttachmentsLookupTablesOffset;

		public uint AnimationEventCount;
		public uint AnimationEventsOffset;
		public uint LightCount;
		public uint LightsOffset;
		public uint CameraCount;
		public uint CamerasOffset;
		public uint CameraLookupTableCount;
		public uint CameraLookupTablesOffset;

		public uint RibbonEmitterCount;
		public uint RibbonEmittersOffset;

		public uint ParticleEmitterCount;
		public uint ParticleEmittersOffset;

		/*
			Conditional content (flags must have EMDXFlags.HasBlendModeOverrides set)
		*/

		public uint BlendMapCount;
		public uint BlendMapsOffset;

		public MDXHeader(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					string Signature = new string(br.ReadChars(4));
					if (Signature != MDXHeader.Signature)
					{
						throw new ArgumentException("The provided data stream does not contain a valid MDX signature. " +
							"It might be a Legion file, or you may have omitted the signature, which should be \"MD20\".");
					}

					this.Version = br.ReadUInt32();
					this.NameLength = br.ReadUInt32();
					this.NameOffset = br.ReadUInt32();
					this.GlobalModelFlags = (ModelObjectFlags)br.ReadUInt32();
					this.GlobalSequenceCount = br.ReadUInt32();
					this.GlobalSequencesOffset = br.ReadUInt32();
					this.AnimationSequenceCount = br.ReadUInt32();
					this.AnimationSequencesOffset = br.ReadUInt32();
					this.AnimationLookupTableEntryCount = br.ReadUInt32();
					this.AnimationLookupTableOffset = br.ReadUInt32();

					if (GetModelVersion(Version) < WarcraftVersion.Wrath)
					{
						this.PlayableAnimationLookupTableEntryCount = br.ReadUInt32();
						this.PlayableAnimationLookupTableOffset = br.ReadUInt32();
					}

					this.BoneCount = br.ReadUInt32();
					this.BonesOffset = br.ReadUInt32();
					this.KeyedBoneLookupTableCount = br.ReadUInt32();
					this.KeyedBoneLookupTablesOffset = br.ReadUInt32();

					this.VertexCount = br.ReadUInt32();
					this.VerticesOffset = br.ReadUInt32();
					this.LODViewsCount = br.ReadUInt32();

					if (GetModelVersion(Version) < WarcraftVersion.Wrath)
					{
						this.LODViewsOffset = br.ReadUInt32();
					}

					this.SubmeshColourAnimationCount = br.ReadUInt32();
					this.SubmeshColourAnimationsOffset = br.ReadUInt32();
					this.TextureCount = br.ReadUInt32();
					this.TexturesOffset = br.ReadUInt32();
					this.TransparencyAnimationCount = br.ReadUInt32();
					this.TransparencyAnimationsOffset = br.ReadUInt32();

					if (GetModelVersion(Version) < WarcraftVersion.Wrath)
					{
						this.UnknownCount = br.ReadUInt32();
						this.UnknownOffset = br.ReadUInt32();
					}

					this.UVTextureAnimationCount = br.ReadUInt32();
					this.UVTextureAnimationsOffset = br.ReadUInt32();
					this.ReplaceableTextureCount = br.ReadUInt32();
					this.ReplaceableTexturesOffset = br.ReadUInt32();
					this.RenderFlagCount = br.ReadUInt32();
					this.RenderFlagsOffset = br.ReadUInt32();

					this.BoneLookupTableCount = br.ReadUInt32();
					this.BoneLookupTablesOffset = br.ReadUInt32();
					this.TextureLookupTableCount = br.ReadUInt32();
					this.TextureLookupTablesOffset = br.ReadUInt32();
					this.TextureUnitCount = br.ReadUInt32();
					this.TextureUnitsOffset = br.ReadUInt32();
					this.TransparencyLookupTableCount = br.ReadUInt32();
					this.TransparencyLookupTablesOffset = br.ReadUInt32();
					this.UVTextureAnimationLookupTableCount = br.ReadUInt32();
					this.UVTextureAnimationLookupTablesOffset = br.ReadUInt32();

					this.BoundingBox = br.ReadBox();
					this.BoundingSphereRadius = br.ReadSingle();

					this.CollisionBox = br.ReadBox();
					this.CollisionSphereRadius = br.ReadSingle();

					this.BoundingTriangleCount = br.ReadUInt32();
					this.BoundingTrianglesOffset = br.ReadUInt32();
					this.BoundingVertexCount = br.ReadUInt32();
					this.BoundingVerticesOffset = br.ReadUInt32();
					this.BoundingNormalCount = br.ReadUInt32();
					this.BoundingNormalsOffset = br.ReadUInt32();

					this.AttachmentsCount = br.ReadUInt32();
					this.AttachmentsOffset = br.ReadUInt32();
					this.AttachmentsLookupTableCount = br.ReadUInt32();
					this.AttachmentsLookupTablesOffset = br.ReadUInt32();

					this.AnimationEventCount = br.ReadUInt32();
					this.AnimationEventsOffset = br.ReadUInt32();
					this.LightCount = br.ReadUInt32();
					this.LightsOffset = br.ReadUInt32();
					this.CameraCount = br.ReadUInt32();
					this.CamerasOffset = br.ReadUInt32();
					this.CameraLookupTableCount = br.ReadUInt32();
					this.CameraLookupTablesOffset = br.ReadUInt32();

					this.RibbonEmitterCount = br.ReadUInt32();
					this.RibbonEmittersOffset = br.ReadUInt32();
					this.ParticleEmitterCount = br.ReadUInt32();
					this.ParticleEmittersOffset = br.ReadUInt32();

					if (GlobalModelFlags.HasFlag(ModelObjectFlags.HasBlendModeOverrides))
					{
						this.BlendMapCount = br.ReadUInt32();
						this.BlendMapsOffset = br.ReadUInt32();
					}
				}
			}
		}

		public static WarcraftVersion GetModelVersion(uint Version)
		{
			if (Version <= 256)
			{
				return WarcraftVersion.Classic;
			}
			else if (Version <= 263 && Version > 256)
			{
				return WarcraftVersion.BurningCrusade;
			}
			else if (Version == 264)
			{
				return WarcraftVersion.Wrath;
			}
			else if (Version <= 272 && Version > 264)
			{
				return WarcraftVersion.Cataclysm;
			}
			else if (Version < 274 && Version > 272)
			{
				// It should be noted that this is a guess based on the newer and older
				// model versions. If it works, great - YMMV
				return WarcraftVersion.Warlords;
			}
			else if (Version <= 274)
			{
				return WarcraftVersion.Legion;
			}
			else
			{
				return WarcraftVersion.Unknown;
			}
		}
	}
}

