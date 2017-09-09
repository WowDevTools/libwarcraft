using NUnit.Framework;
using Warcraft.Core;

namespace libwarcraft.Tests.Integration.DBC.Wrath
{
	[TestFixture]
	public class WrathDefinitionTests : RecordDefinitionTests
	{
		[SetUp]
		public override void Setup()
		{
			this.Version = WarcraftVersion.Wrath;
		}
	}
}
