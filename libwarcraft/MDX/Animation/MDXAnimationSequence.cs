//
//  MDXAnimation.cs
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
using System.IO;
using System.Runtime.InteropServices;

namespace Warcraft.MDX.Animation
{
	public class MDXAnimationSequence
	{
		public uint AnimationID;
		public uint StartTimestamp;
		public uint EndTimestamp;
		public float MovementSpeed;
		public MDXAnimationSequenceFlags Flags;
		public short Probability;
		public ushort Padding;
		public uint MinimumRepetitions;
		public uint MaximumRepetitions;
		public uint BlendTime;
		public Box BoundingBox;
		public float BoundingSphereRadius;
		public short NextAnimationID;
		public ushort NextAliasedAnimationID;

		public MDXAnimationSequence(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					if (data.Length != 68)
					{
						throw new ArgumentException("Animation block data was too long or too short.");
					}

					this.AnimationID = br.ReadUInt32();
					this.StartTimestamp = br.ReadUInt32();
					this.EndTimestamp = br.ReadUInt32();
					this.MovementSpeed = br.ReadSingle();
					this.Flags = (MDXAnimationSequenceFlags)br.ReadUInt32();
					this.Probability = br.ReadInt16();
					this.Padding = br.ReadUInt16();
					this.MinimumRepetitions = br.ReadUInt32();
					this.MaximumRepetitions = br.ReadUInt32();
					this.BlendTime = br.ReadUInt32();
					this.BoundingBox = br.ReadBox();
					this.BoundingSphereRadius = br.ReadSingle();
					this.NextAnimationID = br.ReadInt16();
					this.NextAliasedAnimationID = br.ReadUInt16();
				}
			}
		}

		public static int GetSize()
		{
			return 68;
		}
	}

	[Flags]
	public enum MDXAnimationSequenceFlags : uint
	{
		SetBlendAnimation = 0x01,
		Unknown1 = 0x02,
		Unknown2 = 0x04,
		Unknown3 = 0x08,
		LoadedAsLowPrioritySequence = 0x10,
		Looping = 0x20,
		IsAliasedAndHasFollowupAnimation = 0x40,
		IsBlended = 0x80,
		LocallyStoredSequence = 0x100
	}
}

