using System;

namespace Warcraft.MDX.Visual
{
    [Flags]
    public enum EMDXRenderBatchFlags : byte
    {
        Animated         = 0x0,
        Invert             = 0x1,    // Materials invert something
        Transform         = 0x2,
        Projected         = 0x4,
        Static             = 0x10, // Something batch compatible
        Projected2        = 0x20, // Something to do with projected textures
        Weighted        = 0x40     // Uses textureWeights
    }
}
