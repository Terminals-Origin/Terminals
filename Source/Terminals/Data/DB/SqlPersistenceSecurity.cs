using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Distinquish between the application masterpassword and persistence masterpassword.
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