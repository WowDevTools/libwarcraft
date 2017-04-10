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
using System.Collections.Generic;
using Warcraft.MDX.Visual;
using Warcraft.MDX.Geometry;
using Warcraft.MDX.Animation;
using Warcraft.Core;

namespace Warcraft.MDX
{
	public class MDX : IDisposable
	{
		public MDXHeader Header;

		public string Name;

		public readonly List<uint> GlobalSequenceTimestamps = new List<uint>();

		public readonly List<MDXAnimationSequence> AnimationSequences = new List<MDXAnimationSequence>();
		public readonly List<short> AnimationSequenceLookupTable = new List<short>();
		public readonly List<MDXPlayableAnimationLookupTableEntry> PlayableAnimationLookupTable = new List<MDXPlayableAnimationLookupTableEntry>();

		public readonly List<short> KeyedBoneLookupTable = new List<short>();
		public readonly List<MDXBone> Bones = new List<MDXBone>();

		public readonly List<MDXVertex> Vertices = new List<MDXVertex>();
		public readonly List<MDXSkin> Skins = new List<MDXSkin>();
		public readonly List<MDXSubmeshColourAnimation> ColourAnimations = new List<MDXSubmeshColourAnimation>();
		public readonly List<short> TransparencyLookupTable = new List<short>();
		public readonly List<MDXTrack<short>> TransparencyAnimations = new List<MDXTrack<short>>();
		public readonly List<MDXUVAnimation> UVAnimations = new List<MDXUVAnimation>();
		public readonly List<short> TextureUnitLookupTable = new List<short>();
		public readonly List<MDXRenderFlagPair> RenderFlags = new List<MDXRenderFlagPair>();
		public readonly List<MDXTexture> Textures = new List<MDXTexture>();
		public readonly List<uint> TextureLookupTable = new List<uint>();

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
				// Read Wrath header or read pre-wrath header
				WarcraftVersion format = PeekFormat(br);
				if (format < WarcraftVersion.Wrath)
				{
					this.Header = new MDXHeader(br.ReadBytes(324));
				}
				else
				{
					ModelObjectFlags flags = PeekFlags(br);
					if (flags.HasFlag(ModelObjectFlags.HasBlendModeOverrides))
					{
						this.Header = new MDXHeader(br.ReadBytes(308));
					}
					else
					{
						this.Header = new MDXHeader(br.ReadBytes(312));
					}
				}

				// Seek and read model name
				br.BaseStream.Position = this.Header.NameOffset;
				this.Name = new string(br.ReadChars((int) this.Header.NameLength));

				// Seek to Global Sequences
				br.BaseStream.Position = this.Header.GlobalSequencesOffset;
				for (int i = 0; i < this.Header.GlobalSequenceCount; ++i)
				{
					this.GlobalSequenceTimestamps.Add(br.ReadUInt32());
				}

				// Seek to Animation Sequences
				br.BaseStream.Position = this.Header.AnimationSequencesOffset;
				int sequenceSize = MDXAnimationSequence.GetSize();
				for (int i = 0; i < this.Header.AnimationSequenceCount; ++i)
				{
					this.AnimationSequences.Add(new MDXAnimationSequence(br.ReadBytes(sequenceSize)));
				}

				// Seek to Animation Sequence Lookup Table
				br.BaseStream.Position = this.Header.AnimationLookupTableOffset;
				for (int i = 0; i < this.Header.AnimationLookupTableEntryCount; ++i)
				{
					this.AnimationSequenceLookupTable.Add(br.ReadInt16());
				}

				if (MDXHeader.GetModelVersion(this.Header.Version) < WarcraftVersion.Wrath)
				{
					// Seek to Playable Animations Lookup Table
					br.BaseStream.Position = this.Header.PlayableAnimationLookupTableOffset;
					for (int i = 0; i < this.Header.PlayableAnimationLookupTableEntryCount; ++i)
					{
						this.PlayableAnimationLookupTable.Add(new MDXPlayableAnimationLookupTableEntry(br.ReadBytes(4)));
					}
				}

