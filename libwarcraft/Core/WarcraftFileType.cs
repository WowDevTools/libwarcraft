//
//  FileType.cs
//
//  Copyright (c) 2018 Jarl Gullberg
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

namespace Warcraft.Core
{
    /// <summary>
    /// A statically typed set of filetypes that can be encountered in the Warcraft archives. This allows the program
    /// to easily distinguish file types and associated functionality.
    /// </summary>
    [Flags]
    public enum WarcraftFileType : ulong
    {
        /// <summary>
        /// Unknown file. This file type hasn't been entered into the source code yet.
        /// </summary>
        Unknown                                    = 0x0,

        /// <summary>
        /// A directory. This is not a file.
        /// </summary>
        Directory                                = 0x1,

        /// <summary>
        /// A manifest containing a list of files which belong to an addon.
        /// .toc
        /// </summary>
        AddonManifest                            = 0x2,

        /// <summary>
        /// A manifest signature to verify the integrity of a manifest.
        /// .toc.sig
        /// </summary>
        AddonManifestSignature                    = 0x4,

        /// <summary>
        /// A "Mike O'Brien Package", also known as an MPQ archive.
        /// .mpq
        /// </summary>
        MoPaQArchive                            = 0x8,

        /// <summary>
        /// A configuration file.
        /// .wtf
        /// </summary>
        ConfigurationFile                        = 0x10,

        /// <summary>
        /// A client database container.
        /// .dbc
        /// </summary>
        DatabaseContainer                        = 0x20,

        /// <summary>
        /// A shader container.
        /// .bls
        /// </summary>
        Shader                                    = 0x40,

        /// <summary>
        /// Water definitions for the terrain files.
        /// .wlw
        /// </summary>
        TerrainWater                            = 0x80,

        /// <summary>
        /// Liquid definitions for the terrain files.
        /// .wlq
        /// </summary>
        TerrainLiquid                            = 0x100,

        /// <summary>
        /// Level or map file, containing the layout and general information about a map.
        /// .wdl
        /// </summary>
        TerrainLevel                            = 0x200,

        /// <summary>
        /// Table of all terrain tiles in a map.
        /// .wdt
        /// </summary>
        TerrainTable                            = 0x400,

        /// <summary>
        /// Terrain tile, containing actual terrain data.
        /// .adt
        /// </summary>
        TerrainData                                = 0x800,

        /// <summary>
        /// Binary image in BLP format.
        /// .blp
        /// </summary>
        BinaryImage                                = 0x1000,

        /// <summary>
        /// Hash translation table. Used to translate from minimap paths to MD5 hashes and back.
        /// .trs
        /// </summary>
        Hashmap                                    = 0x2000,

        /// <summary>
        /// Game object that isn't huge and can be animated. Commonly known as M2 files.
        /// .m2, .mdx
        /// </summary>
        GameObjectModel                            = 0x4000,

        /// <summary>
        /// World object root file. Huge, static, no animations. Commonly known as WMO files.
        /// .wmo
        /// </summary>
        WorldObjectModel                        = 0x8000,

        /// <summary>
        /// Group files for world object files. This is where the actual model data lives.
        /// name_000-512.wmo
        /// </summary>
        WorldObjectModelGroup                    = 0x10000,

        // Here come a few "normal" files
        /// <summary>
        /// WAV-compressed audio.
        /// .wav
        /// </summary>
        WaveAudio                                = 0x20000,

        /// <summary>
        /// MP3-compressed audio.
        /// .mp3
        /// </summary>
        MP3Audio                                = 0x40000,

        /// <summary>
        /// EXtensible Markup Language file.
        /// .xml
        /// </summary>
        XML                                        = 0x80000,

        /// <summary>
        /// JPEG image file.
        /// .jpg
        /// .jpeg
        /// </summary>
        JPGImage                                = 0x100000,

        /// <summary>
        /// Animated GIF file. It's pronounced with a hard G.
        /// .gif
        /// </summary>
        GIFImage                                = 0x200000,

        /// <summary>
        /// PNG image.
        /// .png
        /// </summary>
        PNGImage                                = 0x400000,

        /// <summary>
        /// INI configuration file.
        /// .ini
        /// </summary>
        INI                                        = 0x800000,

        /// <summary>
        /// PDF presentation file.
        /// .pdf
        /// </summary>
        PDF                                        = 0x1000000,

        /// <summary>
        /// Web-related file.
        /// .html
        /// .htm
        /// .url
        /// .js
        /// .css
        /// </summary>
        Web                                    = 0x2000000,

        /// <summary>
        /// Some form of assembled code or binary data.
        /// .exe
        /// .dll
        /// .dylib
        /// .zmp
        /// .plist
        /// .xib
        /// .nib
        /// </summary>
        Assembly                                = 0x4000000,

        /// <summary>
        /// Movie subtitles.
        /// .sbt
        /// </summary>
        Subtitles                                = 0x8000000,

        /// <summary>
        /// A font.
        /// .tff
        /// </summary>
        Font                                    = 0x10000000,

        /// <summary>
        /// Simple text.
        /// .txt
        /// </summary>
        Text                                     = 0x20000000,

        /// <summary>
        /// Animation definitions for a model.
        /// </summary>
        Animation                                = 0x40000000,

        /// <summary>
        /// Physics definitions for a model.
        /// </summary>
        Physics                                    = 0x80000000,

        /// <summary>
        /// Skeletal definitons for a model.
        /// </summary>
        Skeleton                                = 0x100000000,

        /// <summary>
        /// A targa image.
        /// </summary>
        TargaImage                                = 0x200000000,

        /// <summary>
        /// A bitmap image.
        /// </summary>
        BitmapImage                                = 0x400000000,

        /// <summary>
        /// OGG Vorbis audio.
        /// </summary>
        VorbisAudio                                = 0x800000000,

        /// <summary>
        /// Windows Media Audio.
        /// </summary>
        WMAAudio                                = 0x1000000000,

        /// <summary>
        /// Cached client data.
        /// </summary>
        DataCache                                = 0x2000000000,

        /// <summary>
        /// Icon storage format.
        /// .icns
        /// </summary>
        IconImage                                = 0x4000000000,

        /// <summary>
        /// Some form of code script.
        /// .lua
        /// </summary>
        Script                                    = 0x8000000000,

        /// <summary>
        /// Light data.
        /// .lit
        /// </summary>
        Lighting                                = 0x10000000000
    }
}

