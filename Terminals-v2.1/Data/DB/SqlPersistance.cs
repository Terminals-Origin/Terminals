using System;
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
        public IGroups Groups { get; private set; }
        public IConnectionHistory ConnectionHistory { get; private set; }

        public ICredentials Credentials
        {
            get { throw new NotImplementedException(); }
        }

        public IFactory Factory { get; private set; }

        public DataDispatcher Dispatcher { get; private set; }

        internal SqlPersistance()
        {
            this.database = DataBase.CreateDatabaseInstance();
            this.Dispatcher = new DataDispatcher();
            this.Favorites = new Favorites(this.database, this.Dispatcher);
            this.Groups = new Groups(this.database, this.Dispatcher);
            this.ConnectionHistory = new ConnectionHistory(this.database);
            this.Factory = new Factory();
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
