using System;
using System.IO;
using NUnit.Framework;
using Warcraft.Core;
using Warcraft.DBC;
using Warcraft.DBC.Definitions;

namespace libwarcraft.Tests.Integration.DBC
{
    public static class DBCTestHelper
    {
        public static DBC<T> LoadDatabase<T>(WarcraftVersion version, DatabaseName databaseName) where T : DBCRecord, new()
        {
            return new DBC<T>(version, GetDatabaseBytes(version, databaseName));
        }

        public static bool HasDatabaseFile(WarcraftVersion version, DatabaseName databaseName)
        {
            return File.Exists(GetDatabaseFilePath(version, databaseName));
        }

        public static byte[] GetDatabaseBytes(WarcraftVersion version, DatabaseName databaseName)
        {
            return File.ReadAllBytes(GetDatabaseFilePath(version, databaseName));
        }

        private static string GetDatabaseFilePath(WarcraftVersion version, DatabaseName databaseName)
        {
            var path = Path.Combine
            (
                TestContext.CurrentContext.WorkDirectory,
                "Content",
                version.ToString(),
                "DBFilesClient",
                $"{databaseName}.dbc"
            );

            return path;
        }

        /// <summary>
        /// Converts a database name into a qualified type.
        /// </summary>
        /// <param name="databaseName">The enumerated name of the database,</param>
        /// <returns>The type mapping to the database name.</returns>
        public static Type GetRecordTypeFromDatabaseName(DatabaseName databaseName)
        {
            return Type.GetType($"Warcraft.DBC.Definitions.{databaseName}Record, libwarcraft");
        }

        /// <summary>
        /// Gets the database name from a type.
        /// </summary>
        /// <param name="recordType">The type of the record.</param>
        /// <returns>The enumerated database name.</returns>
        /// <exception cref="ArgumentException">Thrown if the given type can't be resolved to a database name.</exception>
        public static DatabaseName GetDatabaseNameFromRecordType(Type recordType)
        {
            string recordName = recordType.Name.Replace("Record", string.Empty);
            if (Enum.TryParse(recordName, true, out DatabaseName databaseName))
            {
                return databaseName;
            }

            throw new ArgumentException("The given type could not be resolved to a database name.", nameof(recordType));
        }
    }
}
