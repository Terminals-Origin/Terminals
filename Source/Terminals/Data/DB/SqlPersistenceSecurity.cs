using System;
using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Distinguish between the application master password and persistence master password.
    /// Used only by SqlPersistence. 
    /// </summary>
    internal class SqlPersistenceSecurity : PersistenceSecurity
    {
        private string persistenceKeyMaterial;

        protected override string PersistenceKeyMaterial
        {
            get { return this.persistenceKeyMaterial; }
        }

        internal bool UpdateDatabaseKey()
        {
            return this.UpdateDatabaseKey(Settings.ConnectionString, Settings.DatabaseMasterPassword);
        }

        internal bool UpdateDatabaseKey(string connectionString, string databasePassword)
        {
            try
            {
                string databaseStoredKey = DatabaseConnections.TryGetMasterPasswordHash(connectionString);
                this.persistenceKeyMaterial = PasswordFunctions2.CalculateMasterPasswordKey(databasePassword, databaseStoredKey);
                return true;
            }
            catch
            {
                Logging.Error("Unable to obtain database key from database");
                return false;
            }
        }
    }
}