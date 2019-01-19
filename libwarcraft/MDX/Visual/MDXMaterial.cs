//
//  MDXMaterial.cs
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

using System.IO;
using Warcraft.Core;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Shading.Blending;

namespace Warcraft.MDX.Visual
{
    /// <summary>
    /// An <see cref="MDXMaterial"/> corresponds to a set of graphics states which should be enabled for a draw call,
    /// most commonly occlusion culling, Z buffering, blending, etc.
    /// </summary>
    public class MDXMaterial : IVersionedClass
    {
        /// <summary>
        /// Gets or sets a general set of flags, that is, actions to apply to this material.
        /// </summary>
        public EMDXRenderFlag Flags { get; set; }

        /// <summary>
        /// Gets or sets the framebuffer blending mode to apply with this material.
        /// </summary>
        public BlendingMode BlendMode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXMaterial"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        /// <param name="version">The version to read the instance in the context of.</param>
        public MDXMaterial(BinaryReader br, WarcraftVersion version)
        {
            Flags = (EMDXRenderFlag)br.ReadUInt16();
            if (version >= WarcraftVersion.Cataclysm)
            {
                BlendMode = (BlendingMode)br.ReadUInt16();
            }
            else
            {
                BlendMode = RemapBlendingMode(br.ReadUInt16());
            }
        }

        /// <summary>
        /// Remaps the blending mode according to the following table:
        /// [0, 1, 2, 10, 5, 6, ...]
        ///
        /// This is required before Cataclysm, probably due to a mismatch between the enumerations used in the
        /// client and the exporter.
        /// </summary>
        /// <param name="blendingMode">The blending mode.</param>
        /// <returns>The remapped mode.</returns>
        private static BlendingMode RemapBlendingMode(uint blendingMode)
        {
            if (blendingMode == 3)
            {
                return BlendingMode.NoAlphaAdditive;
            }

            if (blendingMode > 3)
            {
                return (BlendingMode)(blendingMode - 1);
            }

            return (BlendingMode)blendingMode;
        }
    }
}
