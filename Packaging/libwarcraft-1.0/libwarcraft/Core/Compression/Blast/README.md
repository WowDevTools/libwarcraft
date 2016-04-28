# Intro

In various places, a PKWare library is used (often called implode.dll)
that implements an uncommon, proprietary compression algorithm.

This algorithm was reverse-engineered by Ben Rudiak-Gould back in 2001, and 
Mark Adler wrote a C implementation in 2003.

This project is a translation into C# of the PKWare compression algorithm 
implemented in C by [Mark Adler](https://github.com/madler/). 

This implementation varies from the C original. 

# Variations

I found that the files compressed in the legacy system I came into contact with
contained multiple, separate, compressed streams. Each stream is delineated by
an end of stream marker. Where the base implementation stops at the end of stream,
this implementation checks for further data on the stream and will start
decompressing again if it finds any.

# Implementation

This is probably the least elegant way of implementing a decompressor. It would
be much better to implement Stream, for example. However, it does what it says
on the tin; it decompresses the stream. If this was being used for something
that was important, you might go to the trouble of making it work better. For
me, though, this works faster than shelling out to an executable as the previous system did.

# Usage

    new Utils.Blast(sourceStream, destinationStream).Decompress();

# Legal

Any part of this implementation that is not covered by the original
notice is Copyright &copy; 2012 James Telfer, and is released under the 
[Apache 2.0 license](http://www.apache.org/licenses/LICENSE-2.0.html).

It should be noted that this algorithm was originally implemented by
PKWare.
 
The C implementation (upon which this is based) is located 
within the [ZLib source tree](https://github.com/madler/zlib/blob/master/contrib/blast/),
with a copy residing as a reference [within](https://github.com/jamestelfer/Blast/blob/master/Blast/reference-code/) this source tree.

See the original [blast.h](https://github.com/madler/zlib/blob/master/contrib/blast/blast.h)
for the terms of the license under which the C implementation was released.
