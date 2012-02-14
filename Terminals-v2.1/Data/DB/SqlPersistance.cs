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
            // TODO Assign custom connection string from settings (Jiri Pokorny, 13.02.2012)
            string connectionString = BuildConnectionString();
            this.Database = new DataBase(connectionString);
            this.Dispatcher = new DataDispatcher();
        }

        private const string developmentConnectionString = @"Data Source=.\SQLEXPRESS;AttachDbFilename=.\Data\Terminals.mdf;Integrated Security=True;User Instance=False";
        private const string metadata = @"res://*/Data.DB.SQLPersistance.csdl|res://*/Data.DB.SQLPersistance.ssdl|res://*/Data.DB.SQLPersistance.msl";

        private string BuildConnectionString()
        {
            var connectionBuilder = new EntityConnectionStringBuilder
                                        {
                                            Provider = "System.Data.SqlClient",
                                            ProviderConnectionString = developmentConnectionString,
                                            Metadata = metadata
                                        };

            return connectionBuilder.ToString();
        }

        public void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            // nothing to do here
        }

        public void StartDelayedUpdate()
        {
            throw new NotImplementedException();
        }

        public void SaveAndFinishDelayedUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
