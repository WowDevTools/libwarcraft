//
//  ModelDoodadNames.cs
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

using System;
using System.Collections.Generic;
using System.IO;
using Warcraft.ADT.Chunks;
using Warcraft.Core;

namespace Warcraft.WMO.RootFile.Chunks
{
	public class ModelDoodadNames : IChunk
	{
		public const string Signature = "MODN";

		public readonly List<KeyValuePair<long, string>> DoodadNames = new List<KeyValuePair<long, string>>();

		public ModelDoodadNames(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					while (ms.Position < ms.Length)
					{
						this.DoodadNames.Add(new KeyValuePair<long, string>(ms.Position, br.ReadNullTerminatedString()));
					}
				}
			}

			// Remove null entries from the doodad list
			this.DoodadNames.RemoveAll(s => s.Value.Equals("\0"));
		}
	}
}

