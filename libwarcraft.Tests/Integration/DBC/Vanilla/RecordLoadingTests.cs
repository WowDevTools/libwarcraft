using NUnit.Framework;
using Warcraft.Core;

namespace libwarcraft.Tests.Integration.DBC.Vanilla
{
    [TestFixture]
    public class VanillaDefinitionTests : RecordDefinitionTests
    {
        [SetUp]
        public override void Setup()
        {
            this.Version = WarcraftVersion.Classic;
        }
    }
}
