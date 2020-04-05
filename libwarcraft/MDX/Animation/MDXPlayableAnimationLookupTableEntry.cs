//
//  MDXPlayableAnimationLookupTableEntry.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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

namespace Warcraft.MDX.Animation
{
    /// <summary>
    /// An entry in the playable animation lookup table.
    /// </summary>
    public class MDXPlayableAnimationLookupTableEntry
    {
        /// <summary>
        /// Gets or sets the fallback animation ID.
        /// </summary>
        public short FallbackAnimationID { get; set; }

        /// <summary>
        /// Gets or sets the animation flags.
        /// </summary>
        public MDXPlayableAnimationFlags Flags { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDXPlayableAnimationLookupTableEntry"/> class.
        /// </summary>
        /// <param name="inFallbackAnimationID">The fallback animation.</param>
        /// <param name="inFlags">The flags.</param>
        public MDXPlayableAnimationLookupTableEntry(short inFallbackAnimationID, MDXPlayableAnimationFlags inFlags)
        {
            FallbackAnimationID = inFallbackAnimationID;
            Flags = inFlags;
        }
    }
}
