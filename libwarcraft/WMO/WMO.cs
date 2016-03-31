using System;
using Warcraft.WMO.RootFile;
using Warcraft.WMO.GroupFile;
using System.Collections.Generic;

namespace Warcraft.WMO
{
	/// <summary>
	/// Container class for a World Model Object (WMO).
	/// This class hosts the root file with metadata definitions, as well as the 
	/// group files which contain the actual 3D model data.
	/// </summary>
	public class WMO
	{
		private Root WMORootObject;
		private List<Group> WMOGroups;

		public WMO()
		{
		}

		/// <summary>
		/// Adds a model group to the model object. The model group must be listed in the root object, 
		/// or it won't be accepted by the model.
		/// </summary>
		/// <param name="modelGroup">Model group.</param>
		public void AddModelGroup(Group modelGroup)
		{
			
		}
	}
}

