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

        /// <summary>
        /// The Connection string getter is performace expensive because of decryption, so we reuse the connetion instance.
        /// Until there is no possibility to change the persistence by runtime, we dont have to release the connection.
        /// </summary>
        private static EntityConnection cachedConnection;

        internal static Database CreateInstance()
        {
            EntityConnection connection = CacheConnection();
            return new Database(connection);
        }

        private static EntityConnection CacheConnection()
        {
            if (cachedConnection == null)
                cachedConnection = BuildConnection(Settings.ConnectionString);

            return cachedConnection;
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

        /// <summary>
        /// Entity framework performance optimization, the connection is opened once for batch operations.
        /// </summary>
        internal static void OpenConnection()
        {
            cachedConnection.Open();
        }

        /// <summary>
        /// Entity framework performance optimization, the connection is opened once for batch operations.
        /// </summary>
        internal static void CloseConneciton()
        {
            cachedConnection.Close();
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
            return PasswordFunctions.MasterPasswordIsValid(databasePassword, storedHash);
        }

        private static string TryGetMasterPasswordHash(string connectionString)
        {
            using (Database database = CreateInstance(connectionString))
            {
                return database.GetMasterPasswordHash();
            }
        }
    }
}