				// Seek to bone block
				br.BaseStream.Position = this.Header.BonesOffset;
				for (int i = 0; i < this.Header.BoneCount; ++i)
				{
					// TODO: properly skip to the next bone record, data is not aligned
					MDXBone bone = new MDXBone();

					bone.AnimationID = br.ReadInt32();
					bone.Flags = (MDXBoneFlags)br.ReadUInt32();
					bone.ParentBone = br.ReadInt16();
					bone.SubmeshID = br.ReadUInt16();

					if (MDXHeader.GetModelVersion(this.Header.Version) >= WarcraftVersion.BurningCrusade)
					{
						bone.Unknown1 = br.ReadUInt16();
						bone.Unknown1 = br.ReadUInt16();
					}

					// TODO: Rework animation track reading
					// Read bone animation header block
					//Bone.AnimatedTranslation = new MDXTrack<Vector3f>(br, MDXHeader.GetModelVersion(Header.Version));
					//Bone.AnimatedRotation = new MDXTrack<Quaternion>(br, MDXHeader.GetModelVersion(Header.Version));
					//Bone.AnimatedScale = new MDXTrack<Vector3f>(br, MDXHeader.GetModelVersion(Header.Version));

					bone.PivotPoint = br.ReadVector3f();

					this.Bones.Add(bone);
				}

				/*
				// Read bone animation data
				foreach (MDXBone Bone in Bones)
				{
					// Read animation translation block
					br.BaseStream.Position = Bone.AnimatedTranslation.Values.ElementsOffset;
					for (int j = 0; j < Bone.AnimatedTranslation.Values.Count; ++j)
					{
						Bone.AnimatedTranslation.Values.Add(br.ReadVector3f());
					}

					// Read animation rotation block
					br.BaseStream.Position = Bone.AnimatedRotation.ValuesOffset;
					for (int j = 0; j < Bone.AnimatedRotation.ValueCount; ++j)
					{
						if (MDXHeader.GetModelVersion(Header.Version) > MDXFormat.Classic)
						{
							Bone.AnimatedRotation.Values.Add(br.ReadQuaternion16());
						}
						else
						{
							Bone.AnimatedRotation.Values.Add(br.ReadQuaternion32());
						}
					}

					// Read animation scale block
					br.BaseStream.Position = Bone.AnimatedScale.ValuesOffset;
					for (int j = 0; j < Bone.AnimatedScale.ValueCount; ++j)
					{
						Bone.AnimatedScale.Values.Add(br.ReadVector3f());
					}
				}
				*/

				// Seek to Skeletal Bone Lookup Table
				br.BaseStream.Position = this.Header.KeyedBoneLookupTablesOffset;
				for (int i = 0; i < this.Header.KeyedBoneLookupTableCount; ++i)
				{
					this.KeyedBoneLookupTable.Add(br.ReadInt16());
				}

				// Seek to vertex block
				br.BaseStream.Position = this.Header.VerticesOffset;
				for (int i = 0; i < this.Header.VertexCount; ++i)
				{
					this.Vertices.Add(new MDXVertex(br.ReadBytes(48)));
				}

				// Seek to view block
				if (MDXHeader.GetModelVersion(this.Header.Version) < WarcraftVersion.Wrath)
				{
					br.BaseStream.Position = this.Header.LODViewsOffset;

					// Read the view headers
					for (int i = 0; i < this.Header.LODViewsCount; ++i)
					{
						MDXSkinHeader skinHeader = new MDXSkinHeader(br.ReadBytes(44));

						MDXSkin skin = new MDXSkin();
						skin.Header = skinHeader;

						this.Skins.Add(skin);
					}

					// Read view data
					foreach (MDXSkin view in this.Skins)
					{
						// Read view vertex indices
						view.VertexIndices = new List<ushort>();
						br.BaseStream.Position = view.Header.VertexIndicesOffset;
						for (int j = 0; j < view.Header.VertexIndexCount; ++j)
						{
							view.VertexIndices.Add(br.ReadUInt16());
						}

						// Read view triangle vertex indices
						view.Triangles = new List<ushort>();
						br.BaseStream.Position = view.Header.TriangleVertexIndicesOffset;
						for (int j = 0; j < view.Header.TriangleVertexCount; ++j)
						{
							view.Triangles.Add(br.ReadUInt16());
						}

						// Read view vertex properties
						view.VertexProperties = new List<MDXVertexProperty>();
						br.BaseStream.Position = view.Header.VertexPropertiesOffset;
						for (int j = 0; j < view.Header.VertexPropertyCount; ++j)
						{
							view.VertexProperties.Add(new MDXVertexProperty(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte()));
						}

						// Read view submeshes
						view.Submeshes = new List<MDXSkinSection>();
						br.BaseStream.Position = view.Header.SkinSectionOffset;
						for (int j = 0; j < view.Header.SkinSectionCount; ++j)
						{
							byte[] submeshData;
							if (MDXHeader.GetModelVersion(this.Header.Version) >= WarcraftVersion.BurningCrusade)
							{
								submeshData = br.ReadBytes(48);
							}
							else
							{
								submeshData = br.ReadBytes(32);
							}

							view.Submeshes.Add(new MDXSkinSection(submeshData));
						}

						view.TextureUnits = new List<MDXTextureUnit>();
						br.BaseStream.Position = view.Header.RenderBatchOffset;
						for (int j = 0; j < view.Header.RenderBatchCount; ++j)
						{
							view.TextureUnits.Add(new MDXTextureUnit(br.ReadBytes(24)));
						}
					}
				}
				else
				{
					throw new NotImplementedException();
				}

