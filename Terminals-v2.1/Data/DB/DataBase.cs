using System;
using System.Data.EntityClient;
using System.Linq;
using Terminals.Configuration;

namespace Terminals.Data.DB
{
    public partial class DataBase
    {
        private const string PROVIDER = "System.Data.SqlClient";       
        private const string METADATA = @"res://*/Data.DB.SQLPersistance.csdl|res://*/Data.DB.SQLPersistance.ssdl|res://*/Data.DB.SQLPersistance.msl";
        private const string DEVELOPMENT_CONNECTION_STRING = @"Data Source=.\SQLEXPRESS;AttachDbFilename=.\Data\Terminals.mdf;Integrated Security=True;User Instance=False";
        
        private bool delaySave = false;
        // TODO Add eventing, if something is changed in database (Jiri Pokorny, 13.02.2012)
        // TODO Add new table for database extra options, which couldnt be stored localy like masterpassword (Jiri Pokorny, 26.02.2012)

        internal static DataBase CreateDatabaseInstance()
        {
            string connectionString = BuildConnectionString();
            return new DataBase(connectionString);
        }

        private static string BuildConnectionString()
        {
            var connectionBuilder = new EntityConnectionStringBuilder
            {
                Provider = PROVIDER,
                Metadata = METADATA,
                ProviderConnectionString = Settings.ConnectionString
            };

            return connectionBuilder.ToString();
        }

        internal void StartDelayedUpdate()
        {
            delaySave = true;
        }

        internal void SaveAndFinishDelayedUpdate()
        {
            delaySave = false;
            SaveImmediatelyIfRequested();
        }

        internal void SaveImmediatelyIfRequested()
        {
            if (!delaySave)
            {
                // todo Add protection against concurent updates
                this.SaveChanges();
            }
        }

        internal Favorite GetFavoriteByGuid(Guid favoriteId)
        {
            return this.Favorites
                .FirstOrDefault(favorite => favorite.Guid == favoriteId);
        }
    }
}
