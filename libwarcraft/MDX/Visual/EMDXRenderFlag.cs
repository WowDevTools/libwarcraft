using System;

namespace Warcraft.MDX.Visual
{
	[Flags]
	public enum EMDXRenderFlag : ushort
	{
		Unlit				= 0x1,
		NoFog				= 0x2,
		TwoSided			= 0x4,
		Unknown				= 0x8,
		DisableZBuffering	= 0x10
	}
}