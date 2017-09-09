using System;
using System.Linq;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Integration.DBC.BurningCrusade
{
	[TestFixture]
	public class BurningCrusadeDefinitionTests : RecordDefinitionTests
	{
		[SetUp]
		public override void Setup()
		{
			this.Version = WarcraftVersion.BurningCrusade;
		}
	}
}
