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

        internal void UpdateDatabaseKey()
        {
            this.UpdateDatabaseKey(Settings.DatabaseMasterPassword);
        }

        internal void UpdateDatabaseKey(string databasePassword)
        {
            this.persistenceKeyMaterial = PasswordFunctions.CalculateMasterPasswordKey(databasePassword);
        }
    }
}