				/*
				// TODO: Rework animation track reading
				// Seek to submesh animation block
				br.BaseStream.Position = Header.SubmeshColourAnimationsOffset;
				for (int i = 0; i < Header.SubmeshColourAnimationCount; ++i)
				{
					MDXTrack<RGB> ColourTrack = new MDXTrack<RGB>(br, MDXHeader.GetModelVersion(Header.Version));
					MDXTrack<short> OpacityTrack = new MDXTrack<short>(br, MDXHeader.GetModelVersion(Header.Version));

					MDXSubmeshColourAnimation ColourAnimation = new MDXSubmeshColourAnimation();
					ColourAnimation.ColourTrack = ColourTrack;
					ColourAnimation.OpacityTrack = OpacityTrack;

					ColourAnimations.Add(ColourAnimation);
				}
				// Read submesh animation values
				foreach (MDXSubmeshColourAnimation ColourAnimation in ColourAnimations)
				{
					// Read the colour track
					br.BaseStream.Position = ColourAnimation.ColourTrack.ValuesOffset;
					for (int j = 0; j < ColourAnimation.ColourTrack.ValueCount; ++j)
					{
						ColourAnimation.ColourTrack.Values.Add(new RGB(br.ReadVector3f()));
					}

					// Read the opacity track
					br.BaseStream.Position = ColourAnimation.OpacityTrack.ValuesOffset;
					for (int j = 0; j < ColourAnimation.OpacityTrack.ValueCount; ++j)
					{
						ColourAnimation.OpacityTrack.Values.Add(br.ReadInt16());
					}
				}
				*/

				// TODO: Use this pattern for the tracks as well, where values are outreferenced
				// from the block
				// Seek to Texture definition block
				br.BaseStream.Position = this.Header.TexturesOffset;
				for (int i = 0; i < this.Header.TextureCount; ++i)
				{
					MDXTexture texture = new MDXTexture(br.ReadBytes(16));
					this.Textures.Add(texture);
				}

