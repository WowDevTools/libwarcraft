//
//  ModelGroup.cs
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
using System.IO;
using Warcraft.ADT.Chunks;
using Warcraft.Core;
using Warcraft.Core.Interfaces;
using Warcraft.WMO.GroupFile.Chunks;

namespace Warcraft.WMO.GroupFile
{
	public class ModelGroup : IBinarySerializable
	{
		public TerrainVersion Version;
		public ModelGroupData GroupData;

		public string Name
		{
			get;
			set;
		}

		public ModelGroup(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
            {
            	using (BinaryReader br = new BinaryReader(ms))
	            {
		            this.Version = br.ReadIFFChunk<TerrainVersion>();
		            this.GroupData = br.ReadIFFChunk<ModelGroupData>();
	            }
            }
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
            		bw.WriteIFFChunk(this.Version);
		            bw.WriteIFFChunk(this.GroupData
		            );
            	}

            	return ms.ToArray();
            }
		}
	}
}