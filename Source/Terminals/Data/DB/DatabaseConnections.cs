using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data.DB
{
    internal static class DatabaseConnections
    {
        private const string PROVIDER = "System.Data.SqlClient";

        private static readonly Settings settings = Settings.Instance;
        /// <summary>
        /// Load all the EF metadata from current assembly
        /// </summary>
        private const string METADATA = @"res://Terminals/Data.DB.SQLPersistence.csdl|res://Terminals/Data.DB.SQLPersistence.ssdl|res://Terminals/Data.DB.SQLPersistence.msl";

        internal const string DEFAULT_CONNECTION_STRING = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Data\Terminals.mdf;Integrated Security=True;User Instance=False";

        internal static Database CreateInstance()
        {
            EntityConnection connection = BuildConnection(settings.ConnectionString);
            return new Database(connection);
        }

        internal static Database CreateInstance(string connecitonString)
        {
            EntityConnection connection = BuildConnection(connecitonString);
            return new Database(connection);
        }

        private static EntityConnection BuildConnection(string connecitonString)
        {
            string connectionString = BuildConnectionString(connecitonString);
            return new EntityConnection(connectionString);
        }

        private static string BuildConnectionString(string providerConnectionString)
        {
            var connectionBuilder = new EntityConnectionStringBuilder
                                        {
                                            Provider = PROVIDER,
                                            Metadata = METADATA,
                                            ProviderConnectionString = providerConnectionString
                                        };

            return connectionBuilder.ToString();
        }

        internal static bool TestConnection()
        {
            TestConnectionResult result = TestConnection(settings.ConnectionString, settings.DatabaseMasterPassword);
            return result.Successful;
        }

        /// <summary>
        /// Tries to execute simple command on database to ensure, that the connection works.
        /// </summary>
        /// <param name="connectionStringToTest">Not null MS SQL connection string
        ///  to use to create new database instance</param>
        /// <param name="databasePassword">Not encrypted database password</param>
        /// <returns>True, if connection test was successful; otherwise false
        /// and string containing the error message</returns>
        internal static TestConnectionResult TestConnection(string connectionStringToTest, string databasePassword)
        {
            try
            {
                var passwordIsValid = TestDatabasePassword(connectionStringToTest, databasePassword);
                if (passwordIsValid)
                    return new TestConnectionResult();

                return new TestConnectionResult("Database password doesn't match.");
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                if (exception.InnerException != null)
                    message = exception.InnerException.Message;
                return new TestConnectionResult(message);
            }
        }

        private static bool TestDatabasePassword(string connectionStringToTest, string databasePassword)
        {
            string storedHash = TryGetMasterPasswordHash(connectionStringToTest);
            return PasswordFunctions2.MasterPasswordIsValid(databasePassword, storedHash);
        }

        internal static string TryGetMasterPasswordHash(string connectionString)
        {
            using (Database database = CreateInstance(connectionString))
            {
                return database.GetMasterPasswordHash();
            }
        }

        internal static List<string> FindDatabasesOnServer(string connectionString)
        {
            try
            {
                return TryFindDatabases(connectionString);
            }
            catch
            {
                // don't log an exception, because some SqlExceptions contain connection string information
                return new List<string>();
            }
        }

        private static List<string> TryFindDatabases(string connectionString)
        {
            using (Database database = CreateInstance(connectionString))
            {
                const string COMMAND_TEXT = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";
                return database.Database.SqlQuery<string>(COMMAND_TEXT)
                    .ToList();
            }
        }

        internal static TestConnectionResult ValidateDatabaseConnection(string connectionString, string databasePassword)
        {
            TestConnectionResult connectionResult = TestConnection(connectionString, databasePassword);
            // todo enable database versioning
            return connectionResult;
        }
    }
}
