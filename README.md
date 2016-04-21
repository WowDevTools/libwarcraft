libwarcraft
=======

libwarcraft is a managed C# library for interacting with binary file formats created by Blizzard Activision. The goal of the library is to be fully compatible with all proprietary file formats used in their games, and to serve as an all-in-one backend solution for applications wishing to read and modify these files.

The primary focus for the library at the moment is to provide a complete implementation of all formats used up to and including Wrath of the Lich King. Anyone is free to use this library to write their own applications for performing operations on the file formats, and is in fact encouraged. The library is not meant for specialized applications (adding water to terrain, repacking images, adding animations or editing geometry of models) but rather strives to expose all functionality needed for other applications to implement that functionality as is best for their use case.

libwarcraft is Free Software, and is distributed under the GPLv3 license. This means, in simple terms, that you are free to do whatever you want with the source code and any binaries compiled or generated from it as long as you pass on those rights to anyone aquiring a copy of the source code or binaries. The full licence can be read in the file "LICENCE" at the root of the source tree, or at http://choosealicense.com/licenses/gpl-3.0/, where a more people-friendly summary is also available.

Most file documentation has been gathered from https://wowdev.wiki/ - a great big thanks to everyone who contributed and is contributing to that wiki! The implementation of the MPQ format was mainly taken from devklog.net and zezula.net. The latter has shut down, but is still available through the wayback machine, while the latter is nice as complementary information.

Currently implemented file types:
* BLP (Read/Write, versions 0, 1 and 2)
* MPQ (Read, Basic to extended v1 format.)
* DBC (Read, DB1 format.)

In progress:
* MDX (Partial Read, Wrath of the Lich King. No animation data exposed, but it's being read internally.)
* WMO (Stubbed)
* ADT (Partial Read, Wrath of the Lich King)