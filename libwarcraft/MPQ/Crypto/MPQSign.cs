//
//  MPQSign.cs
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

namespace Warcraft.MPQ.Crypto
{
    /// <summary>
    /// This class handles signing of an MPQ archive. Currently, this is unimplemented.
    /// </summary>
    internal static class MPQSign
    {
        static MPQSign()
        {
        }

        /// <summary>
        /// Signs the archive using the given signing strength.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <param name="signingStrength">The signing strength.</param>
        public static void SignArchive(MPQ archive, SigningStrength signingStrength)
        {
        }

        private static void InternalSignWeak(MPQ archive)
        {
        }

        private static void InternalSignString(MPQ archive)
        {
        }

        /// <summary>
        /// Verifies the integrity of the archive, based on the cryptographic signature.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <returns>true if the archive is intact; otherwise, false.</returns>
        public static bool VerifyArchiveIntegrity(MPQ archive)
        {
            return true;
        }
    }
}
