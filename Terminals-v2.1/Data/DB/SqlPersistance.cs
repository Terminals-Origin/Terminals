using System;
using System.ComponentModel;
using System.Data.EntityClient;
using Terminals.History;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL Database store using Entity framework
    /// </summary>
    internal class SqlPersistance : IPersistance
    {
      internal DataBase Database { get; private set; }

        public IFavorites Favorites
        {
            get { throw new NotImplementedException(); }
        }

        public IGroups Groups
        {
            get { throw new NotImplementedException(); }
        }

        public IFactory Factory
        {
            get { throw new NotImplementedException(); }
        }

        public IConnectionHistory ConnectionHistory
        {
            get { throw new NotImplementedException(); }
        }

        public ICredentials Credentials
        {
            get { throw new NotImplementedException(); }
        }

        public DataDispatcher Dispatcher { get; private set; }

        internal SqlPersistance()
        {
            this.Database = DataBase.CreateDatabaseInstance();
            this.Dispatcher = new DataDispatcher();
        }

        public void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            // nothing to do here
        }

        public void StartDelayedUpdate()
        {
            this.Database.StartDelayedUpdate();
        }

        public void SaveAndFinishDelayedUpdate()
        {
            this.Database.SaveAndFinishDelayedUpdate();
        }
    }
}
