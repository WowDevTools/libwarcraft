//
//  ModelGroupNames.cs
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
using System.Linq;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.WMO.GroupFile;

namespace Warcraft.WMO.RootFile.Chunks
{
	// TODO: Rework to support offset-based seeking and adding of strings
	public class ModelGroupNames : IIFFChunk, IBinarySerializable
	{
		public const string Signature = "MOGN";

		public readonly Dictionary<long, string> GroupNames = new Dictionary<long, string>();

		public ModelGroupNames()
		{

		}

		public ModelGroupNames(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					// Skip the first two bytes, since they're always zero
					ms.Position += 2;

					while (ms.Position < ms.Length)
					{
						this.GroupNames.Add(ms.Position, br.ReadNullTerminatedString());
					}
				}
			}
        }

        public string GetSignature()
        {
        	return Signature;
        }

		public string GetInternalGroupName(ModelGroup modelGroup)
		{
			int internalNameOffset = (int) modelGroup.GetInternalNameOffset();
			if (this.GroupNames.ContainsKey(internalNameOffset))
			{
				return this.GroupNames[internalNameOffset];
			}

			throw new ArgumentException("Group name not found.", nameof(modelGroup));
		}

		public string GetInternalDescriptiveGroupName(ModelGroup modelGroup)
		{
			int internalDescriptiveNameOffset = (int) modelGroup.GetInternalDescriptiveNameOffset();
			if (this.GroupNames.ContainsKey(internalDescriptiveNameOffset))
			{
				return this.GroupNames[internalDescriptiveNameOffset];
			}

			throw new ArgumentException("Descriptive group name not found.", nameof(modelGroup));
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter bw = new BinaryWriter(ms))
				{
					// Each block begins with two empty strings
					bw.Write('\0');
					bw.Write('\0');

					// Then the actual data
					for (int i = 0; i < this.GroupNames.Count; ++i)
					{
						bw.WriteNullTerminatedString(this.GroupNames.ElementAt(i).Value);
					}

					// Then zero padding to an even 4-byte boundary at the end
					long count = 4 - (ms.Position % 4);
					if (count >= 4)
					{
						return ms.ToArray();
					}

					for (long i = 0; i < count; ++i)
					{
						bw.Write('\0');
					}
				}

				return ms.ToArray();
			}
		}
	}
}

