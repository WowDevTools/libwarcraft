//
//  FileInfoUtilities.cs
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
using System.Text.RegularExpressions;

namespace Warcraft.Core
{
	/// <summary>
	/// Extension methods for information
	/// </summary>
	public static class FileInfoUtilities
	{
		/// <summary>
		/// Gets the type of the referenced file.
		/// </summary>
		/// <returns>The referenced file type.</returns>
		public static WarcraftFileType GetFileType(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException(nameof(path));
			}

			string fileExtension = Path.GetExtension(path).Replace(".", "").ToLowerInvariant();

			switch (fileExtension)
			{
				case "mpq":
				{
					return WarcraftFileType.MoPaQArchive;
				}
				case "toc":
				{
					return WarcraftFileType.AddonManifest;
				}
				case "sig":
				{
					return WarcraftFileType.AddonManifestSignature;
				}
				case "wtf":
				{
					return WarcraftFileType.ConfigurationFile;
				}
				case "dbc":
				{
					return WarcraftFileType.DatabaseContainer;
				}
				case "bls":
				{
					return WarcraftFileType.Shader;
				}
				case "wlw":
				{
					return WarcraftFileType.TerrainWater;
				}
				case "wlq":
				{
					return WarcraftFileType.TerrainLiquid;
				}
				case "wdl":
				{
					return WarcraftFileType.TerrainLiquid;
				}
				case "wdt":
				{
					return WarcraftFileType.TerrainTable;
				}
				case "adt":
				{
					return WarcraftFileType.TerrainData;
				}
				case "blp":
				{
					return WarcraftFileType.BinaryImage;
				}
				case "trs":
				{
					return WarcraftFileType.Hashmap;
				}
				case "m2":
				case "mdx":
				{
					return WarcraftFileType.GameObjectModel;
				}
				case "wmo":
				{
					Regex groupDetectRegex = new Regex("(.+_[0-9]{3}.wmo)", RegexOptions.Multiline);

					if (groupDetectRegex.IsMatch(path))
					{
						return WarcraftFileType.WorldObjectModelGroup;
					}
					else
					{
						return WarcraftFileType.WorldObjectModel;
					}
				}
				case "mp3":
				{
					return WarcraftFileType.MP3Audio;
				}
				case "wav":
				{
					return WarcraftFileType.WaveAudio;
				}
				case "xml":
				{
					return WarcraftFileType.XML;
				}
				case "jpg":
				case "jpeg":
				{
					return WarcraftFileType.JPGImage;
				}
				case "gif":
				{
					return WarcraftFileType.GIFImage;
				}
				case "png":
				{
					return WarcraftFileType.PNGImage;
				}
				case "ini":
				{
					return WarcraftFileType.INI;
				}
				case "pdf":
				{
					return WarcraftFileType.PDF;
				}
				case "htm":
				case "html":
				{
					return WarcraftFileType.HTML;
				}
				case "dylib":
				case "dll":
				case "exe":
				{
					return WarcraftFileType.Assembly;
				}
				default:
				{
					return WarcraftFileType.Unknown;
				}
			}
		}
	}
}