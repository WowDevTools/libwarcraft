using System;
using System.Linq;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Integration.DBC
{
    [TestFixture]
    public abstract class RecordDefinitionTests
    {
        protected WarcraftVersion Version;

        [SetUp]
        public abstract void Setup();

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

        [Test]
        public void AnimationDataRecord()
        {
            TestLoadRecord<AnimationDataRecord>(this.Version);
        }

        [Test]
        public void CharHairGeosetsRecord()
        {
            TestLoadRecord<CharHairGeosetsRecord>(this.Version);
        }

        [Test]
        public void SoundAmbienceRecord()
        {
            TestLoadRecord<SoundAmbienceRecord>(this.Version);
        }

        [Test]
        public void CharSectionsRecord()
        {
            TestLoadRecord<CharSectionsRecord>(this.Version);
        }

        [Test]
        public void SoundEntriesRecord()
        {
            TestLoadRecord<SoundEntriesRecord>(this.Version);
        }

        [Test]
        public void CreatureDisplayInfoExtraRecord()
        {
            TestLoadRecord<CreatureDisplayInfoExtraRecord>(this.Version);
        }

        [Test]
        public void SoundProviderPreferences()
        {
            TestLoadRecord<SoundProviderPreferencesRecord>(this.Version);
        }

        [Test]
        public void CreatureDisplayInfoRecord()
        {
            TestLoadRecord<CreatureDisplayInfoRecord>(this.Version);
        }

        [Test]
        public void SpellRecord()
        {
            TestLoadRecord<SpellRecord>(this.Version);
        }

        [Test]
        public void CreatureModelDataRecord()
        {
            TestLoadRecord<CreatureModelDataRecord>(this.Version);
        }

        [Test]
        public void WMOAreaTableRecord()
        {
            TestLoadRecord<WMOAreaTableRecord>(this.Version);
        }

        [Test]
        public void LiquidObjectRecord()
        {
            TestLoadRecord<LiquidObjectRecord>(this.Version);
        }

        [Test]
        public void ZoneIntroMusicTableRecord()
        {
            TestLoadRecord<ZoneIntroMusicTableRecord>(this.Version);
        }

        [Test]
        public void LiquidTypeRecord()
        {
            TestLoadRecord<LiquidTypeRecord>(this.Version);
        }

        [Test]
        public void ZoneMusicRecord()
        {
            TestLoadRecord<ZoneMusicRecord>(this.Version);
        }

        [Test]
        public void MapRecord()
        {
            TestLoadRecord<MapRecord>(this.Version);
        }
    }
}
