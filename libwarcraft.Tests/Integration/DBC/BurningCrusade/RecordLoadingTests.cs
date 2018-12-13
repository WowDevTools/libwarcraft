using NUnit.Framework;
using Warcraft.Core;

namespace libwarcraft.Tests.Integration.DBC.BurningCrusade
{
    [TestFixture]
    public class BurningCrusadeDefinitionTests : RecordDefinitionTests
    {
        [SetUp]
        public override void Setup()
        {
            Version = WarcraftVersion.BurningCrusade;
        }
    }
}
