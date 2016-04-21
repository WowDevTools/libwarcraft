libwarcraft
=======

libwarcraft is a managed C# library for interacting with binary file formats created by Blizzard Activision. The goal of the library is to be fully compatible with all proprietary file formats used in their games, and to serve as an all-in-one backend solution for applications wishing to read and modify these files.

The primary focus for the library at the moment is to provide a complete implementation of all formats used up to and including Wrath of the Lich King

Most file documentation has been gathered from https://wowdev.wiki/ - a great big thanks to everyone who contributed and is contributing to that wiki! The implementation of the MPQ format was mainly taken from devklog.net and zezula.net. The latter has shut down, but is still available through the wayback machine, while the latter is nice as complementary information.

Currently implemented file types:
* ADT (Partial Read, Wrath of the Lich King)
* BLP (Read/Write, versions 0, 1 and 2)
* MPQ (Read, Basic to extended v1 format.)
* DBC (Read, DB1 format.)

In progress:
* MDX
* WMO
