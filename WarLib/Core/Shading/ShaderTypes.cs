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

