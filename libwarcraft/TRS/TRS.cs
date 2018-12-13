//
//  TRS.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
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
    public class TRS
    {
        public Dictionary<string, string> HashMappings = new Dictionary<string, string>();

        public TRS(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (TextReader tr = new StreamReader(ms))
                {
                    while (ms.Position != ms.Length)
                    {
                        string mappingLine = tr.ReadLine();
                        if (mappingLine.StartsWith("dir:"))
                        {
                            continue;
                        }

                        string[] lineParts = mappingLine.Split('\t');
                        this.HashMappings.Add(lineParts[0], lineParts[1]);
                    }
                }
            }
        }
    }
}

