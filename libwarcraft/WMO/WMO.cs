//
//  WMO.cs
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

using System.Collections.Generic;
using System.Linq;

using Warcraft.WMO.GroupFile;
using Warcraft.WMO.RootFile;
using Warcraft.WMO.RootFile.Chunks;

namespace Warcraft.WMO
{
    /// <summary>
    /// Container class for a World Model Object (WMO).
    /// This class hosts the root file with metadata definitions, as well as the
    /// group files which contain the actual 3D model data.
    /// </summary>
    public class WMO
    {
        /// <summary>
        /// Gets or sets the root information of the model.
        /// </summary>
        public ModelRoot RootInformation { get; set; }

        /// <summary>
        /// Gets the groups in the model.
        /// </summary>
        public List<ModelGroup> Groups { get; } = new List<ModelGroup>();

        /// <summary>
        /// Gets the number of groups in the model.
        /// </summary>
        public int GroupCount => (int)RootInformation.Header.GroupCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="WMO"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public WMO(byte[] inData)
        {
            RootInformation = new ModelRoot(inData);

            PostResolveStringReferences();
        }

        private void PostResolveStringReferences() // TODO: Refactor
        {
            foreach (var doodadInstance in RootInformation.DoodadInstances.DoodadInstances)
            {
                var doodadPath = RootInformation.DoodadPaths.GetNameByOffset(doodadInstance.NameOffset);
                doodadInstance.Name = doodadPath;
            }

            foreach (var modelMaterial in RootInformation.Materials.Materials)
            {
                var texturePath0 = RootInformation.Textures.GetTexturePathByOffset(modelMaterial.DiffuseTextureOffset);
                var texturePath1 = RootInformation.Textures.GetTexturePathByOffset(modelMaterial.EnvironmentMapTextureOffset);
                var texturePath2 = RootInformation.Textures.GetTexturePathByOffset(modelMaterial.SpecularTextureOffset);

                if (string.IsNullOrEmpty(texturePath0))
                {
                    texturePath0 = "createcrappygreentexture.blp";
                }

                modelMaterial.DiffuseTexture = texturePath0;
                modelMaterial.EnvironmentMapTexture = texturePath1;
                modelMaterial.SpecularTexture = texturePath2;
            }
        }

        /// <summary>
        /// Determines whether or not the given group belongs to the model.
        /// </summary>
        /// <param name="modelGroup">The group.</param>
        /// <returns>true if the group belongs to the model; otherwise, false.</returns>
        public bool DoesGroupBelongToModel(ModelGroup modelGroup)
        {
            return RootInformation.ContainsGroup(modelGroup);
        }

        /// <summary>
        /// Adds a model group to the model object. The model group must be listed in the root object,
        /// or it won't be accepted by the model.
        /// </summary>
        /// <param name="modelGroup">Model group.</param>
        public void AddModelGroup(ModelGroup modelGroup)
        {
            if (!DoesGroupBelongToModel(modelGroup))
            {
                return;
            }

            modelGroup.Name = ResolveInternalGroupName(modelGroup);
            modelGroup.DescriptiveName = ResolveInternalDescriptiveGroupName(modelGroup);
            Groups.Add(modelGroup);
        }

        /// <summary>
        /// Adds a model group to the model object. The model group must be listed in the root object,
        /// or it won't be accepted by the model.
        /// </summary>
        /// <param name="inData">Byte array containing the model group.</param>
        public void AddModelGroup(byte[] inData)
        {
            var group = new ModelGroup(inData);
            AddModelGroup(group);
        }

        /// <summary>
        /// Gets the internal group name of the given group.
        /// </summary>
        /// <param name="modelGroup">The group.</param>
        /// <returns>The name.</returns>
        public string ResolveInternalGroupName(ModelGroup modelGroup)
        {
            return RootInformation.GetInternalGroupName(modelGroup);
        }

        /// <summary>
        /// Gets the internal descriptive group name of the given group.
        /// </summary>
        /// <param name="modelGroup">The group.</param>
        /// <returns>The descriptive name.</returns>
        private string ResolveInternalDescriptiveGroupName(ModelGroup modelGroup)
        {
            return RootInformation.GetInternalDescriptiveGroupName(modelGroup);
        }

        /// <summary>
        /// Gets the texture names of the model.
        /// </summary>
        /// <returns>The texture names.</returns>
        public IEnumerable<string> GetTextures()
        {
            return RootInformation.Textures.Textures.Select(kvp => kvp.Value).Where(s => !string.IsNullOrEmpty(s)).ToList();
        }

        /// <summary>
        /// Gets a material based on its ID.
        /// </summary>
        /// <param name="materialID">The material ID.</param>
        /// <returns>The material.</returns>
        public ModelMaterial GetMaterial(byte materialID)
        {
            return RootInformation.Materials.Materials[materialID];
        }

        /// <summary>
        /// Gets the materials in this model.
        /// </summary>
        /// <returns>The materials.</returns>
        public IEnumerable<ModelMaterial> GetMaterials()
        {
            return RootInformation.Materials.Materials;
        }
    }
}
