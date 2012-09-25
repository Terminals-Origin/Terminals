using System;
using System.Data.EntityClient;
using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data.DB
{
    internal partial class Database
    {
        private const string PROVIDER = "System.Data.SqlClient";
        private const string METADATA = @"res://*/Data.DB.SQLPersistence.csdl|res://*/Data.DB.SQLPersistence.ssdl|res://*/Data.DB.SQLPersistence.msl";
        internal const string DEFAULT_CONNECTION_STRING = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Data\Terminals.mdf;Integrated Security=True;User Instance=False";

        internal static Database CreateInstance()
        {
            return CreateDatabase(Settings.ConnectionString);
        }

        private static Database CreateDatabase(string connecitonString)
        {
            string connectionString = BuildConnectionString(connecitonString);
            return new Database(connectionString);
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
            Tuple<bool, string> result = TestConnection(Settings.ConnectionString, Settings.DatabaseMasterPassword);
            return result.Item1;
        }

        /// <summary>
        /// Tryes to execute simple command on database to ensure, that the conneciton works.
        /// </summary>
        /// <param name="connectionStringToTest">Not null MS SQL connection string
        ///  to use to create new database instance</param>
        /// <param name="databasePassword">Not encrypted database pasword</param>
        /// <returns>True, if connection test was sucessfull; otherwise false
        /// and string containg the error message</returns>
        internal static Tuple<bool, string> TestConnection(string connectionStringToTest, string databasePassword)
        {
            try
            {
                var passwordIsValid = TestDatabasePassword(connectionStringToTest, databasePassword);
                return new Tuple<bool, string>(passwordIsValid, "Database password doesnt match.");
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                if (exception.InnerException != null)
                    message = exception.InnerException.Message;
                return new Tuple<bool, string>(false, message);
            }
        }

        private static bool TestDatabasePassword(string connectionStringToTest, string databasePassword)
        {
            string storedHash = TryGetMasterPasswordHash(connectionStringToTest);
            string hashToCheck = PasswordFunctions.ComputeMasterPasswordHash(databasePassword);
            return hashToCheck == storedHash;
        }

        private static string TryGetMasterPasswordHash(string connectionString)
        {
            using (Database database = CreateDatabase(connectionString))
            {
                return database.GetMasterPasswordHash();
            }
        }

        internal static void UpdateMastrerPassord(string newPassword)
        {
            using (var database = CreateInstance())
            {
                string newHash = PasswordFunctions.ComputeMasterPasswordHash(newPassword);
                database.UpdateMasterPassword(newHash);
            }
        }
    }
}
