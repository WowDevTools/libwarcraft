//
//  ModelGroup.cs
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

namespace Warcraft.WMO.GroupFile.Chunks
{
	public class ModelGroup
	{
		public ModelGroup()
		{
		}
	}

	[Flags]
	public enum GroupFlags : uint
	{
		HasBSP 							= 0x00000001,
		SubtractAmbientColour 			= 0x00000002,
		HasVertexColours 				= 0x00000004,
		IsOutdoors						= 0x00000008,
		// Unused1						= 0x00000010,
		// Unused2						= 0x00000020,
		DoNotUseLocalDiffuseLighting	= 0x00000040,
		Unreachable						= 0x00000080,
		// Unused3						= 0x00000100
		HasLights						= 0x00000200,
		UnknownLODRelated				= 0x00000400,
		HasDoodads						= 0x00000800,
		HasWater						= 0x00001000,
		IsIndoors						= 0x00002000,
		// Unused4						= 0x00004000,
		// Unused5						= 0x00008000,
		AlwaysDrawEvenIfOutdoors		= 0x00010000,
		HasTriangleStrips				= 0x00020000,
		ShowSkybox						= 0x00040000,
		IsOceanicWater					= 0x00080000,
		// Unused6						= 0x00100000,
		// Unused7						= 0x00200000,
		// Unused8						= 0x00400000,
		// Unused9						= 0x00800000,
		ShadeWithCVerts2				= 0x01000000,
		ShadeWithTVerts2				= 0x02000000,
		CreateOccludersWithoutClearBSP	= 0x04000000,
		UnknownOcclusionRelated			= 0x08000000,
		// Unused10						= 0x10000000,
		// Unused11						= 0x20000000,
		ShadeWithTVerts3				= 0x40000000
		// Unused12						= 0x80000000
	}
}

