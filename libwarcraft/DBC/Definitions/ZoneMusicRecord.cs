//
//  ZoneMusicRecord.cs
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

using Warcraft.Core;
using Warcraft.Core.Structures;
using Warcraft.DBC.SpecialFields;

namespace Warcraft.DBC.Definitions
{
	public class ZoneMusicRecord : DBCRecord
	{
		public const string RecordName = "ZoneMusic";
		
		public float Volume;
		public StringReference MusicFileDay;
		public StringReference MusicFileNight;
		public Range SilenceIntervalDay; // These ranges are stored as daymin/nightmin/daymax/nightmax)
		public Range SilenceIntervalNight;
		public uint SegmentLengthDay;
		public uint SegmentLengthNight;
		public Range SegmentPlayCountDay;
		public Range SegmentPlayCountNight;
		public uint SoundsDay;
		public uint SoundsNight;
		
		public override void PostLoad(byte[] data)
		{
			throw new System.NotImplementedException();
		}

		public override int GetFieldCount()
		{
			throw new System.NotImplementedException();
		}

		public override int GetRecordSize()
		{
			throw new System.NotImplementedException();
		}
	}
}