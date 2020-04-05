//
//  RecordDefinitionTests.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Linq;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.DBC.Definitions;

#pragma warning disable 1591, SA1600

namespace Warcraft.Integration.DBC
{
    [TestFixture]
    public abstract class RecordDefinitionTests
    {
        protected WarcraftVersion Version { get; set; }

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
                _ = database.First();
            }
            catch (ArgumentException e)
            {
                Assert.Fail($"Failed to read {databaseName}: {e}");
            }
        }

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
            TestLoadRecord<SoundAmbianceRecord>(Version);
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
