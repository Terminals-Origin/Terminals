using System.ComponentModel;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL Database store using Entity framework
    /// </summary>
    internal class SqlPersistence : IPersistence, IPersistedSecurity
    {
        private Database database;

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

        internal SqlPersistence()
        {
            this.Security = new PersistenceSecurity(this);
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

        public void Initialize()
        {
            this.database = Database.CreateDatabaseInstance();
            this.Dispatcher = new DataDispatcher();
            this.groups = new Groups(this.database, this.Dispatcher);
            this.favorites = new Favorites(this.database, this.groups, this.Dispatcher);
            this.ConnectionHistory = new ConnectionHistory(this.database);
            this.Credentials = new StoredCredentials(this.database);
            this.Factory = new Factory(this.database);
        }

        public void UpdatePasswordsByNewMasterPassword(string newMasterPassword)
        {
            // nothing to do here, the application master password doesnt affect the database
        }
    }
}
