//
//  MDXTextureWeight.cs
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
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;

namespace Warcraft.MDX.Animation
{
    /// <summary>
    /// Defines a texture weight animation.
    /// </summary>
    public class MDXTextureWeight : IVersionedClass
    {
        /// <summary>
        /// Gets or sets the weight track.
        /// </summary>
        public MDXTrack<short> Weight { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXTextureWeight"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        /// <param name="version">The version to read the instance in the context of.</param>
        public MDXTextureWeight(BinaryReader br, WarcraftVersion version)
        {
            Weight = br.ReadMDXTrack<short>(version);
        }
    }
}
