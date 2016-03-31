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
		public uint GlobalModelFlags;
		public uint GlobalSequenceCount;
		public uint GlobalSequencesOffset;
		public uint AnimationCount;
		public uint AnimationsOffset;
		public uint AnimationLookupCount;
		public uint AnimationLookupsOffset;
		public uint BoneCount;
		public uint BonesOffset;
		public uint KeyBoneLookupTableCount;
		public uint KeyBoneLookupTablesOffset;
		public uint VertexCount;
		public uint VerticesOffset;
		public uint LODCount;
		public uint SubmeshAnimationCount;
		public uint SubmeshAnimationsOffset;
		public uint TextureCount;
		public uint TexturesOffset;
		public uint TransparencyCount;
		public uint TransparenciesOffset;
		public uint UVAnimationCount;
		public uint UVAnimationsOffset;
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
		public uint UVAnimationLookupTableCount;
		public uint UVAnimationLookupTablesOffset;

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
					string Signature = br.ReadChars(4).ToString();
					if (Signature != MDXHeader.Signature)
					{
						throw new ArgumentException("The provided data stream does not contain a valid MDX signature.");
					}

					this.Version = br.ReadUInt32();
					this.NameLength = br.ReadUInt32();
					this.NameOffset = br.ReadUInt32();
					this.GlobalModelFlags = br.ReadUInt32();
					this.GlobalSequenceCount = br.ReadUInt32();
					this.GlobalSequencesOffset = br.ReadUInt32();
					this.AnimationCount = br.ReadUInt32();
					this.AnimationsOffset = br.ReadUInt32();
					this.AnimationLookupCount = br.ReadUInt32();
					this.AnimationLookupsOffset = br.ReadUInt32();
					this.BoneCount = br.ReadUInt32();
					this.BonesOffset = br.ReadUInt32();
					this.KeyBoneLookupTableCount = br.ReadUInt32();
					this.KeyBoneLookupTablesOffset = br.ReadUInt32();
					this.VertexCount = br.ReadUInt32();
					this.VerticesOffset = br.ReadUInt32();
					this.LODCount = br.ReadUInt32();
					this.SubmeshAnimationCount = br.ReadUInt32();
					this.SubmeshAnimationsOffset = br.ReadUInt32();
					this.TextureCount = br.ReadUInt32();
					this.TexturesOffset = br.ReadUInt32();
					this.TransparencyCount = br.ReadUInt32();
					this.TransparenciesOffset = br.ReadUInt32();
					this.UVAnimationCount = br.ReadUInt32();
					this.UVAnimationsOffset = br.ReadUInt32();
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
					this.UVAnimationLookupTableCount = br.ReadUInt32();
					this.UVAnimationLookupTablesOffset = br.ReadUInt32();

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

					/*
					if (GlobalModelFlags.HasFlag(EMDXFlags.HasBlendModeOverrides)
					{
						this.BlendMapCount = br.ReadUInt32();
						this.BlendMapsOffset = br.ReadUInt32();
					}
					*/
				}
			}
		}

		public MDXFormat GetModelVersion()
		{
			if (Version <= 256)
			{
				return MDXFormat.Classic;
			}
			else if (Version <= 263 && Version > 256)
			{
				return MDXFormat.BurningCrusade;
			}
			else if (Version == 264)
			{
				return MDXFormat.Wrath;
			}
			else if (Version <= 272 && Version > 264)
			{
				return MDXFormat.Cataclysm;
			}
			else if (Version < 274 && Version > 272)
			{
				// It should be noted that this is a guess based on the newer and older
				// model versions. If it works, great - YMMV
				return MDXFormat.Warlords;
			}
			else if (Version <= 274)
			{
				return MDXFormat.Legion;
			}
			else
			{
				return MDXFormat.Unknown;
			}
		}
	}
}

