# libwarcraft

libwarcraft is a managed C# library for interacting with binary file formats created by Blizzard Activision. The goal of the library is to be fully compatible with all proprietary file formats used in their games, and to serve as an all-in-one backend solution for applications wishing to read and modify these files.

The primary focus for the library at the moment is to provide a complete implementation of all formats used up to and including Wrath of the Lich King. Anyone is free to use this library to write their own applications for performing operations on the file formats, and is in fact encouraged. The library is not meant for specialized applications (adding water to terrain, repacking images, adding animations or editing geometry of models) but rather strives to expose all functionality needed for other applications to implement that functionality as is best for their use case.

libwarcraft currently implements the following file formats:
* BLP (Read/Write, versions 0, 1 and 2)
* MPQ (Read, Basic to extended v1 format.)
* DBC (Read, DB1 format.)
* TRS (Hashmap Translation Table): Read support.

The following formats are still in progress, and may not work as intended (or at all):
* MDX (Model). Partial Read, Wrath of the Lich King. No animation data exposed, but it's being read internally.
* WMO (World Model Object). Classes stubbed.
* ADT (Areadata Tile). Near-full read support, up to and including Wrath of the Lich King.

The following formats are not implemented yet:
* WDT (World Data Tables)
* WDL (World Data Layers)
* WLW (World Liquids - Water)
* WLQ (World Liquids)
* WLM (World Liquid Mapping)
* LIT (Map Lighting (now obsolete)
* CHK.NOT (Unknown. Possibly Godrays)
* DB2 (Cataclysm (and up) DBC files)
* WDB (Client Cache Database)
* TBL (Hotfix Identifier)
* BLS (Blizzard Shader Container)
* TEX (Streaming Textures)
* BLOB (Model Bounding Boxes)
* DNC.DB (Day/Night Cycle Database (now obsolete))

If you want to help out, I'll gladly accept pull requests and patches for the code, as well as further implementations of current or future file formats. If you have a project that uses the library and you want to share it with other people, send me a link and I'll add it to the list below.

## Projects
libwarcraft is still in its infancy, but it is still in a quite usable state. The following is a list of projects that use libwarcraft to power their file format needs. A listing here is not an endorsement, but be sure to check them out! Some of my own projects are listed here as well.

* [Everlook](https://github.com/Nihlus/Everlook) - a cross-platform C# replacement for World of Warcraft Model Viewer.

## Licensing

libwarcraft is Free Software, and is distributed under the GPLv3 license. This means, in simple terms, that you are free to do whatever you want with the source code and any binaries compiled or generated from it as long as you pass on those rights to anyone aquiring a copy of the source code or binaries. The full licence can be read in the file "LICENSE" at the root of the source tree, or at http://choosealicense.com/licenses/gpl-3.0/, where a more people-friendly summary is also available.

The use of the GPLv3 license and not the LGPLv3 license is a conscious choice by me, the author. If you're unsure what this means, it means that any software that links to or makes use of this library must also be licensed under the GPLv3 license and carry the same rights and grants of rights as the library itself. I am of the firm opinion that closed source software stifles creativity and inhibits technological progress. This library is  not only a way to develop tools that can be used to interact with WoW and its ilk, but also a statement from me to the modding scene. Share, develop, and be wonderful to each other. Don't adhere to the old closed-source ways, and embrace the wonderful world of free software.

## Credits
Most file documentation has been gathered from https://wowdev.wiki/ - a great big thanks to everyone who contributed and is contributing to that wiki! The implementation of the MPQ format was mainly taken from devklog.net and zezula.net. The latter has shut down, but is still available through the wayback machine, while the latter is nice as complementary information.

Special thanks to:
* Justin Olbrantz & Jean-Francois Roy for their thorough documentation of the MPQ format. Although their wiki page is down and has been for a while, the Wayback Machine has a cached copy which proved invaluable.
* [Ladislav Zezula](http://www.zezula.net/), whose C/C++ implementation of the MPQ format in StormLib provided me with complementary information to the wiki above. Many small crucial details pile up!
* [Nick Acaves](https://github.com/nickaceves/) for his C# implementation of the MPQ encryption algorithm, without which I never would have understood it.
* [James Telfer](https://github.com/jamestelfer/) & [Mark Adler](https://github.com/madler/) for James' work on the Blast compression library, which is a C# reimplementation of Mark's C implementation of the PKWare Implode compression algorithm. Legacy game modders have you to thank.
* [Sam Hocevar](http://sam.zoy.org/) & [Rogueadyn](https://github.com/Rogueadyn/) - Rogueadyn for the DotSquish DXTC implementation, with which BLP files spring to life, and Sam for his original code - not to mention his work on Debian and Mono, which are my primary development platforms!
* [Brendan Tompkins](http://codebetter.com/brendantompkins/) for the image quantization algorithms used in generating palettized BLP images from normal file types.
* [Jakob "Rayts5"](https://github.com/rayts5), for his endless will to bounce my stupid ideas and listen to my complaints about the idiocy and/or brilliance of the WoW file formats.
