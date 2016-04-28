//
//  ShaderTypes.cs
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

namespace Warcraft.Core.Shading
{
	public enum ShaderTypes
	{
		Diffuse = 0,
		Specular = 1,
		Metal = 2,
		Env = 3,
		Opaque = 4,
		EnvMetal = 5,
		TwoLayerDiffuse = 6,
		TwolayerEnvMetal = 7,
		TwoLayerTerrain = 8,
		DiffuseEmissive = 9,
		WaterWindow = 10,
		MaskedEnvMetal = 11,
		EnvMetalEmissive = 12,
		TwoLayerDiffuseOpaque = 13,
		TwoLayerDiffuseEmissive = 14,
		DiffuseTerrain = 16,
		AdditiveMaskedEnvMetal = 17
	}
}

