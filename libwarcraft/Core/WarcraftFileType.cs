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

namespace Warcraft.Core
{
	/// <summary>
	/// A statically typed set of filetypes that can be encountered in the Warcraft archives. This allows the program
	/// to easily distinguish file types and associated functionality.
	/// </summary>
	public enum WarcraftFileType
	{
		/// <summary>
		/// Unknown files. This file type hasn't been entered into the source code yet.
		/// </summary>
		Unknown,

		/// <summary>
		/// A directory. This is not a file.
		/// </summary>
		Directory,

		/// <summary>
		/// A manifest containing a list of files which belong to an addon.
		/// .toc
		/// </summary>
		AddonManifest,

		/// <summary>
		/// A manifest signature to verify the integrity of a manifest.
		/// .toc.sig
		/// </summary>
		AddonManifestSignature,

		/// <summary>
		/// A "Mike O'Brien Package", also known as an MPQ archive.
		/// .mpq
		/// </summary>
		MoPaQArchive,

		/// <summary>
		/// A configuration file.
		/// .wtf
		/// </summary>
		ConfigurationFile,

		/// <summary>
		/// A client database container.
		/// .dbc
		/// </summary>
		DatabaseContainer,

		/// <summary>
		/// A shader container.
		/// .bls
		/// </summary>
		Shader,

		/// <summary>
		/// Water definitions for the terrain files.
		/// .wlw
		/// </summary>
		TerrainWater,

		/// <summary>
		/// Liquid definitions for the terrain files.
		/// .wlq
		/// </summary>
		TerrainLiquid,

		/// <summary>
		/// Level or map file, containing the layout and general information about a map.
		/// .wdl
		/// </summary>
		TerrainLevel,

		/// <summary>
		/// Table of all terrain tiles in a map.
		/// .wdt
		/// </summary>
		TerrainTable,

		/// <summary>
		/// Terrain tile, containing actual terrain data.
		/// .adt
		/// </summary>
		TerrainData,

		/// <summary>
		/// Binary image in BLP format.
		/// .blp
		/// </summary>
		BinaryImage,

		/// <summary>
		/// Hash translation table. Used to translate from minimap paths to MD5 hashes and back.
		/// .trs
		/// </summary>
		Hashmap,

		/// <summary>
		/// Game object that isn't huge and can be animated. Commonly known as M2 files.
		/// .m2, .mdx
		/// </summary>
		GameObjectModel,

		/// <summary>
		/// World object root file. Huge, static, no animations. Commonly known as WMO files.
		/// .wmo
		/// </summary>
		WorldObjectModel,

		/// <summary>
		/// Group files for world object files. This is where the actual model data lives.
		/// name_000-512.wmo
		/// </summary>
		WorldObjectModelGroup,

		// Here come a few "normal" files
		/// <summary>
		/// WAV-compressed audio.
		/// </summary>
		WaveAudio,

		/// <summary>
		/// MP3-compressed audio.
		/// </summary>
		MP3Audio,

		/// <summary>
		/// EXtensible Markup Language file.
		/// </summary>
		XML,

		/// <summary>
		/// JPEG image file.
		/// </summary>
		JPGImage,

		/// <summary>
		/// Animated GIF file. It's pronounced with a hard G.
		/// </summary>
		GIFImage,

		/// <summary>
		/// PNG image.
		/// </summary>
		PNGImage,

		/// <summary>
		/// INI configuration file.
		/// </summary>
		INI,

		/// <summary>
		/// PDF presentation file.
		/// </summary>
		PDF,

		/// <summary>
		/// Hypertext file. Commonly known as a website.
		/// </summary>
		HTML
	}
}

