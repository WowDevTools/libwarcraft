//
//  RecordValues.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
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
using System.Linq;

#pragma warning disable 1591, SA1600, SA1649, SA1402

namespace Warcraft.Unit.Reflection.DBC.TestData
{
    public class RecordValues
    {
        public static readonly byte[] MultiMoveClassicBytes = new[] { 1, 2, 4, 8, 16, 32 }.SelectMany(BitConverter.GetBytes).ToArray();
        public static readonly byte[] MultiMoveBCBytes = new[] { 1, 8, 2, 32, 4, 16 }.SelectMany(BitConverter.GetBytes).ToArray();
        public static readonly byte[] MultiMoveWrathBytes = new[] { 1, 16, 8, 2, 32, 4 }.SelectMany(BitConverter.GetBytes).ToArray();

        public static readonly byte[] SimpleClassicBytes = new[] { 1, 2, 4, 8 }.SelectMany(BitConverter.GetBytes).ToArray();
        public static readonly byte[] SimpleWrathBytes = new[] { 1, 2, 4, 8, 16 }.SelectMany(BitConverter.GetBytes).ToArray();
        public static readonly byte[] SimpleCataBytes = new[] { 1, 2, 8, 16 }.SelectMany(BitConverter.GetBytes).ToArray();
    }
}
