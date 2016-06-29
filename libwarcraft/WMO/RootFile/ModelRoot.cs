//
//  ModelRoot.cs
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
using Warcraft.ADT.Chunks;
using Warcraft.Core;
using Warcraft.WMO.GroupFile;
using Warcraft.WMO.RootFile.Chunks;

namespace Warcraft.WMO.RootFile
{
	public class ModelRoot
	{
		public TerrainVersion Version;

		public ModelRootHeader Header;

		public ModelTextures Textures;
		public ModelMaterials Materials;

		public ModelGroupNames GroupNames;
		public ModelGroupInformation GroupInformation;

		public ModelSkybox Skybox;

		public ModelPortalVertices PortalVertices;
		public ModelPortals Portals;
		public ModelPortalReferences PortalReferences;

		public ModelVisibleVertices VisibleVertices;
		public ModelVisibleBlocks VisibleBlocks;

		public ModelStaticLighting StaticLighting;

		public ModelDoodadSets DoodadSets;
		public ModelDoodadNames DoodadNames;
		public ModelDoodadInstances DoodadInstances;

		public ModelFog Fog;

		// Optional chunks
		public ModelConvexPlanes ConvexPlanes;

		// Added in Legion
		public ModelGameObjectFileID GameObjectFileID;

		public ModelRoot(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.Version = br.ReadIFFChunk<TerrainVersion>();

					this.Header = br.ReadIFFChunk<ModelRootHeader>();

					this.Textures = br.ReadIFFChunk<ModelTextures>();
					this.Materials = br.ReadIFFChunk<ModelMaterials>();

					this.GroupNames = br.ReadIFFChunk<ModelGroupNames>();
					this.GroupInformation = br.ReadIFFChunk<ModelGroupInformation>();

					this.Skybox = br.ReadIFFChunk<ModelSkybox>();

					this.PortalVertices = br.ReadIFFChunk<ModelPortalVertices>();
					this.Portals = br.ReadIFFChunk<ModelPortals>();
					this.PortalReferences = br.ReadIFFChunk<ModelPortalReferences>();

					this.VisibleVertices = br.ReadIFFChunk<ModelVisibleVertices>();
					this.VisibleBlocks = br.ReadIFFChunk<ModelVisibleBlocks>();

					this.StaticLighting = br.ReadIFFChunk<ModelStaticLighting>();

					this.DoodadSets = br.ReadIFFChunk<ModelDoodadSets>();
					this.DoodadNames = br.ReadIFFChunk<ModelDoodadNames>();
					this.DoodadInstances = br.ReadIFFChunk<ModelDoodadInstances>();

					this.Fog = br.ReadIFFChunk<ModelFog>();

					// Optional chunk
					if ((br.BaseStream.Position != br.BaseStream.Length) && br.PeekChunkSignature() == ModelConvexPlanes.Signature)
					{
						this.ConvexPlanes = br.ReadIFFChunk<ModelConvexPlanes>();
					}

					// Version-dependent chunk
					if ((br.BaseStream.Position != br.BaseStream.Length) && br.PeekChunkSignature() == ModelGameObjectFileID.Signature)
					{
						this.GameObjectFileID = br.ReadIFFChunk<ModelGameObjectFileID>();
					}
				}
			}
		}

		public bool ContainsGroup(ModelGroup modelGroup)
		{
			//return this.GroupNames.GroupNames.Contains(modelGroup.Header.GroupNameOffset);
			return false;
		}
	}
}

