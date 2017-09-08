using System;
using System.Linq;
using NUnit.Framework;
using Warcraft.Core;
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
				var _ = database.First();
			}
			catch (ArgumentException e)
			{
				Assert.Fail($"Failed to read {databaseName}: {e}");
			}
		}

		public class LoadRecord
		{
			[Test]
			public void AnimationDataRecord()
			{
				TestLoadRecord<AnimationDataRecord>(Version);
			}

			[Test]
			public void CharHairGeosetsRecord()
			{
				TestLoadRecord<CharHairGeosetsRecord>(Version);
			}

			[Test]
			public void SoundAmbienceRecord()
			{
				TestLoadRecord<SoundAmbienceRecord>(Version);
			}

			[Test]
			public void CharSectionsRecord()
			{
				TestLoadRecord<CharSectionsRecord>(Version);
			}

			[Test]
			public void SoundEntriesRecord()
			{
				TestLoadRecord<SoundEntriesRecord>(Version);
			}

			[Test]
			public void CreatureDisplayInfoExtraRecord()
			{
				TestLoadRecord<CreatureDisplayInfoExtraRecord>(Version);
			}

			[Test]
			public void SoundProviderPreferences()
			{
				TestLoadRecord<SoundProviderPreferencesRecord>(Version);
			}

			[Test]
			public void CreatureDisplayInfoRecord()
			{
				TestLoadRecord<CreatureDisplayInfoRecord>(Version);
			}

			[Test]
			public void SpellRecord()
			{
				TestLoadRecord<SpellRecord>(Version);
			}

			[Test]
			public void CreatureModelDataRecord()
			{
				TestLoadRecord<CreatureModelDataRecord>(Version);
			}

			[Test]
			public void WMOAreaTableRecord()
			{
				TestLoadRecord<WMOAreaTableRecord>(Version);
			}

			[Test]
			public void LiquidObjectRecord()
			{
				TestLoadRecord<LiquidObjectRecord>(Version);
			}

			[Test]
			public void ZoneIntroMusicTableRecord()
			{
				TestLoadRecord<ZoneIntroMusicTableRecord>(Version);
			}

			[Test]
			public void LiquidTypeRecord()
			{
				TestLoadRecord<LiquidTypeRecord>(Version);
			}

			[Test]
			public void ZoneMusicRecord()
			{
				TestLoadRecord<ZoneMusicRecord>(Version);
			}

			[Test]
			public void MapRecord()
			{
				TestLoadRecord<MapRecord>(Version);
			}
		}
	}
}
