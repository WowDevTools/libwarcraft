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

namespace Warcraft.MDX
{
	public class MDX
	{
		public MDXHeader Header;

		public readonly List<uint> GlobalSequenceTimestamps = new List<uint>();

		public readonly List<MDXAnimationSequence> AnimationSequences = new List<MDXAnimationSequence>();
		public readonly List<short> AnimationSequenceLookupTable = new List<short>();
		public readonly List<MDXPlayableAnimationLookupTableEntry> PlayableAnimationLookupTable = new List<MDXPlayableAnimationLookupTableEntry>();

		public readonly List<short> KeyedBoneLookupTable = new List<short>();

		public readonly List<MDXVertex> Vertices = new List<MDXVertex>();
		public readonly List<MDXView> LODViews = new List<MDXView>();
		public readonly List<MDXTextureUnit> TextureUnits = new List<MDXTextureUnit>();
		public readonly List<MDXRenderFlagPair> RenderFlags = new List<MDXRenderFlagPair>();
		public readonly List<MDXTexture> Textures = new List<MDXTexture>();
		public readonly List<uint> TextureLookupTable = new List<uint>();

		public MDX(Stream MDXStream)
		{
			using (BinaryReader br = new BinaryReader(MDXStream))
			{
				// Read Wrath header or read pre-wrath header
				MDXFormat Format = PeekFormat(br);
				if (Format < MDXFormat.Wrath)
				{
					this.Header = new MDXHeader(br.ReadBytes(324));
				}
				else
				{
					EMDXFlags Flags = PeekFlags(br);
					if (Flags.HasFlag(EMDXFlags.HasBlendModeOverrides))
					{
						this.Header = new MDXHeader(br.ReadBytes(308));
					}
					else
					{
						this.Header = new MDXHeader(br.ReadBytes(312));
					}
				}

				// Seek to Global Sequences
				br.BaseStream.Position = Header.GlobalSequencesOffset;
				for (int i = 0; i < Header.GlobalSequenceCount; ++i)
				{
					GlobalSequenceTimestamps.Add(br.ReadUInt32());
				}

				// Seek to Animation Sequences
				br.BaseStream.Position = Header.AnimationSequencesOffset;
				int sequenceSize = MDXAnimationSequence.GetSize();
				for (int i = 0; i < Header.AnimationSequenceCount; ++i)
				{
					AnimationSequences.Add(new MDXAnimationSequence(br.ReadBytes(sequenceSize)));
				}

				// Seek to Animation Sequence Lookup Table
				br.BaseStream.Position = Header.AnimationLookupTableOffset;
				for (int i = 0; i < Header.AnimationLookupTableEntryCount; ++i)
				{
					AnimationSequenceLookupTable.Add(br.ReadInt16());
				}

				if (MDXHeader.GetModelVersion(Header.Version) < MDXFormat.Wrath)
				{
					// Seek to Playable Animations Lookup Table
					br.BaseStream.Position = Header.PlayableAnimationLookupTableOffset;
					for (int i = 0; i < Header.PlayableAnimationLookupTableEntryCount; ++i)
					{
						PlayableAnimationLookupTable.Add(new MDXPlayableAnimationLookupTableEntry(br.ReadBytes(4)));
					}					
				}

				// Seek to bone block

				// Seek to Skeletal Bone Lookup Table
				br.BaseStream.Position = Header.KeyedBoneLookupTablesOffset;
				for (int i = 0; i < Header.KeyedBoneLookupTableCount; ++i)
				{
					KeyedBoneLookupTable.Add(br.ReadInt16());
				}

				// Read geometry data

				// Seek to vertex block
				br.BaseStream.Position = Header.VerticesOffset;
				for (int i = 0; i < Header.VertexCount; ++i)
				{
					Vertices.Add(new MDXVertex(br.ReadBytes(48)));
				}

				// Seek to view block
				if (MDXHeader.GetModelVersion(Header.Version) < MDXFormat.Wrath)
				{
					br.BaseStream.Position = Header.LODViewsOffset;
					for (int i = 0; i < Header.LODViewsCount; ++i)
					{
						br.BaseStream.Position = Header.LODViewsOffset + (44 * i);
						MDXViewHeader ViewHeader = new MDXViewHeader(br.ReadBytes(44));

						MDXView View = new MDXView();
						View.Header = ViewHeader;

						// Read view vertex indices
						View.VertexIndices = new List<ushort>();
						br.BaseStream.Position = ViewHeader.VertexIndicesOffset;
						for (int j = 0; j < ViewHeader.VertexIndexCount; ++j)
						{
							View.VertexIndices.Add(br.ReadUInt16());
						}

						// Read view triangles
						View.Triangles = new List<MDXTriangle>();
						br.BaseStream.Position = ViewHeader.TrianglesOffset;
						for (int j = 0; j < ViewHeader.TriangleCount; ++j)
						{
							View.Triangles.Add(new MDXTriangle(br.ReadUInt16(), br.ReadUInt16(), br.ReadUInt16()));
						}

						// Read view vertex properties
						View.VertexProperties = new List<MDXVertexProperty>();
						br.BaseStream.Position = ViewHeader.VertexPropertiesOffset;
						for (int j = 0; j < ViewHeader.VertexPropertyCount; ++j)
						{
							View.VertexProperties.Add(new MDXVertexProperty(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte()));
						}

						// Read view submeshes
						View.Submeshes = new List<MDXSubmesh>();
						br.BaseStream.Position = ViewHeader.SubmeshesOffset;
						for (int j = 0; j < ViewHeader.SubmeshCount; ++j)
						{
							byte[] submeshData;
							if (MDXHeader.GetModelVersion(Header.Version) >= MDXFormat.BurningCrusade)
							{
								submeshData = br.ReadBytes(48);
							}
							else
							{
								submeshData = br.ReadBytes(32);
							}

							View.Submeshes.Add(new MDXSubmesh(submeshData));
						}

						LODViews.Add(View);
					}
				}
				else
				{
					throw new NotImplementedException();
				}

				// Seek to submesh animation block

				// Seek to Texture definition block

				// Seek to transparency block

				// Seek to UV animation block

				// Seek to replaceable textures block

				// Seek to render flag block
				br.BaseStream.Position = Header.RenderFlagsOffset;
				for (int i = 0; i < Header.RenderFlagCount; ++i)
				{
					RenderFlags.Add(new MDXRenderFlagPair(br.ReadBytes(4)));
				}
				// Seek to texture unit block
				br.BaseStream.Position = Header.TextureUnitsOffset;
				for (int i = 0; i < Header.TextureUnitCount; ++i)
				{
					TextureUnits.Add(new MDXTextureUnit(br.ReadBytes(24)));
				}



				// Read animation data

				// Read visual data

				// Read texture data

				// Read FX data
			}
		}

		private MDXFormat PeekFormat(BinaryReader br)
		{
			long initialPosition = br.BaseStream.Position;

			// Skip ahead to the version block
			br.BaseStream.Position += 4;

			uint rawVersion = br.ReadUInt32();			

			// Seek back to the initial position
			br.BaseStream.Position = initialPosition;

			return MDXHeader.GetModelVersion(rawVersion);
		}

		private EMDXFlags PeekFlags(BinaryReader br)
		{
			long initialPosition = br.BaseStream.Position;

			// Skip ahead to the flag block
			br.BaseStream.Position += 16;

			EMDXFlags flags = (EMDXFlags)br.ReadUInt32();

			// Seek back to the initial position
			br.BaseStream.Position = initialPosition;

			return flags;
		}
	}
}

