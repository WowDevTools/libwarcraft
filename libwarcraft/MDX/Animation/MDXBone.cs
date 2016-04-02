//
//  MDXBone.cs
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
using Warcraft.Core;

namespace Warcraft.MDX.Animation
{
	public class MDXBone
	{
		public uint AnimationID;
		public MDXBoneFlags Flags;
		public short ParentBone;
		public ushort SubmeshID;
		public ushort Unknown1;
		public ushort Unknown2;
		public MDXTrack<Vector3f> AnimatedTranslation;
		public MDXTrack<Quaternion> AnimatedRotation;
		public MDXTrack<Vector3f> AnimatedScale;
		public Vector3f PivotPoint;

		public MDXBone(byte[] data, MDXFormat Format)
		{
		}
	}

	public enum MDXBoneFlags : uint
	{
		SphericalBillboard = 0x8,
		CylindricalBillboard_LockedX = 0x10,
		CylindricalBillboard_LockedY = 0x20,
		CylindricalBillboard_LockedZ = 0x40,
		Transformed = 0x200,
		KinematicBone = 0x400,
		ScaledAnimation = 0x1000
	}
}

