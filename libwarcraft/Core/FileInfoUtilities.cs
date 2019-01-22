//
//  FileInfoUtilities.cs
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
using System.IO;
using System.Text.RegularExpressions;

namespace Warcraft.Core
{
    /// <summary>
    /// Extension methods for information about files.
    /// </summary>
    public static class FileInfoUtilities
    {
        /// <summary>
        /// Gets the type of the referenced file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The referenced file type.</returns>
        public static WarcraftFileType GetFileType(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            string fileExtension = Path.GetExtension(path).Replace(".", string.Empty).ToLowerInvariant();

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
                case "adb":
                case "db2":
                case "dbc2":
                case "db":
                case "tbl":
                {
                    return WarcraftFileType.DatabaseContainer;
                }

                case "bls":
                case "wfx":
                {
                    return WarcraftFileType.Shader;
                }

                case "wlm":
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
                    var groupDetectRegex = new Regex("(.+_[0-9]{3}.wmo)", RegexOptions.Multiline);

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
                case "url":
                case "js":
                case "css":
                {
                    return WarcraftFileType.Web;
                }

                case "zmp":
                case "dylib":
                case "dll":
                case "exe":
                case "plist":
                case "nib":
                case "xib":
                {
                    return WarcraftFileType.Assembly;
                }

                case "sbt":
                {
                    return WarcraftFileType.Subtitles;
                }

                case "ttf":
                {
                    return WarcraftFileType.Font;
                }

                case "txt":
                {
                    return WarcraftFileType.Text;
                }

                case "anim":
                {
                    return WarcraftFileType.Animation;
                }

                case "phys":
                {
                    return WarcraftFileType.Physics;
                }

                case "bone":
                {
                    return WarcraftFileType.Skeleton;
                }

                case "tga":
                {
                    return WarcraftFileType.TargaImage;
                }

                case "bmp":
                {
                    return WarcraftFileType.BitmapImage;
                }

                case "ogg":
                {
                    return WarcraftFileType.VorbisAudio;
                }

                case "wma":
                {
                    return WarcraftFileType.WMAAudio;
                }

                case "wdb":
                {
                    return WarcraftFileType.DataCache;
                }

                case "icns":
                {
                    return WarcraftFileType.IconImage;
                }

                case "lua":
                {
                    return WarcraftFileType.Script;
                }

                case "lit":
                {
                    return WarcraftFileType.Lighting;
                }

                default:
                {
                    return WarcraftFileType.Unknown;
                }
            }
        }
    }
}
