//
//  TRS.cs
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

using System.Collections.Generic;
using System.IO;

namespace Warcraft.TRS
{
    /// <summary>
    /// Represents a hash translation table file.
    /// </summary>
    public class TRS
    {
        /// <summary>
        /// Gets the mapping table.
        /// </summary>
        public Dictionary<string, string> HashMappings { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TRS"/> class.
        /// </summary>
        /// <param name="data">The binary data.</param>
        public TRS(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (TextReader tr = new StreamReader(ms))
                {
                    while (ms.Position != ms.Length)
                    {
                        var mappingLine = tr.ReadLine();
                        if (mappingLine.StartsWith("dir:"))
                        {
                            continue;
                        }

                        var lineParts = mappingLine.Split('\t');
                        HashMappings.Add(lineParts[0], lineParts[1]);
                    }
                }
            }
        }
    }
}
