//
//  RecordParser.cs
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
using Newtonsoft.Json;
using Warcraft.Core;
using System.Reflection;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Warcraft.DBC.Record
{
	public class RecordParser
	{
		public RecordParser()
		{
		}

		public static DBCRecordStructure TryGetDBCDefinition(string DBCName, WarcraftVersion Version)
		{
			if (String.IsNullOrWhiteSpace(DBCName))
			{
				throw new ArgumentNullException("DBCName", "The provided DBC name must not be null or empty.");
			}

			try
			{
				string DBCDefinitionResource = String.Format("Warcraft.DBC.Definitions.{0}.json", DBCName);
				using (Stream jsonStream = Assembly.GetAssembly(typeof(RecordParser)).GetManifestResourceStream(DBCDefinitionResource))
				{
					using (TextReader tr = new StreamReader(jsonStream))
					{
						JObject jsonObject = new JObject(tr.ReadToEnd());					
					}
				}
			}
			catch
			{

			}

			return null;
		}
	}
}

