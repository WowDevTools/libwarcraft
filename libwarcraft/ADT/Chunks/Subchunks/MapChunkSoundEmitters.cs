//
//  MapChunkSoundEmitters.cs
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
using Warcraft.Core.Interfaces;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.ADT.Chunks.Subchunks
{
	public class MapChunkSoundEmitters : IIFFChunk, IBinarySerializable, IPostLoad<uint>
	{
		public const string Signature = "MCSE";
		private bool hasFinishedLoading;
		private byte[] Data;


		public MapChunkSoundEmitters()
		{

		}

		public MapChunkSoundEmitters(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
		{
			this.Data = inData;
		}

        public string GetSignature()
        {
        	return Signature;
        }

		public byte[] Serialize()
		{
			throw new NotImplementedException();
		}

		public bool HasFinishedLoading()
		{
			throw new NotImplementedException();
		}

		public void PostLoad(uint loadingParameters)
		{
			throw new NotImplementedException();
		}
	}

	public abstract class SoundEmitter
	{

	}

	public class DatabaseSoundEmitter : SoundEmitter, IBinarySerializable
	{
		public UInt32ForeignKey SoundEntryID;
		public Vector3f Position;
		public Vector3f Size;

		public byte[] Serialize()
		{
			throw new NotImplementedException();
		}
	}

	public class PlainSoundEmitter : SoundEmitter, IBinarySerializable
	{
		public uint SoundID;
		public uint SoundNameID;
		public Vector3f Position;
		public float AttenuationRadiusStart;
		public float AttenuationRadiusEnd;
		public float CutoffDistance;
		public ushort StartTime;
		public ushort EndTime;
		public ushort GroupSilenceMin;
		public ushort GroupSilenceMax;
		public ushort PlayInstancesMin;
		public ushort PlayInstancesMax;
		public ushort LoopCountMin;
		public ushort LoopCountMax;
		public ushort InterSoundGapMin;
		public ushort InterSoundMapMax;

		public byte[] Serialize()
		{
			throw new NotImplementedException();
		}
	}
}

