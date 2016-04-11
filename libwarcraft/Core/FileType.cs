//
//  FileType.cs
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

namespace Warcraft.Core
{
	public static class Filetype
	{
		public static EWarcraftFiletype GetFiletypeOfFile(string fileName)
		{
			fileName = fileName.ToLower();
			if (fileName.Contains("."))
			{
				string fileExtension = Path.GetExtension(fileName).Replace(".", "");

				switch (fileExtension)
				{
					case "mpq":
					case "MPQ":
						{	
							return EWarcraftFiletype.MoPaQArchive;
						}
					case "toc":
						{
							return EWarcraftFiletype.AddonManifest;
						}
					case "sig":
					case "SIG":
						{
							return EWarcraftFiletype.AddonManifestSignature;
						}
					case "wtf":
						{
							return EWarcraftFiletype.ConfigurationFile;
						}
					case "dbc":
						{
							return EWarcraftFiletype.DatabaseContainer;
						}
					case "bls":
						{
							return EWarcraftFiletype.Shader;
						}
					case "wlw":
						{
							return EWarcraftFiletype.TerrainWater;
						}
					case "wlq":
						{
							return EWarcraftFiletype.TerrainLiquid;
						}
					case "wdl":
						{
							return EWarcraftFiletype.TerrainLiquid;
						}
					case "wdt":
						{
							return EWarcraftFiletype.TerrainTable;
						}
					case "adt":
						{
							return EWarcraftFiletype.TerrainData;
						}
					case "blp":
						{
							return EWarcraftFiletype.BinaryImage;
						}
					case "trs":
						{
							return EWarcraftFiletype.Hashmap;
						}				
					case "m2":
					case "M2":
					case "MDX":
						{
							return EWarcraftFiletype.GameObjectModel;
						}
					case "wmo":
					case "WMO":
						{
							return EWarcraftFiletype.WorldObjectModel;
						}
					default: 
						{
							return EWarcraftFiletype.Unknown;
						}
				}
			}
			else
			{
				return EWarcraftFiletype.Directory;
			}
		}
	}

	public enum EWarcraftFiletype
	{
		Unknown,
		Directory,
		AddonManifest,
		AddonManifestSignature,
		MoPaQArchive,
		ConfigurationFile,
		DatabaseContainer,
		Shader,
		TerrainWater,
		TerrainLiquid,
		TerrainLevel,
		TerrainTable,
		TerrainData,
		BinaryImage,
		Hashmap,
		GameObjectModel,
		WorldObjectModel
	}
}

