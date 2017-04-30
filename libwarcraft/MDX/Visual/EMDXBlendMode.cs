using System;

namespace Warcraft.MDX.Visual
{
	[Flags]
	public enum EMDXBlendMode : ushort
	{
		Opaque				= 0,
		AlphaTestOnly		= 1,
		AlphaBlending		= 2,
		Additive			= 3,
		AdditiveAlpha		= 4,
		Modulate			= 5,
		DeeprunTramMagic	= 6
	}
}