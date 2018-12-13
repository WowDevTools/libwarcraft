using System;
using System.Numerics;

namespace Warcraft.Core.Compression.Squish
{
    internal struct SourceBlock
    {
        public byte Start;
        public byte End;
        public byte Error;

        public SourceBlock(byte Start, byte End, byte Error)
        {
            this.Start = Start;
            this.End = End;
            this.Error = Error;
        }
    }

    internal struct SingleColourLookup
    {
        public SourceBlock[] Sources;

        public SingleColourLookup(SourceBlock one, SourceBlock two)
        {
            Sources = new SourceBlock[2];
            Sources[0] = one;
            Sources[1] = two;
        }
    }

    internal partial class SingleColourFit : ColourFit
    {
        private byte[] colour = new byte[3];
        private Vector3 start;
        private Vector3 end;
        private byte index;
        private int error;
        private int bestError;

        private SingleColourFit(ColourSet colours, SquishOptions flags)
            : base(colours, flags)
        {
            Vector3 values = _Colours.Points[0];

            colour[0] = (byte) FloatToInt(255.0f * values.X, 255);
            colour[1] = (byte) FloatToInt(255.0f * values.Y, 255);
            colour[2] = (byte) FloatToInt(255.0f * values.Z, 255);

            bestError = int.MaxValue;
        }

        private static int FloatToInt(float a, int limit)
        {
            int i = (int) (a + 0.5f);

            // clamp to limit
            if (i < 0)
            {
                i = 0;
            }
            else if (i > limit)
            {
                i = limit;
            }

            return i;
        }

        protected override void Compress3(byte[] block)
        {
            throw new NotImplementedException();
        }

        protected override void Compress4(byte[] block)
        {
            throw new NotImplementedException();
        }
    }
}
