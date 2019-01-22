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
    /// <summary>
    /// Represents the root chunk of a model.
    /// </summary>
    public class ModelRoot : IBinarySerializable
    {
        /// <summary>
        /// Gets or sets the file format version.
        /// </summary>
        public TerrainVersion Version { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public ModelRootHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the textures.
        /// </summary>
        public ModelTextures Textures { get; set; }

        /// <summary>
        /// Gets or sets the materials.
        /// </summary>
        public ModelMaterials Materials { get; set; }

        /// <summary>
        /// Gets or sets the group names.
        /// </summary>
        public ModelGroupNames GroupNames { get; set; }

        /// <summary>
        /// Gets or sets information about the model's groups.
        /// </summary>
        public ModelGroupInformation GroupInformation { get; set; }

        /// <summary>
        /// Gets or sets the skybox.
        /// </summary>
        public ModelSkybox Skybox { get; set; }

        /// <summary>
        /// Gets or sets the culling portal vertices.
        /// </summary>
        public ModelPortalVertices PortalVertices { get; set; }

        /// <summary>
        /// Gets or sets the culling portals.
        /// </summary>
        public ModelPortals Portals { get; set; }

        /// <summary>
        /// Gets or sets the culling portal references.
        /// </summary>
        public ModelPortalReferences PortalReferences { get; set; }

        /// <summary>
        /// Gets or sets the visible vertices.
        /// </summary>
        public ModelVisibleVertices VisibleVertices { get; set; }

        /// <summary>
        /// Gets or sets the visible blocks.
        /// </summary>
        public ModelVisibleBlocks VisibleBlocks { get; set; }

        /// <summary>
        /// Gets or sets the static lighting.
        /// </summary>
        public ModelStaticLighting StaticLighting { get; set; }

        /// <summary>
        /// Gets or sets the doodad sets.
        /// </summary>
        public ModelDoodadSets DoodadSets { get; set; }

        /// <summary>
        /// Gets or sets the doodad paths.
        /// </summary>
        public ModelDoodadPaths DoodadPaths { get; set; }

        /// <summary>
        /// Gets or sets the doodad instances.
        /// </summary>
        public ModelDoodadInstances DoodadInstances { get; set; }

        /// <summary>
        /// Gets or sets the fog.
        /// </summary>
        public ModelFog Fog { get; set; }

        // Optional chunks

        /// <summary>
        /// Gets or sets the convex planes.
        /// </summary>
        public ModelConvexPlanes ConvexPlanes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelRoot"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public ModelRoot(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            {
                using (var br = new BinaryReader(ms))
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
                }
            }
        }

        /// <summary>
        /// Determines whether or not the model contains a given group.
        /// </summary>
        /// <param name="modelGroup">The group.</param>
        /// <returns>true if the model contains the group; otherwise, false.</returns>
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

        /// <summary>
        /// Gets the internal group name of the given group.
        /// </summary>
        /// <param name="modelGroup">The group.</param>
        /// <returns>The name.</returns>
        public string GetInternalGroupName(ModelGroup modelGroup)
        {
            return GroupNames.GetInternalGroupName(modelGroup);
        }

        /// <summary>
        /// Gets the internal descriptive group name of the given group.
        /// </summary>
        /// <param name="modelGroup">The group.</param>
        /// <returns>The descriptive name.</returns>
        public string GetInternalDescriptiveGroupName(ModelGroup modelGroup)
        {
            return GroupNames.GetInternalDescriptiveGroupName(modelGroup);
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
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
                }

                return ms.ToArray();
            }
        }
    }
}
