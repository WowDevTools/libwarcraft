//
//  MDXTexture.cs
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
using System.Linq;
using Warcraft.Core.Extensions;

namespace Warcraft.MDX.Visual
{
    /// <summary>
    /// Represents a texture in a model.
    /// </summary>
    public class MDXTexture
    {
        /// <summary>
        /// Gets or sets the texture type.
        /// </summary>
        public MDXTextureType TextureType { get; set; }

        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        public MDXTextureFlags Flags { get; set; }

        /// <summary>
        /// Gets or sets the path to the texture.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXTexture"/> class.
        /// </summary>
        /// <param name="br">The reader to read the instance from.</param>
        public MDXTexture(BinaryReader br)
        {
            TextureType = (MDXTextureType)br.ReadUInt32();
            Flags = (MDXTextureFlags)br.ReadUInt32();

            // This points off to a null-terminated string, so we'll pop off the null byte when deserializing it
            Filename = new string(br.ReadMDXArray<char>().ToArray()).TrimEnd('\0');
        }
    }
}
