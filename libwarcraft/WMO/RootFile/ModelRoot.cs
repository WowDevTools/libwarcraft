//
//  ModelRoot.cs
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

using System.IO;
using System.Linq;
using Warcraft.ADT.Chunks;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.WMO.GroupFile;
using Warcraft.WMO.RootFile.Chunks;

namespace Warcraft.WMO.RootFile
{
    public class ModelRoot : IBinarySerializable
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
        public ModelDoodadPaths DoodadPaths;
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
                    this.DoodadPaths = br.ReadIFFChunk<ModelDoodadPaths>();
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
            bool containsGroupName = this.GroupNames.GroupNames.Count(kvp => kvp.Key == modelGroup.GetInternalNameOffset()) > 0;
            bool containsDescriptiveGroupName = this.GroupNames.GroupNames.Count(kvp => kvp.Key == modelGroup.GetInternalDescriptiveNameOffset()) > 0;

            // sometimes, model groups don't contain a descriptive name.
            if (modelGroup.GetInternalDescriptiveNameOffset() > 0)
            {
                return containsGroupName && containsDescriptiveGroupName;
            }

            return containsGroupName;
        }

        public string GetInternalGroupName(ModelGroup modelGroup)
        {
            return this.GroupNames.GetInternalGroupName(modelGroup);
        }

        public string GetInternalDescriptiveGroupName(ModelGroup modelGroup)
        {
            return this.GroupNames.GetInternalDescriptiveGroupName(modelGroup);
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.WriteIFFChunk(this.Version);
                    bw.WriteIFFChunk(this.Header);

                    bw.WriteIFFChunk(this.Textures);
                    bw.WriteIFFChunk(this.Materials);

                    bw.WriteIFFChunk(this.GroupNames);
                    bw.WriteIFFChunk(this.GroupInformation);

                    bw.WriteIFFChunk(this.Skybox);

                    bw.WriteIFFChunk(this.PortalVertices);
                    bw.WriteIFFChunk(this.Portals);
                    bw.WriteIFFChunk(this.PortalReferences);

                    bw.WriteIFFChunk(this.VisibleVertices);
                    bw.WriteIFFChunk(this.VisibleBlocks);

                    bw.WriteIFFChunk(this.StaticLighting);

                    bw.WriteIFFChunk(this.DoodadSets);
                    bw.WriteIFFChunk(this.DoodadPaths);
                    bw.WriteIFFChunk(this.DoodadInstances);

                    bw.WriteIFFChunk(this.Fog);

                    if (this.ConvexPlanes != null)
                    {
                        bw.WriteIFFChunk(this.ConvexPlanes);
                    }

                    if (this.GameObjectFileID != null)
                    {
                        bw.WriteIFFChunk(this.GameObjectFileID);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}

