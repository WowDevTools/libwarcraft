//
//  MDXView.cs
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

using System.Collections.Generic;
using Warcraft.MDX.Visual;

namespace Warcraft.MDX.Geometry.Skin
{
	// TODO: This class is manually filled when reading the different skins in the MDX class
	public class MDXSkin
	{
		public MDXSkinHeader Header;
		public List<ushort> VertexIndices;
		public List<ushort> Triangles;
		public List<MDXVertexProperty> VertexProperties;
		public List<MDXSkinSection> Sections;
		public List<MDXRenderBatch> RenderBatches;
	}
}

