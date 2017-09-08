using System;
using System.Linq;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Integration.DBC.BurningCrusade
{
	[TestFixture]
	public class BurningCrusadeLoadingTests : RecordLoadingTests
	{
		[SetUp]
		public override void Setup()
		{
			this.Version = WarcraftVersion.BurningCrusade;
		}
	}
}
