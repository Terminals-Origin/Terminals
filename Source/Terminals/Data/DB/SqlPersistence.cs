using System;
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
            this.Security = new PersistenceSecurity();
            this.Security.AssignPersistence(this);
        }

        public void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
          // todo this.database.InitializeReLoadClock(synchronizer);
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
            if(!this.TryInitializeDatabase())
                return;

            this.Dispatcher = new DataDispatcher();
            this.groups = new Groups(this.database, this.Dispatcher);
            this.favorites = new Favorites(this.database, this.groups, this.Dispatcher);
            this.ConnectionHistory = new ConnectionHistory(this.database);
            this.Credentials = new StoredCredentials(this.database);
            this.Factory = new Factory(this.database);
        }

        private bool TryInitializeDatabase()
        {
            try
            {
                this.database = Database.CreateDatabaseInstance();
                this.database.GetMasterPasswordHash(); // dummy test
                return true;
            }
            catch (Exception exception)
            {
                Logging.Log.Fatal("SQL Perstance layer failed to load. Fall back to File persistence", exception);
                Persistence.FallBackToPrimaryPersistence(this.Security);
                return false;
            }
        }

        public void UpdatePasswordsByNewMasterPassword(string newMasterPassword)
        {
            // nothing to do here, the application master password doesnt affect the database
        }
    }
}
