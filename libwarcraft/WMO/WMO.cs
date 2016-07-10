using System;
using Warcraft.WMO.RootFile;
using Warcraft.WMO.GroupFile;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Warcraft.WMO.RootFile.Chunks;

namespace Warcraft.WMO
{
	/// <summary>
	/// Container class for a World Model Object (WMO).
	/// This class hosts the root file with metadata definitions, as well as the
	/// group files which contain the actual 3D model data.
	/// </summary>
	public class WMO : IDisposable
	{
		public ModelRoot RootInformation;
		public List<ModelGroup> Groups = new List<ModelGroup>();

		public int GroupCount
		{
			get
			{
				return (int)this.RootInformation.Header.GroupCount;
			}
		}

		public WMO(byte[] inData)
		{
			this.RootInformation = new ModelRoot(inData);

			PostResolveStringReferences();
		}

		private void PostResolveStringReferences()
		{
			foreach (DoodadInstance doodadInstance in this.RootInformation.DoodadInstances.DoodadInstances)
			{
				string doodadPath = this.RootInformation.DoodadNames.GetNameByOffset(doodadInstance.NameOffset);
				doodadInstance.Name = doodadPath;
			}

			foreach (ModelMaterial modelMaterial in this.RootInformation.Materials.Materials)
			{
				string texturePath0 = this.RootInformation.Textures.GetTexturePathByOffset(modelMaterial.FirstTextureOffset);
				string texturePath1 = this.RootInformation.Textures.GetTexturePathByOffset(modelMaterial.SecondTextureOffset);
				string texturePath2 = this.RootInformation.Textures.GetTexturePathByOffset(modelMaterial.ThirdTextureOffset);

				if (string.IsNullOrEmpty(texturePath0))
				{
					texturePath0 = "createcrappygreentexture.blp";
				}

				modelMaterial.Texture0 = texturePath0;
				modelMaterial.Texture1 = texturePath1;
				modelMaterial.Texture2 = texturePath2;
			}
		}

		public bool ContainsGroup(ModelGroup modelGroup)
		{
			return this.RootInformation.ContainsGroup(modelGroup);
		}

		/// <summary>
		/// Adds a model group to the model object. The model group must be listed in the root object,
		/// or it won't be accepted by the model.
		/// </summary>
		/// <param name="modelGroupStream">Stream containing the Model group.</param>
		public void AddModelGroup(Stream modelGroupStream)
		{

		}

		/// <summary>
		/// Adds a model group to the model object. The model group must be listed in the root object,
		/// or it won't be accepted by the model.
		/// </summary>
		/// <param name="modelGroup">Model group.</param>
		public void AddModelGroup(ModelGroup modelGroup)
		{
			if (ContainsGroup(modelGroup))
			{
				modelGroup.Name = ResolveInternalGroupName(modelGroup);
				modelGroup.DescriptiveName = ResolveInternalDescriptiveGroupName(modelGroup);
				this.Groups.Add(modelGroup);
			}
		}

		/// <summary>
		/// Adds a model group to the model object. The model group must be listed in the root object,
		/// or it won't be accepted by the model.
		/// </summary>
		/// <param name="inData">Byte array containing the model group.</param>
		public void AddModelGroup(byte[] inData)
		{
			ModelGroup group = new ModelGroup(inData);
		}

		public string ResolveInternalGroupName(ModelGroup modelGroup)
		{
			return this.RootInformation.GetInternalGroupName(modelGroup);
		}

		private string ResolveInternalDescriptiveGroupName(ModelGroup modelGroup)
		{
			return this.RootInformation.GetInternalDescriptiveGroupName(modelGroup);
		}

		public List<string> GetUsedTextures()
		{
			return this.RootInformation.Textures.Textures.Select(kvp => kvp.Value).Where(s => !string.IsNullOrEmpty(s)).ToList();
		}

		public ModelMaterial GetMaterial(byte materialID)
		{
			return this.RootInformation.Materials.Materials[materialID];
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Warcraft.WMO.WMO"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Warcraft.WMO.WMO"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Warcraft.WMO.WMO"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="Warcraft.WMO.WMO"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Warcraft.WMO.WMO"/> was occupying.</remarks>
		public void Dispose()
		{

		}
	}
}

