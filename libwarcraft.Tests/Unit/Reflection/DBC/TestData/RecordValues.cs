using System;
using System.Linq;

namespace libwarcraft.Tests.Unit.Reflection.DBC.TestData
{
    public class RecordValues
    {
        public static readonly byte[] MultiMoveClassicBytes = new []{ 1, 2, 4, 8, 16, 32 }.SelectMany(BitConverter.GetBytes).ToArray();
        public static readonly byte[] MultiMoveBCBytes = new []{ 1, 8, 2, 32, 4, 16 }.SelectMany(BitConverter.GetBytes).ToArray();
        public static readonly byte[] MultiMoveWrathBytes = new []{ 1, 16, 8, 2, 32, 4 }.SelectMany(BitConverter.GetBytes).ToArray();

        public static readonly byte[] SimpleClassicBytes = new []{ 1, 2, 4, 8}.SelectMany(BitConverter.GetBytes).ToArray();
        public static readonly byte[] SimpleWrathBytes = new []{ 1, 2, 4, 8, 16}.SelectMany(BitConverter.GetBytes).ToArray();
        public static readonly byte[] SimpleCataBytes = new []{ 1, 2, 8, 16}.SelectMany(BitConverter.GetBytes).ToArray();
    }
}
