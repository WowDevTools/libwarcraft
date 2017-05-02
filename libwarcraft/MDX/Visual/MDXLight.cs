//
//  MDXLight.cs
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

using System.IO;
using System.Numerics;
using Warcraft.Core;
using Warcraft.Core.Extensions;
using Warcraft.Core.Interfaces;
using Warcraft.Core.Structures;
using Warcraft.MDX.Animation;

namespace Warcraft.MDX.Visual
{
	public class MDXLight : IVersionedClass
	{
		public MDXLightType Type;
		public short Bone; // -1 if not attached to bone
		public Vector3 Position;
		public MDXTrack<RGB> AmbientColour;
		public MDXTrack<float> AmbientIntensity;
		public MDXTrack<RGB> DiffuseColour;
		public MDXTrack<float> DiffuseIntensity;
		public MDXTrack<float> AttenuationStart;
		public MDXTrack<float> AttenuationEnd;
		public MDXTrack<bool> Visibility;

		public MDXLight(BinaryReader br, WarcraftVersion version)
		{
			this.Type = (MDXLightType)br.ReadUInt32();
			this.Bone = br.ReadInt16();
			this.Position = br.ReadVector3();

			this.AmbientColour = br.ReadMDXTrack<RGB>(version);
			this.AmbientIntensity = br.ReadMDXTrack<float>(version);
			this.DiffuseColour = br.ReadMDXTrack<RGB>(version);
			this.DiffuseIntensity = br.ReadMDXTrack<float>(version);
			this.AttenuationStart = br.ReadMDXTrack<float>(version);
			this.AttenuationEnd = br.ReadMDXTrack<float>(version);

			this.Visibility = br.ReadMDXTrack<bool>(version);
		}
	}
}