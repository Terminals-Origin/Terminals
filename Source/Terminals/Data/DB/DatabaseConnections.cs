using System;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using Terminals.Configuration;
using Terminals.Connections;
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

        private static Database CreateInstance(string connecitonString)
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
            string hashToCheck = string.Empty;
            if (!string.IsNullOrEmpty(databasePassword))
                hashToCheck = PasswordFunctions.ComputeMasterPasswordHash(databasePassword);
            return hashToCheck == storedHash;
        }

        private static string TryGetMasterPasswordHash(string connectionString)
        {
            using (Database database = CreateInstance(connectionString))
            {
                return database.GetMasterPasswordHash();
            }
        }

        private SqlPersistenceSecurity persistenceSecurity;

        internal static void UpdateMastrerPassord(string connectionString, string oldPassword, string newPassword)
        {
            Tuple<bool, string> oldPasswordCheck = TestConnection(connectionString, oldPassword);
            if (oldPasswordCheck.Item1)
                CommitNewMastrerPassord(connectionString, oldPassword, newPassword);
        }

        private static void CommitNewMastrerPassord(string connecitonString, string oldPassword, string newPassword)
        {
            // todo do it in transaction to prevent inconsistent data
            using (Database database = CreateInstance(connecitonString))
            {
                database.persistenceSecurity = new SqlPersistenceSecurity();
                database.persistenceSecurity.UpdateDatabaseKey(oldPassword);
                string newKeyMaterial = PasswordFunctions.CalculateMasterPasswordKey(newPassword);
                database.UpdateStoredPasswords(newKeyMaterial);
                database.UpdateMasterPasswodInOptions(newPassword);
            }
        }

        private void UpdateMasterPasswodInOptions(string newPassword)
        {
            string newHash = string.Empty;
            if (!string.IsNullOrEmpty(newPassword))
                newHash = PasswordFunctions.ComputeMasterPasswordHash(newPassword);
            this.UpdateMasterPassword(newHash);
        }

        private void UpdateStoredPasswords(string newKeyMaterial)
        {
            this.UpdateCredentialsPasswords(newKeyMaterial);
            IQueryable<Favorite> rdpFavorites = this.Favorites.Where(candidate => candidate.Protocol == ConnectionManager.RDP);
            this.UpdateFavoriteProtocolPasswords(rdpFavorites, newKeyMaterial);
        }

        private void UpdateFavoriteProtocolPasswords(IQueryable<Favorite> rdpFavorites, string newKeyMaterial)
        {
            foreach (Favorite favorite in rdpFavorites)
            {
                // when assigning stored, we dont have initialize complete SQLpersistence,
                // we need only the access the paswords stuff
                favorite.AssignStores(null, null, this.persistenceSecurity);
                favorite.UpdatePasswordsByNewKeyMaterial(newKeyMaterial);
            }
        }

        private void UpdateCredentialsPasswords(string newKeyMaterial)
        {
            foreach (CredentialBase credentials in this.CredentialBase)
            {
                credentials.AssignSecurity(persistenceSecurity);
                credentials.UpdatePasswordByNewKeyMaterial(newKeyMaterial);
            }
        }
    }
}