				// Read the texture definition strings
				foreach (MDXTexture texture in this.Textures)
				{
					br.BaseStream.Position = texture.FilenameOffset;
					texture.Filename = new string(br.ReadChars((int)texture.FilenameLength));
				}
				/*
				// TODO: Rework animation track reading
				// Seek to transparency block
				br.BaseStream.Position = Header.TransparencyAnimationsOffset;
				for (int i = 0; i < Header.TransparencyAnimationCount; ++i)
				{
					TransparencyAnimations.Add(new MDXTrack<short>(br, MDXHeader.GetModelVersion(Header.Version)));
				}
				// Read transparency animation block data
				foreach (MDXTrack<short> TransparencyTrack in TransparencyAnimations)
				{
					// Read the opacity track
					br.BaseStream.Position = TransparencyTrack.ValuesOffset;
					for (int j = 0; j < TransparencyTrack.ValueCount; ++j)
					{
						TransparencyTrack.Values.Add(br.ReadInt16());
					}
				}

				// TODO: Rework animation track reading
				// UV Animations
				br.BaseStream.Position = Header.UVTextureAnimationsOffset;
				for (int i = 0; i < Header.UVTextureAnimationCount; ++i)
				{
					br.BaseStream.Position = Header.UVTextureAnimationsOffset + (i * 84);

					MDXUVAnimation UVAnimation = new MDXUVAnimation();
					UVAnimation.TranslationTrack = new MDXTrack<Vector3f>(br, MDXHeader.GetModelVersion(Header.Version));
					UVAnimation.RotationTrack = new MDXTrack<Quaternion>(br, MDXHeader.GetModelVersion(Header.Version));
					UVAnimation.ScaleTrack = new MDXTrack<Vector3f>(br, MDXHeader.GetModelVersion(Header.Version));

					UVAnimations.Add(UVAnimation);
				}
				// Read UV animation track data
				foreach (MDXUVAnimation UVAnimation in UVAnimations)
				{
					// Read animation translation block
					br.BaseStream.Position = UVAnimation.TranslationTrack.ValuesOffset;
					for (int j = 0; j < UVAnimation.TranslationTrack.ValueCount; ++j)
					{
						UVAnimation.TranslationTrack.Values.Add(br.ReadVector3f());
					}

					// Read animation rotation block
					br.BaseStream.Position = UVAnimation.RotationTrack.ValuesOffset;
					for (int j = 0; j < UVAnimation.RotationTrack.ValueCount; ++j)
					{
						if (MDXHeader.GetModelVersion(Header.Version) > MDXFormat.Classic)
						{
							UVAnimation.RotationTrack.Values.Add(br.ReadQuaternion16());
						}
						else
						{
							UVAnimation.RotationTrack.Values.Add(br.ReadQuaternion32());
						}
					}

					// Read animation scale block
					br.BaseStream.Position = UVAnimation.ScaleTrack.ValuesOffset;
					for (int j = 0; j < UVAnimation.ScaleTrack.ValueCount; ++j)
					{
						UVAnimation.ScaleTrack.Values.Add(br.ReadVector3f());
					}
				}
				*/

				// Replaceable textures

				// Render flags
				// Seek to render flag block
				br.BaseStream.Position = this.Header.RenderFlagsOffset;
				for (int i = 0; i < this.Header.RenderFlagCount; ++i)
				{
					this.RenderFlags.Add(new MDXRenderFlagPair(br.ReadBytes(4)));
				}

				// Bone lookup

				// Texture lookup

				// Texture unit lookup
				// Seek to texture unit lookup block
				br.BaseStream.Position = this.Header.TextureUnitsOffset;
				for (int i = 0; i < this.Header.TextureUnitCount; ++i)
				{
					this.TextureUnitLookupTable.Add(br.ReadInt16());
				}

				// Transparency lookup
				// Seek to transparency lookup table
				br.BaseStream.Position = this.Header.TransparencyLookupTablesOffset;
				for (int i = 0; i < this.Header.TransparencyLookupTableCount; ++i)
				{
					this.TransparencyLookupTable.Add(br.ReadInt16());
				}

				// UV animation lookup

				// Bounding box

				// Bounding radius

				// Collision box

				// Collision radius

				// Bounding tris

				// Bounding verts

				// Bounding normals

				// Attachments

				// Attachment lookup

				// Anim notifies (events)

				// Lights

				// Cameras

				// Camera lookup

				// Ribbon Emitters

				// Particle Emitters

				// Blend maps (if flags say they exist)
			}
		}

		private WarcraftVersion PeekFormat(BinaryReader br)
		{
			long initialPosition = br.BaseStream.Position;

			// Skip ahead to the version block
			br.BaseStream.Position += 4;

			uint rawVersion = br.ReadUInt32();

			// Seek back to the initial position
			br.BaseStream.Position = initialPosition;

			return MDXHeader.GetModelVersion(rawVersion);
		}

		private ModelObjectFlags PeekFlags(BinaryReader br)
		{
			long initialPosition = br.BaseStream.Position;

			// Skip ahead to the flag block
			br.BaseStream.Position += 16;

			ModelObjectFlags flags = (ModelObjectFlags)br.ReadUInt32();

			// Seek back to the initial position
			br.BaseStream.Position = initialPosition;

			return flags;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Warcraft.MDX.MDX"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Warcraft.MDX.MDX"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Warcraft.MDX.MDX"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="Warcraft.MDX.MDX"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Warcraft.MDX.MDX"/> was occupying.</remarks>
		public void Dispose()
		{
			// TODO: Release file on disk
		}
	}
}

