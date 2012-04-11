using System.ComponentModel;
using Terminals.Security;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL Database store using Entity framework
    /// </summary>
    internal class SqlPersistance : IPersistance, IPersistedSecurity
    {
        private readonly DataBase database;

        private Favorites favorites;
        public IFavorites Favorites
        {
            get
            {
                return this.favorites;
            }
        }

        private Groups groups;
        public IGroups Groups
        {
            get { return this.groups; }
        }
        public IConnectionHistory ConnectionHistory { get; private set; }
        public IFactory Factory { get; private set; }
        public DataDispatcher Dispatcher { get; private set; }

        public ICredentials Credentials { get; private set; }

        public PersistenceSecurity Security { get; private set; }

        public string MasterPasswordHash
        {
            get
            {
                return this.database.GetMasterPassword();
            }
            set
            {
                this.database.UpdateMasterPassword(value);
            }
        }

        internal SqlPersistance()
        {
            this.database = DataBase.CreateDatabaseInstance();
            this.Security = new PersistenceSecurity(this);
            this.Dispatcher = new DataDispatcher();
            this.groups = new Groups(this.database, this.Dispatcher);
            this.favorites = new Favorites(this.database, this.groups, this.Dispatcher);
            this.ConnectionHistory = new ConnectionHistory(this.database);
            this.Credentials = new StoredCredentials(this.database);
            this.Factory = new Factory(this.database);
        }

        public void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            // nothing to do here
        }

        public void StartDelayedUpdate()
        {
            this.database.StartDelayedUpdate();
        }

        public void SaveAndFinishDelayedUpdate()
        {
            this.database.SaveAndFinishDelayedUpdate();
        }

        public void UpdatePasswordsByNewMasterPassword(string newMasterPassword)
        {
            string newKeyMaterial = PasswordFunctions.CalculateMasterPasswordKey(newMasterPassword);
            this.Credentials.UpdatePasswordsByNewKeyMaterial(newKeyMaterial);
            Data.Favorites.UpdateFavoritePasswordsByNewKeyMaterial(this.favorites, newKeyMaterial);
            this.database.SaveImmediatelyIfRequested();
        }
    }
}
