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
                    Version = br.ReadIFFChunk<TerrainVersion>();

                    Header = br.ReadIFFChunk<ModelRootHeader>();

                    Textures = br.ReadIFFChunk<ModelTextures>();
                    Materials = br.ReadIFFChunk<ModelMaterials>();

                    GroupNames = br.ReadIFFChunk<ModelGroupNames>();
                    GroupInformation = br.ReadIFFChunk<ModelGroupInformation>();

                    Skybox = br.ReadIFFChunk<ModelSkybox>();

                    PortalVertices = br.ReadIFFChunk<ModelPortalVertices>();
                    Portals = br.ReadIFFChunk<ModelPortals>();
                    PortalReferences = br.ReadIFFChunk<ModelPortalReferences>();

                    VisibleVertices = br.ReadIFFChunk<ModelVisibleVertices>();
                    VisibleBlocks = br.ReadIFFChunk<ModelVisibleBlocks>();

                    StaticLighting = br.ReadIFFChunk<ModelStaticLighting>();

                    DoodadSets = br.ReadIFFChunk<ModelDoodadSets>();
                    DoodadPaths = br.ReadIFFChunk<ModelDoodadPaths>();
                    DoodadInstances = br.ReadIFFChunk<ModelDoodadInstances>();

                    Fog = br.ReadIFFChunk<ModelFog>();

                    // Optional chunk
                    if ((br.BaseStream.Position != br.BaseStream.Length) && br.PeekChunkSignature() == ModelConvexPlanes.Signature)
                    {
                        ConvexPlanes = br.ReadIFFChunk<ModelConvexPlanes>();
                    }

                    // Version-dependent chunk
                    if ((br.BaseStream.Position != br.BaseStream.Length) && br.PeekChunkSignature() == ModelGameObjectFileID.Signature)
                    {
                        GameObjectFileID = br.ReadIFFChunk<ModelGameObjectFileID>();
                    }
                }
            }
        }

        public bool ContainsGroup(ModelGroup modelGroup)
        {
            bool containsGroupName = GroupNames.GroupNames.Count(kvp => kvp.Key == modelGroup.GetInternalNameOffset()) > 0;
            bool containsDescriptiveGroupName = GroupNames.GroupNames.Count(kvp => kvp.Key == modelGroup.GetInternalDescriptiveNameOffset()) > 0;

            // sometimes, model groups don't contain a descriptive name.
            if (modelGroup.GetInternalDescriptiveNameOffset() > 0)
            {
                return containsGroupName && containsDescriptiveGroupName;
            }

            return containsGroupName;
        }

        public string GetInternalGroupName(ModelGroup modelGroup)
        {
            return GroupNames.GetInternalGroupName(modelGroup);
        }

        public string GetInternalDescriptiveGroupName(ModelGroup modelGroup)
        {
            return GroupNames.GetInternalDescriptiveGroupName(modelGroup);
        }

        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.WriteIFFChunk(Version);
                    bw.WriteIFFChunk(Header);

                    bw.WriteIFFChunk(Textures);
                    bw.WriteIFFChunk(Materials);

                    bw.WriteIFFChunk(GroupNames);
                    bw.WriteIFFChunk(GroupInformation);

                    bw.WriteIFFChunk(Skybox);

                    bw.WriteIFFChunk(PortalVertices);
                    bw.WriteIFFChunk(Portals);
                    bw.WriteIFFChunk(PortalReferences);

                    bw.WriteIFFChunk(VisibleVertices);
                    bw.WriteIFFChunk(VisibleBlocks);

                    bw.WriteIFFChunk(StaticLighting);

                    bw.WriteIFFChunk(DoodadSets);
                    bw.WriteIFFChunk(DoodadPaths);
                    bw.WriteIFFChunk(DoodadInstances);

                    bw.WriteIFFChunk(Fog);

                    if (ConvexPlanes != null)
                    {
                        bw.WriteIFFChunk(ConvexPlanes);
                    }

                    if (GameObjectFileID != null)
                    {
                        bw.WriteIFFChunk(GameObjectFileID);
                    }
                }

                return ms.ToArray();
            }
        }
    }
}

