using System;
using System.Linq;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Integration.DBC.Vanilla
{
	[TestFixture]
	public class RecordLoadingTests
	{
		private const WarcraftVersion Version = WarcraftVersion.Classic;

		private static void TestLoadRecord<T>(WarcraftVersion version) where T : DBCRecord, new()
		{
			var databaseName = DBCTestHelper.GetDatabaseNameFromRecordType(typeof(T));
			if (!DBCTestHelper.HasDatabaseFile(version, databaseName))
			{
				Assert.Ignore("Database file not present. Skipping.");
			}

			var database = DBCTestHelper.LoadDatabase<T>(version, databaseName);

			try
			{
				database.First();
			}
			catch (ArgumentException e)
			{
				Assert.Fail($"Failed to read {databaseName}: {e}");
			}
		}

		[Test]
		public void TestLoadAnimationDataRecord()
		{
			TestLoadRecord<AnimationDataRecord>(Version);
		}

		[Test]
		public void TestLoadCharHairGeosetsRecord()
		{
			TestLoadRecord<CharHairGeosetsRecord>(Version);
		}

		[Test]
		public void TestLoadSoundAmbienceRecord()
		{
			TestLoadRecord<SoundAmbienceRecord>(Version);
		}

		[Test]
		public void TestLoadCharSectionsRecord()
		{
			TestLoadRecord<CharSectionsRecord>(Version);
		}

		[Test]
		public void TestLoadSoundEntriesRecord()
		{
			TestLoadRecord<SoundEntriesRecord>(Version);
		}

		[Test]
		public void TestLoadCreatureDisplayInfoExtraRecord()
		{
			TestLoadRecord<CreatureDisplayInfoExtraRecord>(Version);
		}

		[Test]
		public void TestLoadSoundProviderPreferences()
		{
			TestLoadRecord<SoundProviderPreferencesRecord>(Version);
		}

		[Test]
		public void TestLoadCreatureDisplayInfoRecord()
		{
			TestLoadRecord<CreatureDisplayInfoRecord>(Version);
		}

		[Test]
		public void TestLoadSpellRecord()
		{
			TestLoadRecord<SpellRecord>(Version);
		}

		[Test]
		public void TestLoadCreatureModelDataRecord()
		{
			TestLoadRecord<CreatureModelDataRecord>(Version);
		}

		[Test]
		public void TestLoadWMOAreaTableRecord()
		{
			TestLoadRecord<WMOAreaTableRecord>(Version);
		}

		[Test]
		public void TestLoadLiquidObjectRecord()
		{
			TestLoadRecord<LiquidObjectRecord>(Version);
		}

		[Test]
		public void TestLoadZoneIntroMusicTableRecord()
		{
			TestLoadRecord<ZoneIntroMusicTableRecord>(Version);
		}

		[Test]
		public void TestLoadLiquidTypeRecord()
		{
			TestLoadRecord<LiquidTypeRecord>(Version);
		}

		[Test]
		public void TestLoadZoneMusicRecord()
		{
			TestLoadRecord<ZoneMusicRecord>(Version);
		}

		[Test]
		public void TestLoadMapRecord()
		{
			TestLoadRecord<MapRecord>(Version);
		}

	}
}
