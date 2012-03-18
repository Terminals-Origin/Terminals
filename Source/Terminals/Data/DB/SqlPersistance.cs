using System.ComponentModel;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL Database store using Entity framework
    /// </summary>
    internal class SqlPersistance : IPersistance
    {
        private readonly DataBase database;
        public IFavorites Favorites { get; private set; }

        private Groups groups;
        public IGroups Groups
        {
            get { return this.groups; }
        }
        public IConnectionHistory ConnectionHistory { get; private set; }
        public IFactory Factory { get; private set; }
        public DataDispatcher Dispatcher { get; private set; }

        public ICredentials Credentials { get; private set; }

        internal SqlPersistance()
        {
            this.database = DataBase.CreateDatabaseInstance();
            this.Dispatcher = new DataDispatcher();
            this.groups = new Groups(this.database, this.Dispatcher);
            this.Favorites = new Favorites(this.database, this.groups, this.Dispatcher);
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
    }
}
